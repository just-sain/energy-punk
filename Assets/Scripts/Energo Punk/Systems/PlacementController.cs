using EnergyPunk.Buildings;
using EnergyPunk.Core;
using EnergyPunk.Population;
using EnergyPunk.Resources;
using UnityEngine;

namespace EnergyPunk.Systems
{
    public class PlacementController : MonoBehaviour
    {
        [Header("Refs")]
        public ResourceBank bank;
        public WorkerPool workerPool;

        [Header("Prefabs")]
        public StationBase generatorPrefab;
        public StationBase kitchenPrefab;
        public StationBase workshopPrefab;
        public StationBase storagePrefab;
        public StationBase housingPrefab;

        public void UI_SelectGenerator() => Select(StationType.Generator);
        public void UI_SelectKitchen() => Select(StationType.Kitchen);
        public void UI_SelectWorkshop() => Select(StationType.Workshop);
        public void UI_SelectStorage() => Select(StationType.Storage);
        public void UI_SelectHousing() => Select(StationType.Housing);


        private StationType _selected = StationType.Generator;

        private StationBase _pendingAssignStation;
        private string _buffer = "";
        private string _lastBufferLogged = "";
        public StationType SelectedType => _selected;
        public bool IsAssigning => _pendingAssignStation != null;
        public string AssignBuffer => _buffer;


        void Update()
        {
            if (_pendingAssignStation == null)
            {
                if (Input.GetKeyDown(KeyCode.G)) Select(StationType.Generator);
                if (Input.GetKeyDown(KeyCode.K)) Select(StationType.Kitchen);
                if (Input.GetKeyDown(KeyCode.W)) Select(StationType.Workshop);
                if (Input.GetKeyDown(KeyCode.S)) Select(StationType.Storage);
                if (Input.GetKeyDown(KeyCode.H)) Select(StationType.Housing);

                if (Input.GetMouseButtonDown(0))
                    TryPlaceOnClickedRoom();
            }
            else
            {
                HandleAssignInput();
            }
        }

        public bool HasPendingStation => _pendingAssignStation != null;

        public void UI_ConfirmAssign(int workers)
        {
            if (_pendingAssignStation == null) return;

            int cap = _pendingAssignStation.capacity;
            int desired = Mathf.Clamp(workers, 0, cap);

            bool ok = _pendingAssignStation.SetWorkers(desired);
            if (!ok)
            {
                Debug.LogWarning($"[ASSIGN] Not enough free workers. Need {desired}, Free {workerPool.Free}.");
                return;
            }

            Debug.Log($"[ASSIGN] Confirmed: {desired}/{cap} -> Efficiency {_pendingAssignStation.Efficiency:0.00}");

            _pendingAssignStation = null;
            _buffer = "";
            _lastBufferLogged = "";
        }


        void Select(StationType t)
        {
            _selected = t;

            if (workerPool == null)
            {
                Debug.LogWarning($"[SELECT] {_selected} | WorkerPool is NULL (assign it in Inspector).");
                return;
            }

            Debug.Log($"[SELECT] {_selected} | Free workers: {workerPool.Free}/{workerPool.Total}");
        }


        void TryPlaceOnClickedRoom()
        {
            var cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("[PLACE] Camera.main is null. Set camera Tag = MainCamera.");
                return;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 500f)) return;

            RoomArea room = hit.collider.GetComponentInParent<RoomArea>();
            if (room == null) return;

            StationBase prefab = GetPrefab(_selected);
            if (prefab == null)
            {
                Debug.LogWarning($"[PLACE] Prefab for {_selected} is not set.");
                return;
            }

            var time = FindObjectOfType<TimeSystem>();

            // Если в комнате уже есть станция — удаляем (замена)
            StationBase existing = room.GetComponentInChildren<StationBase>();
            if (existing != null)
            {
                if (time != null) time.Unregister(existing);
                Destroy(existing.gameObject); // workers вернутся через OnDestroy()
                Debug.Log($"[PLACE] Replaced old station in room '{room.name}'.");
            }

            Vector3 pos = room.GetCenter();
            pos.y += 0.25f;

            var station = Instantiate(prefab, pos, Quaternion.identity, room.transform);
            station.bank = bank;
            station.workerPool = workerPool;

            if (time != null) time.Register(station);

            _pendingAssignStation = station;
            _buffer = "";
            _lastBufferLogged = "";

            Debug.Log($"[ASSIGN] {_selected} placed in '{room.name}'. Capacity={station.capacity}. Digits -> Enter, Esc cancel.");
        }

        StationBase GetPrefab(StationType t)
        {
            switch (t)
            {
                case StationType.Generator: return generatorPrefab;
                case StationType.Kitchen: return kitchenPrefab;
                case StationType.Workshop: return workshopPrefab;
                case StationType.Storage: return storagePrefab;
                case StationType.Housing: return housingPrefab;
                default: return null;
            }
        }

        void HandleAssignInput()
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c))
                    _buffer += c;
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && _buffer.Length > 0)
                _buffer = _buffer.Substring(0, _buffer.Length - 1);

            if (_buffer != _lastBufferLogged)
            {
                _lastBufferLogged = _buffer;
                Debug.Log($"[ASSIGN] Input: '{_buffer}' (Enter=confirm, Esc=cancel)");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var time = FindObjectOfType<TimeSystem>();
                if (time != null) time.Unregister(_pendingAssignStation);

                Destroy(_pendingAssignStation.gameObject);
                _pendingAssignStation = null;
                _buffer = "";
                _lastBufferLogged = "";
                Debug.Log("[ASSIGN] Cancelled (station removed).");
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                int desired = 0;
                if (!string.IsNullOrEmpty(_buffer))
                    int.TryParse(_buffer, out desired);

                int cap = _pendingAssignStation.capacity;
                desired = Mathf.Clamp(desired, 0, cap);

                bool ok = _pendingAssignStation.SetWorkers(desired);
                if (!ok)
                {
                    Debug.LogWarning($"[ASSIGN] Not enough free workers. Need {desired}, Free {workerPool.Free}.");
                    return;
                }

                Debug.Log($"[ASSIGN] Confirmed: {desired}/{cap} -> Efficiency {_pendingAssignStation.Efficiency:0.00}");

                _pendingAssignStation = null;
                _buffer = "";
                _lastBufferLogged = "";
            }
        }
    }
}

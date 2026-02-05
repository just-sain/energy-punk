using TMPro;
using UnityEngine;
using EnergyPunk.Resources;
using EnergyPunk.Population;
using EnergyPunk.Systems;
using EnergyPunk.Buildings;

namespace EnergyPunk.UI
{
    public class HudView : MonoBehaviour
    {
        [Header("Refs")]
        public ResourceBank bank;
        public WorkerPool workerPool;
        public PlacementController placement;

        [Header("Text")]
        public TMP_Text resourcesText;
        public TMP_Text workersText;
        public TMP_Text selectedText;
        public TMP_Text assignText;

        float _acc;

        void Update()
        {
            if (bank == null || workerPool == null || placement == null) return;

            _acc += Time.deltaTime;
            if (_acc < 0.25f) return; // обновляем 4 раза в секунду, чтобы не спамить
            _acc = 0f;

            float fuel = bank.Get(ResourceType.Fuel);
            float energy = bank.Get(ResourceType.Energy);
            float food = bank.Get(ResourceType.Food);
            float mat = bank.Get(ResourceType.Materials);

            resourcesText.text =
                $"Fuel: {fuel:0.0}\nEnergy: {energy:0.0}\nFood: {food:0.0}\nMat: {mat:0.0}";

            workersText.text = $"Workers: {workerPool.Free}/{workerPool.Total}";

            selectedText.text = $"Selected: {placement.SelectedType}";

            if (placement.IsAssigning)
                assignText.text = $"Assign workers: {placement.AssignBuffer} (Enter подтвердить / Esc отмена)";
            else
                assignText.text = "Click room to place station";
        }
    }
}

using UnityEngine;

namespace EnergyPunk.Buildings
{
    public class RoomArea : MonoBehaviour
    {
        public Transform centerOverride; // если хочешь вручную
        private Collider _col;

        void Awake()
        {
            _col = GetComponent<Collider>();
        }

        public Vector3 GetCenter()
        {
            if (centerOverride != null) return centerOverride.position;
            if (_col != null) return _col.bounds.center;
            return transform.position;
        }
    }
}

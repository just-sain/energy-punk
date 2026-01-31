using UnityEngine;
using EnergyPunk.Core;
using EnergyPunk.Resources;

namespace EnergyPunk.Buildings
{
    public class Generator : MonoBehaviour, ITickable
    {
        [Header("References")]
        public ResourceBank bank;

        [Header("Generator Stats")]
        public float maxEnergy = 20f;   // L1
        public float fuelPerHour = 1f;  // L1

        void OnEnable()
        {
            var time = FindObjectOfType<TimeSystem>();
            if (time != null) time.Register(this);
        }

        void OnDisable()
        {
            var time = FindObjectOfType<TimeSystem>();
            if (time != null) time.Unregister(this);
        }

        public void Tick(float deltaMinutes)
        {
            if (bank == null) return;

            float deltaHours = deltaMinutes / 60f;

            // Тратим топливо
            float needFuel = fuelPerHour * deltaHours;
            float haveFuel = bank.Get(ResourceType.Fuel);
            float usedFuel = Mathf.Min(haveFuel, needFuel);
            if (usedFuel > 0f) bank.Spend(ResourceType.Fuel, usedFuel);

            // Если топлива осталось > 0 — генератор даёт энергию, иначе 0
            bool hasFuelLeft = bank.Get(ResourceType.Fuel) > 0f;
            bank.Set(ResourceType.Energy, hasFuelLeft ? maxEnergy : 0f);
            Debug.Log($"[GEN] Fuel: {bank.Get(ResourceType.Fuel)} Energy: {bank.Get(ResourceType.Energy)}");

        }
    }
}

using UnityEngine;
using EnergyPunk.Core;
using EnergyPunk.Resources;

namespace EnergyPunk.Buildings
{
    public abstract class Building : MonoBehaviour, ITickable
    {
        [Header("References")]
        public ResourceBank bank;

        [Header("State")]
        public bool enabledBuilding = true;

        [Header("Energy Consumption")]
        [Tooltip("Энергия (мощность) в час, которую потребляет здание")]
        public float energyPerHour = 1f;

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
            if (!enabledBuilding) return;
            if (bank == null) return;

            float deltaHours = deltaMinutes / 60f;
            float needEnergy = energyPerHour * deltaHours;

            // MVP правило: если энергии не хватает — здание НЕ работает в этот тик
            if (bank.Get(ResourceType.Energy) < needEnergy) return;

            bank.Spend(ResourceType.Energy, needEnergy);
            OnTickPowered(deltaMinutes);
        }

        protected abstract void OnTickPowered(float deltaMinutes);
    }
}

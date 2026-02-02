using UnityEngine;
using EnergyPunk.Resources;

namespace EnergyPunk.Buildings
{
    public class GeneratorStation : StationBase
    {
        [Header("Generator")]
        public float maxEnergy = 20f;
        public float fuelPerHour = 1f;

        void Reset()
        {
            type = StationType.Generator;
            capacity = 1;
        }

        protected override void OnTick(float deltaSeconds, float eff)
        {
            float deltaHours = deltaSeconds / 3600f;

            float needFuel = fuelPerHour * deltaHours * eff;
            float haveFuel = bank.Get(ResourceType.Fuel);
            float usedFuel = Mathf.Min(haveFuel, needFuel);

            if (usedFuel > 0f) bank.Spend(ResourceType.Fuel, usedFuel);

            bool running = bank.Get(ResourceType.Fuel) > 0f;
            bank.Set(ResourceType.Energy, running ? maxEnergy : 0f);
        }
    }
}

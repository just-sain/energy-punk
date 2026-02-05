using UnityEngine;
using EnergyPunk.Resources;

namespace EnergyPunk.Buildings
{
    public class GeneratorStation : StationBase
    {
        [Header("Generator")]
        public float energyPerSecondOut = 2f;   // сколько даёт энергии в сек при eff=1
        public float fuelPerSecond = 0.02f;     // сколько жрёт топлива в сек при eff=1
        public float maxEnergy = 50f;           // лимит накопления энергии в банке

        protected override void OnTick(float deltaSeconds, float eff)
        {
            // если нет топлива — генератор не работает
            float needFuel = fuelPerSecond * deltaSeconds * eff;
            if (bank.Get(ResourceType.Fuel) < needFuel) return;

            bank.Spend(ResourceType.Fuel, needFuel);

            // производим энергию и кладём в банк (с лимитом)
            float currentEnergy = bank.Get(ResourceType.Energy);
            if (currentEnergy >= maxEnergy) return;

            float produced = energyPerSecondOut * deltaSeconds * eff;
            float add = Mathf.Min(produced, maxEnergy - currentEnergy);

            bank.Add(ResourceType.Energy, add);
        }
    }
}

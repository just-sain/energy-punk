using UnityEngine;
using EnergyPunk.Resources;

namespace EnergyPunk.Buildings
{
    public class Canteen : Building
    {
        public EnergyPunk.Population.Population pop;
        public float foodPerPersonPerHour = 0.08f;

        protected override void OnTickPowered(float deltaMinutes)
        {
            if (pop == null || bank == null) return;

            float deltaHours = deltaMinutes / 60f;
            float needFood = pop.people * foodPerPersonPerHour * deltaHours;

            float have = bank.Get(ResourceType.Food);
            float used = Mathf.Min(have, needFood);
            if (used > 0f) bank.Spend(ResourceType.Food, used);
            Debug.Log($"[CANTEEN] Food: {bank.Get(ResourceType.Food)} People: {pop.people}");

        }
    }
}

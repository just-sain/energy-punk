using UnityEngine;

namespace EnergyPunk.Buildings
{
    public class KitchenStation : StationBase
    {
        void Reset()
        {
            type = StationType.Kitchen;
            capacity = 2;
        }

        protected override void OnTick(float deltaSeconds, float eff)
        {
            // MVP: кухня просто "активна"
            // Позже: снижает голод, даёт мораль, уменьшает риск болезней и т.п.
        }
    }
}

using UnityEngine;

namespace EnergyPunk.Buildings
{
    public class Workshop : Building
    {
        // MVP: пока просто потребляет энергию, без эффекта
        protected override void OnTickPowered(float deltaMinutes)
        {
            // Позже: крафт, ускорение ремонта, производство предметов
        }
    }
}

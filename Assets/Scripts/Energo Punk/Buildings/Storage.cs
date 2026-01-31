using UnityEngine;

namespace EnergyPunk.Buildings
{
    public class Storage : Building
    {
        // MVP: пока просто потребляет энергию, без эффекта
        protected override void OnTickPowered(float deltaMinutes)
        {
            // Позже: лимиты хранения, бонусы к луту, логистика
        }
    }
}

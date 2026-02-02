namespace EnergyPunk.Buildings
{
    public class WorkshopStation : StationBase
    {
        void Reset()
        {
            type = StationType.Workshop;
            capacity = 2;
        }

        protected override void OnTick(float deltaSeconds, float eff)
        {
            // MVP: верстак просто "активен"
            // Позже: ремонт/крафт/апгрейды
        }
    }
}

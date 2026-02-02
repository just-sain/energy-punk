namespace EnergyPunk.Buildings
{
    public class HousingStation : StationBase
    {
        void Reset()
        {
            type = StationType.Housing;
            capacity = 10;
        }

        protected override void OnTick(float deltaSeconds, float eff)
        {
            // MVP: позже мораль/комфорт/сон
        }
    }
}

namespace EnergyPunk.Buildings
{
    public class StorageStation : StationBase
    {
        void Reset()
        {
            type = StationType.Storage;
            capacity = 1;
        }

        protected override void OnTick(float deltaSeconds, float eff)
        {
            // MVP: позже лимиты хранения
        }
    }
}

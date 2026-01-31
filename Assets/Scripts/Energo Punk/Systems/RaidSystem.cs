using UnityEngine;
using EnergyPunk.Resources;

namespace EnergyPunk.Systems
{
    public class RaidSystem : MonoBehaviour
    {
        public ResourceBank bank;
        public ThreatSystem threat;

        public void DoRaidMvp()
        {
            if (bank == null || threat == null) return;

            float mat = Random.Range(15f, 30f);
            float fuel = Random.Range(8f, 16f);
            float food = Random.Range(5f, 12f);

            bank.Add(ResourceType.Materials, mat);
            bank.Add(ResourceType.Fuel, fuel);
            bank.Add(ResourceType.Food, food);

            float t = Random.Range(8f, 15f);
            threat.AddThreat(t);

            Debug.Log($"[RAID] +Mat {mat:0} +Fuel {fuel:0} +Food {food:0} Threat +{t:0}");
        }
    }
}

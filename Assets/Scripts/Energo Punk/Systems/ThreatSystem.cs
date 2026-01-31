using UnityEngine;
using EnergyPunk.Core;

namespace EnergyPunk.Systems
{
    public class ThreatSystem : MonoBehaviour, ITickable
    {
        [Range(0f, 100f)]
        public float threat = 0f;

        public float decayPerHour = 2f;

        [Header("Zombie Raid")]
        public Fence fence;
        public float zombieRaidBaseDamage = 35f;

        void OnEnable()
        {
            var time = FindObjectOfType<TimeSystem>();
            if (time != null) time.Register(this);
        }

        void OnDisable()
        {
            var time = FindObjectOfType<TimeSystem>();
            if (time != null) time.Unregister(this);
        }

        public void AddThreat(float value)
        {
            threat = Mathf.Clamp(threat + value, 0f, 100f);
        }

        public void Tick(float deltaMinutes)
        {
            float deltaHours = deltaMinutes / 60f;

            threat = Mathf.Clamp(threat - decayPerHour * deltaHours, 0f, 100f);

            if (threat >= 100f)
            {
                DoZombieRaid();
                threat = Random.Range(40f, 60f);
            }
        }

        void DoZombieRaid()
        {
            if (fence == null) return;

            float dmg = zombieRaidBaseDamage * (1f - fence.damageReduction);
            fence.TakeDamage(dmg);

            if (fence.IsBroken)
                Debug.Log("[BREACH] Fence broken (MVP consequences later).");
        }
    }
}

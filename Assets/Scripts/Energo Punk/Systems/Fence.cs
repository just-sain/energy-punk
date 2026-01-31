using UnityEngine;

namespace EnergyPunk.Systems
{
    public class Fence : MonoBehaviour
    {
        public float maxHp = 100f;
        public float hp = 100f;

        [Range(0f, 0.9f)]
        public float damageReduction = 0.10f;

        public void TakeDamage(float dmg)
        {
            hp = Mathf.Max(0f, hp - dmg);
            Debug.Log($"[FENCE] -{dmg:0} => {hp:0}/{maxHp:0}");
        }

        public bool IsBroken => hp <= 0f;
    }
}

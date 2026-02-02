using UnityEngine;

namespace EnergyPunk.Population
{
    public class WorkerPool : MonoBehaviour
    {
        public Population population;

        [SerializeField] private int assigned;

        public int Total => population != null ? population.people : 0;
        public int Assigned => assigned;
        public int Free => Mathf.Max(0, Total - assigned);

        public bool TryAssign(int count)
        {
            if (count <= 0) return true;
            if (Free < count) return false;
            assigned += count;
            return true;
        }

        public void Release(int count)
        {
            if (count <= 0) return;
            assigned = Mathf.Max(0, assigned - count);
        }
    }
}

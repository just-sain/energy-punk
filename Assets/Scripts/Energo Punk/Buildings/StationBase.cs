using UnityEngine;
using EnergyPunk.Core;
using EnergyPunk.Resources;
using EnergyPunk.Population;

namespace EnergyPunk.Buildings
{
    public abstract class StationBase : MonoBehaviour, ITickable
    {
        [Header("Refs")]
        public ResourceBank bank;
        public WorkerPool workerPool;

        [Header("Station")]
        public StationType type;
        public int capacity = 1;

        [Header("Power")]
        [Tooltip("Сколько энергии потребляет станция в секунду при 100% эффективности")]
        public float energyPerSecond = 0.02f;

        [SerializeField] protected int workersAssigned;

        public int WorkersAssigned => workersAssigned;

        public float Efficiency
        {
            get
            {
                if (capacity <= 0) return 0f;
                return Mathf.Clamp01(workersAssigned / (float)capacity);
            }
        }

        public bool SetWorkers(int desired)
        {
            if (workerPool == null) return false;

            desired = Mathf.Clamp(desired, 0, capacity);

            int delta = desired - workersAssigned;
            if (delta == 0) return true;

            if (delta > 0)
            {
                if (!workerPool.TryAssign(delta)) return false;
                workersAssigned = desired;
                return true;
            }
            else
            {
                workerPool.Release(-delta);
                workersAssigned = desired;
                return true;
            }
        }

        public void Tick(float deltaSeconds)
        {
            if (bank == null || workerPool == null) return;
            if (workersAssigned <= 0) return;

            float eff = Efficiency;

            float needEnergy = energyPerSecond * deltaSeconds * eff;
            if (bank.Get(ResourceType.Energy) < needEnergy) return;

            bank.Spend(ResourceType.Energy, needEnergy);
            OnTick(deltaSeconds, eff);
        }

        protected abstract void OnTick(float deltaSeconds, float eff);

        public void UnassignAll()
        {
            if (workerPool != null && workersAssigned > 0)
                workerPool.Release(workersAssigned);

            workersAssigned = 0;
        }

        void OnDestroy()
        {
            UnassignAll();
        }
    }
}

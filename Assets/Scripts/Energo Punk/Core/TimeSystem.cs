using System.Collections.Generic;
using UnityEngine;

namespace EnergyPunk.Core
{
    public class TimeSystem : MonoBehaviour
    {
        public float gameMinutesPerRealSecond = 10f;
        public float stepMinutes = 10f;

        private float _accumulatedMinutes;
        private readonly List<ITickable> _tickables = new();

        public void Register(ITickable t)
        {
            if (t != null && !_tickables.Contains(t))
                _tickables.Add(t);
        }

        public void Unregister(ITickable t)
        {
            _tickables.Remove(t);
        }

        void Update()
        {
            _accumulatedMinutes += gameMinutesPerRealSecond * Time.deltaTime;

            while (_accumulatedMinutes >= stepMinutes)
            {
                for (int i = 0; i < _tickables.Count; i++)
                    _tickables[i].Tick(stepMinutes);

                _accumulatedMinutes -= stepMinutes;
            }
            Debug.Log("TimeSystem ticking");

        }
    }
}

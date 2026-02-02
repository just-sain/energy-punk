using System.Collections.Generic;
using UnityEngine;

namespace EnergyPunk.Core
{
    public class TimeSystem : MonoBehaviour
    {
        public float stepSeconds = 1f;

        private float _accum;
        private readonly List<ITickable> _tickables = new();

        public void Register(ITickable t)
        {
            if (t != null && !_tickables.Contains(t))
                _tickables.Add(t);
        }

        public void Unregister(ITickable t) => _tickables.Remove(t);

        void Update()
        {
            _accum += Time.deltaTime;
            while (_accum >= stepSeconds)
            {
                for (int i = 0; i < _tickables.Count; i++)
                    _tickables[i].Tick(stepSeconds);

                _accum -= stepSeconds;
            }
        }
    }
}

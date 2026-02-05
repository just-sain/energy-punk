using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnergyPunk.Resources
{
    public class ResourceBank : MonoBehaviour
    {
        [Serializable]
        public class Entry
        {
            public ResourceType type;
            public float amount;
        }

        [Header("Start Values")]
        public List<Entry> start = new();

        private readonly Dictionary<ResourceType, float> _res = new();

        /// <summary>
        /// Событие: (тип ресурса, новое значение)
        /// </summary>
        public event Action<ResourceType, float> OnChanged;

        void Awake()
        {
            _res.Clear();
            foreach (var e in start)
                _res[e.type] = e.amount;
        }

        public float Get(ResourceType t) => _res.TryGetValue(t, out var v) ? v : 0f;

        public void Set(ResourceType t, float amount)
        {
            float v = Mathf.Max(0f, amount);
            _res[t] = v;
            OnChanged?.Invoke(t, v);
        }

        //public void Add(ResourceType t, float amount)
        //{
        //    if (amount <= 0f) return;
        //    float v = Get(t) + amount;
        //    _res[t] = v;
        //    OnChanged?.Invoke(t, v);
        //}

        public void Add(ResourceType type, float amount)
        {
            if (amount <= 0f) return;
            Set(type, Get(type) + amount);
        }


        public bool Spend(ResourceType t, float amount)
        {
            if (amount <= 0f) return true;
            float cur = Get(t);
            if (cur < amount) return false;

            float v = cur - amount;
            _res[t] = v;
            OnChanged?.Invoke(t, v);
            return true;
        }
    }
}

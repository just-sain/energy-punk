using UnityEngine;

namespace EnergyPunk.Systems
{
    public class RaidDebugHotkey : MonoBehaviour
    {
        public RaidSystem raid;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                raid?.DoRaidMvp();
        }
    }
}

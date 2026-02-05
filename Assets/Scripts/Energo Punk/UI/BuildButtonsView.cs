using UnityEngine;
using UnityEngine.UI;
using EnergyPunk.Systems;
using EnergyPunk.Buildings;

namespace EnergyPunk.UI
{
    public class BuildButtonsView : MonoBehaviour
    {
        [Header("Refs")]
        public PlacementController placement;

        [Header("Buttons")]
        public Button btnGenerator;
        public Button btnKitchen;
        public Button btnWorkshop;
        public Button btnStorage;
        public Button btnHousing;

        [Header("Colors")]
        public Color normal = Color.white;
        public Color selected = new Color(0.8f, 0.9f, 1f, 1f);

        void Update()
        {
            if (placement == null) return;

            Set(btnGenerator, placement.SelectedType == StationType.Generator);
            Set(btnKitchen, placement.SelectedType == StationType.Kitchen);
            Set(btnWorkshop, placement.SelectedType == StationType.Workshop);
            Set(btnStorage, placement.SelectedType == StationType.Storage);
            Set(btnHousing, placement.SelectedType == StationType.Housing);
        }

        void Set(Button b, bool isSelected)
        {
            if (b == null) return;
            var img = b.targetGraphic as Image;
            if (img == null) return;
            img.color = isSelected ? selected : normal;
        }
    }
}

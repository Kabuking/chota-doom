using System;
using Modules.Player.Scripts.InputSystem;
using UnityEngine;

namespace Modules.Player.Scripts.Testing
{
    public class TestingRectTransform: MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform1;
        [SerializeField] private RectTransform rectTransform2;

        [SerializeField] private PlayerInputMapping _playerInputMapping;

        private void OnEnable()
        {
            _playerInputMapping.ItemSwitchLeftRightEventPerformed += PerformSwitch;
        }

        private void OnDisable()
        {
            _playerInputMapping.ItemSwitchLeftRightEventPerformed -= PerformSwitch;
        }

        void PerformSwitch(int d)
        {
            if (d < 0)
            {
                rectTransform1.sizeDelta = new Vector2(55, 30);
                rectTransform2.sizeDelta = new Vector2(55, 0);
                
            }else if (d > 0)
            {
                rectTransform1.sizeDelta = new Vector2(55, 0);
                rectTransform2.sizeDelta = new Vector2(55, 30);
            }
        }
    }
}
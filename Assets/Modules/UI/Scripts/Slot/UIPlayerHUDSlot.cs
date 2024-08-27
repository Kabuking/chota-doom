using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Player.Scripts.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.UI.Scripts.Slot
{
    public class UIPlayerHUDSlot: MonoBehaviour
    {
        [SerializeField] private Transform parentBars;
        [SerializeField] private Transform[] allHealthBars;
        [SerializeField] private Transform[] allHealthBarsReversed;

        private PlayerDamageSystem _playerDamageSystem;
        private void Awake()
        {
            allHealthBars = parentBars.GetComponentsInChildren<Transform>();
            allHealthBarsReversed = allHealthBars.Reverse().ToArray();
        }

        public void Init(PlayerInput playerTransform)
        {
            _playerDamageSystem = playerTransform.GetComponent<PlayerDamageSystem>();
            _playerDamageSystem.OnHealthNumberUpdate += OnHealthUpdateEvent;
        }

        private void OnDisable()
        {
            _playerDamageSystem.OnHealthNumberUpdate -= OnHealthUpdateEvent;
        }

        void EnableBar(int index)
        {
            allHealthBars[index].gameObject.SetActive(true);
        }
        
        void DisableBar(int index)
        {
            allHealthBars[index].gameObject.SetActive(false);
        }

        void OnHealthUpdateEvent(int number)
        {
            /*for (int i = 0; i < allHealthBarsReversed.Length; i++)
            {
                DisableBar(i);
            }

            for (int i = 0; i < number; i++)
            {
                if (allHealthBars[i] != null)
                {
                    EnableBar(i);
                }
            }*/
            
            int barsToEnable = Mathf.CeilToInt(number / 10.0f); // Divide by 10 to get the corresponding bar index
            for (int i = 0; i < allHealthBars.Length; i++)
            {
                allHealthBars[i].gameObject.SetActive(i < barsToEnable);
            }
        }
    }
}
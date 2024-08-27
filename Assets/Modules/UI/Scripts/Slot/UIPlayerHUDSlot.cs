using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Enemy;
using Modules.Player.Scripts.Components;
using Modules.Player.Scripts.Components.TargetAssist;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.UI.Scripts.Slot
{
    public class UIPlayerHUDSlot: MonoBehaviour
    {
        [SerializeField] private Transform crosshairImageParent;
        [SerializeField] private Transform parentBars;
        [SerializeField] private Transform[] allHealthBars;
        [SerializeField] private Transform[] allHealthBarsReversed;

        private PlayerDamageSystem _playerDamageSystem;
        
        [SerializeField] private ManualSwitchAtEnemyInRange _enemyBase;
        private Camera _camera;

        [SerializeField] private bool initiated = false;
        private void Awake()
        {
            allHealthBars = parentBars.GetComponentsInChildren<Transform>();
            allHealthBarsReversed = allHealthBars.Reverse().ToArray();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!initiated)
                return;
            UpdateCrossHair(_enemyBase.GetCurrentEnemyTarget);
        }

        public void Init(PlayerInput playerInput)
        {
            _playerDamageSystem = playerInput.GetComponent<PlayerDamageSystem>();
            _playerDamageSystem.OnHealthNumberUpdate += OnHealthUpdateEvent;
            
            _enemyBase = playerInput.GetComponentInChildren<ManualSwitchAtEnemyInRange>();

            initiated = true;
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


        void UpdateCrossHair(EnemyBase targetEnemy)
        {
            if (targetEnemy != null)
            {
                Vector3 screenPos = _camera.WorldToScreenPoint(targetEnemy.transform.position);

                if (screenPos.z > 0) // Ensure enemy is in front of the camera
                {
                    crosshairImageParent.transform.position = screenPos;
                    crosshairImageParent.gameObject.SetActive(true);
                }
                else
                {
                    crosshairImageParent.gameObject.SetActive(false); // Hide crosshair if enemy is behind the camera
                }
            }
            else
            {
                crosshairImageParent.gameObject.SetActive(false); // Hide crosshair if no enemy is found
            }
        }
    }
}
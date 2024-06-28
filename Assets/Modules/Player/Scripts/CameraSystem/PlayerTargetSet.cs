using Cinemachine;
using Modules.CommonEventBus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Player.Scripts.CameraSystem
{
    public class PlayerTargetSet: MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private PlayerCoopEventBus playerCoopEventBus;
        
        [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;

        //TODO: Manage leave action
        private void OnEnable()
        {
            playerCoopEventBus.PlayerJoinedGameEvent += SetCameraTarget;
        }

        private void OnDisable()
        {
            playerCoopEventBus.PlayerJoinedGameEvent -= SetCameraTarget;
        }

        void SetCameraTarget(PlayerInput playerInput)
        {
            if (playerInput.playerIndex % 2 == 0)
                _cinemachineTargetGroup.m_Targets[0].target = playerInput.transform;
            else
                _cinemachineTargetGroup.m_Targets[1].target = playerInput.transform;
        }
    }
}
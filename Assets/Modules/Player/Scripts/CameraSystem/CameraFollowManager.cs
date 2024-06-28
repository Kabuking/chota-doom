using Cinemachine;
using UnityEngine;

namespace Modules.Player.Scripts.CameraSystem
{
    public class CameraFollowManager : MonoBehaviour
    {
        [SerializeField] private float minDistanceMaintainAwayFromCam = 10;
        
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;
        private Cinemachine3rdPersonFollow componentBase;

        private void Awake()
        {
            // _cinemachineVirtualCamera.
            componentBase = _cinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as Cinemachine3rdPersonFollow;

        }

        private void Update()
        {
            
        }

        void DistanceMaintainZ()
        {
            // componentBase.CameraDistance
        }

        void CalculateNearestPlayer()
        {
            // Transform nearestTarget = ;
            // f/
            // _cinemachineTargetGroup.get
        }
    }
}
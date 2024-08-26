using System.Collections.Generic;
using Cinemachine;
using Modules.CommonEventBus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Player.Scripts.CameraSystem
{
    public class TwoPlayerFollowObject: MonoBehaviour
    {
        [SerializeField] private float lerpSpeed = 4;
        [SerializeField] private float yFactor = 1.5f;
        [SerializeField] private float maxHeight = 20f;
        [SerializeField] private float minHeight = 0.5f;
        
        [SerializeField] private float zoomInzoomOutSpeed = 2;
        [SerializeField] private float zoomInzoomOutMinDbp = 5;
        [SerializeField] private float zoomInzoomOutMaxDbp = 10;
        [SerializeField] private float minStayAwayFromNearestPlayer = 2;

        [Space(3)]
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private List<Transform> targetGroups;
        private Transform followObjectTransform;
        
        [SerializeField] private PlayerCoopEventBus playerCoopEventBus;

        private void Awake()
        {
            followObjectTransform = transform;

            _nearestPlayerToCamTransform = targetGroups[0];
            _nearestPlayerCamDistance = Vector3.Distance(_nearestPlayerToCamTransform.position , followObjectTransform.position);

        }

        private void OnEnable()
        {
            playerCoopEventBus.PlayerJoinedGameEvent += SetCameraTarget;
            // playerCoopEventBus.PlayerLeftGameEvent -= SetCameraTarget;

        }

        private void OnDisable()
        {
            playerCoopEventBus.PlayerJoinedGameEvent -= SetCameraTarget;
        }

        void SetCameraTarget(PlayerInput playerInput)
        {
            if (playerInput.playerIndex % 2 == 0)
                targetGroups[0] = playerInput.transform;
            else
                targetGroups[1] = playerInput.transform;

            //TODO: Check for edge cases
            _nearestPlayerToCamTransform = playerInput.transform;
        }
        
        private void Update()
        {

            CacheDistanceBetweenTwoPlayers();
            CacheNearestPlayerFromCamera();
            CamMaintainPosition();
        }

        //CACHE
        [Header("DEBUG")]
        // [SerializeField] private float _lastFrame_DistanceBetweenTwoPlayers;
        
        //Distance between two players
        [SerializeField] private float _currentFrame_distanceBetweenTwoPlayers;
        // [SerializeField] private float _distanceBetweenPlayerDelta;
        
        //Nearest Player Transform to Camera
        [SerializeField] private Transform _nearestPlayerToCamTransform;
        
        //Nearest Player Distance float to Camera
        [SerializeField] private float _nearestPlayerCamDistance;
        
        [SerializeField] private float _nearestPlayerOutOfFrameDelta;
        [SerializeField] private float _extraZoomOutOffset = 0.5f;
        void CacheNearestPlayerFromCamera()
        {
            //Find nearest object from camera
            foreach (var targetGroup in targetGroups)
            {
                float distanceFromPrev = Mathf.Abs(_nearestPlayerToCamTransform.position.z - followObjectTransform.position.z);
                float distanceFromCurrent = Mathf.Abs(targetGroup.position.z - followObjectTransform.position.z);
                
                if (distanceFromCurrent < distanceFromPrev)
                {
                    _nearestPlayerToCamTransform = targetGroup;
                }
            }
            
            _nearestPlayerCamDistance = Mathf.Abs(_nearestPlayerToCamTransform.position.z - followObjectTransform.position.z);
            _nearestPlayerOutOfFrameDelta = zoomInzoomOutMinDbp - Mathf.Abs(targetGroups[0].position.x - targetGroups[1].position.x);
        }

        void CacheDistanceBetweenTwoPlayers()
        {
            _currentFrame_distanceBetweenTwoPlayers = Vector2.Distance(targetGroups[0].position, targetGroups[1].position);
        }
        
        [SerializeField] float newZPosition = 0;
        [SerializeField] float lastFrame_NewZPosition = 0;

        // [SerializeField] private float heightAdapt;
        [SerializeField] private float yMidpoint;
        void CamMaintainPosition()
        {
            newZPosition = 0;
            newZPosition =  (_extraZoomOutOffset + _nearestPlayerOutOfFrameDelta * /*Time.deltaTime * */zoomInzoomOutSpeed);
            if (_currentFrame_distanceBetweenTwoPlayers > zoomInzoomOutMinDbp &&
                _currentFrame_distanceBetweenTwoPlayers < zoomInzoomOutMaxDbp)
            {
                lastFrame_NewZPosition = newZPosition;
            }
            float xMidpoint = (targetGroups[0].position.x + targetGroups[1].position.x) * 0.5f;

            
            yMidpoint = (targetGroups[0].position.y + targetGroups[1].position.y)  * yFactor * _currentFrame_distanceBetweenTwoPlayers;
            
            if (yMidpoint >= maxHeight)
            {
                yMidpoint = maxHeight;
            }
            
            if (yMidpoint <= minHeight)
            {
                yMidpoint = minHeight;
            }
            
            var newPosition = new Vector3(xMidpoint, yMidpoint, _nearestPlayerToCamTransform.position.z - minStayAwayFromNearestPlayer + lastFrame_NewZPosition);
            followObjectTransform.position = Vector3.MoveTowards(followObjectTransform.transform.position, newPosition,
                lerpSpeed * Time.deltaTime);
            // followObjectTransform.position = 
        }
    }
}

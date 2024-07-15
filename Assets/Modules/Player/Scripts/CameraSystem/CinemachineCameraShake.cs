using System;
using Cinemachine;
using UnityEngine;

namespace Modules.Player.Scripts.CameraSystem
{
    public class CinemachineCameraShake: MonoBehaviour
    {
        public static CinemachineCameraShake Instance { get; private set; }
        
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

        private float shakeTimer = 0;
        private float shakeTimerTotal = 0;
        private float startingIntensity = 0;
        private void Awake()
        {
            Instance = this;
            
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cinemachineBasicMultiChannelPerlin =
                _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
        }

        public void ShakeCamera(float shakeIntensity, float time)
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeIntensity;
            shakeTimerTotal = time;
            shakeTimer = time;
            startingIntensity = shakeIntensity;
        }

        private void Update()
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera
                    .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, shakeTimer / shakeTimerTotal);
            }
        }
    }
}
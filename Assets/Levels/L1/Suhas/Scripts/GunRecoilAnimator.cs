using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoilAnimator : MonoBehaviour
{
   [SerializeField] private float positionMagnitude_y = 0.05f; // Magnitude of the positional shake
   [SerializeField] private float rotationMagnitude_y = 1.0f; // Magnitude of the rotational shake

   [SerializeField] private float positionMagnitude_z = 0.05f; // Magnitude of the positional shake
   [SerializeField] private float rotationMagnitude_z = 1.0f; // Magnitude of the rotational shake

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isShaking = true;

    public Transform parentGun;
    private void Start()
    {
        originalPosition = parentGun.localPosition;
        originalRotation = parentGun.localRotation;
    }

    private void Update()
    {
        if (isShaking)
        {
            Shake();
        }
    }

    public void StartShake()
    {
        isShaking = true;
    }

    public void StopShake()
    {
        isShaking = false;
        // Reset to original position and rotation
        parentGun.localPosition = originalPosition;
        parentGun.localRotation = originalRotation;
    }

    private void Shake()
    {
        // Randomly generate position offset
        //float x = Random.Range(-1f, 1f) * positionMagnitude;
        float y = Random.Range(-1f, 1f) * positionMagnitude_y;
        float z = Random.Range(-1f, 1f) * positionMagnitude_z;

        // Randomly generate rotation offset
        //float rotX = Random.Range(-1f, 1f) * rotationMagnitude;
        float rotY = Random.Range(-1f, 1f) * rotationMagnitude_y;
        float rotZ = Random.Range(-1f, 1f) * rotationMagnitude_z;

        // Apply position and rotation offsets
        parentGun.localPosition = new Vector3(0, y, z) + originalPosition;
        parentGun.localRotation = Quaternion.Euler(new Vector3(0, rotY, rotZ)) * originalRotation;
    }


}

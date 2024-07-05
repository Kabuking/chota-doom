using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = playerTransform.position;
    }
}

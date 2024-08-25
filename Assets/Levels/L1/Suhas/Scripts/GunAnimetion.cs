using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimetion : MonoBehaviour
{

    [SerializeField] private float rotation_speed = 45.0f;

    private Vector3 rotation_axis = Vector3.up;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation_axis, rotation_speed* Time.deltaTime );
    }
}

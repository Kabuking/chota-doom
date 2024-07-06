using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathingBase : MonoBehaviour
{
    public Camera mainCamera;
    public float wanderRadius = 10f; // Radius around the camera within which the object can wander
    public float cameraAvoidRange;
    public float moveSpeed = 2f; // Speed at which the object moves

    [Header("Serialized for debugging")]
    [SerializeField] float initialHeight;
    [SerializeField] private Vector3 nextLocation;

    private void Awake()
    {
        initialHeight = transform.position.y;
    }
    void Start()
    {
        SetNewRandomTarget();
    }

    void Update()
    {
        MoveTowardsTarget();

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(nextLocation);
        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            SetNewRandomTarget();
        }

        if (Vector3.Distance(transform.position, mainCamera.transform.position) < cameraAvoidRange)
        {
            SetNewRandomTarget();
        }

        if (Vector3.Distance(transform.position, nextLocation) < 0.1f)
        {
            SetNewRandomTarget();
        }
    }

    void SetNewRandomTarget()
    {
        Vector3 randomPoint = Random.insideUnitSphere * wanderRadius;
        randomPoint.y = initialHeight; 

        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;

        // Ensure the random point is in front of the camera
        randomPoint = cameraPosition + cameraForward * wanderRadius + randomPoint;

        // Ensure the point is within the camera's view frustum
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(randomPoint);
        while (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            randomPoint = Random.insideUnitSphere * wanderRadius;
            randomPoint.y = initialHeight;
            randomPoint = cameraPosition + cameraForward * wanderRadius + randomPoint;
            viewportPoint = mainCamera.WorldToViewportPoint(randomPoint);
        }

        randomPoint.y = initialHeight;
        nextLocation = randomPoint;
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextLocation, moveSpeed * Time.deltaTime);
    }
}

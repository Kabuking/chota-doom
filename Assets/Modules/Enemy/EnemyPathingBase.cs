using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyPathingBase : MonoBehaviour
{
    public Camera mainCamera;
    public float wanderRadius = 10f; // Radius around the camera within which the object can wander
    public float cameraAvoidRange;
    public float moveSpeed = 2f; // Speed at which the object moves
    public SlowProjectile gunScript;
    public float playerAvoidRange;
    public float lookRotationSpeed;

    [Header("Serialized for debugging")]
    [SerializeField] float initialHeight;
    [SerializeField] private Vector3 nextLocation;
    [SerializeField] private Transform leftWall, rightWall;
    [SerializeField] private GameObject[] players;

    private void Awake()
    {
        initialHeight = transform.position.y;
        GetBounds();
        StartCoroutine(GetPlayers());
    }
    void Start()
    {
        SetNewRandomTarget();
    }

    void Update()
    {
        if (players.Length < 2) {
            return;
        }

        if (gunScript.targetTransform != null) {
            Quaternion targetRotation = Quaternion.LookRotation(gunScript.targetTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookRotationSpeed * Time.deltaTime);
            //transform.LookAt(gunScript.targetTransform.position);
        }

        MoveTowardsTarget();

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(nextLocation);

        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            SetNewRandomTarget();
        }

        if (Mathf.Abs(transform.position.x - leftWall.position.x) < 5 ) {
            SetNewRandomTarget();
        }

        if (Mathf.Abs(transform.position.x - rightWall.position.x) < 5 ) {
            SetNewRandomTarget();
        }

        /*foreach (var player in players) {
            if (Vector3.Distance(mainCamera.transform.position, player.transform.position) > cameraAvoidRange + playerAvoidRange) {
                
            }
        }*/

        foreach (var player in players) {
            if (Vector3.Distance(transform.position, player.transform.position) < playerAvoidRange) {
                SetNewRandomTarget();
            }
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

    void GetBounds() {
        RaycastHit hitLeft;
        if (Physics.Raycast(transform.position, -Vector3.right, out hitLeft, 1000)) {
            if (hitLeft.collider.CompareTag("Bounds")) {
                Debug.Log("Hit Bounds on the left: " + hitLeft.collider.name);
                leftWall = hitLeft.transform;
            }
        }

        // Raycast to the right
        RaycastHit hitRight;
        if (Physics.Raycast(transform.position, Vector3.right, out hitRight, 1000)) {
            if (hitRight.collider.CompareTag("Bounds")) {
                Debug.Log("Hit Bounds on the right: " + hitRight.collider.name);
                rightWall = hitRight.transform;
            }
        }
    }

    IEnumerator GetPlayers() {
        while (players.Length < 2) {
            players = GameObject.FindGameObjectsWithTag("Player");
            yield return new WaitForSeconds(1);
        }
    }

}

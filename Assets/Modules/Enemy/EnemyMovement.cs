using System.Collections;
using System.Collections.Generic;
using Modules.Enemy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform[] waypoints;
    [SerializeField] float waypointProximityToTriggerNextWaypoint;
    [SerializeField] float speed;
    [SerializeField] float pauseMin;
    [SerializeField] float pauseMax;
    [SerializeField] float resumeDuration;

    [Header("Serialized for debugging")]
    [SerializeField] int currentWaypointIndex;
    [SerializeField] bool pausing;
    [SerializeField] bool engaging;
    void Start()
    {
        pausing = false;
        engaging = false;

        StartCoroutine(RandomPauser());
    }

    // Update is called once per frame
    void Update()
    {
        if(pausing && engaging)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < waypointProximityToTriggerNextWaypoint) {
            currentWaypointIndex++;
            engaging = true;
        }
        if (currentWaypointIndex == waypoints.Length) {
            currentWaypointIndex = 0;
        }
    }

    IEnumerator RandomPauser() {
        while (true)
        {
            yield return new WaitForSeconds(resumeDuration);
            pausing = true;
            yield return new WaitForSeconds(Random.Range(pauseMin, pauseMax));
            pausing = false;
        }
    }


}

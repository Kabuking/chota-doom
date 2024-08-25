using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompRingBehaviour : MonoBehaviour
{
    public float scaleDuration = 3.0f;  // Duration to scale the object
    public Vector3 targetScale = new Vector3(2.0f, 1.0f, 2.0f);  // Final scale on X and Z axis
    private Vector3 initialScale;
    private float elapsedTime = 0f;

    void Start()
    {
        // Store the initial scale of the object
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the fraction of the scale duration that has passed
        float t = Mathf.Clamp01(elapsedTime / scaleDuration);

        // Lerp the scale between initial and target scale over time
        transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

        // If the object has reached the target scale, destroy it
        if (t >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}

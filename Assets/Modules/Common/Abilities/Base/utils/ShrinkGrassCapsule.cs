using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkGrassCapsule : MonoBehaviour
{
    [SerializeField] float shrinkTime;
    [SerializeField] float elapsedTime = 0;
    [SerializeField] float currentValue;
    [SerializeField] float default_X_scale;
    // Start is called before the first frame update
    void Start()
    {
        default_X_scale = transform.localScale.x;
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / shrinkTime;
        currentValue = Mathf.Lerp(default_X_scale, 0, t);

        transform.localScale = new Vector3(currentValue, transform.localScale.y, transform.localScale.z);
    }
}

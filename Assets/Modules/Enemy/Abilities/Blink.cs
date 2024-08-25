using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] EnemyPathingBase enemyPathingBase;
    // Start is called before the first frame update
    void Start()
    {
        enemyPathingBase = GetComponent<EnemyPathingBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

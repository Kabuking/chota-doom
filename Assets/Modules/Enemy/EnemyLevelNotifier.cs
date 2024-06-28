using System;
using System.Collections;
using Level.Scripts;
using Modules.Enemy;
using UnityEngine;

namespace Characters.Enemy
{
    public class EnemyLevelNotifier: MonoBehaviour
    {
        [SerializeField] private LevelEventBridge _levelEventBridge;

        private void OnEnable()
        {
            StartCoroutine(NotifyEnemyManger());
        }


        private void OnDestroy()
        {
            if(_levelEventBridge.OnEnemyDestroyed != null)
                _levelEventBridge.OnEnemyDestroyed.Invoke(GetComponent<EnemyBase>());
        }

        private void OnDisable()
        {
            //Call before Destroy
            //Generally when dead, call it and after some vfx delay destroy
        }

        IEnumerator NotifyEnemyManger()
        {
            yield return new WaitForSeconds(2f);
            _levelEventBridge.OnEnemyJoined.Invoke(GetComponent<EnemyBase>());

        }
    }
}
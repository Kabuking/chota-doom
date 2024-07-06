using System.Collections;
using Levels.L_Testing.Scripts;
using UnityEngine;

namespace Modules.Enemy
{
    public class EnemyLevelNotifier: MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(NotifyEnemyManger());
        }


        private void OnDestroy()
        {
            if(LevelEventBridge.OnEnemyDestroyed != null)
                LevelEventBridge.OnEnemyDestroyed.Invoke(GetComponent<EnemyBase>());
        }

        private void OnDisable()
        {
            //Call before Destroy
            //Generally when dead, call it and after some vfx delay destroy
        }

        IEnumerator NotifyEnemyManger()
        {
            yield return new WaitForSeconds(2f);
            LevelEventBridge.OnEnemyJoined.Invoke(GetComponent<EnemyBase>());

        }
    }
}
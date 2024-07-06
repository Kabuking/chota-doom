using System.Collections.Generic;
using Levels.L_Testing.Scripts;
using UnityEngine;

namespace Modules.Enemy
{
    public class EnemyManager: MonoBehaviour
    {
        public static EnemyManager instance;

        public List<EnemyBase> allEnemies = new List<EnemyBase>();

        public List<EnemyBase> GetAllEnemies() => allEnemies;
        
        [Header("Debug")]
        [SerializeField] private bool killAllEnemies = false;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            LevelEventBridge.OnEnemyJoined += OnEnemyJoinedBattle;
            LevelEventBridge.OnEnemyDestroyed += OnEnemyLeftBattle;
        }

        private void Update()
        {
            //Debugging only
            if (killAllEnemies)
                KillAllEnemies();

        }

        private void OnDisable()
        {
            LevelEventBridge.OnEnemyJoined -= OnEnemyJoinedBattle;
            LevelEventBridge.OnEnemyDestroyed -= OnEnemyLeftBattle;
        }

        void OnEnemyJoinedBattle(EnemyBase enemyJoined)
        {
            allEnemies.Add(enemyJoined);
            // DebugX.LogWithColorYellow("Enemy joined "+enemyJoined);
        }

        void OnEnemyLeftBattle(EnemyBase enemyBase)
        {
            allEnemies.Remove(enemyBase);
            if (allEnemies.Count == 0)
            {
                LevelEventBridge.NoEnemiesLeftInGame?.Invoke();
            }
        }
        
        public EnemyBase GetNearestEnemy(Vector3 fromPosition, float minDistance)
        {
            // DebugX.LogWithColorCyan("getting nearest enemy "+allEnemies.Count);
            EnemyBase currentTarget = null;
            foreach (EnemyBase enemy in allEnemies)
            {
                // DebugX.LogWithColorCyan(enemy.transform.name);
                float distance = Vector3.Distance(fromPosition, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    currentTarget = enemy;
                }
            }

            return currentTarget;
        }
        
        public void KillAllEnemies()
        {
            // Iterate backwards through the list to safely modify it
            for (int i = allEnemies.Count - 1; i >= 0; i--)
            {
                EnemyBase enemy = allEnemies[i];
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                    Debug.Log("Enemy destroyed: " + enemy.name);
                }
            }

            // Clear the enemies list after destroying all enemies
            allEnemies.Clear();
            Debug.Log("All enemies killed.");
        }
    }
}
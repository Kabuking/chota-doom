using System.Collections.Generic;
using Characters.Enemy;
using Modules.Enemy;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Scripts
{
    
    [CreateAssetMenu(fileName = "LevelEventBridge", menuName = "Gameplay/LevelEventBridge")]
    public class LevelEventBridge: ScriptableObject
    {
        public UnityAction<EnemyBase> OnEnemyJoined;
        public UnityAction<EnemyBase> OnEnemyDestroyed;

        public UnityAction NoEnemiesLeftInGame;
    }
}
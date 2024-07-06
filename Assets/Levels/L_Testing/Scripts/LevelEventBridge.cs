using Modules.Enemy;
using UnityEngine.Events;

namespace Levels.L_Testing.Scripts
{
    
    public static class LevelEventBridge
    {
        public static UnityAction<EnemyBase> OnEnemyJoined;
        public static UnityAction<EnemyBase> OnEnemyDestroyed;

        public static UnityAction NoEnemiesLeftInGame;
    }
}
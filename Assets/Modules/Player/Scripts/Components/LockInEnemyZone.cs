using UnityEngine;

namespace Modules.Player.Scripts.Components
{
    public class LockInEnemyZone: MonoBehaviour
    {
        [SerializeField] private Transform boss;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PCharacterMovement>().ApplyLookTowardEnemy();
            }
        }
    }
}
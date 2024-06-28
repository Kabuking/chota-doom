using UnityEngine;

namespace Modules.Player.Scripts.Testing
{
    public class LookTowardsM: MonoBehaviour
    {
        [SerializeField] private Transform targetT;

        private void Update()
        {
            transform.LookAt(targetT);
        }
    }
}

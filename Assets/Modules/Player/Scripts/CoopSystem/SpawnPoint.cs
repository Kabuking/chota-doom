using Modules.CommonEventBus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Player.Scripts.CoopSystem
{
    public class SpawnPoint: MonoBehaviour
    {
        [SerializeField] private PlayerCoopEventBus playerCoopEventBus;
        [SerializeField] private Transform spawnMesh;

        [Header("Debug")] 
        [SerializeField] private float disableMeshOnJoinThresholdDistance = 0.4f;
        private void OnEnable()
        {
            playerCoopEventBus.PlayerJoinGameSessionFinished += OnPlayerJoin;
        }

        private void OnDisable()
        {
            playerCoopEventBus.PlayerJoinGameSessionFinished -= OnPlayerJoin;
        }

        void OnPlayerJoin(PlayerInput playerInput)
        {
            if (Vector3.Distance(playerInput.transform.position, transform.position) < disableMeshOnJoinThresholdDistance)
            {
                spawnMesh.gameObject.SetActive(false);
            }
        }
    }
}
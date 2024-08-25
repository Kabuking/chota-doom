using System;
using System.Collections;
using Modules.Loadout.Scripts.Manager;
using UnityEngine;

namespace Modules.Loadout.Scripts.Guns
{
    public class WalkOverGun: MonoBehaviour
    {
        [SerializeField] private GameObject gunMesh;
        [SerializeField] private EnumAllItemType.ItemId itemId;

        [SerializeField] private float animationRotationSpeed = 3f;
        private void Awake()
        {
            StartCoroutine(GunAnimate());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<GameplayLoadoutOnPlayerV2>(out GameplayLoadoutOnPlayerV2 loadout))
                {
                    loadout.UpdateActiveItemCarrying(itemId);
                }
                else
                {
                }
            }
        }

        IEnumerator GunAnimate()
        {
            while (true)
            {
                //gunMesh
                gunMesh.transform.Rotate(0f, animationRotationSpeed, 0f); // Adjust the rotation values as needed
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}
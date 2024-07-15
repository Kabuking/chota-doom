using System.Collections;
using UnityEngine;

namespace Modules.Player.Scripts.Testing
{
    public class EBoxShooter: MonoBehaviour
    {
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float shootFrequency = 2;
        [SerializeField] private Transform spawnBulletPosition;
        [SerializeField] private BulletBase _bulletBase;
        [SerializeField] private float bulletLifetime = 4;


        private void Start()
        {
            StartCoroutine(ShootC());
        }

        IEnumerator ShootC()
        {
            yield return new WaitForSeconds(shootFrequency);
            Instantiate(_bulletBase).SetVelocity(bulletSpeed,spawnBulletPosition, bulletLifetime);
            StartCoroutine(ShootC());
        }
        
    }
}
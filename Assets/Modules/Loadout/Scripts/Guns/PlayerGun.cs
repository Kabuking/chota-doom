using System.Collections;
using Characters.Player.Global;
using Modules.Loadout.Scripts.Item;
using Modules.Loadout.Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Loadout.Scripts.Guns
{
    public class PlayerGun: ItemBase
    {
        [Header("Enemy effects")]
        public bool readyToFire = true;

        [Header("Stats")] 
        [SerializeField] private float bulletSpeed = 50;
        [SerializeField] private float bulletDamage = 5;
        [SerializeField] private float fireRate = 0.5f;

        [Header("Ammo")]
        [SerializeField] private int magSize = 6;
        [SerializeField] private float reloadTime = 0.5f;
        [SerializeField] int currentAmmoLeft=0;
        float lastShootTime;
        
        [Header("Rapid Fire")] 
        [SerializeField] private bool hasRapidFire = false;
        [SerializeField] private bool activateAutomaticFire = false;
        
        [Header("Projectile")]
        [SerializeField] BulletBase projectile;
        [SerializeField] private GameObject muzzleFlare; 

        [Header("Shells")] 
        [SerializeField] private bool HasShells = false;
        [SerializeField] private GameObject shellPrefab;
        [SerializeField] private int numberOfShells = 5;

        [Header("Shotgun Behaviour")]
        [SerializeField] private int shotgunBullets = 1;
        
        [Header("CameraShake")]
        [SerializeField] private bool hasCameraShake = false;

        [Header("Transform")]
        [SerializeField] private Transform BulletSpawnPoint;
        [SerializeField] private Transform MuzzleSpawnPoint;

        [Header("Sound")] 
        [SerializeField] private AudioSource _audioSourceGun;
        [SerializeField] private AudioClip _audioClipGunShot;

        private EnumAllItemType.GunState gunState = EnumAllItemType.GunState.Carrying;

        public GameObject GunAnimator;
        protected override void OnAwake()
        {
            base.OnAwake();
            SetAmmoFull();
            
            //Consider PlayerGun attached to Player
            _audioSourceGun = transform.root.GetComponent<AudioSource>();
        }

        void SetAmmoFull()
        {
            currentAmmoLeft = magSize;
        }

        public override void OnItemUse()
        {
            // DebugX.LogWithColorCyan("Item use gun");
            if (hasRapidFire)
            {
                //Rapid fire
                activateAutomaticFire = true;
            }
            else
            {
                //Single shot fire
                ApplyFire();
            }
        }

        public override void OnItemUseStop()
        {
            if (hasRapidFire)
            {
                //Rapid fire
                activateAutomaticFire = false;
                if(GunAnimator != null)
                    GunAnimator.SetActive(false);

            }
        }

        public override int GetItemAmount()
        {
            return currentAmmoLeft;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (activateAutomaticFire)
            {
                RapidFire();
                if (GunAnimator != null)
                    GunAnimator.SetActive(true);
            }
        }

        private float _lastShootTimeForAutomatic;
        void RapidFire()
        {
            if (Time.time > _lastShootTimeForAutomatic + fireRate)
            {
                _lastShootTimeForAutomatic = Time.time;
                ApplyFire();
            }
        }


        IEnumerator ReloadGun()
        {
            yield return new WaitForSeconds(reloadTime);
            SetAmmoFull();
            gunState = EnumAllItemType.GunState.Carrying; //Not used check for usages
            
            DebugX.LogWithColorYellow("Reload complete");

        }

        void ProcessReloadGun()
        {
            if (gunState != EnumAllItemType.GunState.Reloading)
            {
                gunState = EnumAllItemType.GunState.Reloading;
                StartCoroutine(ReloadGun());
            }
        }
        
        //Currently only implement single shot and rapid fire
        void ApplyFire()
        {
            if (!readyToFire) {
                return;
            } 
            
            if (currentAmmoLeft < 1)
            {
                //UnSuccessful Shooting
                DebugX.LogWithColorYellow("No ammo left, currently reloaidng");

                // ProcessReloadGun();
            }
            else
            {

                if(_audioSourceGun!=null)
                    _audioSourceGun.PlayOneShot(_audioClipGunShot);
                
                if (hasCameraShake && _cinemachineImpulseSource != null)
                {
                    _cinemachineImpulseSource.GenerateImpulse();
                }
                
                // DebugX.LogWithColorYellow("applying fire");
                gunState = EnumAllItemType.GunState.Firing;
                
                //Update AMMO
                currentAmmoLeft = currentAmmoLeft > 0 ? currentAmmoLeft - 1 : 0;

                
                
                if (currentAmmoLeft == 0)
                {
                    StartCoroutine(ReloadGun());
                }
                
                if (lastShootTime + fireRate < Time.time)
                {
                    //Shells
                    if (HasShells)
                    {
                        Instantiate(shellPrefab, MuzzleSpawnPoint.position, MuzzleSpawnPoint.rotation);
                    }
                    
                    //Recoil animation
                    if (muzzleFlare != null)
                    {
                        Instantiate(muzzleFlare, MuzzleSpawnPoint);
                    }
                    
                    
                    //Projectile
                    if (projectile != null)
                    {
                        BulletBase bulletSpawned = Instantiate(projectile, BulletSpawnPoint.position, BulletSpawnPoint.rotation);
                        bulletSpawned.SetVelocity(bulletSpeed, BulletSpawnPoint);
                        

                    }
                    
                    
                    if (currentAmmoLeft == 0)
                    {
                        ProcessReloadGun();
                    }

                    lastShootTime = Time.time;
                    /*else
                    {
                        //TODO Need to check
                        lastShootTime = Time.time;
                    }*/
                }
            }
        }
    }
}
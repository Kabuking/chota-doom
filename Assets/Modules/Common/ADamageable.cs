using UnityEngine;

namespace Modules.Common
{
    public class ADamageable: MonoBehaviour
    {
        public virtual void TakeBulletDamage(BulletBase iamBullet)
        {
            
        }
        
        public virtual void TakeLaserDamage(BulletBase.DamageType damageType, int damageValue)
        {
            
        }
    }
}
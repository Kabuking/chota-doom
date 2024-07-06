using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [Header("Bullet stats")]
    public float damage;

    [Header("Bullet type")]
    DamageType damageType;

    public enum DamageType { 
        Normal,
        Bleed,
        Shock
    }
}

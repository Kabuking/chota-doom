using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [Header("Bullet stats")]
    [SerializeField] float damage;

    [Header("Bullet type")]
    [SerializeField] DamageType damageType;

    public enum DamageType { 
        Normal,
        Bleed,
        Shock
    }
}

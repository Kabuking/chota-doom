using Modules.Loadout.Scripts.Manager;
using UnityEngine;

namespace Modules.Loadout.Scripts.Ammo
{
    public class AmmoBase: MonoBehaviour
    {
        [SerializeField] private EnumAllItemType.AmmoType _ammoType;
        [SerializeField] private int amount;
    }
}
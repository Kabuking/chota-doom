using UnityEngine;

namespace Modules.Loadout.Scripts.Item
{
    [CreateAssetMenu(fileName = "ItemDataSo", menuName = "GameplayData/ItemDataSo")]
    public class ItemDataSo: ScriptableObject
    {
        public string itemDisplayName;
        public Sprite itemDisplayIcon;
        public int itemAmount;
    }
}
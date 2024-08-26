using System;
using System.Collections.Generic;
using Modules.Loadout.Scripts.Guns;
using Modules.Loadout.Scripts.Item;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Modules.Loadout.Scripts.Manager
{
    public class GameplayLoadoutOnPlayerV2: GameplayLoadout
    {
        [SerializeField] private List<PlayerGun> allPossibleGuns;
        
        [SerializeField]private ItemBase currentlyCarrying;
        

        public void UpdateActiveItemCarrying(EnumAllItemType.ItemId itemId)
        {
            PlayerGun gun = allPossibleGuns.Find(g => g.GetItemId == itemId);
            if (gun != null )
            {
                if (currentlyCarrying != null)
                {
                    if (currentlyCarrying.GetItemId == itemId)
                        return;
                    
                    currentlyCarrying.OnItemUseStop();
                    currentlyCarrying.gameObject.SetActive(false);
                }
                currentlyCarrying = gun;
                currentlyCarrying.gameObject.SetActive(true);
            }
        }

        public override void OnItemUse()
        {
            if (currentlyCarrying != null)
            {
                currentlyCarrying.OnItemUse();
            }
            else
            {
                //Item is null
            }        }

        public override void OnItemUseStop()
        {
            if (currentlyCarrying != null)
            {
                currentlyCarrying.OnItemUseStop();
            }
        }

        public override void ReceiveItemSwitchLeftRight()
        {
        }

        public override Transform Get_ItemSocketTransform()
        {
            throw new System.NotImplementedException();
        }

        public override PlayerInput Get_OwnerPlayerInput()
        {
            throw new System.NotImplementedException();
        }

        public override List<ItemBase> GetStarterItemList()
        {
            throw new System.NotImplementedException();
        }
    }
}

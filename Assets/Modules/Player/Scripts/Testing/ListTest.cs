using System;
using System.Collections.Generic;
using Characters.Player.Global;
using UnityEngine;

namespace Modules.Player.Scripts.Testing
{
    public class ListTest: MonoBehaviour
    {
        [SerializeField] private List<int> _itemsNear;
        
        private void Awake()
        { 
            _itemsNear= new List<int>();
             
             _itemsNear.Add(1);
             _itemsNear.Add(2);
             DebugX.LogWithColorRed(" --> " +_itemsNear.Count.ToString());
             _itemsNear.Remove(1);
             _itemsNear.Remove(2);
             _itemsNear.Remove(2);
             DebugX.LogWithColorRed(" -->> " +_itemsNear.Count.ToString());

        }
    }
}
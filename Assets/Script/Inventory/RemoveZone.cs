using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.Inventory
{
    public class RemoveZone : MonoBehaviour, IDropHandler
    {
        private IRemoveFiledContract _contract;

        public void InitContract(IRemoveFiledContract contract)
        {
            _contract = contract;
        }

        public void OnDrop(PointerEventData eventData)
        {
            _contract?.OnDropRemoveItem(eventData.pointerDrag.GetComponent<InventorySlot>());
        }
    }
}

using System;
using UnityEngine;

namespace Script.Inventory
{
    public class SlotMaker : MonoBehaviour
    {
        //샘플용 슬롯을 인벤토리에 만든다
        [SerializeField]
        private GameObject _slotPrefab;

        [SerializeField]
        private Transform _slotRoot;

        public InventorySlot MakeSlot(int positionX, int positionY, int sizeX, int sizeY)
        {
            int width = 100;
            int height = 100;

            if (sizeX > 1)
            {
                width += (sizeX - 1) * 114;
            }

            if (sizeY > 1)
            {
                height += (sizeY - 1) * 114;
            }

            var slotObject = Instantiate(_slotPrefab, _slotRoot);
            var slot = slotObject.GetComponent<InventorySlot>();
            slot.gameObject.SetActive(true);
            slot.Initialize(positionX, positionY, sizeX, sizeY, width, height);

            return slot;
        }
    }
}

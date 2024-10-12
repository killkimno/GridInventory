using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.SampleScript.Inventory
{
    public class Inventory : MonoBehaviour, IDropFieldContract
    {
        [SerializeField]
        private DropField _dropField;

        private const int InventorySizeX = 10;
        private const int InventorySizeY = 6;
        private bool[,] _inventoryMatrix = new bool[InventorySizeY, InventorySizeX]; //y,x


        private void Awake()
        {
            _dropField.InitContract(this);
        }

        private void Initialize()
        {
            throw new NotImplementedException();
        }

        public void SetItemPosition(InventorySlot slot, int x, int y)
        {
            if (x < 0 || x + slot.SizeX > InventorySizeX || y < 0 || y + slot.SizeY > InventorySizeY)
            {
                Debug.Log("Out of bounds");
                _dropField.SetPosition(slot, slot.PositionX, slot.PositionY);
                return;
            }

            //원래 위치는 잠시 비워둔다
            for (int indexY = slot.PositionY; indexY < slot.PositionY + slot.SizeY; indexY++)
            {
                for (int indexX = slot.PositionX; indexX < slot.PositionX + slot.SizeX; indexX++)
                {
                    _inventoryMatrix[indexY, indexX] = false;
                }
            }


            bool available = true;
            for (int indexY = y; indexY < y + slot.SizeY; indexY++)
            {
                for (int indexX = x; indexX < x + slot.SizeX; indexX++)
                {
                    if (_inventoryMatrix[indexY, indexX])
                    {
                        available = false;
                        break;
                    }
                }
            }

            if (!available)
            {
                //원래 위치로 롤백한다
                for (int indexY = slot.PositionY; indexY < slot.PositionY + slot.SizeY; indexY++)
                {
                    for (int indexX = slot.PositionX; indexX < slot.PositionX + slot.SizeX; indexX++)
                    {
                        _inventoryMatrix[indexY, indexX] = true;
                    }
                }

                _dropField.SetPosition(slot, slot.PositionX, slot.PositionY);
                return;
            }

            //문제가 없다

            for (int indexY = y; indexY < y + slot.SizeY; indexY++)
            {
                for (int indexX = x; indexX < x + slot.SizeX; indexX++)
                {
                    _inventoryMatrix[indexY, indexX] = true;
                }
            }

            slot.SetPosition(x, y);
            _dropField.SetPosition(slot, x, y);
        }
    }
}

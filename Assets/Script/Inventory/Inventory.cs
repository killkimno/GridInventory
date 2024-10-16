using System;
using UnityEngine;

namespace Script.Inventory
{
    public class Inventory : MonoBehaviour, IDropFieldContract, IRemoveFiledContract, IInventorySlotContract
    {
        [SerializeField]
        private SlotMaker _slotMaker;

        [SerializeField]
        private DropField _dropField;

        [SerializeField]
        private RemoveZone _removeZone;

        private const int InventorySizeX = 10;
        private const int InventorySizeY = 6;
        private bool[,] _inventoryMatrix = new bool[InventorySizeY, InventorySizeX]; //y,x


        [SerializeField]
        private InventorySlot _slot1;

        [SerializeField]
        private InventorySlot _slot2;

        [SerializeField]
        private InventorySlot _slot3;

        [SerializeField]
        private InventorySlot _slot4;

        public int TestSizeX = 1;
        public int TestSizeY = 1;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                var rect = GetAvailablePosition(TestSizeX, TestSizeY);
                Debug.Log($"X = {rect.x}, Y = {rect.y}, W = {rect.width}, H = {rect.height}");
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                GenerateSlot();
            }
        }

        private void Initialize()
        {
            _removeZone.InitContract(this);
            _dropField.InitContract(this);
            _dropField.Initialize();
            ApplySlotPosition(_slot1, 0, 0);
            ApplySlotPosition(_slot2, 4, 3);
            ApplySlotPosition(_slot3, 0, 3);
            ApplySlotPosition(_slot4, 0, 4);
            _slot1.InitializeContent(this);
            _slot2.InitializeContent(this);
            _slot3.InitializeContent(this);
            _slot4.InitializeContent(this);
        }

        private void ApplySlotPosition(InventorySlot slot, int x, int y)
        {
            slot.SetPosition(x, y);
            _dropField.RenderDropSlot(slot, x, y);

            for (int indexY = y; indexY < y + slot.SizeY; indexY++)
            {
                for (int indexX = x; indexX < x + slot.SizeX; indexX++)
                {
                    _inventoryMatrix[indexY, indexX] = true;
                }
            }
        }

        private Rect GetAvailablePosition(int sizeX, int sizeY)
        {
            //1. 인벤토리 메트릭스를 검사한다
            //2. 없으면 pos -1/-1로 리턴한다
            //3. 순차적으로 검색을 진행한다
            int x = 0;
            int y = 0;
            for (int indexY = y; indexY < InventorySizeY - sizeY; indexY++)
            {
                for (int indexX = x; indexX < InventorySizeX - sizeX; indexX++)
                {
                    if (CheckAvailablePosition(indexX, indexY, sizeX, sizeY))
                    {
                        return new Rect(indexX, indexY, sizeX, sizeY);
                    }
                }
            }

            return new Rect();
        }

        private bool CheckAvailablePosition(int x, int y, int sizeX, int sizeY)
        {
            for (int indexY = y; indexY < y + sizeY; indexY++)
            {
                for (int indexX = x; indexX < x + sizeX; indexX++)
                {
                    //한칸이라도 비지 않았으면 불가능하다
                    if (_inventoryMatrix[indexY, indexX])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void GenerateSlot()
        {
            //사이즈는 랜덤으로 처리한다
            int sizeX = UnityEngine.Random.Range(1, 4);
            int sizeY = UnityEngine.Random.Range(1, 4);
            var rect = GetAvailablePosition(sizeX, sizeY);

            if (rect.width == 0 || rect.height == 0)
            {
                //만들 수 없다
                Debug.Log($"Cant Make Slot SizeX: {sizeX}, SizeY: {sizeY}");
                return;
            }

            var slot = _slotMaker.MakeSlot((int)rect.position.x, (int)rect.position.y, sizeX, sizeY, this);
            ApplySlotPosition(slot, (int)rect.position.x, (int)rect.position.y);
        }

        private void RemoveItem(InventorySlot slot)
        {
            for (int indexY = slot.PositionY; indexY < slot.PositionY + slot.SizeY; indexY++)
            {
                for (int indexX = slot.PositionX; indexX < slot.PositionX + slot.SizeX; indexX++)
                {
                    _inventoryMatrix[indexY, indexX] = false;
                }
            }

            Destroy(slot.gameObject);
        }


        public void OnDropSlotItem(InventorySlot slot, int x, int y)
        {
            if (x < 0 || x + slot.SizeX > InventorySizeX || y < 0 || y + slot.SizeY > InventorySizeY)
            {
                Debug.Log("Out of bounds");
                _dropField.RenderDropSlot(slot, slot.PositionX, slot.PositionY);
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

                _dropField.RenderDropSlot(slot, slot.PositionX, slot.PositionY);
                return;
            }

            //문제가 없다
            ApplySlotPosition(slot, x, y);
        }

        public void OnDropRemoveItem(InventorySlot slot)
        {
            RemoveItem(slot);
        }

        public void OnDragSlotItem(InventorySlot slot)
        {
            var rect = _dropField.RecognizeRect(slot);

            if (rect.position.x < 0 || rect.position.y < 0 ||
                rect.position.x + rect.width > InventorySizeX || rect.position.y + rect.height > InventorySizeY)
            {
                _dropField.RenderDisableHighlights();
                return;
            }

            int x = (int)rect.position.x;
            int y = (int)rect.position.y;

            //TODO : 할 때 마다 메트릭스를 새로 만들어야 하나??
            //TODO : 만약 매트릭스를 bool이 아닌 유니크 id로 했다면 새로 만들지 않아도 되었다 -> 고로 새로 만듣나
            bool[,] matrix = (bool[,])_inventoryMatrix.Clone();
            for (int indexY = slot.PositionY; indexY < slot.PositionY + slot.SizeY; indexY++)
            {
                for (int indexX = slot.PositionX; indexX < slot.PositionX + slot.SizeX; indexX++)
                {
                    matrix[indexY, indexX] = false;
                }
            }

            bool available = true;
            for (int indexY = y; indexY < y + slot.SizeY; indexY++)
            {
                for (int indexX = x; indexX < x + slot.SizeX; indexX++)
                {
                    if (matrix[indexY, indexX])
                    {
                        available = false;
                        break;
                    }
                }
            }

            _dropField.RenderHighlights(available, x, y, slot.SizeX, slot.SizeY);
        }

        public void OnDragStartSlotItem(InventorySlot slot) => _dropField.RenderDisableHighlights();

        public void OnDragEndSlotItem(InventorySlot slot) => _dropField.RenderDisableHighlights();
    }
}

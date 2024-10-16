using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Inventory
{
    public class DropField : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        private List<GridSlot> _gridSlots = new List<GridSlot>();

        [SerializeField]
        private RectTransform _first;

        [SerializeField]
        private RectTransform _endPoistin;

        [SerializeField]
        private GridLayoutGroup _layout;

        private IDropFieldContract _contract;
        private float _slotSize;
        private Vector2 _startPosition;

        public void InitContract(IDropFieldContract contract)
        {
            _contract = contract;
        }

        public void Initialize()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layout.GetComponent<RectTransform>());
            var cellSize = _layout.cellSize + _layout.spacing;
            var layoutSize = _layout.GetComponent<RectTransform>();
            _slotSize = cellSize.x;
            _startPosition = new Vector2(_first.anchoredPosition.x - _first.rect.width / 2, _first.anchoredPosition.y + _first.rect.height / 2);
            _startPosition -= new Vector2(layoutSize.rect.width / 2, -layoutSize.rect.height / 2);
            int width = _layout.constraintCount;
            InitializeGridSlots();
        }

        private void InitializeGridSlots()
        {
            int constraintCount = _layout.constraintCount;
            for (int i = 0; i < _gridSlots.Count; i++)
            {
                _gridSlots[i].SetPosition(i / constraintCount, i % constraintCount);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var slot = eventData.pointerDrag.GetComponent<InventorySlot>();
            slot.DropInFiled = true;
            var rect = RecognizeRect(slot);
            _contract.OnDropSlotItem(slot, (int)rect.x, (int)rect.y);
        }

        public Rect RecognizeRect(InventorySlot slot)
        {
            var slotRect = slot.GetComponent<RectTransform>();
            var position = slotRect.anchoredPosition;
            position -= new Vector2(slotRect.rect.width / 2, -slotRect.rect.height / 2);


            float adjustX = position.x - _startPosition.x;
            float adjustY = _startPosition.y - position.y;

            //Debug.Log($"{index} / {eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition} / Adjust {adjustX} /{adjustY}");
            Debug.Log($"Pos : {(int)(adjustX / _slotSize + 0.5f)} / {(int)(adjustY / _slotSize + 0.5f)}");

            return new Rect((int)(adjustX / _slotSize + 0.5f), (int)(adjustY / _slotSize + 0.5f), slot.SizeX, slot.SizeY);
        }

        public void RenderHighlights(bool available, int x, int y, int sizeX, int sizeY)
        {
            foreach (var slot in _gridSlots)
            {
                slot.RenderNormal();
            }

            int constraintCount = _layout.constraintCount;

            for (int indexX = 0; indexX < sizeX; indexX++)
            {
                for (int indexY = 0; indexY < sizeY; indexY++)
                {
                    if (available)
                    {
                        _gridSlots[(indexX + x) + (indexY + y) * constraintCount].RenderAvailable();
                    }
                    else
                    {
                        _gridSlots[(indexX + x) + (indexY + y) * constraintCount].RenderUnavailable();
                    }
                }
            }
        }

        public void RenderDropSlot(InventorySlot slot, int x, int y)
        {
            var slotRect = slot.GetComponent<RectTransform>();
            var position = new Vector2(_startPosition.x + (x * _slotSize), _startPosition.y - (y * _slotSize));
            position += new Vector2(slotRect.rect.width / 2, -slotRect.rect.height / 2);

            slotRect.anchoredPosition = position;
        }
    }
}

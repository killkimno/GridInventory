using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Inventory
{
    public class DropField : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        private RectTransform _first;

        [SerializeField]
        private RectTransform _endPoistin;

        [SerializeField]
        private GridLayoutGroup _layout;

        private IDropFieldContract _contract;
        private float _slotSize;
        private Vector2 _startPosition;


        private void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layout.GetComponent<RectTransform>());
            var cellSize = _layout.cellSize + _layout.spacing;
            var layoutSize = _layout.GetComponent<RectTransform>();
            _slotSize = cellSize.x;
            _startPosition = new Vector2(_first.anchoredPosition.x - _first.rect.width / 2, _first.anchoredPosition.y + _first.rect.height / 2);
            _startPosition -= new Vector2(layoutSize.rect.width / 2, -layoutSize.rect.height / 2);
            int width = _layout.constraintCount;
        }

        public void InitContract(IDropFieldContract contract)
        {
            _contract = contract;
        }

        public void OnDrop(PointerEventData eventData)
        {
            //int index = transform.GetSiblingIndex();
            var slot = eventData.pointerDrag.GetComponent<InventorySlot>();
            slot.DropInFiled = true;
            var rect = RecognizeRect(slot);
            _contract.SetItemPosition(slot, (int)rect.x, (int)rect.y);
            //SetPosition(eventData.pointerDrag.GetComponent<InventorySlot>(), (int)rect.x, (int)rect.y);
            //Debug.Log(rect);
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

        public void SetPosition(InventorySlot slot, int x, int y)
        {
            var slotRect = slot.GetComponent<RectTransform>();
            var position = new Vector2(_startPosition.x + (x * _slotSize), _startPosition.y - (y * _slotSize));
            position += new Vector2(slotRect.rect.width / 2, -slotRect.rect.height / 2);

            slotRect.anchoredPosition = position;
        }
    }
}

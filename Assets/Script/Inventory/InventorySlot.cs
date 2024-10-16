using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Inventory
{
    //TODO : 모델 기반으로 바꿔야 한다
    public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Image _image;

        //시험용
        [SerializeField]
        private int _baseSizeX = 2;

        [SerializeField]
        private int _baseSizeY = 3;

        public int SizeX { get; private set; }
        public int SizeY { get; private set; }

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public bool DropInFiled { get; set; }

        private Vector2 _dragStartPosition;
        private IInventorySlotContract _contract;

        private void Awake()
        {
            SizeX = _baseSizeX;
            SizeY = _baseSizeY;
        }

        public void SetPosition(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }

        public void InitializeContent(IInventorySlotContract contract)
        {
            _contract = contract;
        }

        public void Initialize(int positionX, int positionY, int sizeX, int sizeY, int imageWidth, in int imageHeight)
        {
            _baseSizeX = sizeX;
            _baseSizeY = sizeY;
            SizeX = sizeX;
            SizeY = sizeY;
            PositionX = positionX;
            PositionY = positionY;
            _image.rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DropInFiled = false;
            transform.SetAsLastSibling();
            _image.raycastTarget = false;
            _dragStartPosition = transform.localPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            _contract?.OnDragSlotItem(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _image.raycastTarget = true;
            if (!DropInFiled)
            {
                transform.localPosition = _dragStartPosition;
            }
        }
    }
}

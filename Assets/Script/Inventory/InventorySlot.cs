using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.SampleScript.Inventory
{
    //TODO : 모델 기반으로 바꿔야 한다
    public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Image _image;

        public int SizeX { get; private set; }
        public int SizeY { get; private set; }

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public bool DropInFiled { get; set; }

        private Vector2 _dragStartPosition;

        private void Awake()
        {
            SizeX = 2;
            SizeY = 3;
        }

        public void SetPosition(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DropInFiled = false;
            _image.raycastTarget = false;
            _dragStartPosition = transform.localPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
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

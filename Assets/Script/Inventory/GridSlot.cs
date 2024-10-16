using UnityEngine;
using UnityEngine.UI;

namespace Script.Inventory
{
    public class GridSlot : MonoBehaviour
    {
        [SerializeField]
        private Image _background;

        private int _positionX;
        private int _positionY;

        public void SetPosition(int x, int y)
        {
            _positionX = x;
            _positionY = y;

            Debug.Log($"{_positionX} , {_positionY}");
        }

        public bool CheckPosition(int x, int y) => _positionX == x && _positionY == y;

        public void RenderNormal()
        {
            _background.color = Color.white;
        }

        public void RenderAvailable()
        {
            _background.color = Color.green;
        }

        public void RenderUnavailable()
        {
            _background.color = Color.red;
        }
    }
}

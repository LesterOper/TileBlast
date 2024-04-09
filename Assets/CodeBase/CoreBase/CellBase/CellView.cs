using DG.Tweening;
using UnityEngine;

namespace CodeBase.CoreBase.CellBase
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderComponent;
        public Transform CellTransform => transform;

        public void PaintCell(Sprite coloredSprite)
        {
            _renderComponent.sprite = coloredSprite;
            //_renderComponent.color = color;
        }
    }
}
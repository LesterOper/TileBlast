using CodeBase.CoreBase.FieldBase;
using UnityEngine;

namespace Board
{
    public class BoardTest : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private FieldSize _size;
        [SerializeField] private GameObject _prefab;

        [Space] [SerializeField] private FieldSettings boundsBoardSettings;
        private FieldBoard _boardBounds;

        private void Start()
        {
            _boardBounds = new FieldBoard(boundsBoardSettings);
            _boardBounds.Calculate(_size);
            
            // for (int i = 0; i < _size.Column; i++)
            // {
            //     for (int j = 0; j < _size.Row; j++)
            //     {
            //         var item = Instantiate(_prefab, _boardBounds.WorldPosition(i, j), Quaternion.identity, transform);
            //         item.transform.localScale = Vector3.one * _boardBounds.TileScale;
            //         item.GetComponent<SpriteRenderer>().sortingOrder = i + j;
            //     }
            // }

            for (int i = 0; i < _size._row; i++)
            {
                for (int j = 0; j < _size._column; j++)
                {
                    var item = Instantiate(_prefab, _boardBounds.WorldPosition(i, j), Quaternion.identity, transform);
                    item.transform.localScale = Vector3.one * _boardBounds.TileScale;
                    item.GetComponent<SpriteRenderer>().sortingOrder = i + j;
                }
            }

            //PlaceSprites();
        }

        // private void PlaceSprites()
        // {
        //     float spriteSize = _prefab.GetComponent<SpriteRenderer>().bounds.size.x;
        //
        //     float offsetPercentViewportX = DeviceUtils.IsTablet ? boundsBoardSettings.TabletOffsetPercentViewportX : 0f;
        //     _bounds = new BoardBounds(_camera, boundsBoardSettings.Top, boundsBoardSettings.Bottom, boundsBoardSettings.Edge, offsetPercentViewportX);
        //
        //     float totalWidthX = spriteSize * Size.x;
        //     float totalWidthY = spriteSize * Size.y;
        //
        //     float scaleX = _bounds.BottomSize / totalWidthX;
        //     float scaleY = _bounds.TopSize / totalWidthY;
        //
        //     float scale = scaleX < scaleY ? scaleX : scaleY; 
        //     
        //     spriteSize *= scale;
        //
        //     float offsetX = scaleY < scaleX ? (_bounds.BottomSize - spriteSize * Size.x) * 0.5f : 0f;
        //     
        //     float startX = _bounds.BottomLeft.x + spriteSize * 0.5f + offsetX;
        //     float startY = _bounds.BottomLeft.y + spriteSize * 0.5f;
        //
        //     for (int i = 0; i < Size.y; i++)
        //     {
        //         for (int j = 0; j < Size.x; j++)
        //         {
        //             Vector3 spritePosition = new Vector3(startX + j * spriteSize, startY + i * spriteSize, 0) * OffsetPositionScale;
        //             var item = Instantiate(_prefab, spritePosition, Quaternion.identity, transform);
        //             item.transform.localScale = Vector3.one * scale;
        //         }
        //     }
        // }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(_boardBounds == null) return;
            
            Gizmos.color = Color.red;
            /*Gizmos.DrawSphere(_boardBounds.Edge.BottomLeft, 0.2f);
            Gizmos.DrawSphere(_boardBounds.Edge.BottomRight, 0.2f);
            Gizmos.DrawSphere(_boardBounds.Edge.TopLeft, 0.2f);
            Gizmos.DrawSphere(_boardBounds.Edge.TopRight, 0.2f);*/
        }  
#endif
    }
}
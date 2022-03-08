using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TangramGame.Scripts
{
    public class TileContentController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private TileContentElement elementPrefab;

        private Action<TileContentController> OnContentPicked;
        private Action<TileContentController> OnContentDropped;
        private Action<TileContentController, Vector2> OnContentDragged;

        private List<TileContentElement> elements = new List<TileContentElement>();
        private Camera camera;
        private Vector2 initPos;
        
        public TileContent Content { get; private set; }

        public void Setup(TileContent tp, 
            Action<TileContentController> onPicked = null, 
            Action<TileContentController> onDropped = null, 
            Action<TileContentController, Vector2> onDragged = null)
        {
            Content = tp;
            camera = Camera.main;

            sprite.color = tp.color;

            foreach (var offsetPiece in tp.OffsetPieces)
            {
                var element = Instantiate(elementPrefab.gameObject, transform).GetComponent<TileContentElement>();
                element.gameObject.transform.localPosition = (Vector2) offsetPiece;
                element.Setup(tp.color);
                
                elements.Add(element);
            }

            if (onPicked != null) OnContentPicked += onPicked;
            if (onDropped != null) OnContentDropped += onDropped;
            if (onDragged != null) OnContentDragged += onDragged;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("POINTER DOWN");
            initPos = transform.position;
            transform.position = camera.ScreenToWorldPoint(eventData.position);
            OnContentPicked?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("DRAGGING");
            var finalPos = camera.ScreenToWorldPoint(eventData.position);
            finalPos.z = 0;
            transform.position = finalPos;
            OnContentDragged?.Invoke(this, transform.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnContentDropped?.Invoke(this);
        }
        
        public void ResetPos() => transform.position = initPos;
    }
}
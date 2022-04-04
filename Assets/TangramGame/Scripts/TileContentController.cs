using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TangramGame.Scripts
{
    public class TileContentController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private static int LastOrder = 10;
        
        [SerializeField] private TileContentElement originElement;
        [SerializeField] private TileContentElement elementPrefab;

        private Action<TileContentController> OnContentPicked;
        private Action<TileContentController> OnContentDropped;
        private Action<TileContentController, Vector2> OnContentDragged;

        private List<TileContentElement> elements = new List<TileContentElement>();
        private Camera camera;
        private Vector3 pickedUpOffset;
        
        public Vector2 initPos;

        private int scaleAnimId = Int32.MaxValue;
        
        public TileContent Content { get; private set; }

        public void Setup(TileContent tp, 
            Action<TileContentController> onPicked = null, 
            Action<TileContentController> onDropped = null, 
            Action<TileContentController, Vector2> onDragged = null)
        {
            Content = tp;
            camera = Camera.main;
            
            LastOrder++;

            originElement.Setup(tp.color);
            originElement.SetOrder(LastOrder);
            elements.Add(originElement);

            foreach (var offsetPiece in tp.OffsetPieces)
            {
                var element = Instantiate(elementPrefab.gameObject, transform).GetComponent<TileContentElement>();
                element.gameObject.transform.localPosition = (Vector2) offsetPiece;
                element.Setup(tp.color);
                element.SetOrder(LastOrder);
                Debug.Log($"Setting Order to {LastOrder}");
                
                elements.Add(element);
            }

            if (onPicked != null) OnContentPicked += onPicked;
            if (onDropped != null) OnContentDropped += onDropped;
            if (onDragged != null) OnContentDragged += onDragged;
        }

        public void AnimateScale(float from, float to, float duration)
        {
            if (scaleAnimId != Int32.MaxValue) LeanTween.cancel(scaleAnimId);
            
            transform.localScale = Vector3.one * from;
            scaleAnimId = gameObject.LeanScale(Vector3.one * to, duration).setEase(LeanTweenType.easeOutSine).id;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            initPos = transform.position;

            var worldPos = camera.ScreenToWorldPoint(eventData.position);
            
            pickedUpOffset = transform.position - worldPos;
            var finalPos = worldPos + pickedUpOffset;
            finalPos.z = 0;
            transform.position = finalPos;
            OnContentPicked?.Invoke(this);
            LastOrder++;
            foreach (var element in elements)
                element.SetOrder(LastOrder);
            
            transform.localScale = Vector3.one * 1.05f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var finalPos = camera.ScreenToWorldPoint(eventData.position) + pickedUpOffset;
            finalPos.z = 0;
            transform.position = finalPos;
            OnContentDragged?.Invoke(this, transform.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnContentDropped?.Invoke(this);
            /*foreach (var element in elements)
                element.ResetOrder();*/
            transform.localScale = Vector3.one;
        }
        
        public void ResetPos() => transform.position = initPos;

        public void SetOrder(int order)
        {
            foreach (var element in elements)
                element.SetOrder(order);
        }
    }
}
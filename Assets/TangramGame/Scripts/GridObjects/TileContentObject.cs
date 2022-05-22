using System;
using System.Collections.Generic;
using TangramGame.Scripts.GridSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TangramGame.Scripts
{
    public class TileContentObject : MonoBehaviour, IInteractable
    {
        private static int LastOrder = 10;
        
        [SerializeField] private TileContentElement originElement;
        [SerializeField] private TileContentElement elementPrefab;

        private Action<TileContentObject> OnContentPicked;
        private Action<TileContentObject> OnContentDropped;
        private Action<TileContentObject, Vector2> OnContentDragged;

        private List<TileContentElement> elements = new List<TileContentElement>();
        private Camera camera;
        private Vector3 pickedUpOffset;
        
        public Vector2 initPos;

        private int scaleAnimId = Int32.MaxValue;
        
        public TileContent Content { get; private set; }

        public void Setup(TileContent tp, 
            Action<TileContentObject> onPicked = null, 
            Action<TileContentObject> onDropped = null, 
            Action<TileContentObject, Vector2> onDragged = null)
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
                
                elements.Add(element);
            }

            if (onPicked != null) OnContentPicked += onPicked;
            if (onDropped != null) OnContentDropped += onDropped;
            if (onDragged != null) OnContentDragged += onDragged;
        }

        public void OnDestroy()
        {
            OnContentPicked = null;
            OnContentDropped = null;
            OnContentDragged = null;
        }

        public void AnimateScale(float from, float to, float duration)
        {
            if (scaleAnimId != Int32.MaxValue) LeanTween.cancel(scaleAnimId);
            
            transform.localScale = Vector3.one * from;
            scaleAnimId = gameObject.LeanScale(Vector3.one * to, duration).setEase(LeanTweenType.easeOutSine).id;
        }

        public Transform Transform => transform;

        public void OnGrab(Vector2 worldPosition)
        {
            initPos = transform.position;

            var worldPos = new Vector3(worldPosition.x, worldPosition.y, 0);
            pickedUpOffset = transform.position - worldPos;
            var finalPos = worldPos + pickedUpOffset;
            finalPos.z = 0;
            transform.position = finalPos;
            OnContentPicked?.Invoke(this);
            LastOrder++;
            SetOrder(LastOrder);
            
            transform.localScale = Vector3.one * 1.05f;
        }

        public void OnDrag(Vector2 worldPosition)
        {
            var finalPos = new Vector3(worldPosition.x, worldPosition.y, 0) + pickedUpOffset;
            finalPos.z = 0;
            transform.position = finalPos;
            OnContentDragged?.Invoke(this, transform.position);
        }

        public void OnDrop(Vector2 worldPosition)
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
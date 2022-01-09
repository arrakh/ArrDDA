using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MockupGame
{
    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        public Action<Tile> OnClicked;
        public bool IsOn { get; private set; } = false;
        public Color onColor, offColor;
        public Image image;

        private int scaleTween = 0;
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
            
            if(scaleTween != 0) LeanTween.cancel(scaleTween);
            transform.localScale = Vector3.one * 0.8f; 
            scaleTween = transform.LeanScale(Vector3.one, 0.25f)
                .setEase(LeanTweenType.easeOutBounce).uniqueId;
        }

        public void SetOn(bool on)
        {
            IsOn = on;
            image.color = on ? onColor : offColor;
        }
    }
}

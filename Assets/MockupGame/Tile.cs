using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MockupGame
{
    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        public Action<Tile> OnClicked;
        public bool IsOn = false;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }

        public void SetOn(bool on)
        {
            IsOn = on;
        }
    }
}

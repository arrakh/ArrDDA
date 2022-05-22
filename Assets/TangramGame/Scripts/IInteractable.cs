using System;
using UnityEngine;

namespace TangramGame.Scripts
{
    public interface IInteractable
    {
        public Transform Transform { get; }
        public void OnGrab(Vector2 worldPosition);
        public void OnDrop(Vector2 worldPosition);
        public void OnDrag(Vector2 worldPosition);
    }
}
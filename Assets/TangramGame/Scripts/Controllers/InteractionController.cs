using System;
using UnityEngine;

namespace TangramGame.Scripts.Controllers
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        private IInteractable current;

        private bool shouldDrag;

        private void Update()
        {
            if (shouldDrag)
            {
                var worldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                current.OnDrag(worldPos); 
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (current != null)
                {
                    current.OnDrop(current.Transform.position);
                    current = null;
                    shouldDrag = false;
                }
                
                var worldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(worldPos, Vector2.zero);
                if (hit.collider != null && hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    current = interactable;
                    current.OnGrab(worldPos);
                    shouldDrag = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && current != null)
            {
                var worldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                current.OnDrop(worldPos);
                current = null;
                shouldDrag = false;
            }
        }
    }
}
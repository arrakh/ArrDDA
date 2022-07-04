using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TangramGame.Scripts.Controllers
{
    public class InteractionController : MonoBehaviour
    {
        [FormerlySerializedAs("camera")] [SerializeField] private Camera cam;

        private IInteractable current;

        private bool shouldDrag;

        private void OnEnable() => Events.OnPreRoundOver += OnPreRoundOver;
        private void OnDisable() => Events.OnPreRoundOver -= OnPreRoundOver;

        private void OnPreRoundOver()
        {
            current = null;
            shouldDrag = false;
        }

        private void Update()
        {
            if (shouldDrag)
            {
                var worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
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
                
                var worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
                var hits = Physics2D.RaycastAll(worldPos, Vector2.zero);
                RaycastHit2D hit = new RaycastHit2D();

                foreach (var h in hits)
                {
                    if (hit.collider == null) hit = h;
                    else if (h.collider.transform.position.z > hit.collider.transform.position.z) hit = h;
                }
                
                if (hit.collider != null && hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    current = interactable;
                    current.OnGrab(worldPos);
                    shouldDrag = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && current != null)
            {
                var worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
                current.OnDrop(worldPos);
                current = null;
                shouldDrag = false;
            }
        }
    }
}
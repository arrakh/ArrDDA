using UnityEngine;

namespace TangramGame.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        public void MoveToGrid(GridController controller)
        {
            camera.transform.position = controller.GridWorldPos + new Vector3(-0.5f, -0.5f, -10);
            camera.orthographicSize = controller.Grid.width + 2;
        }
    }
}
using UnityEngine;
using UnityEngine.Serialization;

namespace TangramGame.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [FormerlySerializedAs("camera")] [SerializeField] private Camera cam;

        public void MoveToGrid(GridController controller)
        {
            cam.transform.position = controller.GridWorldPos + new Vector3(-0.5f, -0.5f, -10);
            cam.orthographicSize = controller.Grid.width + 2;
        }
    }
}
using Service.Abstract;
using UnityEngine;

namespace Service.CameraService
{
    public class CameraService : MonoService
    {
        private Camera _camera;

        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;
        }

        public Bounds GetScreenBounds(float offset = 0.5f)
        {
            if (_camera == null)
            {
                Debug.LogError("Main camera not found!");
                return new Bounds();
            }

            var groundHeight = 0f;

            var bottomLeft = GetWorldPointAtHeight(_camera, new Vector3(0, 0, 0), groundHeight);
            var topRight = GetWorldPointAtHeight(_camera, new Vector3(1, 1, 0), groundHeight);

            bottomLeft.x += offset;
            bottomLeft.z += offset;
            topRight.x -= offset;
            topRight.z -= offset;

            var center = (bottomLeft + topRight) / 2;
            var size = topRight - bottomLeft;

            return new Bounds(center, size);
        }

        private Vector3 GetWorldPointAtHeight(Camera cam, Vector3 viewportPoint, float height)
        {
            var ray = cam.ViewportPointToRay(viewportPoint);
            var groundPlane = new Plane(Vector3.up, new Vector3(0, height, 0));

            if (groundPlane.Raycast(ray, out var distance)) return ray.GetPoint(distance);

            return ray.origin;
        }
    }
}
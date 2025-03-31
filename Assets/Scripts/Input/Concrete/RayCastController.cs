using Input.Abstract;
using UnityEngine;

namespace Input.Concrete
{
    public class RayCastController : MonoBehaviour, IRayCaster
    {
        [SerializeField] private LayerMask inputLayer;
        private Camera _main;

        private void Awake()
        {
            _main = Camera.main;
        }

        public T Cast<T>(Vector2 position) where T : IInputReceiver
        {
            var ray = _main.ScreenPointToRay(position);
            Physics.Raycast(ray, out var hitInfo, 200f, inputLayer);

            if (!hitInfo.transform) return default;

            Debug.DrawRay(ray.origin, ray.direction * 200f, Color.green, .25f);

            var receiver = hitInfo.transform.GetComponentInParent<T>();
            return receiver;
        }
    }
}
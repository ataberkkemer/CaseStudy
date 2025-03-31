using UnityEngine;

namespace Input.Abstract
{
    public interface IRayCaster
    {
        T Cast<T>(Vector2 position) where T : IInputReceiver;
    }
}
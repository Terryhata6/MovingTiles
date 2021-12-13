using UnityEngine;

namespace Core.Utilits
{
    public class Utilits
    {
        public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
        {
            return camera.ScreenToWorldPoint(position);
        }
    }
}
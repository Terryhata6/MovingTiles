using UnityEngine;

namespace Core.UtilitsSpace
{
    public class Utilits
    {

        public static Vector3 GetPointFromCamera(Camera camera, Vector3 position)
        {
            /*position.z = camera.nearClipPlane;*/
            if (Physics.Raycast(camera.ScreenPointToRay(position), out RaycastHit hit , float.MaxValue))
            {
                return hit.point;
            }
            return camera.ScreenToWorldPoint(position);
        }
        public static Vector3 GetPointFromCamera(Camera camera, Vector3 position, out RaycastHit hit, LayerMask layer)
        {
            if (Physics.Raycast(camera.ScreenPointToRay(position), out hit , float.MaxValue, layer))
            {
                return hit.point;
            }
            return camera.ScreenToWorldPoint(position);
        }
    }
}
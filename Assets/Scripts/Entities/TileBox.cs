using UnityEngine;

namespace Core.Entities
{
    public class TileBox : MonoBehaviour
    {
        [SerializeField] private int _distanceFromCenter;

        public void SetDistance(int distance)
        {
            _distanceFromCenter = distance * 10;
        }
    }
}
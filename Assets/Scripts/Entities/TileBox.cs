using UnityEngine;

namespace Core.Entities
{
    public class TileBox : MonoBehaviour
    {
        [SerializeField] private int _distanceFromCenter;
        [SerializeField] private int _tileIndex;
        public bool _tileBusy = false;
        
        
        public void SetDistance(int distance)
        {
            _distanceFromCenter = distance * 10;
        }

        public void SetTileIndex(int index)
        {
            _tileIndex = index;
        }
    }
}
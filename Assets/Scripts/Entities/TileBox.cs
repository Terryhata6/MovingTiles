using System.Collections.Generic;
using UnityEngine;

namespace Core.Entities
{
    public class TileBox : MonoBehaviour
    {
        [SerializeField] private int _hCost; //heuric distance from ending
        [SerializeField] private int _tileIndex;
        [SerializeField] private List<TileBox> _neighbours;
        [SerializeField] private bool _walkable = true;
        [SerializeField] public int MovingWeight = 1;
        public List<TileBox> Neighbours => _neighbours;

        public TileBox LeftNeighbour;
        public TileBox RightNeighbour;
        public TileBox ForwardNeighbour;
        public TileBox BackNeighbour;
        public bool LeftNeighbourExists = false;
        public bool RightNeighbourExists = false;
        public bool ForwardNeighbourExists = false;
        public bool BackNeighbourExists = false;
        public bool Walkable => _walkable;


        #region Pathfinding fields
        [HideInInspector]public int GCost = 0; //From startNode to current
        [HideInInspector]public int HCost => _hCost; //heuric distance from ending
        [HideInInspector]public int Fcost => GCost + _hCost;
        [HideInInspector]public TileBox pathfindingParent;
        #endregion
        
        
        public void SetDistance(int distance)
        {
            _hCost = distance * 10;
        }

        public void SetTileIndex(int index)
        {
            _tileIndex = index;
        }

        public void AddNeighbour(TileBox neighbour)
        {
            _neighbours.Add(neighbour);
        }

        public void FindNeighbours()
        {
            TileBox temp;
            var distance = TileController.Instance.Step;
            foreach (var raycastHit in Physics.RaycastAll(transform.position, Vector3.left, distance, 1<<6))
            {
                temp = raycastHit.collider.GetComponent<TileBox>();
                if (!temp.Equals(this))
                {
                    AddNeighbour(temp);
                    LeftNeighbour = temp;
                    LeftNeighbourExists = true;
                }
            }
            foreach (var raycastHit in Physics.RaycastAll(transform.position, Vector3.right, distance, 1<<6))
            {
                temp = raycastHit.collider.GetComponent<TileBox>();
                if (!temp.Equals(this))
                {
                    AddNeighbour(temp);
                    RightNeighbour = temp;
                    RightNeighbourExists = true;
                }
            }
            foreach (var raycastHit in Physics.RaycastAll(transform.position, Vector3.forward, distance, 1<<6))
            {
                temp = raycastHit.collider.GetComponent<TileBox>();
                if (!temp.Equals(this))
                {
                    AddNeighbour(temp);
                    ForwardNeighbour = temp;
                    ForwardNeighbourExists = true;
                }
            }
            foreach (var raycastHit in Physics.RaycastAll(transform.position, Vector3.back, distance, 1<<6))
            {
                temp = raycastHit.collider.GetComponent<TileBox>();
                if (!temp.Equals(this))
                {
                    AddNeighbour(temp);
                    BackNeighbour = temp;
                    BackNeighbourExists = true;
                }
            }

        }
    }
}
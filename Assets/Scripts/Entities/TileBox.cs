using System;
using System.Collections.Generic;
using Core.Interfaces;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Core.Entities
{
    public class TileBox : MonoBehaviour
    {
        [SerializeField] private int _hCost; //heuric distance from ending
        [SerializeField] private int _tileIndex;
        [SerializeField] private ITilable _currentTilableObject;
        [SerializeField] private List<TileBox> _neighbours;
        [SerializeField] private bool _walkable = true;
        [SerializeField] private int _movingWeight = 1;
        [SerializeField] public MMFeedbacks _mmFeedbacks; 
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
        public bool TileBusy => !_currentTilableObject.Equals(null);
        public ITilable TiledObject => _currentTilableObject;
        public int MovingWeight
        {
            get
            {
                if (TileBusy)
                {
                    return 100*_movingWeight;
                }
                return _movingWeight;
            }
        }
        #region Pathfinding fields

        [HideInInspector]public int GCost = 0; //From startNode to current
        [HideInInspector]public int HCost => _hCost * 10; //heuric distance from ending
        [HideInInspector]public int FCost => GCost + _hCost;
        [HideInInspector]public TileBox pathfindingParent;
        #endregion


        public void Awake()
        {
            ExecuteSpawnAnimation();
        }

        public void SetDistance(int distance)
        {
            _hCost = distance;
        }

        public void SetTileIndex(int index)
        {
            _tileIndex = index;
        }

        public void AddNeighbour(TileBox neighbour)
        {
            _neighbours.Add(neighbour);
        }

        public void ExecuteSpawnAnimation()
        {
            //_mmFeedbacks.AutoPlayOnEnable = true;
        }

        TileBox tempTileBox;
        private int neighbourHitsCount;
        public void FindNeighbours(float distance, RaycastHit[] hits){

            neighbourHitsCount = Physics.RaycastNonAlloc(transform.position, Vector3.left, hits, distance, 1 << 6);
            for (int i = 0; i < neighbourHitsCount; i++)
            {
                tempTileBox = hits[i].collider.GetComponent<TileBox>();
                if (!tempTileBox.Equals(this))
                {
                    AddNeighbour(tempTileBox);
                    LeftNeighbour = tempTileBox;
                    LeftNeighbourExists = true;
                }
            }
            neighbourHitsCount = Physics.RaycastNonAlloc(transform.position, Vector3.right, hits, distance, 1 << 6);
            for (int i = 0; i < neighbourHitsCount; i++)
            {
                tempTileBox = hits[i].collider.GetComponent<TileBox>();
                if (!tempTileBox.Equals(this))
                {
                    AddNeighbour(tempTileBox);
                    RightNeighbour = tempTileBox;
                    RightNeighbourExists = true;
                }
            }
            neighbourHitsCount = Physics.RaycastNonAlloc(transform.position, Vector3.forward, hits, distance, 1 << 6);
            for (int i = 0; i < neighbourHitsCount; i++)
            {
                tempTileBox = hits[i].collider.GetComponent<TileBox>();
                if (!tempTileBox.Equals(this))
                {
                    AddNeighbour(tempTileBox);
                    ForwardNeighbour = tempTileBox;
                    ForwardNeighbourExists = true;
                }
            }
            neighbourHitsCount = Physics.RaycastNonAlloc(transform.position, Vector3.back, hits, distance, 1 << 6);
            for (int i = 0; i < neighbourHitsCount; i++)
            {
                tempTileBox = hits[i].collider.GetComponent<TileBox>();
                if (!tempTileBox.Equals(this))
                {
                    AddNeighbour(tempTileBox);
                    BackNeighbour = tempTileBox;
                    BackNeighbourExists = true;
                }
            }

        }


        public void ChangeTiledObject()
        {
            ChangeTiledObject(null);
        }

        public void ChangeTiledObject(ITilable obj)
        {
            _currentTilableObject = obj;
            
        }
    }
}
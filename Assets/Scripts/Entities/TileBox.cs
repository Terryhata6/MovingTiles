using System;
using System.Collections.Generic;
using Core.Interfaces;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Core.Entities
{
    public class TileBox : MonoBehaviour
    {
        [SerializeField] private int _hCostdistance; //heuric distance from ending
        [SerializeField] private int _tileIndex;
        [SerializeField] private BaseTilableObject _currentTilableObject;
        [SerializeField] private List<TileBox> _neighbours;
        [SerializeField] private bool _walkable = true;
        [SerializeField] private int _movingWeight = 1;
        [SerializeField] public MMFeedbacks _mmFeedbacks;
        public List<TileBox> Neighbours => _neighbours;
        public bool WillFree;
        public TileBox LeftNeighbour;
        public TileBox RightNeighbour;
        public TileBox ForwardNeighbour;
        public TileBox BackNeighbour;
        public bool LeftNeighbourExists = false;
        public bool RightNeighbourExists = false;
        public bool ForwardNeighbourExists = false;
        public bool BackNeighbourExists = false;
        public bool Walkable => _walkable;
        public bool TileBusy => _currentTilableObject != null;
        public BaseTilableObject TiledObject => _currentTilableObject;

        public int MovingWeight
        {
            get
            {
                if (TileBusy)
                {
                    return 1000 * _movingWeight;
                }

                return _movingWeight;
            }
        }

        #region Pathfinding fields

        [HideInInspector] public int GCost = 0; //From startNode to current
        [HideInInspector] public int HCost => _hCostdistance * 10; //heuric distance from ending
        [HideInInspector] public int FCost => GCost + _hCostdistance;
        [HideInInspector] public TileBox pathfindingParent;

        #endregion


        public void Awake()
        {
            //ExecuteSpawnAnimation();
        }

        public void Start()
        {
            //ExecuteSpawnAnimation();
        }

        public void SetDistance(int distance)
        {
            _hCostdistance = distance;
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
            _mmFeedbacks = GetComponent<MMFeedbacks>();
            _mmFeedbacks.Initialization();
            var component = _mmFeedbacks.GetComponent<MMFeedbackPosition>();
            component.Space = MMFeedbackPosition.Spaces.Local;
            component.InitialPosition = Vector3.zero;
            component.InitialPositionTransform = transform;
            _mmFeedbacks.PlayFeedbacks();
            //_mmFeedbacks.AutoPlayOnEnable = true;
        }

        TileBox tempTileBox;
        private int neighbourHitsCount;

        public void FindNeighbours(float distance, RaycastHit[] hits)
        {
            hits = new RaycastHit[5];
            neighbourHitsCount = Physics.RaycastNonAlloc(transform.position, Vector3.left, hits, distance, 1 << 6);
            for (int i = 0; i < neighbourHitsCount; i++)
            {
                if (hits[i].collider.TryGetComponent<TileBox>(out tempTileBox))
                {
                    if (tempTileBox._tileIndex != _tileIndex)
                    {
                        AddNeighbour(tempTileBox);
                        LeftNeighbour = tempTileBox;
                        LeftNeighbourExists = true;
                    }
                }
            }
            neighbourHitsCount = Physics.RaycastNonAlloc(transform.position, Vector3.right, hits, distance, 1 << 6);
            for (int i = 0; i < neighbourHitsCount; i++)
            {
                if (hits[i].collider.TryGetComponent<TileBox>(out tempTileBox))
                {
                    if (tempTileBox._tileIndex != _tileIndex)
                    {
                        AddNeighbour(tempTileBox);
                        RightNeighbour = tempTileBox;
                        RightNeighbourExists = true;
                    }
                }
            }
            neighbourHitsCount = Physics.RaycastNonAlloc(transform.position, Vector3.forward, hits, distance, 1 << 6);
            for (int i = 0; i < neighbourHitsCount; i++)
            {
                if (hits[i].collider.TryGetComponent<TileBox>(out tempTileBox))
                {
                    if (tempTileBox._tileIndex != _tileIndex)
                    {
                        AddNeighbour(tempTileBox);
                        ForwardNeighbour = tempTileBox;
                        ForwardNeighbourExists = true;
                    }
                }
            }
            neighbourHitsCount = Physics.RaycastNonAlloc(transform.position, Vector3.back, hits, distance, 1 << 6);
            for (int i = 0;
                i < neighbourHitsCount;
                i++)
            {
                if (hits[i].collider.TryGetComponent<TileBox>(out tempTileBox))
                {
                    if (tempTileBox._tileIndex != _tileIndex)
                    {
                        AddNeighbour(tempTileBox);
                        BackNeighbour = tempTileBox;
                        BackNeighbourExists = true;
                    }
                }
            }
        }


        public void ChangeTiledObject()
        {
            ChangeTiledObject(null);
            WillFree = true;
        }

        public void ChangeTiledObject(BaseTilableObject obj)
        {
            _currentTilableObject = obj;
            WillFree = false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using Core.Entities;
using Core.UtilitsSpace;
using UnityEngine;
using UnityEngine.WSA;

namespace Core
{
    public class TileController : Singleton<TileController>
    {
        [SerializeField] private TileBox tileBoxExample;
        [SerializeField] private int _dimension;
        [SerializeField] private float _step;
        [SerializeField] private List<TileBox> _tiles = new List<TileBox>();
        private Dictionary<int,List<TileBox>> _radiusBasedDictionary = new Dictionary<int, List<TileBox>>();
        private TileBox _centerBox;
        private TileBox _tempTileBox;

        public List<TileBox> AllTiles => _tiles;
        public Dictionary<int, List<TileBox>> TilesForRadius => _radiusBasedDictionary;
        public int MaximumRadius => _dimension;
        public float Step => _step;
        
        public void Awake()
        {
        }

        public void CreateTiles() {
            
            for (int i = -_dimension; i <= _dimension; i++)
            {
                for (int j = -_dimension; j <= _dimension; j++)
                {
                    _tempTileBox = Instantiate(tileBoxExample.gameObject,
                        transform.position + Vector3.right * (i * _step) + Vector3.forward * (j * _step),
                        Quaternion.identity, transform).GetComponent<TileBox>();
                    if (Mathf.Abs(i) > Mathf.Abs(j))
                    {
                        _tempTileBox.SetDistance(Mathf.Abs(i));
                        if (!_radiusBasedDictionary.ContainsKey(Mathf.Abs(i)))
                        {
                            _radiusBasedDictionary.Add(Mathf.Abs(i), new List<TileBox>());
                        }
                        _radiusBasedDictionary[Mathf.Abs(i)].Add(_tempTileBox);
                    }
                    else
                    {
                        _tempTileBox.SetDistance(Mathf.Abs(j));
                        if (!_radiusBasedDictionary.ContainsKey(Mathf.Abs(j)))
                        {
                            _radiusBasedDictionary.Add(Mathf.Abs(j), new List<TileBox>());
                        }
                        _radiusBasedDictionary[Mathf.Abs(j)].Add(_tempTileBox);
                    }

                    _tiles.Add(_tempTileBox);

                    if (j.Equals(0))
                    {
                        if (i.Equals(0))
                        {
                            _centerBox = _tempTileBox;
                        }
                    }
                }

            }
            RaycastHit[] hits = new RaycastHit[5];
            for (int i = 0; i < _tiles.Count; i++)
            {
                _tiles[i].SetTileIndex(i);
                _tiles[i].FindNeighbours(_step,hits);
            }
        }

        
        public TileBox GetTileForenemy()
        {
            return GetTileForenemy(_dimension);
        }
        public TileBox GetTileForenemy(int distance)
        {
            return null;
        }


        #region Pathfinding
        private List<TileBox> openSet = new List<TileBox>();
        private List<TileBox> closedSet = new List<TileBox>();
        private TileBox _tempCurrent;
        public List<TileBox> FindPath(TileBox Start)
        {
            return FindPath(Start, _centerBox);
        }

        public List<TileBox> FindPath(TileBox Start, TileBox Target)
        {
            openSet.Clear();
            closedSet.Clear();

            openSet.Add(Start);

            while (openSet.Count > 0)
            {
                _tempCurrent = openSet[0];
                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < _tempCurrent.FCost || openSet[i].FCost == _tempCurrent.FCost &&
                        openSet[i].HCost < _tempCurrent.HCost)
                    {
                        _tempCurrent = openSet[i];
                    }
                }

                openSet.Remove(_tempCurrent);
                closedSet.Add(_tempCurrent);

                if (_tempCurrent == Target)
                {
                    return RetracePath(Start, Target);
                }

                foreach (var neighbour in _tempCurrent.Neighbours)
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour)) continue;

                    var newMovementCostToNeighbour = _tempCurrent.GCost + neighbour.MovingWeight; //Here questions
                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        //neighbour.HCost = Distance For another from center cells
                        neighbour.pathfindingParent = _tempCurrent;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
            return null;
        }
        private List<TileBox> tempList = new List<TileBox>();
        public List<TileBox> RetracePath(TileBox Start, TileBox Target)
        {
            tempList.Clear();
            _tempCurrent = Target;
            while (_tempCurrent != Start)
            {
                tempList.Add(_tempCurrent);
                _tempCurrent = _tempCurrent.pathfindingParent;
            }
            
            tempList.Reverse();
            Debug.Log("PathFinded");
            return tempList;
        }
        #endregion
    }
}
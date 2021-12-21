using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Entities;
using Core.UtilitsSpace;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    [DefaultExecutionOrder(-1)]
    public class TileController : MonoBehaviour
    {
        public static TileController Instance;
        [SerializeField] private bool _useInstantiate = false;
        [SerializeField] private TileBoxType _spawnType;
        [SerializeField] private GameObject _tileBoxExample;


        [SerializeField] private GameObject _tileGrassPrefabParent;
        [SerializeField] private GameObject _tileGrassAndStonePrefabParent;
        [SerializeField] private GameObject _tileStonePrefabParent;
        [SerializeField] private List<GameObject> _tileGrassPrefabGameObjects;
        [SerializeField] private List<GameObject> _tileGrassAndStonePrefabGameObjects;
        [SerializeField] private List<GameObject> _tileStonePrefabGameObjects;


        [SerializeField] private int _dimension;
        [SerializeField] private float _step;
        [SerializeField] private List<TileBox> _tiles = new List<TileBox>();
        private Dictionary<int, List<TileBox>> _radiusBasedDictionary = new Dictionary<int, List<TileBox>>();

        private TileBox _centerBox;
        private TileBox _tempTileBox;

        public List<TileBox> AllTiles => _tiles;
        public Dictionary<int, List<TileBox>> TilesForRadius => _radiusBasedDictionary;
        public int MaximumRadius => _dimension;
        public float Step => _step;
        public TileBox Center => _centerBox;


        public void Awake()
        {
            Instance = this;
            if (_tileBoxExample == null)
            {
                Debug.Log("WHAT THE HELL", _tileBoxExample);
            }
        }

        private void OnEnable()
        {
            GameEvents.Instance.OnLevelEnd += StopAllCoroutinesFroEvent;
        }

        public void OnDisable()
        {
            GameEvents.Instance.OnLevelEnd -= StopAllCoroutinesFroEvent;
        }

        public void StopAllCoroutinesFroEvent()
        {
            StopAllCoroutines();
        }

        public void CreateTiles(TileBoxType tilType)
        {
            _spawnType = tilType;
            CreateTiles();
        }

        public void CreateTiles()
        {
            if (!_useInstantiate)
            {
                switch (_spawnType)
                {
                    case TileBoxType.Grass:
                    {
                        if (_tileGrassPrefabParent != null)
                        {
                            _tileGrassPrefabParent.SetActive(true);
                        }
                        break;
                    }
                    case TileBoxType.GrassAndStone:
                    {
                        if (_tileGrassAndStonePrefabParent != null)
                        {
                            _tileGrassAndStonePrefabParent.SetActive(true);
                        }
                        break;
                    }
                    case TileBoxType.Stone:
                    {
                        if (_tileStonePrefabParent != null)
                        {
                            _tileStonePrefabParent.SetActive(true);
                        }
                        break;
                    }
                    case TileBoxType.Random:
                    {
                        _spawnType = TileBoxType.Grass;
                        if (_tileGrassPrefabParent != null)
                        {
                            _tileGrassPrefabParent.SetActive(true);
                        }
                          /*  
                        _tileGrassPrefabParent.SetActive(true);
                        _tileGrassAndStonePrefabParent.SetActive(true);
                        _tileStonePrefabParent.SetActive(true);*/
                        break;
                    }
                    default:
                        break;
                }
            }

            int iterator = 0;
            for (int i = -_dimension; i <= _dimension; i++)
            {
                for (int j = -_dimension; j <= _dimension; j++)
                {
                    if (_useInstantiate)
                    {
                        _tempTileBox = Instantiate(_tileBoxExample,
                            (transform.position + Vector3.right * (i * _step) + Vector3.forward * (j * _step)),
                            Quaternion.identity, transform).GetComponent<TileBox>();
                    }
                    else
                    {
                        switch (_spawnType)
                        {
                            case TileBoxType.Grass:
                            {
                                _tempTileBox = _tileGrassPrefabGameObjects[iterator].GetComponent<TileBox>();
                                iterator++;
                                break;
                            }
                            case TileBoxType.GrassAndStone:
                            {
                                _tempTileBox = _tileGrassAndStonePrefabGameObjects[iterator].GetComponent<TileBox>();
                                iterator++;
                                break;
                            }
                            case TileBoxType.Stone:
                            {
                                _tempTileBox = _tileStonePrefabGameObjects[iterator].GetComponent<TileBox>();
                                iterator++;
                                break;
                            }
                            case TileBoxType.Random:
                            {
                                Debug.LogError("NotImplemented");
                                break;
                            }
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        _tempTileBox.transform.position = transform.position + Vector3.right * (i * _step) +
                                                          Vector3.forward * (j * _step);
                    }

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
            StartCoroutine(UpdateIndexAndStartAnimation());
        }
        
        public IEnumerator UpdateIndexAndStartAnimation()
        {
            yield return new WaitForFixedUpdate();
            RaycastHit[] hits = new RaycastHit[3];
            for (int p = 0; p < _tiles.Count; p++)
            {
                _tiles[p].SetTileIndex(p);
                _tiles[p].gameObject.name = $"TileBox{p}";
            }
            for (int p = 0; p < _tiles.Count; p++)
            {
                _tiles[p].FindNeighbours(_step, hits);
                _tiles[p].ExecuteSpawnAnimation();
            }
        }


        public TileBox GetTileForenemy()
        {
            return GetTileForenemy(_dimension);
        }

        public List<TileBox> spawnerList = new List<TileBox>();

        public TileBox GetTileForenemy(int distance)
        {
            spawnerList.Clear();
            for (int i = 0; i < _radiusBasedDictionary[distance].Count; i++)
            {
                if (!_radiusBasedDictionary[distance][i].TileBusy && _radiusBasedDictionary[distance][i].Walkable)
                {
                    spawnerList.Add(_radiusBasedDictionary[distance][i]);
                }
            }

            if (spawnerList.Count > 0)
            {
                return spawnerList[Random.Range(0, spawnerList.Count)];
            }

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
                    if (!neighbour.Walkable || closedSet.Contains(neighbour) ||
                        (neighbour.TileBusy && neighbour != Target)) continue;

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
            return tempList;
        }

        #endregion
    }

    public enum TileBoxType
    {
        Grass,
        GrassAndStone,
        Stone,
        Random
    }
}
using System;
using System.Collections.Generic;
using Core.Entities;
using UnityEngine;
using UnityEngine.WSA;

namespace Core
{
    public class TileController : MonoBehaviour
    {
        [SerializeField] private TileBox tileBoxExample;
        [SerializeField] private int _dimension;
        [SerializeField] private float _step;
        [SerializeField] private List<TileBox> _tiles = new List<TileBox>();
        private TileBox _tempTileBox;
        public void Awake()
        {
            CreateTiles();
        }

        public void CreateTiles()
        {
            for (int i = -_dimension; i <= _dimension; i++)
            {
                for (int j = -_dimension; j <=_dimension; j++)
                {
                    _tempTileBox = Instantiate(tileBoxExample.gameObject,
                        transform.position + Vector3.right * (i * _step) + Vector3.forward * (j * _step),
                        Quaternion.identity, transform).GetComponent<TileBox>();
                    if (Mathf.Abs(i) > Mathf.Abs(j))
                    {
                        _tempTileBox.SetDistance(Mathf.Abs(i));
                    }
                    else
                    {
                        _tempTileBox.SetDistance(Mathf.Abs(j));
                    }
                    _tiles.Add(_tempTileBox);
                }
            }

            for (int i = 0; i < _tiles.Count; i++)
            {
                _tiles[i].SetTileIndex(i);
            }
        }
    }
}
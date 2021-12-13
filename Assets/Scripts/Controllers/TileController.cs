using System;
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
        private TileBox tempTileBox;
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
                    tempTileBox = Instantiate(tileBoxExample.gameObject,
                        transform.position + Vector3.right * (i * _step) + Vector3.forward * (j * _step),
                        Quaternion.identity, transform).GetComponent<TileBox>();
                    if (Mathf.Abs(i) > Mathf.Abs(j))
                    {
                        tempTileBox.SetDistance(Mathf.Abs(i));
                    }
                    else
                    {
                        tempTileBox.SetDistance(Mathf.Abs(j));
                    }
                }
            }
        }
    }
}
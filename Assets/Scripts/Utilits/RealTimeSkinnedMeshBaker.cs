using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeSkinnedMeshBaker : MonoBehaviour
{
    [SerializeField] private MB3_MeshBaker _meshBaker;
    
    [Header("MeshRandomiser")][SerializeField] private List<GameObject> _coreGO;
    [SerializeField] private List<GameObject> _randomiseGO;

    public IEnumerator StartBaking()
    {
        if (!_meshBaker)
        {
            _meshBaker = GetComponentInChildren<MB3_MeshBaker>();
        }

        yield return BakeSkinnedMesh(FillRenderers());
        Debug.Log("go little rockstar");
    }

    public IEnumerator StartBaking(GameObject[] objs)
    {
        if (!_meshBaker)
        {
            _meshBaker = GetComponentInChildren<MB3_MeshBaker>();
        }

        yield return BakeSkinnedMesh(objs);
        Debug.Log("go little rockstar");
    }


    private IEnumerator BakeSkinnedMesh(GameObject[] gos)
    {
        _meshBaker.AddDeleteGameObjects(gos, gos, disableRendererInSource: true);
        _meshBaker.Apply();
        yield break;
    }

    private GameObject[] FillRenderers()
    {
        List<GameObject> _meshRenderers = new List<GameObject>();
        _meshRenderers.AddRange(_coreGO);
        for (int i = 0; i < _randomiseGO.Count; i++)
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                _meshRenderers.Add(_randomiseGO[i]);
            }
        }

        return _meshRenderers.ToArray();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeSkinnedMeshBaker : MonoBehaviour
{
    [SerializeField]private MB3_MeshBaker _meshBaker;
    [SerializeField] private GameObject[] _meshRenderers;
    [SerializeField] private SkinnedMeshRenderer _targetMeshRenderer;
    IEnumerator Start()
    {
        if (!_meshBaker)
        {
            _meshBaker = GetComponentInChildren<MB3_MeshBaker>();
        }

        yield return BakeSkinnedMesh(_meshRenderers);
        Debug.LogWarning("go little rockstar");
        
    }

    public IEnumerator BakeSkinnedMesh(GameObject[] gos)
    {
        _meshBaker.AddDeleteGameObjects(gos, gos ,disableRendererInSource: true);
        _meshBaker.Apply();
        yield break;
    }

}

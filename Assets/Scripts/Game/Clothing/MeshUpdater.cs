using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUpdater : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshCollider _meshCollider;

    public void UpdateMesh(Mesh newMesh)
    {
        _meshFilter.mesh = newMesh;
        _meshCollider.sharedMesh = newMesh;
    }

    private void OnValidate()
    {
        
    }
}

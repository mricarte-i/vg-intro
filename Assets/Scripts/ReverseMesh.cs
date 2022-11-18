using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReverseMesh : MonoBehaviour
{
    public MeshCollider MeshCollider => _meshCollider;
    [SerializeField] private MeshCollider _meshCollider;
    
    private void Awake ()
    {
        if(!_meshCollider) _meshCollider = GetComponent<MeshCollider>();       
    
        var mesh = _meshCollider.sharedMesh;
    
        // Reverse the triangles
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    
        // also invert the normals
        mesh.normals = mesh.normals.Select(n => -n).ToArray();
    }
}

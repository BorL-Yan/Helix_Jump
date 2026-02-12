using System.Collections.Generic;
using UnityEngine;

public class MeshCopyGenerator : MonoBehaviour
{   
    [SerializeField] private bool _isActive;
    [SerializeField] private int _count;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private float _rotationStep = 5.63f; 
    [SerializeField] private float _maxDistance = 0.001f;

    private Mesh[] _unificationMesh;
    private int _lastCount;
    public Mesh mesa;

#if UNITY_EDITOR
    void Update()
    {
        if(!Application.isPlaying) return;
        if(_isActive)
        {
            UpdateMeshes();
           
        }   
    }
#endif

    public void UpdateMeshes() //
    {
        if(_count != _lastCount) 
        {
            MeshFilter targetMesh = _gameObject.GetComponentInChildren<MeshFilter>();    
            _unificationMesh = new Mesh[_count];
            for (int i = 0; _count > i; i++)
            {          
                Mesh copy = Instantiate(targetMesh.sharedMesh);
                _unificationMesh[i] = copy;          
            }  
            _lastCount = _count; 
            CombineMeshes(_unificationMesh);
        }    
       
    }
    public void CombineMeshes(Mesh[] meshes) //private sarqir verevinel
    {
        CombineInstance[] combine = new CombineInstance[meshes.Length];
        for (int i = 0; i < meshes.Length; i++)
        {
            combine[i].mesh = meshes[i];

            // создаём поворот для i-й копии
            Quaternion rot = Quaternion.Euler(0f, i * _rotationStep, 0f);

            // если нужно — можно добавить позицию
            Vector3 pos = Vector3.zero;

            // масштаб обычно 1
            Vector3 scale = Vector3.one;

            // применяем трансформацию (поворот + позиция + масштаб)
            combine[i].transform = Matrix4x4.TRS(pos, rot, scale);

        }

        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(combine);
        finalMesh = WeldVerts(finalMesh, _maxDistance);
        GetComponent<MeshFilter>().mesh = finalMesh;
     
    }
    public static Mesh WeldVerts(Mesh mesh, float threshold)
    {
        Vector3[] oldVerts = mesh.vertices;
        int[] oldTris = mesh.triangles;

        List<Vector3> newVerts = new List<Vector3>();
        List<int> newTris = new List<int>();

        int[] map = new int[oldVerts.Length];

        for (int i = 0; i < oldVerts.Length; i++)
        {
            Vector3 p = oldVerts[i];
            bool found = false;
            for (int j = 0; j < newVerts.Count; j++)
            {
                if ((newVerts[j] - p).sqrMagnitude < threshold * threshold)
                {
                    map[i] = j;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                map[i] = newVerts.Count;
                newVerts.Add(p);
            }
        }
        for (int i = 0; i < oldTris.Length; i++)
        {
            newTris.Add(map[oldTris[i]]);
        }
        Mesh newMesh = new Mesh();
        newMesh.vertices = newVerts.ToArray();
        newMesh.triangles = newTris.ToArray();
        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();
        return newMesh;
    }
}





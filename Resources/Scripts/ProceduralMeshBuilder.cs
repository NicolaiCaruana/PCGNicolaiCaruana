using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public abstract class ProceduralMeshBuilder
{
    private List<Vector3> Vertices = new List<Vector3>();
    private List<int> Index = new List<int>();
    private List<Vector3> Normal = new List<Vector3>();
    private List<Vector2> UV = new List<Vector2>();
    public List<int>[] submeshIndices = new List<int>[] { };

    public abstract void AddMaterials(MeshRenderer renderer);

    public void SetUpSubmeshes(int submeshCount)
    {
        submeshIndices = new List<int>[submeshCount];
        for (int i = 0; i < submeshCount; i++)
        {
            submeshIndices[i] = new List<int>();
        }
    }

    public void BuildMeshTriangle(Vector3 p0, Vector3 p1, Vector3 p2, int subMesh)
    {
        Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;

        int p0Index = Vertices.Count;
        int p1Index = Vertices.Count + 1;
        int p2Index = Vertices.Count + 2;

        Index.Add(p0Index);
        Index.Add(p1Index);
        Index.Add(p2Index);

        submeshIndices[subMesh].Add(p0Index);
        submeshIndices[subMesh].Add(p1Index);
        submeshIndices[subMesh].Add(p2Index);

        Vertices.Add(p0);
        Vertices.Add(p1);
        Vertices.Add(p2);

        Normal.Add(normal);
        Normal.Add(normal);
        Normal.Add(normal);

        UV.Add(new Vector2(0, 0));
        UV.Add(new Vector2(0, 1));
        UV.Add(new Vector2(1, 1));
    }

    public void VerifySubmeshes(Mesh m)
    {
        for (int i = 0; i < submeshIndices.Length; i++)
        {
            if (submeshIndices[i].Count < 3)
            {
                m.SetTriangles(new int[3] { 0, 0, 0 }, i);
            }

            else
            {
                m.SetTriangles(submeshIndices[i].ToArray(), i);
            }
        }
    }

    public Mesh CreateMesh()
    {
        //Mesh is built here
        Mesh mesh = new Mesh();
        mesh.vertices = Vertices.ToArray();
        mesh.triangles = Index.ToArray();
        mesh.normals = Normal.ToArray();
        mesh.uv = UV.ToArray();
        mesh.subMeshCount = submeshIndices.Length;

        VerifySubmeshes(mesh);
        return mesh;
    }
}
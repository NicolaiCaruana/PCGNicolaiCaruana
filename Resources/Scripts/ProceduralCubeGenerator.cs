using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PyramidMaker : ProceduralMeshBuilder
{
    public override void AddMaterials(MeshRenderer renderer)
    {
        List<Material> materialList = new List<Material>();
        Material red = new Material(Shader.Find("Specular"));
        red.color = Color.red;
        Material red2 = new Material(Shader.Find("Specular"));
        red2.color = Color.red;
        Material red3 = new Material(Shader.Find("Specular"));
        red3.color = Color.red;
        Material red4 = new Material(Shader.Find("Specular"));
        red4.color = Color.red;
        Material red5 = new Material(Shader.Find("Specular"));
        red5.color = Color.red;
        Material red6 = new Material(Shader.Find("Specular"));
        red6.color = Color.red;

        materialList.Add(red);
        materialList.Add(red2);
        materialList.Add(red3);
        materialList.Add(red4);
        materialList.Add(red5);
        materialList.Add(red6);

        renderer.materials = materialList.ToArray();
    }
}


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralCubeGenerator : MonoBehaviour
{
    [SerializeField] private Vector3 cubeSize = Vector3.one;

    // Update is called once per frame
    void Update()
    {
        BuildCube();
    }

    public void BuildCube()
    {
        PyramidMaker cubeBuilder = new PyramidMaker();
        cubeBuilder.SetUpSubmeshes(6);

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshRenderer meshrenderer = this.GetComponent<MeshRenderer>();

        //Top Vertices
        Vector3 t0 = new Vector3(cubeSize.x, cubeSize.y, -cubeSize.z);
        Vector3 t1 = new Vector3(-cubeSize.x, cubeSize.y, -cubeSize.z);
        Vector3 t2 = new Vector3(-cubeSize.x, cubeSize.y, cubeSize.z);
        Vector3 t3 = new Vector3(cubeSize.x, cubeSize.y, cubeSize.z);

        //Bottom Vertices
        Vector3 b0 = new Vector3(cubeSize.x, -cubeSize.y, -cubeSize.z);
        Vector3 b1 = new Vector3(-cubeSize.x, -cubeSize.y, -cubeSize.z);
        Vector3 b2 = new Vector3(-cubeSize.x, -cubeSize.y, cubeSize.z);
        Vector3 b3 = new Vector3(cubeSize.x, -cubeSize.y, cubeSize.z);

        //Top
        cubeBuilder.BuildMeshTriangle(t0, t1, t2, 0);
        cubeBuilder.BuildMeshTriangle(t0, t2, t3, 0);

        //Bottom
        cubeBuilder.BuildMeshTriangle(b2, b1, b0, 1);
        cubeBuilder.BuildMeshTriangle(b3, b2, b0, 1);

        //Back
        cubeBuilder.BuildMeshTriangle(b0, t1, t0, 2);
        cubeBuilder.BuildMeshTriangle(b0, b1, t1, 2);

        //Left
        cubeBuilder.BuildMeshTriangle(b1, t2, t1, 3);
        cubeBuilder.BuildMeshTriangle(b1, b2, t2, 3);

        //Right
        cubeBuilder.BuildMeshTriangle(b2, t3, t2, 4);
        cubeBuilder.BuildMeshTriangle(b2, b3, t3, 4);

        //Front
        cubeBuilder.BuildMeshTriangle(b3, t0, t3, 5);
        cubeBuilder.BuildMeshTriangle(b3, b0, t0, 5);

        meshFilter.mesh = cubeBuilder.CreateMesh();

        cubeBuilder.AddMaterials(meshrenderer);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidBuilder : ProceduralMeshBuilder
{
    public override void AddMaterials(MeshRenderer renderer)
    {
        List<Material> materialList = new List<Material>();
        Material yellow1 = new Material(Shader.Find("Specular"));
        yellow1.color = Color.yellow;
        Material yellow2 = new Material(Shader.Find("Specular"));
        yellow2.color = Color.yellow;
        Material yellow3 = new Material(Shader.Find("Specular"));
        yellow3.color = Color.yellow;
        Material yellow4 = new Material(Shader.Find("Specular"));
        yellow4.color = Color.yellow;

        materialList.Add(yellow1);
        materialList.Add(yellow2);
        materialList.Add(yellow3);
        materialList.Add(yellow4);
        renderer.materials = materialList.ToArray();
    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralPyramidGenerator : MonoBehaviour
{
    [SerializeField] private float pyramidSize = 5f;

    // Update is called once per frame
    void Update()
    {
        PyramidBuilder pyramidBuilder = new PyramidBuilder();
        pyramidBuilder.SetUpSubmeshes(4);

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshRenderer meshrenderer = this.GetComponent<MeshRenderer>();

        //Points of the pyramid are declared here
        Vector3 topPoint = new Vector3(0, pyramidSize, 0);
        Vector3 base0 = Quaternion.AngleAxis(0f, Vector3.up) * Vector3.forward * pyramidSize;
        Vector3 base1 = Quaternion.AngleAxis(240f, Vector3.up) * Vector3.forward * pyramidSize;
        Vector3 base2 = Quaternion.AngleAxis(120f, Vector3.up) * Vector3.forward * pyramidSize;

        //Triangles for the Pyramids are built here
        pyramidBuilder.BuildMeshTriangle(base0, base1, base2, 0);
        pyramidBuilder.BuildMeshTriangle(base1, base0, topPoint, 1);
        pyramidBuilder.BuildMeshTriangle(base2, topPoint, base0, 2);
        pyramidBuilder.BuildMeshTriangle(topPoint, base2, base1, 3);

        meshFilter.mesh = pyramidBuilder.CreateMesh();
        pyramidBuilder.AddMaterials(meshrenderer);
    }
}

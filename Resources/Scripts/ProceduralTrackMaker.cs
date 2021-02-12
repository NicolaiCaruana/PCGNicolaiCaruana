using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackBuilder : ProceduralMeshBuilder
{
    public override void AddMaterials(MeshRenderer renderer)
    {
        List<Material> materialsList = new List<Material>();
        Material whiteMat = new Material(Shader.Find("Specular"));
        whiteMat.color = Color.white;
        Material blackMat = new Material(Shader.Find("Specular"));
        blackMat.color = Color.black;
        Material redMat = new Material(Shader.Find("Specular"));
        redMat.color = Color.red;

        materialsList.Add(whiteMat);
        materialsList.Add(blackMat);
        materialsList.Add(redMat);
        materialsList.Add(whiteMat);

        renderer.materials = materialsList.ToArray();
    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ProceduralTrackMaker : MonoBehaviour
{
    [SerializeField] public float radius; //Radius of Path
    [SerializeField] private float segments = 300f;
    [SerializeField] private float lineWidth = 0.3f; //Line width of white seperation lines
    [SerializeField] private float roadWidth = 8f; //Road width on each side of line
    [SerializeField] private float edgeWidth = 1f;//Width of Barrier
    [SerializeField] private float edgeHeight = 1f;
    [SerializeField] private int subMeshSize = 4;
    [SerializeField] private float wavyness;
    [SerializeField] private float waveScale;
    [SerializeField] private bool stripeCheck = true;

    [SerializeField]
    private GameObject Car;
    [SerializeField]
    private Vector2 waveOffset;
    [SerializeField]
    private Vector2 waveStep = new Vector2(0.01f, 0.01f);

    // Start is called before the first frame update
    void Start()
    {
        radius = Random.Range(60f, 300f);
        wavyness = Random.Range(5f, 50f);
        waveScale = Random.Range(1f, 15f);
        Debug.Log(radius % waveScale);
        waveOffset = new Vector2(Random.Range(10f, 50f), Random.Range(10f,50f));
        StartCoroutine(GenerateRoad());
    }

    IEnumerator GenerateRoad()
    {
        TrackBuilder trackBuilder = new TrackBuilder();
        trackBuilder.SetUpSubmeshes(subMeshSize);

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshRenderer meshrenderer = this.GetComponent<MeshRenderer>();
        MeshCollider meshCollider = this.GetComponent<MeshCollider>();

        List<Vector3> segments = createSegments();
        CreatePath(segments, trackBuilder);
        //Car spawned on random segment
        int CarSpawnIndex = Random.Range(0, 298);

        Car.transform.position = segments[CarSpawnIndex];
        Car.transform.LookAt(segments[CarSpawnIndex + 1]);

        meshFilter.mesh = trackBuilder.CreateMesh();

        meshCollider.sharedMesh = meshFilter.mesh;

        trackBuilder.AddMaterials(meshrenderer);

        Vector3 markerLocation = Car.transform.position;

        yield return new WaitForSeconds(3f);

        //Marker placed behind car
        GameObject startMarker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        startMarker.GetComponent<CapsuleCollider>().isTrigger = true;
        startMarker.tag = "finishLine";
        startMarker.transform.localScale = new Vector3(5f, 1f, roadWidth * 2.2f);
        startMarker.transform.LookAt(segments[CarSpawnIndex + 1]);
        startMarker.transform.position = markerLocation;
        startMarker.name = "Start Marker";

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Vector3> createSegments()
    {
        //Circular race track divided into segments denoted
        //in degrees and each point is defined by each segment
        //Points created and stored into list
        float segmentDegrees = 360f / segments;
        List<Vector3> points = new List<Vector3>();
        for (float degrees = 0; degrees < 360f; degrees += segmentDegrees)
        {
            Vector3 point = Quaternion.AngleAxis(degrees, Vector3.up) * Vector3.forward * radius;
            points.Add(point);
        }

        Vector2 wave = this.waveOffset;

        for (int i = 0; i < points.Count; i++)
        {
            wave += waveStep;

            Vector3 point = points[i];
            Vector3 centreDirection = point.normalized;

            float noise = Mathf.PerlinNoise((wave.x * waveScale), (wave.y * waveScale));
            noise *= wavyness;
            float control = Mathf.PingPong(i, points.Count / 2f) / (points.Count / 2f);
            points[i] += centreDirection * noise * control;
        }
        return points;
}

    void CreatePath(List<Vector3> points, TrackBuilder trackBuilder)
    {
        for (int i = 1; i < points.Count + 1; i++)
        {
            Vector3 pPrev = points[i - 1];
            Vector3 pCurr = points[i % points.Count];
            Vector3 pNext = points[(i + 1) % points.Count];

            ExtrudeRoad(trackBuilder, pPrev, pCurr, pNext);
        }
    }

    private void ExtrudeRoad(TrackBuilder trackBuilder, Vector3 pPrev, Vector3 pCurr, Vector3 pNext)
    {

        Vector3 offset = Vector3.zero;
        Vector3 targetOffset = Vector3.forward * lineWidth;
        MakeRoadQuad(trackBuilder, pPrev, pCurr, pNext, offset, targetOffset, 0);
        offset += targetOffset;
        targetOffset = Vector3.forward * roadWidth;

        MakeRoadQuad(trackBuilder, pPrev, pCurr, pNext, offset, targetOffset, 1);

        int stripeSubmesh = 2;
        if (stripeCheck)
        {
            stripeSubmesh = 3;
        }

        stripeCheck = !stripeCheck;

        offset += targetOffset;
        targetOffset = Vector3.up * edgeHeight;
        MakeRoadQuad(trackBuilder, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);
        offset += targetOffset;
        targetOffset = Vector3.forward * edgeWidth;
        MakeRoadQuad(trackBuilder, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);
        offset += targetOffset;
        targetOffset = -Vector3.up * edgeHeight;
        MakeRoadQuad(trackBuilder, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);
    }

    private void MakeRoadQuad(TrackBuilder trackBuilder, Vector3 pPrev, Vector3 pCurr, Vector3 pNext,
                              Vector3 offset, Vector3 targetOffset, int submesh)
    {
        Vector3 forward = (pNext - pCurr).normalized;
        Vector3 forwardPrev = (pCurr - pPrev).normalized;

        BuildHalfTrack(forward, forwardPrev, trackBuilder, pCurr, pNext, offset, targetOffset, submesh,false);
        BuildHalfTrack(-forward, -forwardPrev, trackBuilder, pCurr, pNext, offset, targetOffset, submesh, true);
    }

    private void BuildHalfTrack(Vector3 direction, Vector3 previousDirection, TrackBuilder trackBuilder, Vector3 pCurr, Vector3 pNext,
                              Vector3 offset, Vector3 targetOffset, int submesh,bool IsInner)
    {
        Quaternion perp = Quaternion.LookRotation(Vector3.Cross(direction, Vector3.up));
        Quaternion perpPrev = Quaternion.LookRotation(Vector3.Cross(previousDirection, Vector3.up));

        Vector3 topLeft = pCurr + (perpPrev * offset);
        Vector3 topRight = pCurr + (perpPrev * (offset + targetOffset));

        Vector3 bottomLeft = pNext + (perp * offset);
        Vector3 bottomRight = pNext + (perp * (offset + targetOffset));

        if (!IsInner)
        {
            trackBuilder.BuildMeshTriangle(topLeft, topRight, bottomLeft, submesh);
            trackBuilder.BuildMeshTriangle(topRight, bottomRight, bottomLeft, submesh);
        }

        else
        {
            trackBuilder.BuildMeshTriangle(bottomLeft, bottomRight, topLeft, submesh);
            trackBuilder.BuildMeshTriangle(bottomRight, topRight, topLeft, submesh);
        }
    }
}


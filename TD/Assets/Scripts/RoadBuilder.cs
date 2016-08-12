using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(EdgeCollider2D))]
public class RoadBuilder : MonoBehaviour
{

    public bool hasToSnap = false;
    public float snapDistance = 100f;

    [HideInInspector]
    public Vector2 p1 = Vector2.zero;
    public Vector2 p2;
    public Vector2 p3;
    public Vector2 p4;

    [HideInInspector]
    public Vector2 worldP1;
    [HideInInspector]
    public Vector2 worldP2;
    [HideInInspector]
    public Vector2 worldP3;
    [HideInInspector]
    public Vector2 worldP4;

    [HideInInspector]
    public int elementNumber = 10;
    [HideInInspector]
    public float textureRepeatWidth = 33;
    [HideInInspector]
    public float textureRepeatHeight = 33;
    [HideInInspector]
    public float segmentHeight = 100;
    [HideInInspector]
    public bool groundCollider = true;

    [System.NonSerialized]
    public Vector2[] physicsVertices;
    [System.NonSerialized]
    public Vector2[] groundVertices;
    [System.NonSerialized]
    public Vector2[] meshVertices;
    [HideInInspector]
    public MeshFilter meshFilter;
    [HideInInspector]
    public Mesh mesh;
    [HideInInspector]
    private bool updateToggle = true;
    [HideInInspector]
    public bool destrucible = false;

    [HideInInspector]
    public string sortingLayer;

    [HideInInspector]
    public int sortingOrder;

    [Range(0f, 1f)]
    public float percent = 1f;
    void Start()
    {
        if (!Application.isPlaying)
        {
            СalculateMesh();
        }
        GetComponent<Renderer>().sortingLayerName = sortingLayer;
        GetComponent<Renderer>().sortingOrder = sortingOrder;
    }

    void Awake()
    {
        if (!Application.isPlaying)
        {
            meshFilter = this.gameObject.GetComponent<MeshFilter>();
            if (meshFilter != null)
                meshFilter.mesh = new Mesh();

            if (groundCollider)
            {
                EdgeCollider2D a = this.gameObject.GetComponent<EdgeCollider2D>();
                if (a != null)
                    a.Reset();
            }

            EasyHill2DManager.Instance().getAllHill2DNodes();
        }
        else
        {
            СalculateMesh();
        }
    }

    void onEnable()
    {

    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            if (this.gameObject.transform.localScale != Vector3.one)
                this.gameObject.transform.localScale = Vector3.one;
        }
    }

    //public void MakeCollider()
    //{
    //    EdgeCollider2D col = this.GetComponent<EdgeCollider2D>();

    //    if (col == null)
    //        col = this.gameObject.AddComponent<EdgeCollider2D>();
    //    Vector2[] v = new Vector2[physicsVertices.Length];

    //    for (int i = 0; i < physicsVertices.Length; i++)
    //    {
    //        v[i] = physicsVertices[i] - new Vector2(transform.position.x, transform.position.y);
    //    }


    //    col.points = v;
    //}

    public void UpdateMesh()
    {
        Vector3[] allVerts3 = new Vector3[physicsVertices.Length * 2];
        int a = 0;
        for (int i = 0; i < physicsVertices.Length; i += 1)
        {
            allVerts3[a++] = (Vector3)physicsVertices[i] - transform.position;
            allVerts3[a++] = (Vector3)groundVertices[i] - transform.position;
        }

        mesh.vertices = allVerts3;

        int[] triangles = new int[allVerts3.Length * 3];
        //        int vLength = groundVertices.Length;
        int b = 0;

        for (int i = 0; i < allVerts3.Length - 2; i += 2)
        {

            triangles[b++] = i;
            triangles[b++] = i + 3;
            triangles[b++] = i + 1;

            triangles[b++] = i;
            triangles[b++] = i + 2;
            triangles[b++] = i + 3;
        }
        mesh.triangles = triangles;

        CalculateUVs();

        //if (groundCollider)
            //MakeCollider();
    }

    public void CalculateUVs()
    {
        float textureWidth = this.GetComponent<Renderer>().sharedMaterial.mainTexture.width;
        float textureHeight = this.GetComponent<Renderer>().sharedMaterial.mainTexture.height;
        float segmentWidth = Mathf.Abs(p4.x - p1.x);
        Vector2[] uvs = new Vector2[mesh.vertices.Length];
        textureRepeatWidth = textureRepeatHeight * (textureWidth / textureHeight);
        //for (int x2 = 0; x2 < mesh.vertices.Length; x2++)
        //{
        //    float u2 = (mesh.vertices[x2].x + transform.position.x) / textureRepeatWidth;
        //    float v2 = (mesh.vertices[x2].y + transform.position.y) / textureRepeatHeight;
        //    uvs[x2] = new Vector2(u2, v2);
        //}

        bool top3 = true;
        textureRepeatHeight = 1f;
        for (int x = 0; x < mesh.vertices.Length; x++)
        {
            float u, v;
            v = top3 ? 0 : -textureRepeatHeight;

            u = (mesh.vertices[x].x);
            top3 = !top3;
            uvs[x] = new Vector2(u, v);
        }


        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
    }

    public void СalculateMesh()
    {
        meshFilter = this.gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = this.gameObject.AddComponent<MeshFilter>();
        }
        mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            meshFilter.mesh = new Mesh();
            mesh = meshFilter.sharedMesh;
        }

        mesh.Clear();

        physicsVertices = calculatePhysicVertices(p4, p3, p2, p1, elementNumber);

        groundVertices = new Vector2[physicsVertices.Length];

        for (int i = 0; i < groundVertices.Length; i++)
        {
            Vector3 temp = physicsVertices[i];
            if (i < groundVertices.Length - 1)
            {
                Vector3 bv = (physicsVertices[i] - physicsVertices[i + 1]).normalized;
                bv = Quaternion.AngleAxis(90, Vector3.forward) * bv;
                temp += bv * segmentHeight;

            }
            else
            {
                Vector3 bv = (physicsVertices[i] - physicsVertices[i - 1]).normalized;
                bv = Quaternion.AngleAxis(-90, Vector3.forward) * bv;
                temp += bv * segmentHeight;
            }

            groundVertices[i] = temp;
        }

        //for (int i = 0; i < groundVertices.Length / 2 - 1; i++)
        //{
        //    var tmp = groundVertices[i];
        //    groundVertices[i] = groundVertices[groundVertices.Length - i - 1];
        //    groundVertices[groundVertices.Length - i - 1] = tmp;
        //}
        UpdateMesh();



    }

    public void checkColliderEnabled()
    {
        if (this.gameObject.GetComponent<EdgeCollider2D>() != null && !groundCollider)
            DestroyImmediate(this.gameObject.GetComponent<EdgeCollider2D>());
    }

    void OnDrawGizmos()
    {
    }

    void drawGizmoLines()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < physicsVertices.Length - 1; i++)
        {
            Gizmos.DrawLine(physicsVertices[i], physicsVertices[i + 1]);
        }

        Gizmos.color = Color.cyan;
        for (int i = 0; i < groundVertices.Length - 1; i++)
        {
            Gizmos.DrawLine(groundVertices[i], groundVertices[i + 1]);
        }
    }

    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {

            if (updateToggle)
            {
                EasyHill2DManager.Instance().getAllHill2DNodes();
                updateToggle = false;
            }

            if (GUI.changed)
            {

                if (elementNumber < 1)
                    elementNumber = 1;
                СalculateMesh();

            }

            if (Selection.activeInstanceID == this.gameObject.GetInstanceID())
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawLine(new Vector3(p1.x, p1.y) + transform.position, new Vector3(p2.x, p2.y) + transform.position);
                Gizmos.DrawLine(new Vector3(p3.x, p3.y) + transform.position, new Vector3(p4.x, p4.y) + transform.position);
            }
        }
#endif
    }

    private Vector2[] calculatePhysicVertices(Vector2 pP1, Vector2 pP2, Vector2 pP3, Vector2 pP4, int pElementNumber)
    {
        pP1 += new Vector2(transform.position.x, transform.position.y);
        pP2 += new Vector2(transform.position.x, transform.position.y);
        pP3 += new Vector2(transform.position.x, transform.position.y);
        pP4 += new Vector2(transform.position.x, transform.position.y);
        Vector2[] verts;
        verts = new Vector2[pElementNumber + 2];

        float d = Mathf.Abs(pP4.x - pP1.x);
        float segLength = d / pElementNumber;

        Vector2 a = pP1;
        for (int i = 0; i <= pElementNumber + 1; i++)
        {
            verts[i] = a;
            var coof = (i * segLength) / d;
            var realCoof = coof * percent; 
            a = getBezierCoordinates(pP1, pP2, pP3, pP4, realCoof);
        }
        return verts;
    }

    Vector2[] CombineVector2Arrays(Vector2[] array1, Vector2[] array2)
    {
        Vector2[] array3 = new Vector2[array1.Length + array2.Length];
        System.Array.Copy(array1, array3, array1.Length);
        System.Array.Copy(array2, 0, array3, array1.Length, array2.Length);
        return array3;
    }

    private Vector2 getBezierCoordinates(Vector2 pP1, Vector2 pP2, Vector2 pP3, Vector2 pP4, float pPercentages)
    {
        return getBezierCoordinates(pP1.x, pP1.y, pP2.x, pP2.y, pP3.x, pP3.y, pP4.x, pP4.y, pPercentages);
    }

    private Vector2 getBezierCoordinates(float pX1, float pY1, float pX2, float pY2, float pX3, float pY3, float pX4, float pY4, float pPercentages)
    {
        float percentageDone = pPercentages;

        float u = 1 - percentageDone;
        float tt = percentageDone * percentageDone;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * percentageDone;

        float ut3 = 3 * uu * percentageDone;
        float utt3 = 3 * u * tt;

        float x = (uuu * pX1) + (ut3 * pX2) + (utt3 * pX3) + (ttt * pX4);
        float y = (uuu * pY1) + (ut3 * pY2) + (utt3 * pY3) + (ttt * pY4);

        return (new Vector2(x, y));
    }
}

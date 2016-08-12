using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif

public class EasyHill2DNode : MonoBehaviour
{
    [HideInInspector]
    public Vector2 p1 = Vector2.zero;

    public Vector2 p2, p3, p4;

    [HideInInspector]
    public Vector2
            worldP1, worldP2, worldP3, worldP4;
    [HideInInspector]
    public int
            elementNumber = 10;
    [HideInInspector]
    public float
            textureRepeatWidth = 33;
    [HideInInspector]
    public float
            textureRepeatHeight = 33;
    [HideInInspector]
    public float
            segmentHeight = 100;
    [HideInInspector]
    public bool
            groundCollider = true;
    [HideInInspector]
    public bool
            snapToParent = false;
    public GroundStyle groundStyle = GroundStyle.BEZIER;
    public TextureStyle textureStyle = TextureStyle.STRETCH;
    [System.NonSerialized]
    public Vector2[]
            physicsVertices;
    [System.NonSerialized]
    public Vector2[]
            groundVertices;
    [System.NonSerialized]
    public Vector2[]
            meshVertices;
    [HideInInspector]
    public MeshFilter
            meshFilter;
    [HideInInspector]
    public Mesh
            mesh;
    [HideInInspector]
    public EasyHill2DManager
            easyHill2DManager;
    private bool updateToggle = true;
    [HideInInspector]
    public bool
            destrucible = false;

    [HideInInspector]
    public string sortingLayer;

    [HideInInspector]
    public int sortingOrder;

    public enum GroundStyle
    {
        STRAIGHT,
        BEZIER,
        PIPE,
    }

    public enum TextureStyle
    {
        FIXED_WIDTH = 0,
        STRETCH,
        CONSTANT,
    }

	// Use this for initialization
	void Start ()
	{

			if (!Application.isPlaying) {
					if (snapToParent)
							SnapToParent ();
					calculateMesh ();

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
            calculateMesh();
        }
    }

    void onEnable()
    {

    }

    void OnDestroy()
    {
        if (!Application.isPlaying)
            EasyHill2DManager.Instance().getAllHill2DNodes();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            if (this.gameObject.transform.localScale != Vector3.one)
                this.gameObject.transform.localScale = Vector3.one;
        }
    }

    public void checkDestructible()
    {
        if (groundStyle != GroundStyle.STRAIGHT)
            destrucible = false;

        if (destrucible && this.gameObject.isStatic)
            this.gameObject.isStatic = false;
        if (!destrucible && !this.gameObject.isStatic)
            this.gameObject.isStatic = false;
    }

    public void checkLockY()
    {
        if (EasyHill2DManager.Instance().myLockY && transform.position.y != EasyHill2DManager.Instance().myLockYValue && !snapToParent && groundStyle == GroundStyle.STRAIGHT && GUI.changed)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, EasyHill2DManager.Instance().myLockYValue, this.gameObject.transform.position.z);
        }
    }

    public void checkDekoOffset()
    {
        if (EasyHill2DManager.Instance().myDekoOffset && snapToParent && this.gameObject.transform.localPosition.y != EasyHill2DManager.Instance().myDekoOffsetValue)
        {
            this.gameObject.transform.localPosition = new Vector3(0, EasyHill2DManager.Instance().myDekoOffsetValue, this.gameObject.transform.localPosition.z);
        }
        if (EasyHill2DManager.Instance().myDekoOffset && snapToParent && this.gameObject.transform.localPosition.x != 0)
        {
            this.gameObject.transform.localPosition = new Vector3(0, EasyHill2DManager.Instance().myDekoOffsetValue, this.gameObject.transform.localPosition.z);
        }
    }

    //Vector3[] _a = null;
    //Vector3[] a {
    //    get
    //    {
    //        if (_a == null)
    //        {
    //            _a = calculatePhysicVertices(p1, p2, p3, p4, elementNumber);
    //        }
    //        return _a;
    //    }
    //}

	public void makeCollider ()
	{
			EdgeCollider2D col = this.GetComponent<EdgeCollider2D> ();

			if (col == null)
				col = this.gameObject.AddComponent<EdgeCollider2D> ();
			Vector2[] v = new Vector2[physicsVertices.Length];
			switch(groundStyle){
			case GroundStyle.STRAIGHT:
                //    Vector3[] realPhysic;

                //    if (Application.isPlaying)
                //    {
                //        realPhysic = physicsVertices;
                //    }
                //    else
                //    {
                //        realPhysic = a;// calculatePhysicVertices(p1, p2, p3, p4, elementNumber);
                //    }
                //    for (int i = 0; i < physicsVertices.Length; i++)
                //    {
                //        if (Application.isPlaying)
                //        {
                //            v[i] = realPhysic[i] - transform.position;
                //        }
                //        else {
                //            v[i] = realPhysic[i];
                //        }
                //    }
                //break;
			case GroundStyle.BEZIER:
			case GroundStyle.PIPE:
			default:
				for (int i=0; i<physicsVertices.Length; i++) {
                    v[i] = physicsVertices[i] - new Vector2(transform.position.x, transform.position.y);
				}
				break;

			}

			col.points = v;
	}

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

        if (groundCollider)
            makeCollider();
    }

    public void CalculateUVs()
    {

        float textureWidth = this.GetComponent<Renderer>().sharedMaterial.mainTexture.width;
        float textureHeight = this.GetComponent<Renderer>().sharedMaterial.mainTexture.height;
        float segmentWidth = Mathf.Abs(p4.x - p1.x);
//        float num = Mathf.Ceil(segmentWidth / textureWidth);

        Vector2[] uvs = new Vector2[mesh.vertices.Length];

        switch (textureStyle)
        {
            case (TextureStyle.STRETCH):
                bool top = true;
                for (int x = 0; x < mesh.vertices.Length; x++)
                {
                    float u, v;
                    if (top)
                        v = 0;
                    else
                        v = -textureRepeatHeight;
                    float dist = mesh.vertices[x].x - p1.x;
                    u = (dist / segmentWidth) * textureRepeatWidth;
                    top = !top;
                    uvs[x] = new Vector2(u, v);
                }
                break;

            case (TextureStyle.CONSTANT):
//                bool top2 = true;
//                float x0 = 0;
//                float y0 = 0;
                textureRepeatWidth = textureRepeatHeight * (textureWidth / textureHeight);
                for (int x2 = 0; x2 < mesh.vertices.Length; x2++)
                {
                    float u2 = (mesh.vertices[x2].x + transform.position.x) / textureRepeatWidth;
                    float v2 = (mesh.vertices[x2].y + transform.position.y) / textureRepeatHeight;
                    uvs[x2] = new Vector2(u2, v2);
                }
                break;
            case (TextureStyle.FIXED_WIDTH):
                bool top3 = true;
                textureRepeatWidth = segmentHeight * (textureWidth / textureHeight);
                textureRepeatHeight = 1f;
                for (int x = 0; x < mesh.vertices.Length; x++)
                {
                    float u, v;
                    if (top3)
                        v = 0;
                    else
                        v = -textureRepeatHeight;
//                    float dist = mesh.vertices[x].x - p1.x;
                    u = (mesh.vertices[x].x + transform.position.x) * (textureHeight / textureWidth / segmentHeight);
                    top3 = !top3;
                    uvs[x] = new Vector2(u, v);
                }
                break;
            default:
                break;
        }


        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
    }

    public void calculateMesh()
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

        physicsVertices = calculatePhysicVertices(p1, p2, p3, p4, elementNumber);


        switch (groundStyle)
        {
            case GroundStyle.BEZIER:
                groundVertices = new Vector2[physicsVertices.Length];

                for (int i = 0; i < groundVertices.Length; i++)
                {
                    Vector3 temp = physicsVertices[i];
                    temp -= new Vector3(0, segmentHeight, 0);
                    groundVertices[i] = temp;
                }

                break;

            case GroundStyle.PIPE:
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

                break;
            case GroundStyle.STRAIGHT:
                groundVertices = new Vector2[physicsVertices.Length];

                for (int i = 0; i < groundVertices.Length; i++)
                {

                    Vector3 temp = new Vector3(physicsVertices[i].x, transform.position.y, transform.position.z) - segmentHeight * Vector3.down;

                    if (physicsVertices[i].y > temp.y)
                    {
                        groundVertices[i] = temp;
                    }
                    else
                    {
                        groundVertices[i] = physicsVertices[i];
                        physicsVertices[i] = temp;
                    }
                }
                break;

            default:
                break;
        }

        UpdateMesh();



    }

    public void SnapChildren()
    {
        if (!snapToParent)
        {
            EasyHill2DNode[] a = this.gameObject.transform.GetComponentsInChildren<EasyHill2DNode>();
            foreach (EasyHill2DNode n in a)
            {
                if (n.snapToParent)
                    n.SnapToParent();
            }
        }
    }

	public void SnapToParent ()
    {
        GameObject p = this.gameObject;
        if (p.transform.parent != null)
        {
            GameObject a = this.gameObject.transform.parent.gameObject;
            if (a != null)
            {
                EasyHill2DNode par = a.GetComponent<EasyHill2DNode>();
                if (par != null)
                {

                    this.gameObject.transform.localPosition = new Vector3(0, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);

                    p1 = par.p1;
                    p2 = par.p2;
                    p3 = par.p3;
                    p4 = par.p4;
                    elementNumber = par.elementNumber;
                    calculateMesh();
                }
            }
        }
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

	void OnDrawGizmosSelected ()
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
                calculateMesh();

            }


            if (Selection.activeInstanceID == this.gameObject.GetInstanceID() && !snapToParent)
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
            a = getBezierCoordinates(pP1, pP2, pP3, pP4, (i * segLength) / d);
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

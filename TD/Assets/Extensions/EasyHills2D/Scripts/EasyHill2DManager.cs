using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EasyHill2DManager
{
    public List<EasyHill2DNode> easyHill2DNodes;
    public List<EasyHill2DNode> easyHill2DDeko;
    private static EasyHill2DManager _instance = null;
    public bool mySnap;
    public bool myLockY;
    public float mySnapDistance;
    public float myLockYValue;
    public float myDekoOffsetValue;
    public bool myDekoOffset;

	Camera cam;
	float height,width;

    public static EasyHill2DManager Instance()
    {
        if (_instance == null)
        {
            _instance = new EasyHill2DManager();
            _instance.easyHill2DNodes = new List<EasyHill2DNode>();
            _instance.easyHill2DDeko = new List<EasyHill2DNode>();
            _instance.getAllHill2DNodes();
            _instance.updateEditorWindowVariables();
        }

        return _instance;
    }

	public void getAllHill2DNodes ()
	{
        easyHill2DNodes = new List<EasyHill2DNode>();
        easyHill2DDeko = new List<EasyHill2DNode>();
        EasyHill2DNode[] a = GameObject.FindObjectsOfType<EasyHill2DNode>();
        foreach(EasyHill2DNode node in a)
        {
            if (node.groundCollider)
                easyHill2DNodes.Add(node);
            else
                easyHill2DDeko.Add(node);
        }
	}

	public void InitCulling()
	{
		cam = Camera.main;
		height = 2f * cam.orthographicSize;
		width = height * cam.aspect;
		getAllHill2DNodes();
	}

	public void CullSegments()
	{
		foreach (EasyHill2DNode n in easyHill2DNodes) {
			if(n.p1.x+n.gameObject.transform.position.x<cam.transform.position.x+width/2 && n.p4.x+n.gameObject.transform.position.x>cam.transform.position.x-width/2)
				n.gameObject.SetActive(true);
			else n.gameObject.SetActive(false);
		}
	}

	public void DestructSegmentCircle(float pX, float pY, float pRadius, float pStrength)
	{
		foreach (EasyHill2DNode n in easyHill2DNodes) {
			if(n.destrucible)
			{
                if (pX + pRadius > n.p1.x + n.gameObject.transform.position.x && pX - pRadius < n.p4.x + n.gameObject.transform.position.x)
                {
                    for (int i = 0; i < n.physicsVertices.Length; i++)
                    {
                        float s = Vector2.Distance(new Vector3(n.physicsVertices[i].x, n.physicsVertices[i].y) + n.gameObject.transform.position, new Vector3(pX, pY, 0) + n.gameObject.transform.position);

                        if (s < pRadius)
                        {
                            float a = Mathf.Abs(pRadius - s) * pStrength;
                            if (n.physicsVertices[i].y - a < 0f) a = n.physicsVertices[i].y;
                            n.physicsVertices[i] = new Vector3(n.physicsVertices[i].x, n.physicsVertices[i].y - a);
                        }
                    }
                    n.UpdateMesh();
                }
			}
		}

	}

    public void selectAllSegments()
    {
        GameObject[] select = new GameObject[easyHill2DNodes.Count];

        for (int i = 0; i < easyHill2DNodes.Count; i++)
        {
            select[i] = easyHill2DNodes[i].gameObject;
        }
#if UNITY_EDITOR
        Selection.objects = select;
#endif
    }

    public void selectAllDeko()
    {
        GameObject[] select = new GameObject[easyHill2DDeko.Count];

        for (int i = 0; i < easyHill2DDeko.Count; i++)
        {
            select[i] = easyHill2DDeko[i].gameObject;
        }
#if UNITY_EDITOR
        Selection.objects = select;
#endif
    }

    public void selectAllSegmentsAndDeko()
    {
        GameObject[] select = new GameObject[easyHill2DDeko.Count + easyHill2DNodes.Count];

        for (int i = 0; i < easyHill2DNodes.Count; i++)
        {
            select[i] = easyHill2DNodes[i].gameObject;
        }

        for (int ii = easyHill2DNodes.Count; ii < easyHill2DDeko.Count + easyHill2DNodes.Count; ii++)
        {
            select[ii] = easyHill2DDeko[ii - easyHill2DNodes.Count].gameObject;
        }
#if UNITY_EDITOR
        Selection.objects = select;
#endif
    }

    public Vector2 getSnapPoint(Vector2 pPosition)
    {
        Vector3 ret = pPosition;

        foreach (EasyHill2DNode n in easyHill2DNodes)
        {
            if (n != null)
            {
                float dist = Vector3.Distance(pPosition, new Vector3(n.p4.x, n.p4.y) + n.transform.position);
                float dist2 = Vector3.Distance(pPosition, new Vector3(n.p1.x, n.p1.y) + n.transform.position);

                if (dist < mySnapDistance && dist != 0)
                {
                    ret = new Vector3(n.p4.x, n.p4.y) + n.transform.position;
                }
                else
                    if (dist2 < mySnapDistance && dist2 != 0)
                    {
                        ret = new Vector3(n.p1.x, n.p1.y) + n.transform.position;
                    }
            }
        }
        return ret;
    }

	public float getMinX()
	{

		getAllHill2DNodes ();
		float a=easyHill2DNodes[0].p1.x+easyHill2DNodes[0].gameObject.transform.position.x;
		for(int i=0; i<easyHill2DNodes.Count; i++)
		{
			if(easyHill2DNodes[i].p1.x+easyHill2DNodes[i].gameObject.transform.position.x<a)a=easyHill2DNodes[i].p1.x+easyHill2DNodes[i].gameObject.transform.position.x;
		}
		return a;
	}

	public float getMaxX()
	{
		
		getAllHill2DNodes ();
		float a=easyHill2DNodes[0].p4.x+easyHill2DNodes[0].gameObject.transform.position.x;

		for(int i=0; i<easyHill2DNodes.Count; i++)
		{
			if(easyHill2DNodes[i].p4.x+easyHill2DNodes[i].gameObject.transform.position.x>a)a=easyHill2DNodes[i].p4.x+easyHill2DNodes[i].gameObject.transform.position.x;

		}
		return a;
	}

    public void createSegment(Material mat)
    {
        GameObject newSegment = new GameObject();
        newSegment.name = "EasyHill2DSegment";
        newSegment.AddComponent<MeshFilter>();
        newSegment.AddComponent<MeshRenderer>();
        newSegment.GetComponent<Renderer>().sharedMaterial = mat;
        EasyHill2DNode newNode = newSegment.AddComponent<EasyHill2DNode>();
        //newNode.p1 = new Vector2(0, 0);
        newNode.p2 = new Vector2(5, 0);
        newNode.p3 = new Vector2(10, 0);
        newNode.p4 = new Vector2(15, 0);
        newNode.segmentHeight = -5;

        newNode.groundStyle = EasyHill2DNode.GroundStyle.STRAIGHT;
        newNode.textureStyle = EasyHill2DNode.TextureStyle.CONSTANT;
        newNode.calculateMesh();
        newSegment.isStatic = false;

#if UNITY_EDITOR
        Selection.activeGameObject = newSegment;
        Undo.RegisterCreatedObjectUndo(newSegment, "newSegment");
#endif

    }

    public void createSegmentRight(EasyHill2DNode p)
    {
        GameObject newSegment = GameObject.Instantiate(p.gameObject, p.gameObject.transform.position, Quaternion.identity) as GameObject;
        newSegment.name = p.gameObject.name;

        EasyHill2DNode newNode = newSegment.GetComponent<EasyHill2DNode>();
        newSegment.transform.position = p.transform.position + new Vector3(p.p4.x, p.p4.y);
        //float l = Vector3.Distance(p.p1, p.p4);
        //newNode.p1 = p.p4 - Vector2.right * l;

        //if (newNode.groundStyle == EasyHill2DNode.GroundStyle.STRAIGHT)
        //{
        //    Vector3 add = new Vector3(0, 0, 0);
        //    newSegment.transform.position += add + new Vector3(l, 0, 0);

        //}

        newNode.calculateMesh();
#if UNITY_EDITOR
        Selection.activeGameObject = newSegment;
        Undo.RegisterCreatedObjectUndo(newSegment, "newSegment");

#endif
    }

    public void createSegmentLeft(EasyHill2DNode p)
    {
        GameObject newSegment = GameObject.Instantiate(p.gameObject, p.gameObject.transform.position, Quaternion.identity) as GameObject;
        newSegment.name = p.gameObject.name;

        EasyHill2DNode newNode = newSegment.GetComponent<EasyHill2DNode>();
        newSegment.transform.position = p.transform.position - new Vector3(p.p4.x, p.p4.y);
        //float l = Vector3.Distance(p.p1, p.p4);
        //newNode.p4 = p.p1 + Vector2.right * l;

        //if (newNode.groundStyle == EasyHill2DNode.GroundStyle.STRAIGHT)
        //{
        //    Vector3 add = new Vector3(0, myLockYValue, 0);
        //    newSegment.transform.position -= add + new Vector3(l, 0, 0);

        //}
        newNode.calculateMesh();
#if UNITY_EDITOR
        Selection.activeGameObject = newSegment;
        Undo.RegisterCreatedObjectUndo(newSegment, "newSegment");

#endif
    }

    public void createDeko(Material mat, EasyHill2DNode p)
    {
        GameObject newSegment = new GameObject();
        newSegment.name = p.gameObject.name + "Deko";
        newSegment.AddComponent<MeshFilter>();
        newSegment.AddComponent<MeshRenderer>();
        newSegment.GetComponent<Renderer>().sharedMaterial = mat;
        EasyHill2DNode newNode = newSegment.AddComponent<EasyHill2DNode>();
        //newNode.p1 = new Vector3(0, 10, 0);
        newNode.p2 = new Vector2(5, 10);
        newNode.p3 = new Vector2(10, 10);
        newNode.p4 = new Vector2(15, 10);
        newNode.groundStyle = EasyHill2DNode.GroundStyle.BEZIER;
        newNode.textureStyle = EasyHill2DNode.TextureStyle.FIXED_WIDTH;
        newNode.sortingOrder = 1;

        newNode.snapToParent = true;
        newNode.segmentHeight = 3;
        newNode.groundCollider = false;
        newNode.textureRepeatHeight = 0.95f;
        newSegment.transform.parent = p.gameObject.transform;
        newNode.checkColliderEnabled();
        newNode.SnapToParent();
        newNode.calculateMesh();
        newSegment.isStatic = false;
#if UNITY_EDITOR
        Selection.activeGameObject = newSegment;
        Undo.RegisterCreatedObjectUndo(newSegment, "newSegment");
#endif
    }

    public void setSegementsStatic()
    {
        foreach (EasyHill2DNode n in easyHill2DDeko)
        {

            n.gameObject.isStatic = false;
        }
        foreach (EasyHill2DNode n in easyHill2DNodes)
        {

            n.gameObject.isStatic = false;
        }
    }

    public void CleanForSaving()
    {
        getAllHill2DNodes();

        foreach (EasyHill2DNode n2 in easyHill2DNodes)
        {
            Object.DestroyImmediate(n2.mesh);
            Object.DestroyImmediate(n2.meshFilter);
            Object.DestroyImmediate(n2.gameObject.GetComponent<EdgeCollider2D>());
            n2.physicsVertices = null;
            n2.groundVertices = null;
            n2.meshVertices = null;
            n2.gameObject.isStatic = false;
        }

        foreach (EasyHill2DNode n in easyHill2DDeko)
        {
            Object.DestroyImmediate(n.mesh);
            Object.DestroyImmediate(n.meshFilter);
            Object.DestroyImmediate(n.gameObject.GetComponent<EdgeCollider2D>());
            n.physicsVertices = null;
            n.groundVertices = null;
            n.meshVertices = null;
            n.gameObject.isStatic = false;
        }
    }

    public void restoreSegments()
    {
        getAllHill2DNodes();


        foreach (EasyHill2DNode n in easyHill2DDeko)
        {
            n.calculateMesh();
        }
        foreach (EasyHill2DNode n in easyHill2DNodes)
        {
            n.calculateMesh();
        }

    }

    public void ConvertToIdentity()
    {        
        foreach(EasyHill2DNode n in easyHill2DNodes)
        {
            Vector2 p1 = n.p1;
            n.p1 -= p1;
            n.p3 -= p1;
            n.p2 -= p1;
            n.p4 -= p1;
            n.gameObject.transform.position += new Vector3(p1.x, p1.y);
            float localMin = Mathf.Min(n.p1.y, n.p2.y, n.p3.y, n.p4.y) - 30f;
            n.segmentHeight = localMin;
            n.calculateMesh();
        }
        
        foreach (EasyHill2DNode n in easyHill2DDeko)
        {
            Vector2 p1 = n.p1;
            n.p1 -= p1;
            n.p3 -= p1;
            n.p2 -= p1;
            n.p4 -= p1;
            n.calculateMesh();
        }
    }

    public void AlignHeight()
    {
        float minPos = float.MaxValue;

        foreach (EasyHill2DNode n in easyHill2DNodes)
        {
            float localMin = n.gameObject.transform.position.y + Mathf.Min(n.p1.y, n.p2.y, n.p3.y, n.p4.y);
            if (localMin < minPos) minPos = localMin;
        }
        foreach (EasyHill2DNode n in easyHill2DNodes)
        {
            n.segmentHeight = -30f - n.transform.position.y + minPos;
            n.calculateMesh();
        }
    }

    Vector2 lerpPoint(Vector2 v1, Vector2 v2, float t)
    {
        return new Vector2(v1.x * (1 - t) + v2.x * t, v1.y * (1 - t) + v2.y * t);
    }

    public void DivideSegment(EasyHill2DNode node, float t = 0.5f)
    {
        Vector2 p0 =  node.p1, p1 = node.p2, p2 = node.p3, p3 = node.p4;
        Vector2 p4 = lerpPoint(p0, p1, t);
        Vector2 p5 = lerpPoint(p1, p2, t);
        Vector2 p6 = lerpPoint(p2, p3, t);
        Vector2 p7 = lerpPoint(p4, p5, t);
        Vector2 p8 = lerpPoint(p5, p6, t);
        Vector2 p9 = lerpPoint(p7, p8, t);

        Vector2[] leftPoints = new Vector2[] { p0, p4, p7, p9 };
        Vector2[] rightPoints = new Vector2[] { p9, p8, p6, p3 };

        float localMin;

        EasyHill2DNode leftNode = (GameObject.Instantiate(node.gameObject) as GameObject).GetComponent<EasyHill2DNode>();
        leftNode.gameObject.name = leftNode.gameObject.name.Replace("(Clone)", "");
        leftNode.p1 = leftPoints[0];
        leftNode.p2 = leftPoints[1];
        leftNode.p3 = leftPoints[2];
        leftNode.p4 = leftPoints[3];
        localMin = Mathf.Min(leftNode.p1.y, leftNode.p2.y, leftNode.p3.y, leftNode.p4.y);
        leftNode.segmentHeight = localMin - 30f;
        leftNode.calculateMesh();
        leftNode.UpdateMesh();

        EasyHill2DNode rightNode = (GameObject.Instantiate(node.gameObject) as GameObject).GetComponent<EasyHill2DNode>();
        rightNode.gameObject.name = rightNode.gameObject.name.Replace("(Clone)", "");
        rightNode.p1 = rightPoints[0];
        rightNode.p2 = rightPoints[1];
        rightNode.p3 = rightPoints[2];
        rightNode.p4 = rightPoints[3];
        localMin = Mathf.Min(rightNode.p1.y, rightNode.p2.y, rightNode.p3.y, rightNode.p4.y);
        rightNode.segmentHeight = localMin - 30f;
        rightNode.calculateMesh();
        rightNode.UpdateMesh();        
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

    public void checkDekoOffset()
    {
        foreach (EasyHill2DNode n in easyHill2DDeko)
        {
            n.checkDekoOffset();
        }
    }

    public void checkLockY()
    {
        foreach (EasyHill2DNode n in easyHill2DNodes)
        {
            n.checkLockY();
        }
    }

    public void updateEditorWindowVariables()
    {
        mySnap = getSnapping();
        myLockY = getLockY();
        mySnapDistance = getSnappingDistance();
        myLockYValue = getLockYValue();
        myDekoOffset = getDekoOffset();
        myDekoOffsetValue = getDekoOffsetValue();
    }

    public float getSnappingDistance()
    {
#if UNITY_EDITOR
        return EditorPrefs.GetFloat("EasyHill2DSnapDistance", 2);
#else
			return 0;
#endif
    }

	public float getLockYValue ()
    {
#if UNITY_EDITOR
        return EditorPrefs.GetFloat("EasyHill2DLockYValue", 0);
#else
			return 0;
#endif
    }

    public bool getLockY()
    {
#if UNITY_EDITOR
        return EditorPrefs.GetBool("EasyHill2DLockY", true);
#else
			return true;
#endif
    }

    public bool getSnapping()
    {
#if UNITY_EDITOR
        return EditorPrefs.GetBool("EasyHill2DSnap", true);
#else
			return true;
#endif
    }

    public float getDekoOffsetValue()
    {
#if UNITY_EDITOR
        return EditorPrefs.GetFloat("EasyHill2DdekoOffsetValue", 0);
#else
			return 0;
#endif
    }

    public bool getDekoOffset()
    {
#if UNITY_EDITOR
        return EditorPrefs.GetBool("EasyHill2DdekoOffset", true);
#else
			return true;
#endif
    }
	
}

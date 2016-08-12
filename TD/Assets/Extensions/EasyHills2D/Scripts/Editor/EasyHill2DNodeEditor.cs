using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor (typeof(EasyHill2DNode)), CanEditMultipleObjects]
public class EasyHill2DNodeEditor : Editor
{

    SerializedProperty el, gc, sp, tw, th, sh, dd, sl, so;

#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
    private static System.Reflection.PropertyInfo sortingLayerNamesPropInfo = null;
    private static bool sortingLayerNamesChecked = false;

    private Renderer[] renderers = new Renderer[0];
    private static string[] GetSortingLayerNames()
    {
        if (sortingLayerNamesPropInfo == null && !sortingLayerNamesChecked)
        {
            sortingLayerNamesChecked = true;
            try
            {
                System.Type IEU = System.Type.GetType("UnityEditorInternal.InternalEditorUtility,UnityEditor");
                if (IEU != null)
                {
                    sortingLayerNamesPropInfo = IEU.GetProperty("sortingLayerNames", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                }
            }
            catch { }
            if (sortingLayerNamesPropInfo == null)
            {
                Debug.Log("tk2dEditorUtility - Unable to get sorting layer names.");
            }
        }

        if (sortingLayerNamesPropInfo != null)
        {
            return sortingLayerNamesPropInfo.GetValue(null, null) as string[];
        }
        else
        {
            return new string[0];
        }
    }

    public static string SortingLayerNamePopup(string label, string value)
    {
        if (value == "")
        {
            value = "Ground";
        }
        string[] names = GetSortingLayerNames();
        if (names.Length == 0)
        {
            return EditorGUILayout.TextField(label, value);
        }
        else
        {
            int sel = 0;
            for (int i = 0; i < names.Length; ++i)
            {
                if (names[i] == value)
                {
                    sel = i;
                    break;
                }
            }
            sel = EditorGUILayout.Popup(label, sel, names);
            return names[sel];
        }
    }
#endif
    protected T[] GetTargetsOfType<T>(Object[] objects) where T : UnityEngine.Object
    {
        System.Collections.Generic.List<T> ts = new System.Collections.Generic.List<T>();
        foreach (Object o in objects)
        {
            T s = o as T;
            if (s != null)
                ts.Add(s);
        }
        return ts.ToArray();
    }

    void OnEnable()
    {
        el = serializedObject.FindProperty("elementNumber");
        gc = serializedObject.FindProperty("groundCollider");
        sp = serializedObject.FindProperty("snapToParent");

        tw = serializedObject.FindProperty("textureRepeatWidth");
        th = serializedObject.FindProperty("textureRepeatHeight");

        sh = serializedObject.FindProperty("segmentHeight");
        dd = serializedObject.FindProperty("destrucible");

        sl = serializedObject.FindProperty("sortingLayer");
        so = serializedObject.FindProperty("sortingOrder");

#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
        EasyHill2DNode[] ms = GetTargetsOfType<EasyHill2DNode>(targets);
        System.Collections.Generic.List<Renderer> rs = new System.Collections.Generic.List<Renderer>();
        foreach (var v in ms)
        {
            if (v != null && v.GetComponent<Renderer>() != null)
            {
                rs.Add(v.GetComponent<Renderer>());
            }
        }
        renderers = rs.ToArray();
#endif
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EasyHill2DNode t = (EasyHill2DNode)target;
        DrawDefaultInspector();

        if (t.textureStyle != EasyHill2DNode.TextureStyle.FIXED_WIDTH)
        {
            if (t.textureStyle != EasyHill2DNode.TextureStyle.CONSTANT)
                tw.floatValue = EditorGUILayout.FloatField("Texture Repeat Width", tw.floatValue);
            th.floatValue = EditorGUILayout.FloatField("Texture Repeat Height", th.floatValue);
        }

        sh.floatValue = EditorGUILayout.FloatField("Segment Height", sh.floatValue);

        GUILayout.Label("Resolution of Segment: " + t.elementNumber);

        EditorGUILayout.IntSlider(el, 1, 148);

        gc.boolValue = GUILayout.Toggle(gc.boolValue, "Use Ground Collider");
        sp.boolValue = GUILayout.Toggle(sp.boolValue, "Is Deko?");
        dd.boolValue = GUILayout.Toggle(dd.boolValue, "Is Destructible?");

        GUILayout.Space(8);
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
		int sortingOrder = EditorGUILayout.IntField("Sorting Order In Layer", targetSprites[0].SortingOrder);
		if (sortingOrder != targetSprites[0].SortingOrder) {
            tk2dUndo.RecordObjects(targetSprites, "Sorting Order In Layer");
            foreach (tk2dBaseSprite s in targetSprites) {
            	s.SortingOrder = sortingOrder;
            }
		}
#else

        string sortingLayerName = SortingLayerNamePopup("Sorting Layer", sl.stringValue);
        if (sortingLayerName != renderers[0].sortingLayerName)
        {
            foreach (Renderer r in renderers)
            {
                r.sortingLayerName = sortingLayerName;
                sl.stringValue = sortingLayerName;
            }
        }

        int sortingOrder = EditorGUILayout.IntField("Order In Layer", so.intValue);
        if (sortingOrder != renderers[0].sortingOrder)
        {
            foreach (Renderer r in renderers)
            {
                r.sortingOrder = sortingOrder;
                so.intValue = sortingOrder;
            }
        }

#endif
        GUILayout.Space(8);

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            t.SnapChildren();

            for (int i = 0; i < targets.Length; i++)
            {
                EasyHill2DNode a = (EasyHill2DNode)targets[i];
                a.calculateMesh();
                a.checkColliderEnabled();
                a.checkDestructible();
                if (a.snapToParent) a.SnapToParent();
                EditorUtility.SetDirty(a);
            }
        }
    }

    private bool designMode = false;
    void OnSceneGUI()
    {
        CheckKeyDesignPressed();

        EasyHill2DNode t = (EasyHill2DNode)target;

        if (Selection.activeInstanceID == t.gameObject.GetInstanceID() && !t.snapToParent && designMode)
        {
            Tools.hidden = true;

            t.worldP1 = new Vector3(t.p1.x, t.p1.y) + t.transform.position;
            t.worldP2 = new Vector3(t.p2.x, t.p2.y) + t.transform.position;
            t.worldP3 = new Vector3(t.p3.x, t.p3.y) + t.transform.position;
            t.worldP4 = new Vector3(t.p4.x, t.p4.y) + t.transform.position;

            t.p1 = Handles.PositionHandle(t.worldP1, Quaternion.identity);
            t.p2 = Handles.PositionHandle(t.worldP2, Quaternion.identity);
            t.p3 = Handles.PositionHandle(t.worldP3, Quaternion.identity);
            t.p4 = Handles.PositionHandle(t.worldP4, Quaternion.identity);

            t.p1 -= new Vector2(t.transform.position.x, t.transform.position.y);
            t.p2 -= new Vector2(t.transform.position.x, t.transform.position.y);
            t.p3 -= new Vector2(t.transform.position.x, t.transform.position.y);
            t.p4 -= new Vector2(t.transform.position.x, t.transform.position.y);

            t.p1 = new Vector3(t.p1.x, t.p1.y, 0);
            t.p2 = new Vector3(t.p2.x, t.p2.y, 0);
            t.p3 = new Vector3(t.p3.x, t.p3.y, 0);
            t.p4 = new Vector3(t.p4.x, t.p4.y, 0);
        }
        else if(!designMode)
        {
            Tools.hidden = false;
            ConvertToIdentity(t);
            t.SnapChildren();
            t.calculateMesh();             
        }

        if (GUI.changed)
        {
            if (EasyHill2DManager.Instance().getSnapping())
            {
                EasyHill2DManager.Instance().getAllHill2DNodes();
                t.p1 = EasyHill2DManager.Instance().getSnapPoint(t.p1 + new Vector2(t.transform.position.x, t.transform.position.y));
                t.p4 = EasyHill2DManager.Instance().getSnapPoint(t.p4 + new Vector2(t.transform.position.x, t.transform.position.y));

                t.p1 -= new Vector2(t.transform.position.x, t.transform.position.y);
                t.p4 -= new Vector2(t.transform.position.x, t.transform.position.y);

            }
            ConvertToIdentity(t);
            t.SnapChildren();
            t.calculateMesh();            
            SceneView.RepaintAll();
            EditorUtility.SetDirty(t);
        }
    }
    private void ConvertToIdentity(EasyHill2DNode n)
    {
        Vector2 p1 = n.p1;
        n.p1 -= p1;
        n.p3 -= p1;
        n.p2 -= p1;
        n.p4 -= p1;
        n.gameObject.transform.position += new Vector3(p1.x, p1.y);
        if (n.textureStyle != EasyHill2DNode.TextureStyle.FIXED_WIDTH)
        {
            float localMin = Mathf.Min(n.p1.y, n.p2.y, n.p3.y, n.p4.y) - 30f;
            n.segmentHeight = localMin < n.segmentHeight ? localMin : n.segmentHeight;
        }        
        n.calculateMesh();
    }

    private void CheckKeyDesignPressed()
    {
        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.T)   designMode = true;
                if (e.keyCode == KeyCode.LeftCommand)   designMode = true;
                if (designMode) Undo.RecordObjects(new Object[] { target, ((target) as EasyHill2DNode).gameObject.transform }, "bezierCoordChange");
                break;
            case EventType.KeyUp:
                if (e.keyCode == KeyCode.T)   designMode = false;
                if (e.keyCode == KeyCode.LeftCommand)   designMode = false;
                break;
        }
    }

}

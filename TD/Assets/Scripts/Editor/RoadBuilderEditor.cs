using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEditorInternal;
using System.Reflection;

[CustomEditor(typeof(RoadBuilder)), CanEditMultipleObjects]
public class RoadBuilderEditor : Editor
{
    SerializedProperty el, gc, sp, tw, th, sh, dd, sl, so;

    private Renderer[] renderers = new Renderer[0];

    public string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    public string SortingLayerNamePopup(string label, string value)
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

    void OnEnable()
    {
        el = serializedObject.FindProperty("elementNumber");

        //tw = serializedObject.FindProperty("textureRepeatWidth");
        //th = serializedObject.FindProperty("textureRepeatHeight");

        sh = serializedObject.FindProperty("segmentHeight");

        sl = serializedObject.FindProperty("sortingLayer");
        so = serializedObject.FindProperty("sortingOrder");


        List<Renderer> rs = new List<Renderer>();
        rs.Add((target as RoadBuilder).GetComponent<Renderer>());

        renderers = rs.ToArray();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        RoadBuilder t = (RoadBuilder)target;
        DrawDefaultInspector();

        //tw.floatValue = EditorGUILayout.FloatField("Texture Repeat Width", tw.floatValue);
        //th.floatValue = EditorGUILayout.FloatField("Texture Repeat Height", th.floatValue);
        

        sh.floatValue = EditorGUILayout.FloatField("Segment Height", sh.floatValue);

        GUILayout.Label("Resolution of Segment: " + t.elementNumber);

        EditorGUILayout.IntSlider(el, 1, 148);

        //gc.boolValue = GUILayout.Toggle(gc.boolValue, "Use Ground Collider");

        GUILayout.Space(8);

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
        GUILayout.Space(8);

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {

            for (int i = 0; i < targets.Length; i++)
            {
                RoadBuilder a = (RoadBuilder)targets[i];
                a.СalculateMesh();
                //a.checkColliderEnabled();
                EditorUtility.SetDirty(a);
            }
        }
    }

    private bool designMode = false;
    void OnSceneGUI()
    {
        CheckKeyDesignPressed();

        RoadBuilder t = (RoadBuilder)target;

        if (Selection.activeInstanceID == t.gameObject.GetInstanceID() && designMode)
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
        else if (!designMode)
        {
            Tools.hidden = false;
            ConvertToIdentity(t);
            t.СalculateMesh();
        }

        if (GUI.changed)
        {
            if (t.hasToSnap)
            {
                TryToSnap();
               
            }
            ConvertToIdentity(t);
            t.СalculateMesh();
            SceneView.RepaintAll();
            EditorUtility.SetDirty(t);
        }
    }
    private void ConvertToIdentity(RoadBuilder n)
    {
        Vector2 p1 = n.p1;
        n.p1 -= p1;
        n.p3 -= p1;
        n.p2 -= p1;
        n.p4 -= p1;
        n.gameObject.transform.position += new Vector3(p1.x, p1.y);
        float localMin = Mathf.Min(n.p1.y, n.p2.y, n.p3.y, n.p4.y) - 30f;
        //n.segmentHeight = localMin < n.segmentHeight ? localMin : n.segmentHeight;

        n.СalculateMesh();
    }

    private void CheckKeyDesignPressed()
    {
        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.T) designMode = true;
                if (e.keyCode == KeyCode.LeftCommand) designMode = true;
                if (designMode) Undo.RecordObjects(new UnityEngine.Object[] { target, ((target) as RoadBuilder).gameObject.transform }, "bezierCoordChange");
                break;
            case EventType.KeyUp:
                if (e.keyCode == KeyCode.T) designMode = false;
                if (e.keyCode == KeyCode.LeftCommand) designMode = false;
                break;
        }
    }

    private void TryToSnap()
    {



        var t = target as RoadBuilder;
        t.p1 = GetSnapPoint(t.p1 + new Vector2(t.transform.position.x, t.transform.position.y));
        t.p4 = GetSnapPoint(t.p4 + new Vector2(t.transform.position.x, t.transform.position.y));

        t.p1 -= new Vector2(t.transform.position.x, t.transform.position.y);
        t.p4 -= new Vector2(t.transform.position.x, t.transform.position.y);
    }
        

    private Vector3 GetSnapPoint(Vector2 position)
    {
        var roads = new List<RoadBuilder>();
        roads.AddRange(FindObjectsOfType<RoadBuilder>());
        roads.Remove(target as RoadBuilder);

        var curRoad = target as RoadBuilder;

        Vector3 destination = position;

        foreach (var road in roads)
        {
            var distanceFirst = Vector3.Distance(position, new Vector3(road.p4.x, road.p4.y) + road.transform.position);
            var distanceSecond = Vector3.Distance(position, new Vector3(road.p1.x, road.p1.y) + road.transform.position);

            if (distanceFirst < curRoad.snapDistance && distanceFirst != 0)
            {
                destination = new Vector3(road.p4.x, road.p4.y) + road.transform.position;
            }
            else if (distanceSecond < curRoad.snapDistance && distanceSecond != 0)
            {
                destination = new Vector3(road.p1.x, road.p1.y) + road.transform.position;
            }
        }
        return destination;

    }
}

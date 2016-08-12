using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class EasyHill2D : EditorWindow
{
    public bool snap = true;
    public float snapDistance = 2f;
    public bool lockY;
    public float lockYValue;
    public bool dekoOffset;
    public float dekoOffsetValue;
    public Material mat, dekomat;

    private bool isCreateNew = false;

    [MenuItem("Tools/EasyHills2D/EasyHill2D Manager", false, 10501)]
    static void Init()
    {
        Selection.activeObject = (EasyHill2D)EditorWindow.GetWindow(typeof(EasyHill2D));
    }

	void OnGUI ()
	{
        snap = EditorGUILayout.BeginToggleGroup("Snap Anchor Points", snap);
        snapDistance = EditorGUILayout.FloatField("Snap Distance", snapDistance);
        EditorGUILayout.EndToggleGroup();
        GUILayout.Space(10);

        isCreateNew = EditorGUILayout.BeginToggleGroup("Create new segment", isCreateNew);
        if(isCreateNew)
        {
            //new segment
            EditorGUILayout.LabelField("Segment Material:");
            mat = EditorGUILayout.ObjectField(mat, typeof(Material), true) as Material;
            if (mat == null)
            {
                EditorGUILayout.HelpBox("Choose a Segment Material", MessageType.Warning);
            }
            else if (GUILayout.Button("Create Segment"))
            {
                EasyHill2DManager.Instance().createSegment(mat);
            }    
            //new deko
            EditorGUILayout.LabelField("Deko Material:");
            dekomat = EditorGUILayout.ObjectField(dekomat, typeof(Material), true) as Material;
            if (dekomat == null)
            {
                EditorGUILayout.HelpBox("Choose a Deko Material", MessageType.Warning);
            }
            else if (GUILayout.Button("Create Deko"))
            {
                GameObject s = Selection.activeGameObject;
                if (s != null)
                {
                    EasyHill2DNode nn = s.GetComponent<EasyHill2DNode>();
                    if (nn != null)
                    {
                        if (!nn.snapToParent)
                            EasyHill2DManager.Instance().createDeko(dekomat, nn);
                        else
                            EditorUtility.DisplayDialog("EasyHills2D", "Select a Segment.", "Ok");
                    }
                }
                else
                    EditorUtility.DisplayDialog("EasyHills2D", "Select a Segment.", "Ok");
            }
        }
        EditorGUILayout.EndToggleGroup();
        
        GUILayout.Space(10);


        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Select all Segments"))
            {
                EasyHill2DManager.Instance().selectAllSegments();
            }
            if (GUILayout.Button("Select all Deko"))
            {
                EasyHill2DManager.Instance().selectAllDeko();
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        //lockY = EditorGUILayout.BeginToggleGroup("Lock Y coordinates of segments", lockY);
        //lockYValue = EditorGUILayout.FloatField("Y Value", lockYValue);
        //if (GUILayout.Button("Update Y Coordinates of Segments"))
        //{
        //    EasyHill2DManager.Instance().checkLockY();
        //}
        //EditorGUILayout.EndToggleGroup();

        //dekoOffset = EditorGUILayout.BeginToggleGroup("Offset Deko-Segements", dekoOffset);
        //dekoOffsetValue = EditorGUILayout.FloatField("Offset Value", dekoOffsetValue);
        //if (GUILayout.Button("Update Offset of Deko-Segements"))
        //{
        //    EasyHill2DManager.Instance().checkDekoOffset();
        //}
        //EditorGUILayout.EndToggleGroup();


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Convert Segments to Identity"))
        {
            EasyHill2DManager.Instance().ConvertToIdentity();
        }
        if (GUILayout.Button("Align Segments Height"))
        {
            EasyHill2DManager.Instance().AlignHeight();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("<--Duplicate to Left"))
        {
            GameObject s = Selection.activeGameObject;
            if (s != null)
            {
                EasyHill2DNode nn = s.GetComponent<EasyHill2DNode>();
                if (nn != null)
                {
                    if (!nn.snapToParent)
                        EasyHill2DManager.Instance().createSegmentLeft(nn);
                    else
                        EditorUtility.DisplayDialog("EasyHills2D", "Select a Segment.", "Ok");
                }
            }
            else
                EditorUtility.DisplayDialog("EasyHills2D", "Select a Segment.", "Ok");
        }
        //if (GUILayout.Button("<-|->"))
        //{
        //    Stack<EasyHill2DNode> oldNodes = new Stack<EasyHill2DNode>();
        //    foreach (GameObject select in Selection.objects)
        //    {
        //        EasyHill2DNode node = select.GetComponent<EasyHill2DNode>();
        //        if (node != null && node.groundCollider)
        //        {
        //            EasyHill2DManager.Instance().DivideSegment(node);
        //            oldNodes.Push(node);
        //        }
        //    }
        //    Selection.activeObject = null;
        //    while (oldNodes.Count > 0)
        //        GameObject.DestroyImmediate(oldNodes.Pop().gameObject);
        //}
        if (GUILayout.Button("Duplicate to Right-->"))
        {
            GameObject s = Selection.activeGameObject;
            if (s != null)
            {
                EasyHill2DNode nn = s.GetComponent<EasyHill2DNode>();
                if (nn != null)
                {
                    if (!nn.snapToParent)
                        EasyHill2DManager.Instance().createSegmentRight(nn);
                    else
                        EditorUtility.DisplayDialog("EasyHills2D", "Select a Segment.", "Ok");
                }
            }
            else
                EditorUtility.DisplayDialog("EasyHills2D", "Select a Segment.", "Ok");
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(10);

        /*
        GUI.color = Color.green;
        if (GUILayout.Button("Restore Segments for Editing"))
        {
            EasyHill2DManager.Instance().restoreSegments();
        }
        GUI.color = Color.red;
        if (GUILayout.Button("Clean Segments for Saving and Building"))
        {
            EasyHill2DManager.Instance().CleanForSaving();
        }

        GUI.color = Color.white;
        EditorGUILayout.HelpBox("Remember to 'Clean Segments' before saving/building your scene! Otherwise your saved Scenes can get very big. Segements will be dynamically generated at level start.", MessageType.Warning);
        */
        if (GUI.changed)
        {
            saveVariables();
        }
	}

    void saveVariables()
    {
        EditorPrefs.SetBool("EasyHill2DSnap", snap);
        EditorPrefs.SetFloat("EasyHill2DSnapDistance", snapDistance);
        EditorPrefs.SetBool("EasyHill2DLockY", lockY);
        EditorPrefs.SetFloat("EasyHill2DLockYValue", lockYValue);
        EditorPrefs.SetBool("EasyHill2DdekoOffset", dekoOffset);
        EditorPrefs.SetFloat("EasyHill2DdekoOffsetValue", dekoOffsetValue);
        EasyHill2DManager.Instance().updateEditorWindowVariables();
    }

    void OnEnable()
    {
        snap = EditorPrefs.GetBool("EasyHill2DSnap", true);
        snapDistance = EditorPrefs.GetFloat("EasyHill2DSnapDistance", 2);
        lockY = EditorPrefs.GetBool("EasyHill2DLockY", true);
        lockYValue = EditorPrefs.GetFloat("EasyHill2DLockYValue", 0);
        dekoOffset = EditorPrefs.GetBool("EasyHill2DdekoOffset", true);
        dekoOffsetValue = EditorPrefs.GetFloat("EasyHill2DdekoOffsetValue", 0);
        EasyHill2DManager.Instance().updateEditorWindowVariables();
    }

    void OnDisable()
    {
        saveVariables();
    }
}

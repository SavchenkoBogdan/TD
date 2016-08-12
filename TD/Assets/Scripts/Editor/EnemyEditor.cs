using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

//[CustomEditor(typeof(EnemyController)), CanEditMultipleObjects]
public class EnemyEditor : Editor
{
    private static int currentItem = 0;
    public List<string> items = new List<string>();
    public string[] variants;
    void OnEnable()
    {
        items.Clear();
        for (int i = 0; i < ObjectsConfig.instance.enemy.transform.childCount; i++)
        {
            items.Add(ObjectsConfig.instance.enemy.transform.GetChild(i).name);
        }
        variants = new string[items.Count];
        for (int i = 0; i < variants.Length; i++)
            variants[i] = items[i];
    }

    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();
    //    DrawDefaultInspector();
    //    //if (GUILayout.Button("Update"))
    //    //{

    //    //}

    //    currentItem = EditorGUILayout.Popup("Active Enemy", currentItem, variants);
    //    foreach (var t in targets)
    //    {
    //        (t as EnemyController).SetEnemyType(variants[currentItem]);
    //        serializedObject.ApplyModifiedProperties();
    //    }
    //    if (GUI.changed)
    //        EditorUtility.SetDirty(target as EnemyController);
    //}
}

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(Bullet)), CanEditMultipleObjects]
public class BulletEditor : Editor
{
    private int currentItem = 0;
    public List<string> items = new List<string>();

    void OnEnable()
    {
        if (Application.isPlaying)
            return;
        Bullet bullet = target as Bullet;
        for (int i = 0; i < bullet.transform.childCount; i++)
        {
            if (bullet.transform.GetChild(i).gameObject.activeSelf)
            {
                currentItem = i;
                break;
            }
        }

        items.Clear();
        for (int i = 0; i < ObjectsConfig.instance.bullet.transform.childCount; i++)
        {
            items.Add(ObjectsConfig.instance.bullet.transform.GetChild(i).name);
        }

    }

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
            return;
        serializedObject.Update();
        DrawDefaultInspector();
        currentItem = EditorGUILayout.Popup("Active Bullet", currentItem, items.ToArray());
        (target as Bullet).SetType(items[currentItem]);
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
            EditorUtility.SetDirty(target as Bullet);
    }
}

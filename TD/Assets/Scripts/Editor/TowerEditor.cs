using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(Tower)), CanEditMultipleObjects]
public class TowerEditor : Editor
{
    private int currentItem = 0;
    public List<string> items = new List<string>();
    public string[] variants;
    void OnEnable()
    {
        Tower tower = target as Tower;
        //for (int i = 0; i < tower.transform.childCount; i++)
        //{
        //    if (tower.transform.GetChild(i).gameObject.activeSelf)
        //    {
        //        currentItem = i;
        //        break;
        //    }
        //}

        items.Clear();
        for (int i = 0; i < ObjectsConfig.instance.tower.transform.childCount; i++)
        {
            items.Add(ObjectsConfig.instance.tower.transform.GetChild(i).name);
        }
        var currentTowerName = "T" + tower.towerType.ToString() + tower.towerLevel.ToString();
        currentItem = items.IndexOf(currentTowerName);
    }

    public override void OnInspectorGUI()
    {
        Tower tower = target as Tower;
        serializedObject.Update();
        DrawDefaultInspector();
        currentItem = EditorGUILayout.Popup("Active Tower", currentItem, items.ToArray());
        tower.towerType = Convert.ToInt32(items[currentItem][1]) - '0';
        tower.towerLevel = Convert.ToInt32(items[currentItem][2]) - '0';
        tower.SetTowerType(items[currentItem]);
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
            EditorUtility.SetDirty(tower);
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(WaveManager)), CanEditMultipleObjects]
public class WaveManagerEditor : Editor
{

    private ReorderableList list;

    private static string[] waveIndexes;

    private static List<string> enemiesList = new List<string>();
    private static List<string> waysList = new List<string>();
    private static List<string> spawnPointsList = new List<string>();
    private static List<string> endPointsList = new List<string>();


    private bool MissingSomeElement()
    {
        return GameObject.Find("Spawn1") == null || GameObject.Find("End1") == null || GameObject.Find("Way1") == null;
    }

    private static bool canBeInspectable = false;
    private void OnEnable()
    {
        canBeInspectable = false;
        if (MissingSomeElement())
        {
            EditorUtility.DisplayDialog("Error!", "You need at least <Spawn1 | End1 | Way1> gameobjects", "Ok");
            return;
        }
        canBeInspectable = true;

        waysList.Clear();
        spawnPointsList.Clear();
        endPointsList.Clear();
        enemiesList.Clear();

        WaveManager waveManager = (target as WaveManager);
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("waves"),
                true, true, true, true);

        waveIndexes = new string[waveManager.countOfWaves];
        for (int i = 0; i < waveManager.countOfWaves; i++)
            waveIndexes[i] = System.Convert.ToString(i + 1);

        GameObject enemyPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemies/Bot.prefab", typeof(GameObject)) as GameObject;
        for (int i = 0; i < enemyPrefab.transform.childCount; i++)
            enemiesList.Add(enemyPrefab.transform.GetChild(i).name);


        for (int i = 1; ; i++)
        {
            GameObject way = GameObject.Find("Way" + i.ToString());
            if (way == null)
                break;
            waysList.Add(way.name);
        }

        for (int i = 1; ; i++)
        {
            GameObject spawnPoint = GameObject.Find("Spawn" + i.ToString());
            if (spawnPoint == null)
                break;
            spawnPointsList.Add(spawnPoint.name);
        }

        for (int i = 1; ; i++)
        {
            GameObject endPoint = GameObject.Find("End" + i.ToString());
            if (endPoint == null)
                break;
            endPointsList.Add(endPoint.name);
        }



        /* THIS GETS ALL ELEMENTS OF ONE FOLDER */
        //var guids = AssetDatabase.FindAssets("", new[] { "Assets/Prefabs/Bot" });
        //enemiesList = new string[guids.Length];
        //for (int i = 0; i < enemiesList.Length; i++)
        //    enemiesList[i] = Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guids[i]));
        /* ------------------------------------- */


        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Wave Index");
        };
        list.elementHeight = 130;

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            float standartHeight = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(new Rect(rect.x, rect.y + 3f * standartHeight, 30, standartHeight),
                System.Convert.ToString(element.FindPropertyRelative("waveIndex").intValue + 1));

            EditorGUI.LabelField(new Rect(rect.x + 30, rect.y, 100, standartHeight), "Index:");
            element.FindPropertyRelative("waveIndex").intValue = EditorGUI.Popup(new Rect(rect.x + 130, rect.y, 100, standartHeight),
                element.FindPropertyRelative("waveIndex").intValue, waveIndexes);

            EditorGUI.LabelField(new Rect(rect.x + 30, rect.y + standartHeight, 100, standartHeight), "Enemy:");
            element.FindPropertyRelative("enemy").stringValue = enemiesList[EditorGUI.Popup(new Rect(rect.x + 130, rect.y + standartHeight, 100,
                        standartHeight), enemiesList.IndexOf(element.FindPropertyRelative("enemy").stringValue), enemiesList.ToArray())];

            EditorGUI.LabelField(new Rect(rect.x + 30, rect.y + 2 * standartHeight, 100, standartHeight), "Count:");
            EditorGUI.PropertyField(new Rect(rect.x + 130, rect.y + 2 * standartHeight, 100, standartHeight),
                element.FindPropertyRelative("count"), GUIContent.none);

            EditorGUI.LabelField(new Rect(rect.x + 30, rect.y + 3 * standartHeight, 100, standartHeight), "Ways:");
            element.FindPropertyRelative("way").stringValue = waysList[EditorGUI.Popup(new Rect(rect.x + 130, rect.y + 3 * standartHeight,
                100, standartHeight), waysList.IndexOf(element.FindPropertyRelative("way").stringValue), waysList.ToArray())];

            EditorGUI.LabelField(new Rect(rect.x + 30, rect.y + 4 * standartHeight, 100, standartHeight), "Spawn Point:");
            element.FindPropertyRelative("spawnPoint").stringValue = spawnPointsList[EditorGUI.Popup(new Rect(rect.x + 130, rect.y + 4 * standartHeight,
                100, standartHeight), spawnPointsList.IndexOf(element.FindPropertyRelative("spawnPoint").stringValue), spawnPointsList.ToArray())];

            EditorGUI.LabelField(new Rect(rect.x + 30, rect.y + 5 * standartHeight, 100, standartHeight), "End Point:");
            element.FindPropertyRelative("endPoint").stringValue = endPointsList[EditorGUI.Popup(new Rect(rect.x + 130, rect.y + 5 * standartHeight,
                    100, standartHeight), endPointsList.IndexOf(element.FindPropertyRelative("endPoint").stringValue), endPointsList.ToArray())];

            EditorGUI.LabelField(new Rect(rect.x + 30, rect.y + 6 * standartHeight, 100, standartHeight), "Delay:");
            EditorGUI.PropertyField(new Rect(rect.x + 130, rect.y + 6 * standartHeight, 100, standartHeight),
                element.FindPropertyRelative("delay"), GUIContent.none);
        };

        list.onSelectCallback = (ReorderableList l) => {

            string currentEnemyName = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("enemy").stringValue;
            GameObject currentEnemyPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemies/Bot.prefab", typeof(GameObject)) as GameObject;

            for (int i = 0; i < currentEnemyPrefab.transform.childCount; i++)
            {
                if (currentEnemyPrefab.transform.GetChild(i).name == currentEnemyName)
                    EditorGUIUtility.PingObject(currentEnemyPrefab.transform.GetChild(i).gameObject);
            }
        };
        list.onCanRemoveCallback = (ReorderableList l) => {
            return l.count > 1;
        };
        list.onRemoveCallback = (ReorderableList l) => {
            if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to delete the wave?", "Yes", "No"))
            {
                Debug.Log(l.index);
                if (l.index != -1)
                {
                    //enemiesIndexes.Remove(enemiesIndexes[l.index]);
                    //endPointsIndexes.Remove(endPointsIndexes[l.index]);
                    //spawnPointsIndexes.Remove(spawnPointsIndexes[l.index]);
                    //waysIndexes.Remove(waysIndexes[l.index]);
                }
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            }
        };
        list.onAddCallback = (ReorderableList l) => {
            var index = l.serializedProperty.arraySize;
            l.serializedProperty.arraySize++;
            l.index = index;
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("waveIndex").intValue = 0;
            element.FindPropertyRelative("count").intValue = _defaultCount;
            element.FindPropertyRelative("enemy").stringValue = enemiesList[_defaultEnemyIndex];
            element.FindPropertyRelative("spawnPoint").stringValue = spawnPointsList[_defaultSpawnPointIndex];
            element.FindPropertyRelative("endPoint").stringValue = endPointsList[_defaultEndPointIndex];
            element.FindPropertyRelative("way").stringValue = waysList[_defaultWayIndex];
            element.FindPropertyRelative("delay").floatValue = _defaultDelay;

        };
    }


    private static int _defaultEnemyIndex = 0;
    private static int _defaultWaveIndex = 0;
    private static int _defaultWayIndex = 0;
    private static int _defaultSpawnPointIndex = 0;
    private static int _defaultEndPointIndex = 0;
    private static float _defaultDelay = 0;
    private static int _defaultCount = 0;
    private static int _currentLevel;

    public override void OnInspectorGUI()
    {
        if (!canBeInspectable)
            return;
        WaveManager waveManager = target as WaveManager;
        serializedObject.Update();
        waveManager.countOfWaves = EditorGUILayout.IntField("Count of waves: ", waveManager.countOfWaves);
        _defaultCount = EditorGUILayout.IntField("Default count: ", waveManager.defaultCount);
        _defaultDelay = EditorGUILayout.FloatField("Default delay: ", waveManager.defaultDelay);
        _defaultWaveIndex = EditorGUILayout.Popup("Default index: ", _defaultWaveIndex, waveIndexes);
        _defaultEnemyIndex = EditorGUILayout.Popup("Default enemy: ", _defaultEnemyIndex, enemiesList.ToArray());
        _defaultWayIndex = EditorGUILayout.Popup("Default way: ", _defaultWayIndex, waysList.ToArray());
        _defaultSpawnPointIndex = EditorGUILayout.Popup("Default spawn: ", _defaultSpawnPointIndex, spawnPointsList.ToArray());
        _defaultEndPointIndex = EditorGUILayout.Popup("Default end: ", _defaultEndPointIndex, endPointsList.ToArray());
        _currentLevel = EditorGUILayout.IntField("Current level: ", _currentLevel);
        waveManager.defaultEnemy = enemiesList[_defaultEnemyIndex];

        if (GUI.changed)
        {
            waveIndexes = new string[waveManager.countOfWaves];
            for (int i = 0; i < waveManager.countOfWaves; i++)
                waveIndexes[i] = System.Convert.ToString(i + 1);
        }
        if (GUILayout.Button("Set all to DEFAULT"))
        {
            if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to reset all data to default?", "Yes", "No"))
            {
                foreach (var wave in waveManager.waves)
                {
                    wave.count = waveManager.defaultCount;
                    wave.enemy = enemiesList[_defaultEnemyIndex];
                    wave.waveIndex = _defaultWaveIndex;
                    wave.delay = _defaultDelay;
                    wave.way = waysList[_defaultWayIndex];
                    wave.spawnPoint = spawnPointsList[_defaultSpawnPointIndex];
                    wave.endPoint = endPointsList[_defaultEndPointIndex];
                }
            }
        }
        if (GUILayout.Button("Sort all by waves indexes"))
        {
            waveManager.waves = waveManager.waves.OrderBy(item => item.waveIndex).ToList();
            //WaveManager.waves.Sort((a,b)=> {});   
        }
        list.DoLayoutList();
        if (GUILayout.Button("Clear ALL!"))
        {
            if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to clear all data?", "Yes", "No"))
            {
                waveManager.waves.Clear();
            }
        }
        if (GUILayout.Button("Save to file"))
        {
            if (EditorUtility.DisplayDialog("Warning!",
                "Save data? LEVEL = " + _currentLevel.ToString(), "Yes", "No"))
            {
                SaveToFile();
            }
        }
        if (GUILayout.Button("Load from file"))
        {
            LoadFromFile();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void SaveToFile()
    {
        WaveManagerSaver.Level level = new WaveManagerSaver.Level();
        level.waves = (target as WaveManager).waves;
        level.name = _currentLevel.ToString();

        var path = Path.Combine(Application.dataPath, "waveManager.xml");
        if (!File.Exists(path))
        {
            WaveManagerSaver saver = new WaveManagerSaver();
            saver.levels.Add(level);
            saver.Save(Path.Combine(Application.dataPath, "waveManager.xml"));
            return;

        }

        WaveManagerSaver loader = WaveManagerSaver.Load(Path.Combine(Application.dataPath, "waveManager.xml"));
        bool alreadyExists = false;
        for (int i = 0; i < loader.levels.Count; i++)
        {
            if (loader.levels[i].name == level.name)
            {
                loader.levels[i] = level;
                alreadyExists = true;
            }
        }
        if (!alreadyExists)
            loader.levels.Add(level);
        loader.Save(Path.Combine(Application.dataPath, "waveManager.xml"));
        //WaveManagerSaver saver = new WaveManagerSaver();
        //saver.levels.Add(level);
        //saver.Save(Path.Combine(Application.dataPath, "waveManager.xml"));

        //saver.waves = (target as WaveManager).waves;
        //saver.Save(Path.Combine(Application.dataPath, "waveManager.xml"));
        //Debug.Log(saver.waves.Count);

        //StreamWriter writer = new StreamWriter(Application.dataPath);
        //writer.Write(XML.Serialize<WaveManager.MiniWave>((target as WaveManager).waves));
        //writer.Close();
    }

    private void LoadFromFile()
    {
        //(target as WaveManager).waves.Clear();
        //list.onRemoveCallback(list);

        var path = Path.Combine(Application.dataPath, "waveManager.xml");
        if (!File.Exists(path))
        {
            EditorUtility.DisplayDialog("Warning!", "File doesn't exist, create one", "Ok");
            return;
        }

        WaveManagerSaver loader = WaveManagerSaver.Load(Path.Combine(Application.dataPath, "waveManager.xml"));

        for (int i = 0; i < loader.levels.Count; i++)
        {
            if (loader.levels[i].name == _currentLevel.ToString())
            {
                
                List<WaveManager.MiniWave> waves = loader.levels[i].waves;
                Debug.Log(string.Format("Count of object = {0}", (target as WaveManager).waves.Count));
                for (int j = 0; j < (target as WaveManager).waves.Count; j++)
                    ReorderableList.defaultBehaviours.DoRemoveButton(list);

                (target as WaveManager).waves.Clear();
                Debug.Log(string.Format("New Count of object = {0}", (target as WaveManager).waves.Count));

                //list.onRemoveCallback(list)

                (target as WaveManager).waves = waves;
                for (int j = 0; j < waves.Count; j++)
                    list.onAddCallback(list);
                Debug.Log(string.Format("New NEW Count of object = {0}", (target as WaveManager).waves.Count));

                break;
            }

        }




    }
}
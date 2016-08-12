using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System;

public class WavesConfig : ScriptableObject {

#if UNITY_EDITOR
    [MenuItem("Tools/WavesConfig")]
    public static void Edit()
    {
        Selection.activeObject = WavesConfig.instance;
    }
#endif

    private const string ISNSettingsAssetName = "WavesConfigSettings";
    private const string ISNSettingsPath = "Resources";
    private const string ISNSettingsAssetExtension = ".asset";

    private static WavesConfig _instance = null;

    public static WavesConfig instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load(ISNSettingsAssetName) as WavesConfig;

                if (_instance == null)
                {
                    _instance = CreateInstance<WavesConfig>();
#if UNITY_EDITOR
                    string fullPath = Path.Combine(Path.Combine("Assets", ISNSettingsPath),
                                                   ISNSettingsAssetName + ISNSettingsAssetExtension
                                                   );
                    AssetDatabase.CreateAsset(_instance, fullPath);
#endif
                }
            }
            return _instance;
        }
    }

    //[Serializable]
    //public class WaveObject
    //{
    //    public GameObject spawnPoint;
    //    public GameObject endPoint;
    //    public GameObject enemy;
    //    public int count;
    //    public float delay;
    //}

    //public WaveObject[] waves;


}

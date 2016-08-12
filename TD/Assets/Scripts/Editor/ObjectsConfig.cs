using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class ObjectsConfig : ScriptableObject
{
#if UNITY_EDITOR
    [MenuItem("Tools/ObjectsConfig")]
    public static void Edit()
    {
        Selection.activeObject = ObjectsConfig.instance;
    }
#endif

    private const string ISNSettingsAssetName = "ObjectsConfigSettings";
    private const string ISNSettingsPath = "Resources";
    private const string ISNSettingsAssetExtension = ".asset";

    private static ObjectsConfig _instance = null;

    public static ObjectsConfig instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load(ISNSettingsAssetName) as ObjectsConfig;

                if (_instance == null)
                {
                    _instance = CreateInstance<ObjectsConfig>();
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
    [SerializeField]
    public GameObject tower;
    [SerializeField]
    public GameObject bullet;
    [SerializeField]
    public GameObject enemy;



}


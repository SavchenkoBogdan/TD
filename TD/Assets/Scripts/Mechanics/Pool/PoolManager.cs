using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

    public static PoolManager Instance { get; set; }

    [SerializeField]
    private GameObject poolStorage;
    [SerializeField]
    public GameObject enemy;
    [SerializeField]
    public GameObject bullet;

    //public static Stack <GameObject> bullets = new Stack <GameObject>();
    //public static Stack <GameObject> enemies = new Stack <GameObject>();

    public static List<GameObject> bullets = new List<GameObject>();
    public static List<GameObject> enemies = new List<GameObject>();

    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        PreSetup();
    }

    GameObject CreateUnit(GameObject unit)
    {
        GameObject poolUnit = Instantiate(unit);
        poolUnit.transform.SetParent(poolStorage.transform, false);
        poolUnit.SetActive(false);
        return poolUnit;
    }

    void PreSetup()
    {
        for (int i = 0; i < 20; i++)
            enemies.Add(CreateUnit(enemy));
        for (int i = 0; i < 20; i++)
            bullets.Add(CreateUnit(bullet));
    }

    public GameObject GetEnemy()
    {
        if (enemies.Count > 0)
        {
            GameObject tmpGameObject = enemies[enemies.Count - 1];
            enemies.RemoveAt(enemies.Count - 1);
            return tmpGameObject;
        }
        //return enemies.Pop();
        return CreateUnit(enemy);
    }

    public GameObject GetBullet()
    {
        if (bullets.Count > 0)
        {
            GameObject tmpGameObject = bullets[bullets.Count - 1];
            bullets.RemoveAt(bullets.Count - 1);
            return tmpGameObject;
        }
            //return bullets.Pop();
        return CreateUnit(bullet);
    }

   
	
	void Update () {

	}

    private void SetToStorage(GameObject unit)
    {
        unit.transform.SetParent(poolStorage.transform, false);
        unit.SetActive(false);
    }
    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
        SetToStorage(enemy);
    }

    public void AddBullet(GameObject bullet)
    {
        bullets.Add(bullet);
        SetToStorage(bullet);
    }
}






//public static PoolManager instance;

///* Prefabs */
//[Serializable]
//public struct PoolObject
//{
//    public string name;
//    public GameObject prefab;
//}

//[Serializable]
//public struct TowerObject
//{
//    public string name;
//    public GameObject tower;
//    public GameObject bullet;
//}


//[SerializeField]
//private PoolObject[] bulletsPrefabs;

//[SerializeField]
//private PoolObject[] enemiesPrefabs;

////[SerializeField]
////private PoolObject[] towersPrefabs;

//private Dictionary<string, GameObject> bulletsDictionary = new Dictionary<string, GameObject>();
//private Dictionary<string, GameObject> enemiesDictionary = new Dictionary<string, GameObject>();
//private Dictionary<string, GameObject> towersDictionary = new Dictionary<string, GameObject>();

//public static Stack<GameObject> bullets = new Stack<GameObject>();
//public static Stack<GameObject> enemies = new Stack<GameObject>();
//public static Stack<GameObject> towers = new Stack<GameObject>();


//[SerializeField]
//private GameObject[] enemiezPrefabs;

//void OnEnable()
//{
//    DontDestroyOnLoad(gameObject);
//    instance = this;
//    for (int i = 0; i < bulletsPrefabs.Length; i++)
//    {
//        bulletsDictionary[bulletsPrefabs[i].name] = bulletsPrefabs[i].prefab;
//    }

//    for (int i = 0; i < enemiesPrefabs.Length; i++)
//    {
//        enemiesDictionary[enemiesPrefabs[i].name] = enemiesPrefabs[i].prefab;
//    }

//    //for (int i = 0; i < towersPrefabs.Length; i++)
//    //{
//    //towersDictionary[towersPrefabs[i].name] = towersPrefabs[i].prefab;
//    //}
//}

//public GameObject GetBullet(string type)
//{
//    if (bullets.Count == 0)
//        return Instantiate(bulletsDictionary[type]);
//    return null;
//}


    // NEW POOOOOL
//GameObject CreateUnit(GameObject unit)
//{
//    GameObject poolUnit = Instantiate(unit);
//    poolUnit.transform.SetParent(poolStorage.transform, false);
//    poolUnit.SetActive(false);
//    return poolUnit;
//}

//void PreSetup()
//{
//    for (int i = 0; i < enemies.Count; i++)
//    {
//        for (int j = 0; j < 10; j++)
//            enemies.Push(CreateUnit(enemiesPrefabs[i]));
//    }
//}
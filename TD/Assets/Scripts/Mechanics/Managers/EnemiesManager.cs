using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class EnemiesManager: MonoBehaviour
{
    public static EnemiesManager Instance { get; private set; }
    //public List<GameObject> enemies = new List<GameObject>();
    public List<BaseEnemyController> enemies = new List<BaseEnemyController>();

    //private int gameCounter = 0;

 
    void OnEnable()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //gameCounter = 0;
        //WaveManager.Instance.StartWave();
    }


    //internal void SpawnEnemy(GameObject enemy, GameObject spawnPoint, GameObject endPoint, GameObject[] checkPoints)
    //{
    //    GameObject go = PoolManager.Instance.GetEnemy();
    //    go.transform.SetParent(GameObject.Find("Game_Objects_Storage").transform, false);
    //    go.SetActive(true);
    //    go.GetComponent<IBaseEnemyController>().InitPath(spawnPoint, endPoint, checkPoints);
    //    //go.transform.position = spawnPoint.transform.position;

    //}

    void Update()
    {

    }

    public void SpawnEnemy(GameObject enemyPrefab, string enemy, GameObject spawn, GameObject end, GameObject[] checkPoints)
    {
        GameObject go = PoolManager.Instance.GetEnemy();
        for (int i = 0; i < go.transform.childCount; i++)
        {
            GameObject obj = go.transform.GetChild(i).gameObject;
            obj.SetActive(obj.name == enemy);
            if (obj.name == enemy)
            {
                go.transform.SetParent(GameObject.Find("Game_Objects_Storage").transform, false);
                go.SetActive(true);
                BaseEnemyController enemyController = obj.GetComponent<BaseEnemyController>();
                enemyController.InitPath(checkPoints, spawn, end);
                enemyController.OnDeath += OnEnemyDeath;
                enemies.Add(enemyController);
            }
        //go.transform.GetChild(i).gameObject.SetActive(go.transform.GetChild(i).name == enemy);
        }
        //go.transform.SetParent(GameObject.Find("Game_Objects_Storage").transform, false);
        //go.SetActive(true);
        //BaseEnemyController enemyController = go.GetComponent<BaseEnemyController>();
        //enemyController.InitPath(checkPoints, spawn, end);
        //enemies.Add(enemyController);
    }

    private void OnEnemyDeath(BaseEnemyController enemy)
    {
        enemy.OnDeath -= OnEnemyDeath;
        enemies.Remove(enemy);
    }
}


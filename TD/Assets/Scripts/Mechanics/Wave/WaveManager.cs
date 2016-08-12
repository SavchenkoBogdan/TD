using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

//using UnityEditor;

public class WaveManager : MonoBehaviour
{

    [Serializable]
    public class MiniWave
    {
        public int waveIndex;
        public string enemy;
        public int count;
        public string way;
        public string spawnPoint;
        public string endPoint;
        public float delay;
    }

    public List<MiniWave> waves = new List<MiniWave>();
    public int countOfWaves;
    public int defaultCount;
    public float defaultDelay;
    public string defaultEnemy;

    public int gameHealth = 10;

    #region private variables

    public int currentWave;
    private float counter;
    //private int delayTime;

    #endregion

    private static WaveManager instance;
    public static WaveManager Instance;

    void OnEnable()
    {
        //counterWaveTmp = 0;
        Instance = this;
        currentWave = 0;
        //delayTime = 5;
        //SpawnWave()/*;*/
        //EventAggregator.OnBotReachedFinishEvent += OnBotReachedFinishEvent;
    }

    private void OnBotReachedFinishEvent()
    {
        gameHealth--;
    }

    public void StartWave()
   { 
        SpawnWave();
   }

    //private IEnumerator SpawnWavePart(WaveObject wave)
    //{
    //    int countOfEnemies = 0;
    //    while (countOfEnemies < wave.count)
    //    {
    //        GameObject[] checkPoints = new GameObject[wave.checkPointBlock.transform.childCount];
    //        for (int i = 0; i < wave.checkPointBlock.transform.childCount; i++)
    //            checkPoints[i] = wave.checkPointBlock.transform.GetChild(i).gameObject;

    //        EnemiesManager.Instance.SpawnEnemy(wave.enemy, wave.spawnPoint, wave.endPoint, checkPoints);
    //        countOfEnemies++;
    //        yield return new WaitForSeconds(wave.delay);
    //    }
    //    yield break;
    //}

    private IEnumerator SpawnWavePart(MiniWave miniWave)
    {

        int countOfEnemies = 0;
        GameObject way = GameObject.Find(miniWave.way);
        GameObject[]checkPoints = new GameObject[way.transform.childCount];
        for (int i = 0; i < way.transform.childCount; i++)
        {
            checkPoints[i] = way.transform.GetChild(i).gameObject;
        }
        GameObject spawn = GameObject.Find(miniWave.spawnPoint);
        GameObject end = GameObject.Find(miniWave.endPoint);

        GameObject enemy = PoolManager.Instance.enemy;
//#if !UNITY_EDITOR
//        GameObject enemy = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemies/Bot.prefab", typeof (GameObject)) as GameObject;
//#endif
        while (countOfEnemies < miniWave.count)
        {
            EnemiesManager.Instance.SpawnEnemy(enemy, miniWave.enemy, spawn, end, checkPoints);
            countOfEnemies++;
            yield return new WaitForSeconds(miniWave.delay);
        }
        yield break;
        
    }

    private void SpawnWave()
    {
        foreach (var wave in waves.Where(wave => wave.waveIndex == currentWave))
            StartCoroutine(SpawnWavePart(wave));
        //foreach (var wave in waves)
        //    if (wave.waveIndex == currentWave)
        //        StartCoroutine(SpawnWavePart(wave));
        currentWave++;
    }

    //private float counterWaveTmp = 0;

    void Update()
    {
        //counterWaveTmp += Time.deltaTime;
        //if (counterWaveTmp > 5)
        //{
        //    SpawnWave();
        //    counterWaveTmp = 0;
        //}

if (Input.GetKeyDown(KeyCode.Space))
            SpawnWave();
    }

}
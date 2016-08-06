using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PoolObject : MonoBehaviour {

    [SerializeField]
    public GameObject parent;
    [SerializeField]
    public GameObject prefab;
    [SerializeField]
    public int poolLength = 10;
    //private Stack<GameObject> pool;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

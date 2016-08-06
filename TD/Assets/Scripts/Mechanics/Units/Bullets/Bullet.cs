using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour
{
    public string bulletType;
    public BaseBulletController currentBullet;

    public void SetType(string bulletName)
    {
        bulletType = bulletName;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            bool goActive = child.name == bulletName;
            child.gameObject.SetActive(goActive);
            if (goActive)
            {
                currentBullet = child.gameObject.GetComponent<BaseBulletController>();
            }
        }
    }

    void Start()
    {

    }

    void Update()
    {


    }

    public void Init(string bulletName, BaseEnemyController enemy, Transform towerTransform)
    {
        SetType("B" + bulletName);
        //Debug.Log(string.Format("{0}, {1}, {2},",bulletName, enemy, transform));
        currentBullet.Init(enemy, towerTransform, transform);
    }
}

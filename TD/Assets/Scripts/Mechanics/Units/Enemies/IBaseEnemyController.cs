using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IBaseEnemyController
{
    void TakeDamage(float damage);
    void Die();
    void Move();
    void UseSkill();
    void InitPath(GameObject[] checkPoints, GameObject spawnPoint, GameObject endPoint);
}


using UnityEngine;
using System.Collections;

public interface IBaseBulletController
{
    void Init(BaseEnemyController enemy, Transform towerTransform, Transform parentTransform);
    void OnTargetReached();
    void OnEnemyDeath(BaseEnemyController enemy);
    void Die();
}

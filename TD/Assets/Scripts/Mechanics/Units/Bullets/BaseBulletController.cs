using UnityEngine;
using System.Collections;
using System;

public class BaseBulletController : MonoBehaviour, IBaseBulletController
{
    [SerializeField] private float _speed;
    [SerializeField] protected float _damage;

    protected BaseEnemyController _enemy;
    protected Transform _parentTransform;
    protected Vector3 _oldEnemyPosition;
    protected bool _enemyIsAlive;

    private void OnEnable()
    {

    }

    public void Init(BaseEnemyController enemy, Transform towerTransform, Transform parentTransform)
    {
        _enemy = enemy;
        _enemy.OnDeath += OnEnemyDeath;
        _enemyIsAlive = true;
        _parentTransform = parentTransform;
        _parentTransform.position = towerTransform.position;
    }


    public void Update()
    {
        if (_enemyIsAlive)
            _oldEnemyPosition = _enemy.transform.position;
        Vector3 vectorToTarget = _oldEnemyPosition - _parentTransform.position;
        Vector3 directionVector = vectorToTarget.normalized;
        float angle = Mathf.Atan2(directionVector.y, directionVector.x);
        _parentTransform.position += directionVector * _speed * Time.deltaTime;
        _parentTransform.localRotation = Quaternion.Euler(0, 0, angle * 180f / Mathf.PI);
        if (vectorToTarget.magnitude < 0.25)
        {
            OnTargetReached();
            Die();
        }
    }

    public virtual void OnTargetReached()
    {
        if (_enemyIsAlive)
            _enemy.TakeDamage(_damage);
    }

    public void Die()
    {
        _parentTransform.gameObject.SetActive(false);
        if (PoolManager.Instance)
            PoolManager.Instance.AddBullet(_parentTransform.gameObject);
    }

    public void OnEnemyDeath(BaseEnemyController enemy)
    {
        _enemy.OnDeath -= OnEnemyDeath;
        _enemyIsAlive = false;
    }

 

}

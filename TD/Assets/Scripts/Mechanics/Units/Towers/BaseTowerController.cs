using UnityEngine;
using System.Collections;
using System;

public class BaseTowerController : MonoBehaviour, ITowerController
{

    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    public float damage;
    [SerializeField]
    public float attackRange;
    [SerializeField]
    public float cooldown;

    [SerializeField]
    public bool isStatic;

    [HideInInspector]
    protected bool canShoot;
    [HideInInspector]
    protected float cooldownCounter;

    public string bulletType;
    protected BaseEnemyController target;
    public TargetType currentTargetType = TargetType.Closest;


    protected Transform _parentTransform;
    private float towerAngle;
    public enum TargetType
    {
        Closest,
        Fastest,
        Toughest
    }

    public virtual void OnEnable()
    {
        canShoot = false;
        cooldownCounter = 0;
        //bulletPrefab = ObjectsConfig.instance.bullet;
        currentTargetType = TargetType.Closest;
        _parentTransform = transform.parent != null ? transform.parent.transform : transform as Transform;
        _parentTransform = transform;
        //bulletPrefab = PoolManager.Instance.bullet;
    }

    public virtual void Update()
    {
        if (target != null)
        {
            var dx = target.parentTransform.position.x - _parentTransform.position.x;
            var dy = target.parentTransform.position.y - _parentTransform.position.y;
            towerAngle = Mathf.Atan2(dy, dx);
            //_parentTransform.localRotation = Quaternion.Euler(0f, 0f, towerAngle * 180f / Mathf.PI);
        }
        if (!isStatic)
            _parentTransform.localRotation = Quaternion.Euler(0f, 0f, towerAngle * 180f / Mathf.PI);
        
        if (canShoot)
        {
            if (target != null)
            {
                CheckIfTargetAlive(Shoot);
            }
            else
                FindTarget();
        }
        else
        {
            cooldownCounter += Time.deltaTime;
            if (cooldownCounter >= cooldown)
            {
                canShoot = true;
            }
        }
    }

    private void CheckIfTargetAlive(Action<BaseEnemyController> shoot)
    {
        if (target.gameObject.activeInHierarchy)
        {
            shoot(target);
        }
        else
            target = null;
    }

    public virtual void Shoot(BaseEnemyController enemy)
    {

    }

    public void FindTarget()
    {
        switch(currentTargetType)
        {
            case TargetType.Closest:
                FindClosestTarget();
                break;
            case TargetType.Fastest:
                FindFastestTarget();
                break;
            case TargetType.Toughest:
                FindToughestTarget();
                break;
        }
        //foreach (var enemy in EnemiesManager.Instance.enemies)
        //{
        //    if (!enemy.activeSelf)
        //        continue;
        //    float distance = (enemy.transform.position - transform.position).magnitude;
        //    if (distance < attackRange)
        //    {
        //        Shoot(enemy);
        //        return;
        //    }
        //}
    }

    

    public virtual void FindClosestTarget()
    {
        int closestEnemyIndex = -1;
        float closestEnemyDistance = int.MaxValue;
        for (int i = 0; i < EnemiesManager.Instance.enemies.Count; i++)
        {
            var enemy = EnemiesManager.Instance.enemies[i];
            if (enemy == null)
                continue;
            var enemyTransform = enemy.parentTransform;
            if (!enemy.gameObject.activeSelf)
                continue;
            //float dx = enemy.transform.position.x - transform.position.x;
            //float dy = enemy.transform.position.y - transform.position.y;
            float dx = enemyTransform.position.x - _parentTransform.position.x;
            float dy = enemyTransform.position.y - _parentTransform.position.y;
            //Debug.Log(dx + " : " +  dy);

            float dist = Mathf.Sqrt(dx * dx + dy * dy);
            //float distance = (enemy.transform.position - transform.position).magnitude;
            //Debug.Log(distance == dist);
            if (dist < closestEnemyDistance)
            {
                closestEnemyIndex = i;
                closestEnemyDistance = dist;
            }
        }
        if (closestEnemyDistance < attackRange)
        {
            //Debug.Log(closestEnemyDistance);
            target = EnemiesManager.Instance.enemies[closestEnemyIndex];
        }
    }

    private void FindFastestTarget()
    {

    }

    private void FindToughestTarget()
    {
        int toughestEnemyIndex = -1;
        float toughestEnemyHealth = 0;
        for (int i = 0; i < EnemiesManager.Instance.enemies.Count; i++)
        {
            BaseEnemyController enemy = EnemiesManager.Instance.enemies[i];
            if (!enemy.gameObject.activeSelf)
                continue;
           
            float distance = (enemy.transform.position - transform.position).magnitude;
            if (distance < attackRange && enemy.health > toughestEnemyHealth)
            { 
                toughestEnemyHealth = enemy.health;
                toughestEnemyIndex = i;
            }
        }

        if (toughestEnemyIndex != -1)
        {
            target = EnemiesManager.Instance.enemies[toughestEnemyIndex];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;

public class BaseEnemyController : MonoBehaviour, IBaseEnemyController
{
    #region serialized variables

    [SerializeField] private float originalHealth;

    [SerializeField] private float originalMoveSpeed;

    public bool canControllItself;

    private float _health;
    [SerializeField]
    private float _moveSpeed;

    public float health
    {
        get { return _health; }
        set { _health = value; }
    }

    public float moveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    public Transform parentTransform;
    #endregion

    #region private variables

    public DebuffController debuffController;
    private bool isMoving;
    private int currentCheckPointIndex;
    private List<GameObject> checkPointsList = new List<GameObject>();

    public event Action<BaseEnemyController> OnDeath = delegate { }; 
    public event Action OnFinishReached = delegate { }; 

    #endregion

    private void OnEnable()
    {
        debuffController = GetComponent<DebuffController>();
        debuffController.Init(this);
        health = originalHealth;
        moveSpeed = originalMoveSpeed;
        isMoving = true;
        currentCheckPointIndex = 0;
        canControllItself = true;
    }

    private void Update()
    {
        if (!isMoving)
        {
            OnFinishReached();
            //EventAggregator.UnitReachedFinish.Publish();
            Die();
            return;
        }
        if (canControllItself)
            Move();
    }

    private float bufferAngle;
    public void Move()
    {
        Transform currentCheckPoint = checkPointsList[currentCheckPointIndex].transform;
        float dx = currentCheckPoint.position.x - transform.position.x;
        float dy = currentCheckPoint.position.y - transform.position.y;
        float distance = Mathf.Sqrt(dx * dx + dy * dy);
        float angle = Mathf.Atan2(dy, dx);
        if (angle <= -Math.PI * 0.5f || angle >= Mathf.PI * 0.5f)
        {
            parentTransform.localScale = new Vector3(parentTransform.localScale.x, -Mathf.Abs(parentTransform.localScale.y), 1);
        }
        else
        {
            parentTransform.localScale = new Vector3(parentTransform.localScale.x, Mathf.Abs(parentTransform.localScale.y), 1);
        }
        //parentTransform.Rotate(Vector3.forward * (angle * 180f / (float)Math.PI) * 0.1f);

        Vector3 directionVector = new Vector3(dx, dy).normalized;
        parentTransform.localRotation = Quaternion.Euler(0f, 0f, angle * 180f / Mathf.PI);

        parentTransform.position += directionVector * (moveSpeed * Time.deltaTime);
        if (distance > 0.1f)
            return;
        currentCheckPointIndex++;
        isMoving = currentCheckPointIndex != checkPointsList.Count;
    }

    public void UseSkill()
    {

    }

    public void Die()
    {
        parentTransform.gameObject.SetActive(false);
        if (PoolManager.Instance)
            PoolManager.Instance.AddEnemy(transform.parent.gameObject);
        if (OnDeath != null)
            OnDeath.Invoke(this);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    public void InitPath(GameObject[] checkPoints, GameObject spawnPoint, GameObject endPoint)
    {
        checkPointsList.Clear();
        checkPointsList.AddRange(checkPoints);
        checkPointsList.Add(endPoint);
        parentTransform = transform.parent ? transform.parent.transform : transform;
        parentTransform.position = spawnPoint.transform.position;
        parentTransform.gameObject.SetActive(true);
    }

}


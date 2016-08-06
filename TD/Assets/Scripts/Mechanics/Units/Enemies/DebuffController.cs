using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DebuffController :MonoBehaviour
{
    private BaseEnemyController _owner;

    public bool isSlowed;
    public bool isPoisoned;
    //private float slowCoof;

    //public class Something
    //{
    //    public string id;
    //    public string name;

    //    public void DoSomething()   { }
    //}

    //public void Update ()
    //{
    //    List<Something> list = new List<Something>();
    //    foreach (Something t in list.Where(t => t != null && t.id != string.Empty && t.name == "Pasha"))
    //    {
    //        t.DoSomething();
    //    }
    //}

    public void Refresh()
    {
        
    }

    public void Init(BaseEnemyController owner)
    {
        _owner = owner;
        isPoisoned = false;
        isSlowed = false;
    }

    public void SetSlow(float slowCoof, float slowTime)
    {
        if (!_owner.gameObject.activeSelf || isSlowed)
            return;
        StartCoroutine(SlowDebuff(slowCoof, slowTime));
    }

    public void SetPoison(float damagePerSecond, float debuffTime)
    {
        if (!_owner.gameObject.activeSelf || isPoisoned)
            return;
        StartCoroutine(DamageDebuff(damagePerSecond, debuffTime));
    }

    private IEnumerator SlowDebuff(float slowCoof, float debuffTime)
    {
        isSlowed = true;
        float originalSpeed = _owner.moveSpeed;
        Debug.Log(originalSpeed);
        _owner.moveSpeed *= slowCoof;
        Debug.Log(_owner.moveSpeed);
        yield return new WaitForSeconds(debuffTime);
        if (!_owner.gameObject.activeSelf)
            yield break;
        _owner.moveSpeed = originalSpeed;
        isSlowed = false;
    }

    private IEnumerator DamageDebuff(float damagePerSecond, float debuffTime)
    {
        isPoisoned = true;
        float damagePerFrame = damagePerSecond / debuffTime;
        float timeElapsed = 0;
        while (timeElapsed < debuffTime)
        {
            if (!_owner.gameObject.activeSelf)
                yield break;
            timeElapsed += Time.deltaTime;
            _owner.TakeDamage(damagePerSecond * Time.deltaTime);
            yield return 0;
        }
        isPoisoned = false;
    }
}

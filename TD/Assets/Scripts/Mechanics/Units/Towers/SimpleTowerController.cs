using UnityEngine;
using System.Collections;

public class SimpleTowerController : BaseTowerController {

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Update()
    {
        base.Update();
    }

    


    public override void Shoot(BaseEnemyController enemy)
    {
        base.Shoot(enemy);
        cooldownCounter = 0f;
        canShoot = false;
        GameObject bullet = PoolManager.Instance.GetBullet();
        bullet.transform.SetParent(GameObject.Find("Game_Objects_Storage").transform, false);
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().Init(bulletType, enemy, transform);
        //bullet.GetComponent<IBaseBulletController>().Init(enemy, transform);
    }
}

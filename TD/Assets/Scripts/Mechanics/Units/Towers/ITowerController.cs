using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface ITowerController
{
    void Shoot(BaseEnemyController enemy);
    void FindTarget();
}


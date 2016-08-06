using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class WaveDataBase : ScriptableObject
{
    [SerializeField]
    private List<string> database;

    void OnEnable()
    {
        if (database == null)
            database = new List<string> ();
    }


}


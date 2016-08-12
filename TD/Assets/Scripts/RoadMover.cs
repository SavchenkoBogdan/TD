using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class RoadMover : MonoBehaviour
{
    [System.Serializable]
    public class Pack
    {
        public List<RoadBuilder> roads;
    }

    public List<Pack> packs;

    //public List<RoadBuilder> roads;
    [Range(0f, 1f)]
    public float percent = 0;

    public void Update()
    {
        var sum = (float)packs.Count;
        var neededValue = sum * percent;

        for (int i = 0; i < packs.Count; i++)
        {
            foreach(var road in packs[i].roads)
                road.percent = 0;
        }


        for (int i = 0; i < packs.Count && neededValue >= 0f; i++)
        {
            var dif = neededValue >= 1f ? 1f : neededValue;
            neededValue -= dif;
            foreach (var road in packs[i].roads)
                road.percent = dif;
        }

        for (int i = 0; i < packs.Count; i++)
        {
            foreach (var road in packs[i].roads)
                road.СalculateMesh();
        }
    }

    //public void Update()
    //{
    //    var sum = (float)roads.Count;
    //    var neededValue = sum * percent;

    //    for (int i = 0; i < roads.Count; i++)
    //    {
    //        roads[i].percent = 0;
    //    }

    //    for (int i = 0; i < roads.Count && neededValue >= 0f; i++)
    //    {
    //        var dif = neededValue >= 1f ? 1f : neededValue;
    //        neededValue -= dif;
    //        roads[i].percent = dif;
    //    }

    //    for (int i = 0; i < roads.Count; i++)
    //        roads[i].СalculateMesh();
    //}
}

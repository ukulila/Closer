using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lister
{
    public List<Transform> listOfWaypoint;
}
[System.Serializable]
public class WayPointList
{
    public List<Lister> list;
}

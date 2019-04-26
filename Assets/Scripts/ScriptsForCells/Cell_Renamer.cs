using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Renamer : MonoBehaviour
{

    public string PositionName;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cell" && other.gameObject.name.Contains(PositionName) == false)
        {
            other.gameObject.name = PositionName;
        }
    }
    
}

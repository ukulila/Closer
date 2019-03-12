using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Renamer : MonoBehaviour
{

    public string PositionName;
    public List<GameObject> brothers;
    public bool manager;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cell" && other.gameObject.name.Contains(PositionName) == false)
        {
            other.gameObject.name = PositionName;
            
        }
    }
    /*
    public void ResetFacesNames()
    {
        if (manager)
        {
            for (int i = 0; i < brothers.Count; i++)
            {
                for (int v = 0; v < brothers[i].transform.childCount; v++)
                {
                    if (brothers[i].transform.GetChild(v).transform.localPosition.y == 0.5f)
                    {
                        brothers[i].transform.GetChild(v).name = "PlaneUp";
                    }
                    else if (brothers[i].transform.GetChild(v).transform.localPosition.y == -0.5f)
                    {
                        brothers[i].transform.GetChild(v).name = "PlaneDown";
                    }
                    else if (brothers[i].transform.GetChild(v).transform.localPosition.x == 0.5f)
                    {
                        brothers[i].transform.GetChild(v).name = "PlaneForward";
                        Debug.Log("renamed  " + "   Forward");
                    }
                    else if (brothers[i].transform.GetChild(v).transform.localPosition.x == -0.5f)
                    {
                        brothers[i].transform.GetChild(v).name = "PlaneAway";
                        Debug.Log("renamed  " + "   Away");
                    }
                    else if (brothers[i].transform.GetChild(v).transform.localPosition.z == 0.5f)
                    {
                        brothers[i].transform.GetChild(v).name = "PlaneRight";
                        Debug.Log("renamed  " + "   Right");
                    }
                    else if (brothers[i].transform.GetChild(v).transform.localPosition.z == -0.5f)
                    {
                        brothers[i].transform.GetChild(v).name = "PlaneLeft";
                        Debug.Log("renamed  " + "   Left");

                    }
                }

            }

        }
        
    }*/
}

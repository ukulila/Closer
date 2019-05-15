using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCells : MonoBehaviour
{

    public bool isSpawn;
    public bool selected;
    public Material myMaterial;
    public bool desactivate;
    public List<GameObject> cells;
    public bool isWaiting;
    public PlayerBehaviour pM;


    void Update()
    {

        if (desactivate)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                for (int x = 0; x < 7; x++)
                {
                    cells[i].transform.GetChild(x).GetComponent<Renderer>().material.SetInt("_isActive", 0);
                }
            }

            isWaiting = false;
            desactivate = false;
        }

    }


    public void OnMouseDown()
    {
        if (isWaiting)
        {
            desactivate = true;
        }
        else
        {
            selected = true;
        }

    }
}

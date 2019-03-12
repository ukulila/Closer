using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCells : MonoBehaviour {

    public bool isSpawn;
    public bool selected;
    public Material myMaterial;
    public bool desactivate;
    public List<GameObject> cells;
    public bool isWaiting;

    
	
	void Update () {
        
        RaycastHit hit;

        if(desactivate)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                for (int x = 0; x < cells[i].transform.childCount; x++)
                {
                    cells[i].transform.GetChild(x).GetComponent<Renderer>().material.SetInt("_isActive", 0);
                }
            }
            isWaiting = false;
            desactivate = false;
        }



        if (isSpawn && selected)
        {
            if (Physics.Raycast(transform.position, new Vector3(0, 0, -1), out hit))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 1);
                    transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_isActive", Color.white);

                }

                /*myMaterial = hit.transform.GetComponent<Renderer>().material;
                myMaterial.SetInt("_isActive", 1);*/
          //      hit.transform.GetComponent<CellMovement>().isOpen = true;
                Debug.DrawRay(transform.position, new Vector3(0, 0, -1),Color.red, 100);
                selected = false;
                isWaiting = true;
            }

            if (Physics.Raycast(transform.position, new Vector3(0, 0, 1), out hit))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 1);
                    transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_isActive", Color.white);

                }
           //     hit.transform.GetComponent<CellMovement>().isOpen = true;
                Debug.DrawRay(transform.position, new Vector3(0, 0, 1), Color.red, 100);
                selected = false;
                isWaiting = true;

            }

            if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), out hit))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 1);
                    transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_isActive", Color.white);

                }
        //        hit.transform.GetComponent<CellMovement>().isOpen = true;
                Debug.DrawRay(transform.position, new Vector3(1, 0, 0), Color.red, 100);
                selected = false;
                isWaiting = true;
            }

            if (Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out hit))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 1);
                    transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_isActive", Color.white);

                }
       //         hit.transform.GetComponent<CellMovement>().isOpen = true;
                Debug.DrawRay(transform.position, new Vector3(-1, 0, 0), Color.red, 100);
                selected = false;
                isWaiting = true;
            }

            if (Physics.Raycast(transform.position, new Vector3(0, 1, 0), out hit))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 1);
                    transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_isActive", Color.white);

                }
      //          hit.transform.GetComponent<CellMovement>().isOpen = true;
                Debug.DrawRay(transform.position, new Vector3(0, 1, 0), Color.red, 100);
                selected = false;
                isWaiting = true;
            }

            if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 1);
                    transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_isActive", Color.white);

                }
    //            hit.transform.GetComponent<CellMovement>().isOpen = true;
                Debug.DrawRay(transform.position, new Vector3(0, -1, 0), Color.red, 100);
                selected = false;
                isWaiting = true;
            }
        }

    }


    public void OnMouseDown()
    {
        if(isWaiting)
        {
            desactivate = true;
        }
        else
        {
            selected = true;
        }
    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour {


    public bool first;
    public int timer;
    public int nbrTouch;
    public bool rotation;
    public bool set;
    public Quaternion myRot;
    public int timeRot = 0;
    public int fin;
    public Quaternion wtf;
    public bool isCurrent;
    public List<CellMovement> brothers;
    public Cell_Renamer renameManager;

	void Start () {

        first = false;

	}
	
	void Update () {

        wtf =  myRot * new Quaternion(0, 90, 0, 0);

        if (first)
        {
            timer++;

            if(timer>= 30)
            {
                nbrTouch = 0;
                timer = 0;
                first = false;
            }
        }

        if(rotation)
        {
            transform.Rotate(new Vector3 (0,90,0), 1.2f);
            
            timeRot++;

            if (/*transform.rotation == myRot*new Quaternion(0,90,0,0) */timeRot > fin)
            {
                timeRot = 0;
                rotation = false;

                for (int r = 0; r < brothers.Count; r++)
                {
                    brothers[r].hasEnded = true;

                }
            }
        }
      
	}

    public void OnMouseDown()
    {
        first = true;
        nbrTouch += 1;

        #region ColorFreeCells
        if(isCurrent)
        {
            RaycastHit hit;

            if(Physics.Raycast(gameObject.transform.position, new Vector3(0, 0, -1), out hit))
            {
                if (hit.transform.tag == "Cell")
                {
                    if (hit.transform.gameObject.GetComponent<CellMovement>().isOpen == false)
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = true;
                    }
                    else
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = false;
                    }
                }
            }
            if (Physics.Raycast(gameObject.transform.position, new Vector3(0, 0, 1), out hit))
            {
                if (hit.transform.tag == "Cell")
                {
                    if (hit.transform.gameObject.GetComponent<CellMovement>().isOpen == false)
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = true;
                    }
                    else
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = false;
                    }
                }
            }
            if (Physics.Raycast(gameObject.transform.position, new Vector3(0, -1, 0), out hit))
            {
                if (hit.transform.tag == "Cell")
                {
                    if (hit.transform.gameObject.GetComponent<CellMovement>().isOpen == false)
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = true;
                    }
                    else
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = false;
                    }
                }
            }
            if (Physics.Raycast(gameObject.transform.position, new Vector3(0, 1, 0), out hit))
            {
                if (hit.transform.tag == "Cell")
                {
                    if (hit.transform.gameObject.GetComponent<CellMovement>().isOpen == false)
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = true;
                    }
                    else
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = false;
                    }
                }
            }
            if (Physics.Raycast(gameObject.transform.position, new Vector3(-1, 0, 0), out hit))
            {
                if (hit.transform.tag == "Cell")
                {
                    if (hit.transform.gameObject.GetComponent<CellMovement>().isOpen == false)
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = true;
                    }
                    else
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = false;
                    }
                }
            }
            if (Physics.Raycast(gameObject.transform.position, new Vector3(1, 0, 0), out hit))
            {
                if (hit.transform.tag == "Cell")
                {
                    if (hit.transform.gameObject.GetComponent<CellMovement>().isOpen == false)
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = true;
                    }
                    else
                    {
                        hit.transform.gameObject.GetComponent<CellMovement>().isOpen = false;
                    }
                }
            }
        }

        
        #endregion

        if (first && nbrTouch>1)
        {
            for (int r = 0; r < brothers.Count; r++)
            {
                brothers[r].hasEnded = false;

            }

            set = true;
            if (set)
            {
                myRot = transform.rotation;
                set = false;
            }
            rotation = true;
            nbrTouch = 0;
            
        }

    }
    /*public void OnMouseUp()
    {
        nbrTouch += 1;

    }*/

}

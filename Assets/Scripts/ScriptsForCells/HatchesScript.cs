using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchesScript : MonoBehaviour
{
    private bool checkOpenDoor;
    private Transform otherDoor;
    private Vector3 offset = new Vector3(0, 0, 0);
    private int timerOpenDoor;
    // Start is called before the first frame update
    void Start()
    {
        checkOpenDoor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<CellMovement>().isSpawn)
        {
            CheckForDoors();
        }
    }


    public void CheckForDoors()
    {

        if (checkOpenDoor)
        {
            //Debug.Log("           I draw          ");

            RaycastHit hit;
            int layerMaskDoor = LayerMask.GetMask("Door");


            if (Physics.Raycast(transform.transform.position + offset, -transform.forward, out hit, 5, layerMaskDoor) && hit.transform.gameObject != gameObject)
            {
               // Debug.DrawRay(transform.transform.position + offset, -transform.forward,Color.black, 500);

                otherDoor = hit.transform;

                transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);
                otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);

                checkOpenDoor = false;

            }
            else
            {

            //     Debug.DrawRay(transform.transform.position + offset, -transform.forward, Color.black, 500);
                if (otherDoor != null)
                {
                    transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
                    otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
                }

                checkOpenDoor = false;

            }

            
        }
        if (!checkOpenDoor)
        {
            timerOpenDoor++;

            if (timerOpenDoor > 80)
            {
                checkOpenDoor = true;
                timerOpenDoor = 0;
            }
        }
    }
}

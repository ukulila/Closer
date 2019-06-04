using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool checkOpenDoor;
    private Transform otherDoor;
    private Vector3 offset = new Vector3(0, 2, 0);
    private int timerOpenDoor;

    public Color OpenDoorColor;
    public Color ClosedDoorColor;

    // Start is called before the first frame update
    void Start()
    {
        checkOpenDoor = true;
        transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedDoorColor);
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
            //   Debug.Log("           I draw          ");

            RaycastHit hit;
            int layerMaskDoor = LayerMask.GetMask("Door");


            if (Physics.Raycast(transform.GetChild(0).transform.position + offset, -transform.up, out hit, 5, layerMaskDoor) && hit.transform.gameObject != gameObject)
            {
                //  Debug.Log("           I touch         " + hit.transform.name);

                //  Debug.DrawRay (transform.position + offset, -transform.up, Color.blue, 500);

                otherDoor = hit.transform;

                transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", OpenDoorColor);
               // transform.GetChild(1).gameObject.SetActive(true);
                otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", OpenDoorColor);

                checkOpenDoor = false;

            }
            else
            {
                //    Debug.Log("           I touch      nothing   ");

                //  Debug.DrawRay(transform.position + offset, -transform.up, Color.black, 500);

                if (otherDoor != null)
                {
                    if (transform.childCount > 1)
                        transform.GetChild(1).gameObject.SetActive(false);

                    transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedDoorColor);
                    otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedDoorColor);
                }
                checkOpenDoor = false;

            }


        }

        if (!checkOpenDoor)
        {
            //  Debug.Log("           timerOpenDoor      " + timerOpenDoor);

            timerOpenDoor++;

            if (timerOpenDoor > 80)
            {
                checkOpenDoor = true;
                timerOpenDoor = 0;
            }
        }

    }
    /*

    public void OnDrawGizmos()
    {
        //transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", OpenDoorColor);

        transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedDoorColor);
    }*/
}
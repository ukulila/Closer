using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool checkOpenDoor;
    private Transform otherDoor;
    private Vector3 offset = new Vector3(0, 1.5f, 0);
    private int timerOpenDoor;

    public Color OpenDoorColor;
    public Color ClosedDoorColor;

    public bool playerBeenHere;
    public bool isTuto02;
    public bool connection;
    private bool once;

    // Start is called before the first frame update
    void Start()
    {
        playerBeenHere = true;
        once = true;
        checkOpenDoor = true;
        transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedDoorColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<CellMovement>().isSpawn)
        {
            playerBeenHere = true;
            CheckForDoors();
        }


        if (playerBeenHere && !transform.parent.GetComponent<CellMovement>().isSpawn)
        {
            OnExitPlayer();
        }

        if(isTuto02)
        {
            if(connection && once)
            {
                
                TutorialDispatcher.Instance.ThrowDoorsAlignFctn();
                once = false;
                /*if(Camera_Rotation.Instance.aboutCamera == false)
                {

                    TutorialDispatcher.Instance.ThrowInvestigationFctn();
                    
                }*/
            }

           
           /* if (!connection && TutorialDispatcher.Instance.DoorsAlignedTuto.activeInHierarchy ==true)
            {
                TutorialDispatcher.Instance.InverseThrowDoorsAlignFctn();
            }*/
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

                connection = true;

                if (otherDoor != null && otherDoor.GetChild(0).GetComponent<Renderer>().material.GetColor("_EmissionColor") == OpenDoorColor)
                {

                    otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedDoorColor);

                }
                otherDoor = hit.transform;

                transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", OpenDoorColor);
                otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", OpenDoorColor);
                checkOpenDoor = false;

                if (CellPlacement.Instance.canPlayDoorSound == false)
                {
                  //  print("i make canPlaySound true");

                    CellPlacement.Instance.canPlayDoorSound = true;
                }


            }
            else
            {
                //    Debug.Log("           I touch      nothing   ");

                //  Debug.DrawRay(transform.position + offset, -transform.up, Color.black, 500);

                connection = false;

                if (otherDoor != null)
                {
                    if (transform.childCount > 1)
                        transform.GetChild(1).gameObject.SetActive(false);

                    transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedDoorColor);
                    otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedDoorColor);
                }
              CellPlacement.Instance.canPlayDoorSound = false;
              CellPlacement.Instance.onlyOnce = true;
             //   print("i make canPlaySound false AND onlyOnce true");


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




    public void OnExitPlayer()
    {

        RaycastHit hit;
        int layerMaskDoor = LayerMask.GetMask("Door");

        if (Physics.Raycast(transform.GetChild(0).transform.position + offset, -transform.up, out hit, 5, layerMaskDoor) && hit.transform.gameObject != gameObject)
        {

        }
        else
        {
            CellPlacement.Instance.canPlayDoorSound = false;
            CellPlacement.Instance.onlyOnce = true;
           // print("OnExitPlayer i make canPlaySound false AND onlyOnce true");


            if (transform.GetChild(0).GetComponent<Renderer>().material.GetColor("_EmissionColor") == OpenDoorColor)
            {

                transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedDoorColor);
                otherDoor = hit.transform;
                playerBeenHere = false;

            }
            else
            {
                playerBeenHere = false;
            }
        }
        /*

        public void OnDrawGizmos()
        {
            //transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", OpenDoorColor);

            transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedDoorColor);
        }*/
    }
}
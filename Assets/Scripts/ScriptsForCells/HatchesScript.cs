using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchesScript : MonoBehaviour
{
    private bool checkOpenDoor;
    private Transform otherDoor;
    private Vector3 offset = new Vector3(0, 0, 0);
    private int timerOpenDoor;
    public Color openHatchColor;
    public Color ClosedHatchColor;
    public bool isForEnding;

    public bool isTuto03;
    private bool connection;
    private bool once;


    // Start is called before the first frame update
    void Start()
    {
        once = true;
        checkOpenDoor = true;
        transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedHatchColor);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<CellMovement>().isSpawn)
        {
            CheckForDoors();
        }

        if (isTuto03)
        {
            if(connection && once)
            {
                TutorialDispatcher.Instance.SlideTuto.SetActive(false);
                TutorialDispatcher.Instance.DoorsAlignedTuto.SetActive(false);
                //TutorialDispatcher.Instance.ThrowDoorsAlignFctn();
                once = false;
            }


        }
    }


    public void CheckForDoors()
    {

        if (checkOpenDoor && !isForEnding)
        {
            //Debug.Log("           I draw          ");

            RaycastHit hit;
            int layerMaskDoor = LayerMask.GetMask("Door");


            if (Physics.Raycast(transform.transform.position + offset, -transform.forward, out hit, 5, layerMaskDoor) && hit.transform.gameObject != gameObject)
            {
                 Debug.DrawRay(transform.transform.position + offset, -transform.forward,Color.black, 500);

                connection = true;

                if (otherDoor != null && otherDoor.GetChild(0).GetComponent<Renderer>().material.GetColor("_EmissionColor") == openHatchColor)
                {
                    otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", openHatchColor);


                }
                otherDoor = hit.transform;

                transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", openHatchColor);
                otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", openHatchColor);

                checkOpenDoor = false;
                if (CellPlacement.Instance.canPlayDoorSound == false)
                    CellPlacement.Instance.canPlayDoorSound = true;

            }
            else
            {
                connection = false;

                Debug.DrawRay(transform.transform.position + offset, -transform.forward, Color.black, 500);
                if (otherDoor != null)
                {
                    transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedHatchColor);
                    otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedHatchColor);
                }

                checkOpenDoor = false;
                CellPlacement.Instance.canPlayDoorSound = false;
                CellPlacement.Instance.onlyOnce = true;
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

    /*
    public void OnDrawGizmos()
    {
        transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedHatchColor);

    }*/
}

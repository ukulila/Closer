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

    // Start is called before the first frame update
    void Start()
    {
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

                if (otherDoor != null && otherDoor.GetChild(0).GetComponent<Renderer>().material.GetColor("_EmissionColor") == openHatchColor)
                {
                    otherDoor = hit.transform;

                }

                transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", openHatchColor);
                otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", openHatchColor);

                checkOpenDoor = false;

            }
            else
            {

                //     Debug.DrawRay(transform.transform.position + offset, -transform.forward, Color.black, 500);
                if (otherDoor != null)
                {
                    transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedHatchColor);
                    otherDoor.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedHatchColor);
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

    /*
    public void OnDrawGizmos()
    {
        transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", ClosedHatchColor);

    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{

    public Vector3 originMousePos;
    public Vector3 mousePos;
    public Vector3 myRotation;
    public List<CellMovement> cell;
    public bool isOk;
    public bool inRotation;

    void Start()
    {

    }

    void Update()
    {

        myRotation.x = Mathf.Clamp(myRotation.x, -1, 1);

        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < cell.Count; i++)
            {

                if (cell[i].over == true)
                {
                   // Debug.Log("No, we Are over");
                    isOk = true;
                }

            }

            originMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && !isOk)
        {

            inRotation = true;

            mousePos = Input.mousePosition;

            transform.rotation *= Quaternion.Euler(transform.rotation * new Vector3(0, myRotation.x, 0));

        }
        if (Input.GetMouseButtonUp(0) )
        {
            isOk = false;
            inRotation = false;

            myRotation = Vector3.zero;
            //myRotation = transform.rotation.eulerAngles;
        }

        myRotation = (originMousePos - mousePos) / 75;

        //transform.rotation *= Quaternion.Euler(transform.rotation * new Vector3(0, myRotation.x, 0));









    }
}

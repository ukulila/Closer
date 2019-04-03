using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CellPlacement : MonoBehaviour
{

    public List<CellMovement> cM;
    public int nbrTouch;
    public GameObject FirstTouched;
    public GameObject SecondTouched;
    public Vector3 direction;

    public Camera fingerCamera;
    public CinemachineBrain myBrain;
    public bool once;
    public GameObject facingPlane;

    private PlayerMovement pM;
    public CameraBehaviour cbbbb;



    void Start()
    {
        nbrTouch = 0;
    }

    void Update()
    {

        //CheckForDirection();


        if (Input.GetMouseButtonDown(0))
        {
            once = true;

            RaycastHit hit;

            if (Physics.Raycast(myBrain.OutputCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.gameObject.GetComponent<CellMovement>() != null)
                {
                    //Debug.Log("ok so I work");
                    hit.collider.gameObject.GetComponent<CellMovement>().click = true;
                    hit.collider.gameObject.GetComponent<CellMovement>().over = true;

                    hit.collider.gameObject.GetComponent<CellMovement>().originPos = Input.mousePosition;


                    // HEAD:Assets/PackagesKillian/Package/CellPlacement.cs

                    /*for(int i = 0; i< hit.collider.transform.childCount; i++)
                    {

                    }*/
                    //
                    cbbbb.rotateAroundCube = false;
                    /*
                    CameraMovement.Instance.aboutCamera = false;
                    */
                    // Develop:Assets/Scripts/CellPlacement.cs
                }
                else
                {
                    cbbbb.rotateAroundCube = true;



                    /*
                    if (CameraMovement.Instance.switchToUI == false)
                        CameraMovement.Instance.aboutCamera = true;
                        */

                }

                if (hit.collider.gameObject.GetComponent<ScrEnvironment>() != null)
                {
                    pM.nextContext = hit.collider.gameObject.GetComponent<ScrEnvironment>();
                    hit.collider.gameObject.GetComponent<ScrEnvironment>().touched = true;
                }
            }
            else
            {


                for (int i = 0; i < cM.Count; i++)
                {

                    cM[i].over = false;

                }
            }
        }

        if (Input.GetMouseButton(0) && once)
        {
            RaycastHit[] hits;

            hits = Physics.RaycastAll(myBrain.OutputCamera.ScreenPointToRay(Input.mousePosition), 25);

            if (hits.Length != 0 && hits[0].transform.name.Contains("Cell") == false)
            {
                Debug.LogWarning(hits[0].transform.name);
                facingPlane = hits[0].transform.gameObject;
                once = false;
            }
            else if (hits.Length > 1)
            {
                Debug.LogWarning(hits[1].transform.name);
                facingPlane = hits[1].transform.gameObject;
                once = false;
            }
        }
    }





    public void CheckForDirection()
    {

        if (Input.GetMouseButton(0))
        {
            for (int i = 0; i < cM.Count; i++)
            {

                if (cM[i].selected == true && cM[i] != FirstTouched)
                {
                    if (FirstTouched == null)
                    {

                        FirstTouched = cM[i].gameObject;
                        cM[i].selected = false;
                        cM[i].over = false;
                    }
                    else if (FirstTouched != null && cM[i] != FirstTouched)
                    {
                        SecondTouched = cM[i].gameObject;
                        cM[i].selected = false;
                        cM[i].over = false;


                    }

                    nbrTouch += 1;

                }

            }



        }

        if (nbrTouch > 2 || Input.GetMouseButtonUp(0))
        {
            if (SecondTouched != null)
            {
                direction = SecondTouched.transform.position - FirstTouched.transform.position;
                // Debug.Log(direction);
                FirstTouched = null;
                SecondTouched = null;
            }


            nbrTouch = 0;

        }

    }
}

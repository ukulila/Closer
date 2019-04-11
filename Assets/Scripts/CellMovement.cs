using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CellMovement : MonoBehaviour
{

    public Transform directionObj;
    public bool click;
    public bool over;
    public bool once;
    public bool movement;
    public bool selected;
    public bool isOpen;
    public int timer;
    public Vector3 originPos;
    public Vector3 thenPos;
    public Vector3 distanceMove;
    public int direction;

    public GameObject exitCell;
    public GameObject actualCell;
    public List<Transform> brothers;
    public bool isSpawn;

    public CubeScript cS;
    public CameraBehaviour cB;

    public List<Transform> toRotate;
    public bool moveHorizontal;
    public bool moveVerticalZ;
    public bool freezePosValue;
    public Vector3 myPosFreeze;
    public bool hasEnded;

    public bool reverse;
    public CellPlacement cP;
    public bool moveVerticalX;
    public Cell_Renamer renameManager;
    public PlayerBehaviour pM;
    public bool clickDirection;
    public bool raycastAutor;




    #region Init

    public void OnEnable()
    {
        over = true;

        // Fills List of other Cells
        for (int x = 0; x < transform.parent.childCount; x++)
        {
            brothers.Add(transform.parent.GetChild(x));

        }

        hasEnded = true;
        freezePosValue = true;

        //Wait a minute, this is not necesssary right ?
        if (gameObject.name.Contains("Exit"))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains("Plane"))
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 1);
                }

            }
        }


    }
    #endregion


    void Update()
    {
        if (isSpawn)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains("Plane"))
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_myColor", Color.green);

                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 1);
                }

            }

            if (selected && raycastAutor)
            {
                pM.castingRay = true;
                raycastAutor = false;
                
            }
        }


        if (clickDirection)
        {
            pM.nextContext = gameObject.GetComponent<ScrEnvironment>();
            pM.add = true;
            clickDirection = false;
        }


        #region ---- RenameByPLace ----


        brothers = brothers.OrderBy(go => go.name).ToList();




        #endregion


        #region ---- RESET ----

        //Deselects everything when click over
        if (Input.GetMouseButtonUp(0))
        {
            once = false;

            click = false;
            timer = 0;

        }

        if (toRotate.Count < 4)
        {

            toRotate.Clear();

        }


        #endregion

        #region ---- Selection ----

        if (click)
        {
            selected = true;
        }



        //Takes off the Outline when click elsewhere
        if (Input.GetMouseButtonDown(0) && !isSpawn && gameObject.name.Contains("Exit") == false)
        {
            selected = false;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains("Plane"))
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_myColor", Color.green);
                }

            }
        }



        #endregion

        #region ---- Update Movement ----

        #region ---- Temporary DEBUG position ----
        //DebugWeirdPosition
        if (hasEnded)
        {

            if (transform.position.x < 0 && (transform.position.x != 0.255f || transform.position.x != -0.255f))
            {
                transform.position = new Vector3(-0.255f, transform.position.y, transform.position.z);
            }
            else if (transform.position.x > 0 && (transform.position.x != 0.255f || transform.position.x != -0.255f))
            {
                transform.position = new Vector3(0.255f, transform.position.y, transform.position.z);
            }

            if (transform.position.y < 0 && (transform.position.y != 0.255f || transform.position.y != -0.255f))
            {
                transform.position = new Vector3(transform.position.x, -0.255f, transform.position.z);
            }
            else if (transform.position.y > 0 && (transform.position.y != 0.255f || transform.position.y != -0.255f))
            {
                transform.position = new Vector3(transform.position.x, 0.255f, transform.position.z);
            }

            if (transform.position.z < 0 && (transform.position.z != 0.255f || transform.position.z != -0.255f))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -0.255f);
            }
            else if (transform.position.z > 0 && (transform.position.z != 0.255f || transform.position.z != -0.255f))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0.255f);
            }

        }
        #endregion


        if (reverse)
        {
            toRotate.Reverse();
            reverse = false;
        }

        //Increment a value (timer) when OnMouseDown
        if (click)
        {
            timer++;

        }

        //Stores position of The Mouse after timer is 30
        if (click && timer >= 15)
        {
            thenPos = Input.mousePosition;
            movement = true;
        }

        //Stores the position value when true
        if (freezePosValue)
        {
            myPosFreeze = transform.position;

            freezePosValue = false;
        }

        //Starts to check for an other Drag/Swipe only if the previous one has ended
        if (movement && hasEnded)
        {
            CheckMove();


        }

        //Makes the actual Position of Cell Change. The 1rst position --> the 2nd etc..
        if (moveHorizontal)
        {

            for (int r = 0; r < brothers.Count; r++)
            {
                brothers[r].GetComponent<CellMovement>().hasEnded = false;

            }


            for (int v = 0; v < toRotate.Count; v++)
            {
                if (v != toRotate.Count - 1)
                {
                    toRotate[v].transform.position = new Vector3((Mathf.Lerp(toRotate[v].transform.position.x, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.x, 0.1f)), toRotate[v].transform.position.y, (Mathf.Lerp(toRotate[v].transform.position.z, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.z, 0.1f)));

                }
                else
                {
                    toRotate[v].transform.position = new Vector3((Mathf.Lerp(toRotate[v].transform.position.x, toRotate[0].GetComponent<CellMovement>().myPosFreeze.x, 0.1f)), toRotate[v].transform.position.y, (Mathf.Lerp(toRotate[v].transform.position.z, toRotate[0].GetComponent<CellMovement>().myPosFreeze.z, 0.1f)));

                }

            }

            ///There May Be A Delay Between Two Movement with this way to check
            ///
            if (toRotate[0].position == toRotate[1].GetComponent<CellMovement>().myPosFreeze && toRotate.Count == 4)
            {
                Debug.LogWarning("__HAS__ENDED__");
                movement = false;

                toRotate[0].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[1].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[2].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[3].GetComponent<CellMovement>().freezePosValue = true;

                for (int o = 0; o < brothers.Count; o++)
                {
                    brothers[o].GetComponent<CellMovement>().hasEnded = true;
                }

                once = false;
                selected = false;
                click = false;
                timer = 0;

                moveHorizontal = false;
            }

        }


        #region [COMMENTEE]

        //Makes the actual Position of Cell Change. The 1rst position --> the 2nd etc..  BUT INVERSE
        if (moveVerticalZ)
        {
            Debug.LogWarning("YOU DEMANDED THE INVERSE");

            for (int r = 0; r < brothers.Count; r++)
            {
                brothers[r].GetComponent<CellMovement>().hasEnded = false;

            }

            for (int v = 0; v < toRotate.Count; v++)
            {
                Debug.LogWarning("Playing");

                if (v != toRotate.Count - 1)
                {
                    toRotate[v].transform.position = new Vector3((Mathf.Lerp(toRotate[v].transform.position.x, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.x, 0.1f)), (Mathf.Lerp(toRotate[v].transform.position.y, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.y, 0.1f)), toRotate[v].transform.position.z);

                }
                else
                {
                    toRotate[v].transform.position = new Vector3((Mathf.Lerp(toRotate[v].transform.position.x, toRotate[0].GetComponent<CellMovement>().myPosFreeze.x, 0.1f)), (Mathf.Lerp(toRotate[v].transform.position.y, toRotate[0].GetComponent<CellMovement>().myPosFreeze.y, 0.1f)), toRotate[v].transform.position.z);

                }
            }

            ///There May Be A Delay Between Two Movement with this way to check
            ///
            if (toRotate[0].position == toRotate[1].GetComponent<CellMovement>().myPosFreeze && toRotate.Count == 4)
            {
                Debug.LogWarning("__HAS__ENDED__");
                movement = false;

                toRotate[0].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[1].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[2].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[3].GetComponent<CellMovement>().freezePosValue = true;


                for (int o = 0; o < brothers.Count; o++)
                {
                    brothers[o].GetComponent<CellMovement>().hasEnded = true;
                }


                once = false;
                selected = false;
                click = false;
                timer = 0;

                moveVerticalZ = false;
            }
        }

        if (moveVerticalX)
        {
            Debug.LogWarning("YOU DEMANDED THE INVERSE BUT IN X");

            for (int r = 0; r < brothers.Count; r++)
            {
                brothers[r].GetComponent<CellMovement>().hasEnded = false;

            }


            for (int v = 0; v < toRotate.Count; v++)
            {
                Debug.LogWarning("PlayingX");

                if (v != toRotate.Count - 1)
                {
                    toRotate[v].transform.position = new Vector3(toRotate[v].transform.position.x, (Mathf.Lerp(toRotate[v].transform.position.y, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.y, 0.1f)), (Mathf.Lerp(toRotate[v].transform.position.z, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.z, 0.1f)));

                }
                else
                {
                    toRotate[v].transform.position = new Vector3(toRotate[v].transform.position.x, (Mathf.Lerp(toRotate[v].transform.position.y, toRotate[0].GetComponent<CellMovement>().myPosFreeze.y, 0.1f)), (Mathf.Lerp(toRotate[v].transform.position.z, toRotate[0].GetComponent<CellMovement>().myPosFreeze.z, 0.1f)));

                }
            }

            ///There May Be A Delay Between Two Movement with this way to check
            ///
            if (toRotate[0].position == toRotate[1].GetComponent<CellMovement>().myPosFreeze && toRotate.Count == 4)
            {
                Debug.LogWarning("__HAS__ENDED__");
                movement = false;

                toRotate[0].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[1].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[2].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[3].GetComponent<CellMovement>().freezePosValue = true;

                for (int o = 0; o < brothers.Count; o++)
                {
                    brothers[o].GetComponent<CellMovement>().hasEnded = true;
                }


                once = false;
                selected = false;
                click = false;
                timer = 0;

                moveVerticalX = false;
            }
        }
    }
    #endregion
    #endregion


    #region ---- CheckDirection ----
    public void CheckMove()
    {
        distanceMove = thenPos - originPos;

        ///These return a swipe direction and starts the Moving Functions accordingly.
        if (distanceMove.x >= 100)
        {
            Debug.Log("Right");
            movement = false;

            if (!once)
            {
                toRotate.Clear();
                HorizontalRotateSide(1);
            }
        }
        else if (distanceMove.x <= -100)
        {
            Debug.Log("Left");
            movement = false;
            if (!once)
            {
                toRotate.Clear();
                HorizontalRotateSide(2);
            }
        }
        else if (distanceMove.y <= -100)
        {
            Debug.Log("Down");
            movement = false;
            if (!once)
            {
                toRotate.Clear();
                HorizontalRotateSide(3);
            }
        }
        else if (distanceMove.y >= 100)
        {
            Debug.Log("Up");
            movement = false;
            if (!once)
            {
                toRotate.Clear();
                HorizontalRotateSide(4);
            }
        }
        else
        {
            return;
        }


    }
    #endregion


    #region ---- CreateList ----


    //Rotation for Horizontal sclice of the cube, pairs every cell on the same level and triggers the movement.
    ///ADD STUFF IF UP OR DOWN && MAYBE IF RIGHT AND LEFT
    public void HorizontalRotateSide(int dir)
    {


        //Debug.Log("I play myself normaly");

        for (int u = 0; u < brothers.Count; u++)
        {


            switch (dir)
            {
                case 1:   ///so the swipe is right                                                                          /////////

                    if (brothers[u].transform.position.y < 0 && transform.position.y < 0)
                    {
                        if (toRotate.Count <= 4)
                        {
                            toRotate.Add(brothers[u]);

                        }

                        moveHorizontal = true;
                        once = true;
                    }
                    else if (brothers[u].transform.position.y > 0 && transform.position.y > 0)
                    {
                        if (toRotate.Count <= 4)
                        {
                            toRotate.Add(brothers[u]);
                            reverse = true;
                        }


                        moveHorizontal = true;
                        once = true;

                    }

                    break;
                case 2:   ///so the swipe is left                                                                            /////////

                    if (brothers[u].position.y == transform.position.y)
                    {
                        if (brothers[u].transform.position.y < 0 && transform.position.y < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }


                            moveHorizontal = true;
                            once = true;

                        }
                        else if (brothers[u].transform.position.y > 0 && transform.position.y > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                            }
                            moveHorizontal = true;
                            once = true;

                        }
                    }

                    break;

                case 3:   ///so the swipe is down                                                                           ////////

                    if (cP.facingPlane.name == "PlaneForward")
                    {
                        if (brothers[u].transform.position.z < 0 && transform.position.z < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }

                            moveVerticalZ = true;
                            once = true;
                        }

                        else if (brothers[u].transform.position.z > 0 && transform.position.z > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                            }


                            moveVerticalZ = true;
                            once = true;

                        }
                    }

                    if (cP.facingPlane.name == "PlaneAway")
                    {
                        if (brothers[u].transform.position.z < 0 && transform.position.z < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                            }

                            moveVerticalZ = true;
                            once = true;
                        }

                        else if (brothers[u].transform.position.z > 0 && transform.position.z > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }


                            moveVerticalZ = true;
                            once = true;

                        }
                    }

                    if (cP.facingPlane.name == "PlaneRight")
                    {
                        if (brothers[u].transform.position.x < 0 && transform.position.x < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }

                            moveVerticalX = true;
                            once = true;
                        }

                        else if (brothers[u].transform.position.x > 0 && transform.position.x > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }


                            moveVerticalX = true;
                            once = true;
                        }
                    }

                    if (cP.facingPlane.name == "PlaneLeft")
                    {
                        if (brothers[u].transform.position.x < 0 && transform.position.x < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                            }

                            moveVerticalX = true;
                            once = true;
                        }

                        else if (brothers[u].transform.position.x > 0 && transform.position.x > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                            }

                            moveVerticalX = true;
                            once = true;

                        }
                    }
                    break;


                case 4:   ///so the swipe is UP


                    if (cP.facingPlane.name == "PlaneForward")
                    {
                        if (brothers[u].transform.position.z < 0 && transform.position.z < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                            }

                            moveVerticalZ = true;
                            once = true;
                        }

                        else if (brothers[u].transform.position.z > 0 && transform.position.z > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }


                            moveVerticalZ = true;
                            once = true;

                        }
                    }

                    if (cP.facingPlane.name == "PlaneAway")
                    {
                        if (brothers[u].transform.position.z < 0 && transform.position.z < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }

                            moveVerticalZ = true;
                            once = true;
                        }

                        else if (brothers[u].transform.position.z > 0 && transform.position.z > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                            }
                            moveVerticalZ = true;
                            once = true;

                        }
                    }

                    if (cP.facingPlane.name == "PlaneRight")
                    {
                        if (brothers[u].transform.position.x < 0 && transform.position.x < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                            }

                            moveVerticalX = true;
                            once = true;
                        }

                        else if (brothers[u].transform.position.x > 0 && transform.position.x > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                            }

                            moveVerticalX = true;
                            once = true;
                        }
                    }

                    if (cP.facingPlane.name == "PlaneLeft")
                    {
                        if (brothers[u].transform.position.x < 0 && transform.position.x < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }

                            moveVerticalX = true;
                            once = true;
                        }

                        else if (brothers[u].transform.position.x > 0 && transform.position.x > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }


                            moveVerticalX = true;
                            once = true;

                        }
                    }
                    break;
            }

        }

    }



    public void OnTriggerEnter(Collider other)
    {

        if (other.transform.name.Contains("Player"))
        {
            isOpen = false;
            selected = true;
            isSpawn = true;
            other.transform.SetParent(transform);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.name.Contains("Player"))
        {
            isSpawn = false;
            selected = false;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains("Plane"))
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 0);

                }

            }
        }


    }

    #endregion
}


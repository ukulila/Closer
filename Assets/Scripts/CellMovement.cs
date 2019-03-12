using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    private string myName;
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

    #region Init
    void Start()
    {
        over = true;
        myName = gameObject.name;

    }


    public void OnEnable()
    {
        // Fills List of other Cells
        for (int x = 0; x < transform.parent.childCount; x++)
        {
            brothers.Add(transform.parent.GetChild(x));

        }

        hasEnded = true;
        freezePosValue = true;
    }
    #endregion


    void Update()
    {
        /*for (int i = 0; i < brothers.Count; i++)
        {
            if (brothers[i].GetComponent<CellMovement>().isSpawn == false)
            {
                for (int f = 0; f < brothers[i].transform.childCount; f++)
                {
                    transform.GetChild(f).GetComponent<Renderer>().material.SetInt("_isActive", 0);
                }
            }
        }*/
        #region ---- RenameByPLace ----


        brothers = brothers.OrderBy(go => go.name).ToList();




        #endregion


        #region ---- RESET ----
        //Deselects everything when click over
        if (Input.GetMouseButtonUp(0))
        {
            once = false;
            selected = false;
            click = false;
            timer = 0;
            CameraBehaviour.Instance.rotateAroundCube = true;
            isOpen = false;
            if(!isSpawn)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 0);
                }
            }
        }

        if(toRotate.Count < 4)
        {

            toRotate.Clear();

        }
        

        #endregion

        #region ---- Selection ----

        selected = click;

        //Takes off the Outline when click elsewhere
        if (Input.GetMouseButtonDown(0) && !over && !isSpawn)
        {
            
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 0);
                
            }

        }

        //Adds the outline if neither spawn nor exit Cell // Changes the name too for Debug
        if (selected)
        {
            if (gameObject != actualCell && gameObject != exitCell)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 1);

                }
            }
            //gameObject.name = myName + "-----------";
        }
        else
        {
            //gameObject.name = myName;
        }

        //Debug Name to Check for free Cells around player
        if (isOpen)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_myColor", new Color32(140,140,140,255));

                transform.GetChild(i).GetComponent<Renderer>().material.SetInt("_isActive", 1);


            }
        }
        else if (!isOpen)
        {
            for (int i = 0; i < transform.childCount; i++)
            {

                transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_myColor", Color.green);
                


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
            for (int i = 0; i < brothers.Count; i++)
            {
                isOpen = false;
                if (!isSpawn)
                {
                    for (int f = 0; f < transform.childCount; f++)
                    {
                        transform.GetChild(f).GetComponent<Renderer>().material.SetInt("_isActive", 0);
                    }
                }
            }
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
            // Debug.Log(myName + "      ResetGood");
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
            

            //Debug.Log(gameObject.name);

            for (int r = 0; r < brothers.Count; r++)
            {
                brothers[r].GetComponent<CellMovement>().hasEnded = false;

            }
            /*toRotate[0].GetComponent<CellMovement>().hasEnded = false;
            toRotate[1].GetComponent<CellMovement>().hasEnded = false;
            toRotate[2].GetComponent<CellMovement>().hasEnded = false;
            toRotate[3].GetComponent<CellMovement>().hasEnded = false;*/

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

                ///Maybe CHECK THIS ---EDIT : SEEMS OK BUT LATE ?
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

                /*toRotate[0].GetComponent<CellMovement>().hasEnded = true;
                toRotate[1].GetComponent<CellMovement>().hasEnded = true;
                toRotate[2].GetComponent<CellMovement>().hasEnded = true;
                toRotate[3].GetComponent<CellMovement>().hasEnded = true;*/

                for (int o = 0; o < brothers.Count; o++)
                {
                    brothers[o].GetComponent<CellMovement>().hasEnded = true;
                }

                /*
                toRotate[0].GetComponent<CellMovement>().distanceMove = new Vector2(0, 0);
                toRotate[1].GetComponent<CellMovement>().distanceMove = new Vector2(0, 0);
                toRotate[2].GetComponent<CellMovement>().distanceMove = new Vector2(0, 0);
                toRotate[3].GetComponent<CellMovement>().distanceMove = new Vector2(0, 0);
                */
                //distanceMove = new Vector2(0,0);
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
            /*
            toRotate[0].GetComponent<CellMovement>().hasEnded = false;
            toRotate[1].GetComponent<CellMovement>().hasEnded = false;
            toRotate[2].GetComponent<CellMovement>().hasEnded = false;
            toRotate[3].GetComponent<CellMovement>().hasEnded = false;*/

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


                /*
                toRotate[0].GetComponent<CellMovement>().hasEnded = true;
                toRotate[1].GetComponent<CellMovement>().hasEnded = true;
                toRotate[2].GetComponent<CellMovement>().hasEnded = true;
                toRotate[3].GetComponent<CellMovement>().hasEnded = true;*/

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

            /*toRotate[0].GetComponent<CellMovement>().hasEnded = false;
            toRotate[1].GetComponent<CellMovement>().hasEnded = false;
            toRotate[2].GetComponent<CellMovement>().hasEnded = false;
            toRotate[3].GetComponent<CellMovement>().hasEnded = false;*/

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

                /*toRotate[0].GetComponent<CellMovement>().hasEnded = true;
                toRotate[1].GetComponent<CellMovement>().hasEnded = true;
                toRotate[2].GetComponent<CellMovement>().hasEnded = true;
                toRotate[3].GetComponent<CellMovement>().hasEnded = true;*/

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
                //CastRay(1);
                //once = true;
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
                //CastRay(2);
                //once = true;
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

                //CastRay(3);
                // once = true;
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

                //CastRay(4);
                // once = true;
            }
        }
        else
        {
            Debug.LogWarning("Hey That's Too Short");
            return;
        }


    }
    #endregion


    #region [Commentée] ---- ShootRay ----
    /*
    //Function that checks if there is something to rotate in the direction of the swipe by drawing a ray in the swipe direction
    public void CastRay(int direction)
    {
        ///CHECK THE OUTPUT AND CHANGE LEFT IF UP AND RIGHT IF DOWN 
        RaycastHit hit;

        switch (direction)
        {
            case 1:                                                                 //RIGHT


                if (Physics.Raycast(transform.position, new Vector3(0, 0, -1), out hit))
                {
                    Debug.DrawRay(transform.position, new Vector3(0, 0, -1), Color.green, 100);
                    Debug.Log(hit.transform.name);
                    directionObj = hit.transform;

                    HorizontalRotateSide(0);

                }
                else
                {
                    Debug.DrawRay(transform.position, Vector3.up, Color.red, 100);

                }
                direction = 0;

                break;
            case 2:                                                                 //LEFT

                if (Physics.Raycast(transform.position, new Vector3(0, 0, 1), out hit))
                {
                    Debug.DrawRay(transform.position, new Vector3(0, 0, 1), Color.green, 100);
                    Debug.Log(hit.transform.name);
                    directionObj = hit.transform;

                    HorizontalRotateSide(0);

                }
                else
                {
                    Debug.DrawRay(transform.position, Vector3.up, Color.red, 100);

                }
                direction = 0;

                break;
            case 3:                                                                 //DOWN

                if (Physics.Raycast(transform.position, -Vector3.up, out hit))
                {
                    Debug.DrawRay(transform.position, -Vector3.up, Color.green, 100);
                    Debug.Log(hit.transform.name);
                }
                else
                {
                    Debug.DrawRay(transform.position, Vector3.up, Color.red, 100);

                }
                direction = 0;
                break;
            case 4:                                                                   //UP
                if (Physics.Raycast(transform.position, Vector3.up, out hit))
                {
                    Debug.DrawRay(transform.position, Vector3.up, Color.green, 100);
                    Debug.Log(hit.transform.name);
                }
                else
                {
                    Debug.DrawRay(transform.position, Vector3.up, Color.red, 100);

                }
                direction = 0;

                break;



        }


    }
    */
    #endregion


    #region ---- CreateList ----

    //Rotation for Horizontal sclice of the cube, pairs every cell on the same level and triggers the movement.
    ///ADD STUFF IF UP OR DOWN && MAYBE IF RIGHT AND LEFT
    public void HorizontalRotateSide(int dir)
    {

        //    11  >0    --> LEFT MOVEMENT IS OK, RIGHT IS LEFT TOO
        //    00  <0    --> RIGHT MOVEMENT IS OK, LEFT IS RIGHT TOO

        // 1 = RIGHT
        // 2 = LEFT


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
                        //return;
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
                        //return;

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
                            // return;

                        }
                        else if (brothers[u].transform.position.y > 0 && transform.position.y > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                            }
                            moveHorizontal = true;
                            once = true;
                            // return;

                        }
                    }

                    break;

                case 3:   ///so the swipe is down                                                                           ////////


                    /* if (brothers[u].position.z == transform.position.z)
                     {*/

                    if (cP.facingPlane.name == "PlaneForward")
                    {
                       // Debug.Log("I play Forward");
                        if (brothers[u].transform.position.z < 0 && transform.position.z < 0)
                        {
                          // Debug.Log("I play Forward Two");
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }

                            moveVerticalZ = true;
                            once = true;
                            //return;
                        }

                        else if (brothers[u].transform.position.z > 0 && transform.position.z > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                //reverse = true;
                            }


                            moveVerticalZ = true;
                            once = true;
                            //return;

                        }
                    }

                    if (cP.facingPlane.name == "PlaneAway")
                    {
                        //Debug.Log("I play Away");
                        if (brothers[u].transform.position.z < 0 && transform.position.z < 0)
                        {
                            //Debug.Log("I play Away Two");
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                //reverse = true;
                            }

                            moveVerticalZ = true;
                            once = true;
                            //return;
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
                            //return;

                        }
                    }

                    if (cP.facingPlane.name == "PlaneRight")
                    {
                       // Debug.Log("I play Right");
                        if (brothers[u].transform.position.x < 0 && transform.position.x < 0)
                        {
                          //  Debug.Log("I play Right Two");
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                reverse = true;
                            }

                            moveVerticalX = true;
                            once = true;
                            //return;
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
                            //return;

                        }
                    }

                    if (cP.facingPlane.name == "PlaneLeft")
                    {
                       // Debug.Log("I play Left");
                        if (brothers[u].transform.position.x < 0 && transform.position.x < 0)
                        {
                         //   Debug.Log("I play Left Two");
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                //reverse = true;
                            }

                            moveVerticalX = true;
                            once = true;
                            //return;
                        }

                        else if (brothers[u].transform.position.x > 0 && transform.position.x > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                //reverse = true;
                            }


                            moveVerticalX = true;
                            once = true;
                            //return;

                        }
                    }
                    // }
                    break;


                case 4:   ///so the swipe is UP                                                                                     ////////


                    /*if (brothers[u].position.z == transform.position.z)
                    {*/
                    if (cP.facingPlane.name == "PlaneForward")
                    {
                        if (brothers[u].transform.position.z < 0 && transform.position.z < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                //reverse = true;
                            }

                            moveVerticalZ = true;
                            once = true;
                            //return;
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
                            //return;

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
                            //return;
                        }

                        else if (brothers[u].transform.position.z > 0 && transform.position.z > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                //reverse = true;
                            }


                            moveVerticalZ = true;
                            once = true;
                            //return;

                        }
                    }

                    if (cP.facingPlane.name == "PlaneRight")
                    {
                        if (brothers[u].transform.position.x < 0 && transform.position.x < 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                //reverse = true;
                            }

                            moveVerticalX = true;
                            once = true;
                            //return;
                        }

                        else if (brothers[u].transform.position.x > 0 && transform.position.x > 0)
                        {
                            if (toRotate.Count <= 4)
                            {
                                toRotate.Add(brothers[u]);
                                //reverse = true;
                            }


                            moveVerticalX = true;
                            once = true;
                            //return;

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
                            //return;
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
                            //return;

                        }
                    }
                    break;
            }

        }

    }


}
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public bool reset;
    public bool lookCam;
    public bool movement;
    public Vector3 origin;
    public bool once;
    public Lister waypoints;
    public ScrEnvironment context;
    public ScrEnvironment nextContext;
    public float distance;

    [Range(0.1f, 2)]
    public float minDist = 0.1f;
    public Vector3 direction;

    public int myPath;
    public List<bool> ways;

    public int index;
    public bool next = true;
    public int boolIndex;
    public float animspeed;
    public bool openDoor;
    public bool baseMovement;
    private bool firstPath;
    public GameObject door;
    public GameObject nextDoor;
    public Transform doorDirection;
    public bool isValid;
    public List<Transform> inverseList;
    public Transform myDoor;
    public bool reverse;
    private Vector3 doorRot;
    public bool closeDoor;
    private GameObject coneDoor;
    public bool add;

    public CinemachineBrain Camera;
    public List<Transform> ultimateList;
    public bool castingRay;
    public CellPlacement cP;
    public Vector3 offset;
    //private bool onlyOne;
    public Animator animator;
    private float x;
    private float y;

    public Transform RoomChecker;
    public int onlyTwo;
    public bool checkOpenDoor;
    private int checkInt;
    public AlwaysLookAtCam highlight;
    private bool one;
    public AnimationCurve animCurve;
    public float speedModifier;

    public List<CellMovement> Rooms;
    public List<CellScript> CellScr;

    private bool oneRef;
    private bool oneRef01;
    private bool oneRef02;

    public Vector3 offsetTrap;
    public int timerOpenDoor;

    void Start()
    {
        add = false;
        reset = true;
        //     onlyOne = true;
        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {

        animator.SetFloat("ValueX", x);
        animator.SetFloat("ValueY", y);

        CheckConeLight();

        if (lookCam)
        {
            Vector3 camX = new Vector3(Camera.transform.position.x, transform.position.y, Camera.transform.position.z);
            transform.LookAt(camX);


            highlight.enabled = false;

            if (one)
            {
                highlight.transform.localEulerAngles = new Vector3(-90, 90, 56);
                one = false;
            }

            if (x > 0 && y > 0)
            {
                y -= 0.05f;
                x -= 0.05f;

            }

            //animator.SetBool("Walk", false);
        }
        else if (!lookCam)
        {

            highlight.enabled = true;

        }

        #region isDoorIn


        if (castingRay)
        {
            if (context.doorWayPoints.Count != 0)
            {
                for (int u = 0; u < context.doorWayPoints.Count; u++)
                {
                    RaycastHit hit;
                    int layerMaskDoor = LayerMask.GetMask("Door");


                    if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                    {
                        Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.green, 50);
                        cP.okToSetup = true;


                        //Debug.Log(hit.transform.name);


                        if (hit.transform.parent.GetComponent<CellMovement>() && !hit.transform.parent.GetComponent<OpacityKiller>().BlackRoom)
                        {
                            //Debug.Log("parentparentconatinsCellmovement");

                            CellMovement cellMove = hit.transform.parent.GetComponent<CellMovement>();

                            if (cellMove.isOpen == false)
                            {
                                //Debug.Log("here is a door i could take" + hit.transform.parent.name);
                                cellMove.isOpen = true;
                            }
                            else if (cellMove.isOpen == true)
                            {
                                //Debug.Log("close");
                                cellMove.isOpen = false;
                            }
                        }

                        castingRay = false;
                    }
                    else
                    {
                        Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.red, 50);
                        castingRay = false;

                    }

                    if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.up, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                    {
                        Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.up, Color.green, 50);
                        cP.okToSetup = true;


                        //Debug.Log(hit.transform.parent.name);

                        if (hit.transform.parent.GetComponent<CellMovement>() && !hit.transform.parent.GetComponent<OpacityKiller>().BlackRoom)
                        {
                            //Debug.Log("parentparentconatinsCellmovement");

                            CellMovement cellMove = hit.transform.parent.GetComponent<CellMovement>();

                            if (cellMove.isOpen == false)
                            {
                                //Debug.Log("here is a door i could take" + hit.transform.parent.name);
                                cellMove.isOpen = true;
                            }
                            else if (cellMove.isOpen == true)
                            {
                                //Debug.Log("close");
                                cellMove.isOpen = false;
                            }
                        }
                        castingRay = false;

                    }
                    else
                    {
                        Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.up, Color.red, 50);
                        castingRay = false;

                    }


                }
            }



            //////////////////////////////////////////////                                    //////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////// Hatches Only /////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////                                    /////////////////////////////////////////////////////

            if (context.HatchesWayPoints.Count != 0)
            {
                for (int u = 0; u < context.HatchesWayPoints.Count; u++)
                {

                    RaycastHit hit;
                    int layerMaskDoor = LayerMask.GetMask("Door");
                    Debug.Log("coucou");


                    if (Physics.Raycast(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                    {
                        Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, Color.green, 50);
                        cP.okToSetup = true;


                        Debug.Log(hit.transform.name);


                        if (hit.transform.parent.GetComponent<CellMovement>())
                        {
                            //Debug.Log("parentparentconatinsCellmovement");

                            CellMovement cellMove = hit.transform.parent.GetComponent<CellMovement>();

                            if (cellMove.isOpen == false)
                            {
                                //Debug.Log("here is a door i could take" + hit.transform.parent.name);
                                cellMove.isOpen = true;
                            }
                            else if (cellMove.isOpen == true)
                            {
                                //Debug.Log("close");
                                cellMove.isOpen = false;
                            }
                        }

                        castingRay = false;
                    }
                    else
                    {
                        Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, Color.red, 50);
                        castingRay = false;

                    }

                    if (Physics.Raycast(context.HatchesWayPoints[u].transform.position + offsetTrap, context.HatchesWayPoints[u].transform.forward, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                    {
                        Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, context.HatchesWayPoints[u].transform.forward, Color.green, 50);
                        cP.okToSetup = true;


                        //Debug.Log(hit.transform.parent.name);

                        if (hit.transform.parent.GetComponent<CellMovement>())
                        {
                            //Debug.Log("parentparentconatinsCellmovement");

                            CellMovement cellMove = hit.transform.parent.GetComponent<CellMovement>();

                            if (cellMove.isOpen == false)
                            {
                                //Debug.Log("here is a door i could take" + hit.transform.parent.name);
                                cellMove.isOpen = true;
                            }
                            else if (cellMove.isOpen == true)
                            {
                                //Debug.Log("close");
                                cellMove.isOpen = false;
                            }
                        }
                        castingRay = false;

                    }
                    else
                    {
                        Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, context.HatchesWayPoints[u].transform.forward, Color.red, 50);
                        castingRay = false;

                    }
                }

            }
        }


        if (add)
        {
            if (context.doorWayPoints.Count != 0)
            {
                //myDoorList = context.doorWayPoints;
                //Debug.Log("Je me tire Second ray");

                for (int u = 0; u < context.doorWayPoints.Count; u++)
                {

                    RaycastHit hit;
                    int layerMaskDoor = LayerMask.GetMask("Door");


                    if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, out hit, 5, layerMaskDoor))
                    {
                        //Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.green, 050);
                        //cP.okToSetup = true;

                        myDoor = context.doorWayPoints[u];
                        doorDirection = (hit.collider.transform);
                        castingRay = false;
                    }
                    else
                    {
                        //Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.red, 50);
                        castingRay = false;

                    }

                    if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.up, out hit, 5, layerMaskDoor))
                    {
                        //Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.green, 50);
                        myDoor = context.doorWayPoints[u];
                        doorDirection = (hit.collider.transform);
                        castingRay = false;

                    }
                    else
                    {
                        //Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.up, Color.red, 50);
                        castingRay = false;

                    }


                }

                if (doorDirection != null && myDoor != null && nextContext != null)
                {
                    CheckList();
                }

                add = false;

            }

            if (context.HatchesWayPoints.Count != 0)
            {
                for (int u = 0; u < context.HatchesWayPoints.Count; u++)
                {

                    RaycastHit hit;
                    int layerMaskDoor = LayerMask.GetMask("Door");


                    if (Physics.Raycast(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, out hit, 5, layerMaskDoor))
                    {
                        Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, Color.green, 050);
                        //cP.okToSetup = true;

                        myDoor = context.HatchesWayPoints[u];
                        doorDirection = (hit.collider.transform);
                        castingRay = false;
                    }
                    else
                    {
                        Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, Color.red, 50);
                        castingRay = false;

                    }

                    if (Physics.Raycast(context.HatchesWayPoints[u].transform.position + offsetTrap, context.HatchesWayPoints[u].transform.forward, out hit, 5, layerMaskDoor))
                    {
                        Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, Color.green, 50);
                        myDoor = context.HatchesWayPoints[u];
                        doorDirection = (hit.collider.transform);
                        castingRay = false;

                    }
                    else
                    {
                        Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, context.HatchesWayPoints[u].transform.forward, Color.red, 50);
                        castingRay = false;

                    }


                }

                if (doorDirection != null && myDoor != null && nextContext != null)
                {
                    CheckList();
                }

                add = false;
            }
        }



        #endregion

        #region isTrapIn

        /*  if (context.HatchesWayPoints.Count != 0)
          {*/
        if (castingRay)
        {

        }

        if (add)
        {
            //myDoorList = context.doorWayPoints;
            //Debug.Log("Je me tire Second ray");

            


        }


        // }


        #endregion



        boolIndex = Mathf.Clamp(boolIndex, 0, ways.Count);

        if (ways.Count < context.paths.list.Count)
        {
            ways.Add(new bool());
        }
        if (ways.Count > context.paths.list.Count)
        {
            ways.RemoveAt(ways.Count - 1);
        }



        if (reset)
        {
            ResetWhenTooFar();

            if (x < 1 && y < 1)
            {
                y += 0.05f;
                x += 0.05f;

            }
            //animator.SetBool("Walk", true);


        }



        for (int i = 0; i < ways.Count; i++)
        {
            if (ways[i])
            {
                myPath = i;
                GetPath(myPath);
                ways[i] = false;
            }
        }

        if (movement && !context.gameObject.GetComponent<CellMovement>().once)
        {

            Movement(waypoints.listOfWaypoint);

        }
        /*
        if(!context.gameObject.GetComponent<CellMovement>().once)
        {
            waypoints.listOfWaypoint.Clear();
        }*/

        if (openDoor)
        {

            doorRot = nextDoor.transform.localEulerAngles;

            if (nextDoor.transform.localEulerAngles.y < 90)
            {
                nextDoor.transform.localEulerAngles = new Vector3(0, Mathf.LerpAngle(doorRot.y, doorRot.y + 90, Time.deltaTime * animspeed), transform.localEulerAngles.z);

            }
            else
            {
                doorRot = nextDoor.transform.localEulerAngles;


                isValid = false;
                openDoor = false;
            }


        }


        if (closeDoor)
        {




            if (door.transform.localEulerAngles.y >= -90)
            {
                door.transform.localEulerAngles = new Vector3(0, Mathf.LerpAngle(doorRot.y, doorRot.y - 119.984f, Time.deltaTime * animspeed), transform.localEulerAngles.z);

                //door.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                doorRot = door.transform.localEulerAngles;
                //door = null;
                closeDoor = false;
            }

        }
    }

    public void CheckConeLight()
    {

        if (checkOpenDoor)
        {

            if (context.gameObject.GetComponent<CellMovement>() != null)
            {
                if (context.gameObject.GetComponent<CellMovement>().hasEnded == true)
                {
                    // Debug.Log("           I draw          ");

                    RaycastHit hit;
                    int layerMaskDoor = LayerMask.GetMask("Door");

                    for (int u = 0; u < context.doorWayPoints.Count; u++)
                    {
                        if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                        {
                            //    Debug.Log("                     " + hit.transform.name);

                            //Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.blue, 500);

                            coneDoor = hit.transform.gameObject;
                            if (hit.transform.parent.GetComponent<CellScript>().coneRed.Count != 0)
                            {
                                hit.transform.parent.GetComponent<CellScript>().ConeFunction(0);
                                hit.transform.parent.GetComponent<CellScript>().freeRoom = true;

                                transform.parent.GetComponent<CellScript>().ConeFunction(0);
                                transform.parent.GetComponent<CellScript>().freeRoom = true;
                            }


                            if (checkInt >= 3)
                            {
                                checkOpenDoor = false;
                                checkInt = 0;
                            }
                            else
                            {
                                checkInt++;
                            }
                        }
                        else
                        {
                            //   Debug.Log("            I touch Nothing         ");

                            //Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.black, 500);
                            if (transform.parent.GetComponent<CellScript>().coneRed.Count != 0)
                            {
                                transform.parent.GetComponent<CellScript>().ConeFunction(0);
                                transform.parent.GetComponent<CellScript>().freeRoom = false;

                                if (coneDoor != null && coneDoor.transform.parent.GetComponent<CellScript>().coneRed.Count != 0)
                                {
                                    coneDoor.transform.parent.GetComponent<CellScript>().freeRoom = false;
                                    coneDoor.transform.parent.GetComponent<CellScript>().ConeFunction(0);
                                }
                            }

                            if (checkInt >= 3)
                            {
                                coneDoor = null;
                                checkOpenDoor = false;
                                checkInt = 0;
                            }
                            else
                            {
                                checkInt++;
                            }
                        }
                    }
                }
            }
        }

        if (!checkOpenDoor)
        {

            timerOpenDoor++;

            if (timerOpenDoor > 120)
            {
                checkOpenDoor = true;
            }

        }

    }

    public void GetPath(int path)
    {
        if (path < context.paths.list.Count)
        {
            waypoints = context.paths.list[path];
        }
        myPath = 4;
        movement = true;
    }

    private void ResetWhenTooFar()
    {


        if (Vector3.Distance(transform.position, context.basePos.position) >= 0.1f && !movement)
        {
            float curvePercent = animCurve.Evaluate(Time.deltaTime * speedModifier);
            transform.position = Vector3.Lerp(transform.position, context.basePos.position, curvePercent); /// 0.05f
            Vector3 targetPos = new Vector3(context.basePos.position.x, transform.position.y, context.basePos.position.z);
            transform.LookAt(targetPos);

        }

        if (Vector3.Distance(transform.position, context.basePos.position) <= 0.1f)
        {
            reset = false;

            if (x > 0 && y > 0)
            {
                y -= 0.05f;
                x -= 0.05f;

            }
            //animator.SetBool("Walk", false);
            lookCam = true;
            one = true;
        }

        for (int i = 0; i < Rooms.Count; i++)
        {
            CellScr[i].playerMoving = false;
            Rooms[i].hasEnded = true;
            Rooms[i].once = true;
        }
        // Debug.Log("ResetWhenTooFar");
        oneRef = true;
        oneRef01 = true;
        oneRef02 = true;


    }

    public void CalculateDistance(Vector3 objectif)
    {

        distance = Vector3.Distance(transform.position, objectif);
        if (distance > minDist)
        {
            // Debug.Log("Distanceok");
            next = true;
        }

    }


    public void CheckList()
    {


        for (int i = 0; i < context.paths.list.Count; i++)
        {
            if (context.paths.list[i].listOfWaypoint.Contains(myDoor) && oneRef)
            {
                for (int u = 0; u < context.paths.list[i].listOfWaypoint.Count; u++)
                {
                    //Debug.Log("FirstToPatch");
                    ultimateList.Add(context.paths.list[i].listOfWaypoint[u]);
                }

                oneRef = false;
            }
        }

        for (int h = 0; h < nextContext.paths.list.Count; h++)
        {
            if (nextContext.paths.list[h].listOfWaypoint.Contains(doorDirection))
            {
                if (inverseList.Count != nextContext.paths.list[h].listOfWaypoint.Count)
                {
                    for (int u = 0; u < nextContext.paths.list[h].listOfWaypoint.Count; u++)
                    {
                        inverseList.Add(nextContext.paths.list[h].listOfWaypoint[u]);
                    }
                }

                if (inverseList.Count == nextContext.paths.list[h].listOfWaypoint.Count)
                {
                    inverseList.Reverse();
                }

            }
        }

        if (oneRef01)
        {
            for (int t = 0; t < inverseList.Count; t++)
            {
                ultimateList.Add(inverseList[t]);
            }
            oneRef01 = false;
        }

        //Debug.Log("LastToPatch");

        if (oneRef02)
        {
            waypoints.listOfWaypoint = ultimateList;
        }
        movement = true;
        //Movement(waypoints.listOfWaypoint);



    }







    float curvePercent;
    private int timer;

    public void Movement(List<Transform> listToMove)
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            Rooms[i].hasEnded = false;
            Rooms[i].once = false;
            CellScr[i].playerMoving = true;
        }


        lookCam = false;
        //print(listToMove[0]);

        //AllPlayerMovement
        if (listToMove.Count != 0)
        {
            //index = 0;

            //Check distance btw player and waypoints
            CalculateDistance(listToMove[index].position);

            //gets the originPos to reset character pos if there is a problem;
            if (!once)
            {
                origin = transform.position;
                once = true;
            }

            //Actual movement to selected waypoint
            curvePercent = animCurve.Evaluate(Time.deltaTime * speedModifier);
            transform.position = Vector3.Lerp(transform.position, listToMove[index].position, curvePercent); ///0.05f

            if (x < 1 && y < 1)
            {
                y += 0.05f;
                x += 0.05f;

            }

            animator.SetBool("Walk", true);

            Vector3 targetPos = new Vector3(listToMove[index].transform.position.x, transform.position.y, listToMove[index].transform.position.z);

            transform.LookAt(targetPos/*listToMove[index].transform.position*/);


            //Select the next waypoint when last is reached
            if (distance <= minDist && index != listToMove.Count - 1 && next)
            {
                //once = false;
                //movement = false;
                //reset = true;
                index++;
                next = false;
            }
            else if (distance <= minDist && index == listToMove.Count - 1)        //if last point is reached, check if door 
            {
                index = 0;
                // transform.LookAt(listToMove[listToMove.Count - 1].transform.position);

                context = nextContext;
                nextContext = null;
                once = false;
                next = false;

                waypoints.listOfWaypoint.Clear();
                ultimateList.Clear();
                inverseList.Clear();
                myDoor = null;
                doorDirection = null;

                isValid = true;

                //onlyOne = true;
                reset = true;                                                                                                                                 //////////////////
                add = false;
                movement = false;

                //  door = listToMove[listToMove.Count - 1].gameObject;
                return;

            }

            if (index + 1 < listToMove.Count)
            {
                if (listToMove[index + 1].name.Contains("Door"))
                {
                    nextDoor = listToMove[index + 1].gameObject;
                    // openDoor = true;

                }
            }

            if (index - 1 > -1)
            {
                if (listToMove[index - 1].name.Contains("Door"))
                {
                    door = listToMove[index - 1].gameObject;
                    // closeDoor = true;

                }
            }

        }
    }
}
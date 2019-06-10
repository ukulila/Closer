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
    public List<Transform> freeDoor;
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

    //public Transform RoomChecker;
    public int onlyTwo;
    public bool checkOpenDoor;
    private int checkInt;
    public AlwaysLookAtCam highlight;
    public Vector3 rotationHighlight;
    private bool one;
    public AnimationCurve animCurve;
    public float speedModifier;

    public List<CellMovement> Rooms;
    public List<CellScript> CellScr;

    public float ctrlTime = 0;


    public bool oneRef;
    public bool oneRef01;
    public bool oneRef02;
    public bool oneList;
    public bool oneList02;

    public Vector3 offsetTrap;
    public int timerOpenDoor;
    //private bool doorBool;
    //private bool HatchesBool;
    public int DoorsToCheck;

    public static PlayerBehaviour Instance;
    public List<GameObject> childs;


    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < transform.childCount-1; i++)
        {
            childs.Add(transform.GetChild(i).gameObject);
        }
    }

    void Start()
    {
        moveEnded = true;

        oneRef = true;
        oneRef01 = true;
        oneRef02 = true;
        oneList = true;
        oneList02 = true;

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

        //CheckConeLight();

        if (lookCam)
        {
            Vector3 camX = new Vector3(Camera.transform.position.x, transform.position.y, Camera.transform.position.z);
            transform.LookAt(camX);

            highlight.gameObject.SetActive(true);
            //highlight.enabled = false;


            if (one)
            {
                highlight.transform.localEulerAngles = rotationHighlight;

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
            highlight.gameObject.SetActive(false);
            //highlight.enabled = true;

        }

        #region isDoorIn


        if (castingRay && moveEnded)
        {
            if (context.doorWayPoints.Count != 0)
            {
                for (int u = 0; u < context.doorWayPoints.Count; u++)
                {
                    RaycastHit hit;
                    int layerMaskDoor = LayerMask.GetMask("Door");


                    if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                    {
                     //   Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.green, 50);
                        cP.okToSetup = true;


                        // doorBool = true;
                        // HatchesBool = false;

                        if (hit.transform.parent.GetComponent<CellMovement>() && !hit.transform.parent.GetComponent<OpacityKiller>().BlackRoom)
                        {
                          //  Debug.Log("parentparentconatinsCellmovement");

                            CellMovement cellMove = hit.transform.parent.GetComponent<CellMovement>();

                            if (cellMove.isOpen == false)
                            {
                                cellMove.isOpen = true;
                            }
                            else
                            {
                                cellMove.isOpen = false;
                            }
                        }

                        castingRay = false;
                    }
                    else
                    {
                       // Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.red, 50);
                        castingRay = false;

                    }

                    if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.up, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                    {
                       // Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.up, Color.green, 50);
                        cP.okToSetup = true;

                        // doorBool = true;
                        // HatchesBool = false;

                        //Debug.Log(hit.transform.parent.name);

                        if (hit.transform.parent.GetComponent<CellMovement>() && !hit.transform.parent.GetComponent<OpacityKiller>().BlackRoom)
                        {
                            //Debug.Log("parentparentconatinsCellmovement");

                            CellMovement cellMove = hit.transform.parent.GetComponent<CellMovement>();

                            if (cellMove.isOpen == false)
                            {
                                cellMove.isOpen = true;
                            }
                            else
                            {
                                cellMove.isOpen = false;
                            }
                        }
                        castingRay = false;

                    }
                    else
                    {
                        //Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.up, Color.red, 50);
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


                    if (Physics.Raycast(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                    {
                        //Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, Color.green, 50);
                        cP.okToSetup = true;


                        // HatchesBool = true;
                        // doorBool = false;

                        if (hit.transform.parent.GetComponent<CellMovement>())
                        {
                           // Debug.Log("parentparentconatinsCellmovement");

                            CellMovement cellMove = hit.transform.parent.GetComponent<CellMovement>();

                            if (cellMove.isOpen == false)
                            {
                                cellMove.isOpen = true;
                            }
                            else
                            {
                                cellMove.isOpen = false;
                            }
                        }

                        castingRay = false;
                    }
                    else
                    {
                     //   Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, Color.red, 50);
                        castingRay = false;

                    }

                    if (Physics.Raycast(context.HatchesWayPoints[u].transform.position + offsetTrap, context.HatchesWayPoints[u].transform.forward, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                    {
                       // Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, context.HatchesWayPoints[u].transform.forward, Color.green, 50);
                        cP.okToSetup = true;

                        // HatchesBool = true;
                        // doorBool = false;
                        //Debug.Log(hit.transform.parent.name);

                        if (hit.transform.parent.GetComponent<CellMovement>())
                        {
                         //   Debug.Log("parentparentconatinsCellmovement");

                            CellMovement cellMove = hit.transform.parent.GetComponent<CellMovement>();

                            if (cellMove.isOpen == false)
                            {
                                cellMove.isOpen = true;
                            }
                            else
                            {
                                cellMove.isOpen = false;

                            }
                        }
                        castingRay = false;

                    }
                    else
                    {
                      //  Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, context.HatchesWayPoints[u].transform.forward, Color.red, 50);
                        castingRay = false;

                    }
                }

            }



        }


        if (add)
        {
            moveEnded = false;
            movement = false;
            reset = true;

            for (int i = 0; i < Rooms.Count; i++)
            {
                if (Rooms[i] != nextContext.gameObject)
                {
                    Rooms[i].isOpen = false;
                }
            }

            /*  if (doorBool)
              {*/

            if (context.doorWayPoints.Count != 0)
            {
                //myDoorList = context.doorWayPoints;
                //Debug.Log("Je me tire Second ray");

                for (int u = 0; u < context.doorWayPoints.Count; u++)
                {

                    RaycastHit hit;
                    int layerMaskDoor = LayerMask.GetMask("Door");

                    // Debug.Log("DoorWaypointCount");

                    if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject && hit.transform.parent.gameObject == nextContext.gameObject)
                    {
                        Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.green, 050);
                        //cP.okToSetup = true;
                        Debug.Log("1st raycastHit" + hit.collider.transform.name);


                        if (nextContext.GetComponent<ScrEnvironment>().doorWayPoints.Contains(hit.collider.transform))
                        {
                            doorDirection = (hit.collider.transform);
                            myDoor = context.doorWayPoints[u];

                        }

                        castingRay = false;
                    }
                    else
                    {
                        Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.red, 50);
                        castingRay = false;
                        Debug.Log("1st NoRaycastHit");

                    }

                    if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.up, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject && hit.transform.parent.gameObject == nextContext.gameObject)
                    {
                        Debug.Log("2st RaycastHit");

                        Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.green, 50);

                        if (nextContext.GetComponent<ScrEnvironment>().doorWayPoints.Contains(hit.collider.transform))
                        {
                            myDoor = context.doorWayPoints[u];

                            doorDirection = (hit.collider.transform);
                        }

                        castingRay = false;

                    }
                    else
                    {
                        Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.up, Color.red, 50);
                        castingRay = false;
                        Debug.Log("2st NoRaycastHit");

                    }


                }

                if (doorDirection != null && myDoor != null && nextContext != null)
                {
                    // Debug.Log("2st NoRaycastHit");

                    CheckList();
                }

                DoorsToCheck -= 1;
                if (DoorsToCheck <= 0)
                {
                    add = false;
                }

                //  }
            }
            //////////////////////////////////////////////                                    //////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////// Hatches Only /////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////                                    /////////////////////////////////////////////////////

            /*       if (HatchesBool)
                   {*/
            if (context.HatchesWayPoints.Count != 0)
            {
                for (int u = 0; u < context.HatchesWayPoints.Count; u++)
                {

                    RaycastHit hit;
                    int layerMaskDoor = LayerMask.GetMask("Door");


                    if (Physics.Raycast(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject && hit.transform.parent.gameObject == nextContext.gameObject)
                    {
                        //Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, Color.green, 050);
                        //cP.okToSetup = true;


                        if (nextContext.GetComponent<ScrEnvironment>().HatchesWayPoints.Contains(hit.collider.transform))
                        {
                            doorDirection = (hit.collider.transform);
                            myDoor = context.HatchesWayPoints[u];

                        }

                        castingRay = false;
                    }
                    else
                    {
                        //Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, Color.red, 50);
                        castingRay = false;

                    }

                    if (Physics.Raycast(context.HatchesWayPoints[u].transform.position + offsetTrap, context.HatchesWayPoints[u].transform.forward, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject && hit.transform.parent.gameObject == nextContext.gameObject)
                    {
                        //Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, -context.HatchesWayPoints[u].transform.forward, Color.green, 50);
                        myDoor = context.HatchesWayPoints[u];

                        if (nextContext.GetComponent<ScrEnvironment>().HatchesWayPoints.Contains(hit.collider.transform))
                        {
                            doorDirection = (hit.collider.transform);
                        }

                        castingRay = false;

                    }
                    else
                    {
                        //Debug.DrawRay(context.HatchesWayPoints[u].transform.position + offsetTrap, context.HatchesWayPoints[u].transform.forward, Color.red, 50);
                        castingRay = false;

                    }


                }

                if (doorDirection != null && myDoor != null && nextContext != null)
                {
                    CheckList();
                }

                DoorsToCheck -= 1;
                if (DoorsToCheck <= 0)
                {
                    add = false;
                }
            }


            // }
        }



        #endregion

        #region isTrapIn

        /*  if (context.HatchesWayPoints.Count != 0)
          {*/
        /*  if (castingRay)
          {

          }

          if (add)
          {
              //myDoorList = context.doorWayPoints;
              //Debug.Log("Je me tire Second ray");




          }
          */

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
            moveEnded = false;

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
    /*
    public void CheckConeLight()
    {

        if (checkOpenDoor)
        {
            freeDoor.Clear();
            if (context.gameObject.GetComponent<CellMovement>() != null)
            {

                if (context.gameObject.GetComponent<CellMovement>().hasEnded == true)
                {
                    //Debug.Log("           I draw          ");

                    RaycastHit hit;
                    int layerMaskDoor = LayerMask.GetMask("Door");

                    for (int u = 0; u < context.doorWayPoints.Count; u++)
                    {
                        if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                        {
                            

                            if (freeDoor.Contains(hit.transform) == false)
                            {
                                freeDoor.Add(hit.transform);
                            }

                            //Debug.Log(coneDoor);
                            context.doorWayPoints[u].GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);

                            for (int d = 0; d < freeDoor.Count; d++)
                            {
                                freeDoor[d].GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);
                            }
                            checkOpenDoor = false;

                        }
                        else
                        {
                          //  Debug.Log("            I touch Nothing         ");

                           // Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.up, Color.black, 500);

                            context.doorWayPoints[u].GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);

                            if (freeDoor.Count >= u && freeDoor.Count != 0)
                            {
                                for (int i = 0; i < freeDoor.Count; i++)
                                {
                                    freeDoor[i].GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
                                }
                                //coneDoor = null;
                            }

                            checkOpenDoor = false;

                        }
                    }

                    for (int z = 0; z < context.HatchesWayPoints.Count; z++)
                    {

                        if (Physics.Raycast(context.HatchesWayPoints[z].GetChild(0).transform.position, -context.HatchesWayPoints[z].transform.forward, out hit, 5, layerMaskDoor) && hit.transform.parent.gameObject != context.gameObject)
                        {
                            //Debug.Log("                     " + hit.transform.name);

                            // Debug.DrawRay(context.HatchesWayPoints[z].GetChild(0).transform.position, -context.HatchesWayPoints[z].transform.forward, Color.blue, 500);

                            if (freeDoor.Count != 0 && freeDoor.Contains(hit.transform) == false)
                            {
                                freeDoor.Add(hit.transform);
                            }
                            context.HatchesWayPoints[z].GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);

                            for (int i = 0; i < freeDoor.Count; i++)
                            {
                                freeDoor[i].GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);
                            }

                            checkOpenDoor = false;

                        }
                        else
                        {
                            //Debug.Log("            I touch Nothing         ");

                           // Debug.DrawRay(context.HatchesWayPoints[z].GetChild(0).transform.position, -context.HatchesWayPoints[z].transform.forward, Color.black, 500);

                            context.HatchesWayPoints[z].GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);

                            if ( freeDoor.Count >= z)
                            {
                                freeDoor[z].GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
                                //freeDoor = null;
                            }

                            checkOpenDoor = false;

                        }

                    }
                }
            }
            Debug.Log(freeDoor.Count);
        }

        if (!checkOpenDoor)
        {
            timerOpenDoor++;

            if (timerOpenDoor > 120)
            {
                checkOpenDoor = true;
                timerOpenDoor = 0;
            }

         /*   if (freeDoor.Count != 0 && freeDoor.Count >= 8)
            {
                for (int i = 0; i < freeDoor.Count; i++)
                {

                    freeDoor[i].GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);

                }
                checkOpenDoor = true;

                freeDoor.Clear();
            }
        }

    }*/

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
            moveEnded = true;



            lookCam = true;
            one = true;

            for (int i = 0; i < Rooms.Count; i++)
            {
                CellScr[i].playerMoving = false;

                Rooms[i].hasEnded = true;
                Rooms[i].once = true;
            }

        }


        //  Debug.Log("ResetWhenTooFar");

        //moveEnded = true;

    }

    public void CalculateDistance(Vector3 objectif)
    {

        distance = Vector3.Distance(transform.position, objectif);
        if (distance < minDist)
        {
            //Debug.Log("Distanceok");
            ctrlTime = 0f;
            next = true;
        }

    }


    public void CheckList()
    {

        if (oneList)
        {
            for (int i = 0; i < context.paths.list.Count; i++)
            {
                if (context.paths.list[i].listOfWaypoint.Contains(myDoor) && oneRef)
                {
                    for (int u = 0; u < context.paths.list[i].listOfWaypoint.Count; u++)
                    {
                        // Debug.Log("FirstToPatch");
                        ultimateList.Add(context.paths.list[i].listOfWaypoint[u]);

                    }
                    oneList = false;
                    oneRef = false;
                }
            }
        }

        if (oneList02)
        {
            for (int h = 0; h < nextContext.paths.list.Count; h++)
            {
                if (nextContext.paths.list[h].listOfWaypoint.Contains(doorDirection))
                {
                    if (inverseList.Count != nextContext.paths.list[h].listOfWaypoint.Count)
                    {
                        for (int u = 0; u < nextContext.paths.list[h].listOfWaypoint.Count; u++)
                        {
                            inverseList.Add(nextContext.paths.list[h].listOfWaypoint[u]);
                            //    Debug.Log("SecondToPatch");

                        }
                    }

                    if (inverseList.Count == nextContext.paths.list[h].listOfWaypoint.Count)
                    {
                        inverseList.Reverse();
                        oneList02 = false;
                    }

                }
            }
        }

        if (oneRef01)
        {
            for (int t = 0; t < inverseList.Count; t++)
            {
                ultimateList.Add(inverseList[t]);
                //  Debug.Log("LastToPatch");
            }
            oneRef01 = false;
        }



        if (oneRef02)
        {
            waypoints.listOfWaypoint = ultimateList;
            //   Debug.Log("TurboLast");
            oneRef02 = false;
        }
        movement = true;
        //Movement(waypoints.listOfWaypoint);



    }







    float curvePercent;
    private int timer;
    public bool moveEnded;

    public void Movement(List<Transform> listToMove)
    {
        reset = false;

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
            curvePercent = animCurve.Evaluate(ctrlTime);
            ctrlTime += speedModifier;
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
                ctrlTime = 0;
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
                door = null;
                nextDoor = null;

                isValid = true;

                //onlyOne = true;
                reset = true;                                                                                                                                 //////////////////
                add = false;
                movement = false;
                oneList = true;
                oneList02 = true;

                oneRef = true;
                oneRef02 = true;
                oneRef01 = true;

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

                if ( listToMove[index-1].name.Contains("Porte") && listToMove[index].name.Contains("Porte"))
                {
                    for (int i = 0; i < childs.Count; i++)
                    {
                        childs[i].SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < childs.Count; i++)
                    {
                        childs[i].SetActive(true);
                    }
                }
            }


        }
    }
}
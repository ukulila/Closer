using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool reset;
    private bool lookCam;
    public bool movement;
    public Vector3 origin;
    public bool once;
    public Lister waypoints;
    public ScrEnvironment context;
    public ScrEnvironment nextContext;
    public float distance;
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
    public List<Transform> doorDirection;
    public bool isValid;
    public List<Transform> inverseList;
    public List<Transform> myDoorList;
    public bool reverse;
    private Vector3 doorRot;
    public bool closeDoor;
    public bool add;

    public CinemachineBrain Camera;
    public List<Transform> ultimateList;


    void Start()
    {
        minDist = 0.1f;

    }

    void Update()
    {
        if(lookCam)
        {
            transform.LookAt(Camera.transform.position);
        }


        if (context.touched && nextContext != context && !add)
        {
            //myDoorList = context.doorWayPoints;

            for (int u = 0; u < context.doorWayPoints.Count; u++)
            {

                RaycastHit hit;
                int layerMaskDoor = LayerMask.GetMask("Door");


                if (Physics.Raycast(context.doorWayPoints[u].transform.position , -context.doorWayPoints[u].transform.forward, out hit, Mathf.Infinity, layerMaskDoor))
                {
                    Debug.DrawRay(context.doorWayPoints[u].transform.position , -context.doorWayPoints[u].transform.forward, Color.red, 500.0F);
                    myDoorList.Add(context.doorWayPoints[u]);
                    doorDirection.Add(hit.collider.transform.parent.transform);
                }
                else
                {
                    Debug.DrawRay(context.doorWayPoints[u].transform.position, -context.doorWayPoints[u].transform.forward, Color.red, 500.0F);

                }

                if (Physics.Raycast(context.doorWayPoints[u].transform.position, context.doorWayPoints[u].transform.forward, out hit, Mathf.Infinity, layerMaskDoor))
                {
                    Debug.DrawRay(context.doorWayPoints[u].transform.position, -context.doorWayPoints[u].transform.forward, Color.red, 500.0F);
                    myDoorList.Add(context.doorWayPoints[u]);
                    doorDirection.Add(hit.collider.transform.parent.transform);
                }
                else
                {
                    Debug.DrawRay(context.doorWayPoints[u].transform.position, context.doorWayPoints[u].transform.forward, Color.red, 500.0F);

                }


            }
            if (doorDirection.Count != 0 && myDoorList.Count != 0)
            {
                CheckList();
            }
            add = true;
            

        }





        /*if(Input.GetKeyDown(KeyCode.A))
        {

           // Debug.LogError(context.paths.list.Count);

        }*/

        boolIndex = Mathf.Clamp(boolIndex, 0, ways.Count);

        if (ways.Count < context.paths.list.Count)
        {
            ways.Add(new bool());
        }
        if (ways.Count > context.paths.list.Count)
        {
            ways.RemoveAt(ways.Count - 1);
        }

        /* = context.paths.list;*/

        if (reset)
        {
            ResetWhenTooFar();
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

        if (movement)
        {
            Movement(waypoints.listOfWaypoint);

            /*
            //AllPlayerMovement
            if (waypoints.listOfWaypoint.Count != 0)
            {

                //Check distance btw player and waypoints
                CalculateDistance(waypoints.listOfWaypoint[index].position);

                //direction = waypoints[0].position;
                if (!once)
                {
                    origin = transform.position;
                    once = true;
                }

                //Actual movement to selected waypoint
                transform.position = Vector3.Lerp(transform.position, waypoints.listOfWaypoint[index].position, 0.05f);
                transform.LookAt(waypoints.listOfWaypoint[index].transform.position);


                //Select the next waypoint when last is reached
                if (distance <= minDist && index != waypoints.listOfWaypoint.Count - 2 && next)
                {
                    //once = false;
                    //movement = false;
                    //reset = true;
                    index++;
                    next = false;
                }
                else if (distance <= minDist && index == waypoints.listOfWaypoint.Count - 2)        //if last point is reached, check if door 
                {
                    index = 0;
                    movement = false;
                    transform.LookAt(waypoints.listOfWaypoint[waypoints.listOfWaypoint.Count - 1].transform.position);

                    context = nextContext;
                    isValid = true;

                    reset = true;
                    door = waypoints.listOfWaypoint[waypoints.listOfWaypoint.Count - 1].gameObject;




                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(transform.position, waypoints.listOfWaypoint[index].transform.position/*transform.forward, 100.0F);
                    Debug.DrawRay(transform.position, transform.forward, Color.red, 500.0F);
                    for (int i = 0; i < hits.Length; i++)
                    {
                        RaycastHit hit = hits[i];

                        if (hit.transform.tag == "Cell" && hit.transform.name != context.transform.name)
                        {
                            Debug.Log("true, it's   " + hit.transform.name);
                            isValid = true;
                            context = hit.transform.GetComponent<ScrEnvironment>();
                        }

                        /* else
                         {
                             waypoints.listOfWaypoint.Reverse();
                             baseMovement = true;
                         }

                    }

                    /*

                    RaycastHit[] hit;
                if (Physics.RaycastAll(/*door.transform.position, new Vector3 (0,0, -Vector3.Distance(door.transform.position, transform.position) * 10), out hit))
                {

                    Debug.DrawRay(/*door.transform.position, new Vector3(0, 0, -Vector3.Distance(door.transform.position, transform.position) * 10), Color.red, 10);
                    if (hit.transform.tag == "Cell" && hit.transform.name != context.gameObject.name)
                    {
                        isValid = true;
                        context = hit.transform.GetComponent<ScrEnvironment>();
                    }
                }

                    if (waypoints.listOfWaypoint[waypoints.listOfWaypoint.Count - 1].name.Contains("Door") && isValid)
                    {

                        openDoor = true;
                    }
                    else
                    {
                        waypoints.listOfWaypoint.Reverse();
                        reverse = true;
                        baseMovement = true;
                    }
                    }
                }*/

        }

        if (baseMovement)
        {
            if (waypoints.listOfWaypoint.Count != 0)
            {
                CalculateDistance(waypoints.listOfWaypoint[index].position);
                //direction = waypoints[0].position;
                if (!once)
                {
                    origin = transform.position;
                    once = true;
                }

                transform.position = Vector3.Lerp(transform.position, waypoints.listOfWaypoint[index].position, 0.05f);
                transform.LookAt(waypoints.listOfWaypoint[index].position);


                if (distance <= minDist && index != waypoints.listOfWaypoint.Count - 1 && next)
                {
                    //once = false;
                    //movement = false;
                    //reset = true;
                    index++;
                    next = false;


                }
                else if (distance <= minDist && index == waypoints.listOfWaypoint.Count - 1)
                {
                    index = 0;
                    movement = false;

                    if (reverse)
                    {
                        reset = true;
                        //waypoints.listOfWaypoint.Clear();
                        reverse = false;
                        baseMovement = false;
                    }

                    if (nextContext != null)
                    {
                        context = nextContext;
                        nextContext = null;
                        baseMovement = false;

                    }

                    /*
                    if (waypoints.listOfWaypoint[waypoints.listOfWaypoint.Count - 1].name.Contains("Door"))
                    {

                        openDoor = true;

                    }
                    */
                }
                else
                {



                }
            }
        }

        if (openDoor)
        {


            if (nextDoor.transform.localEulerAngles.y < 90)
            {
                doorRot = nextDoor.transform.localEulerAngles;
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
            if (door.transform.localEulerAngles.y >= 5)
            {
                doorRot = door.transform.localEulerAngles;
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
            transform.position = Vector3.Lerp(transform.position, context.basePos.position, 0.05f);
            transform.LookAt(context.basePos.position);

        }
        else
        {
            reset = false;
            
            lookCam = true;
        }
    }

    public void CalculateDistance(Vector3 objectif)
    {

        distance = Vector3.Distance(transform.position, objectif);
        if (distance > minDist)
        {
            next = true;
        }

    }



    public void CheckList()
    {


        for (int i = 0; i < context.paths.list.Count; i++)
        {
            if (context.paths.list[i].listOfWaypoint.Contains(myDoorList[0]))
            {
                for (int u = 0; u < context.paths.list[i].listOfWaypoint.Count; u++)
                {
                    ultimateList.Add(context.paths.list[i].listOfWaypoint[u]);
                }
            }
        }

        for (int h = 0; h < nextContext.paths.list.Count; h++)
        {
            if (nextContext.paths.list[h].listOfWaypoint.Contains(doorDirection[0]))
            {

                for (int u = 0; u < nextContext.paths.list[h].listOfWaypoint.Count; u++)
                {
                    inverseList.Add(nextContext.paths.list[h].listOfWaypoint[u]);
                    inverseList.Reverse();

                }

            }
        }

        for (int t = 0; t < inverseList.Count; t++)
        {
            ultimateList.Add(inverseList[t]);
        }

        waypoints.listOfWaypoint = ultimateList;
        movement = true;
        //Movement(waypoints.listOfWaypoint);



    }











    public void Movement(List<Transform> listToMove)
    {
        lookCam = false;
        print(listToMove[0]);

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
            transform.position = Vector3.Lerp(transform.position, listToMove[index].position, 0.05f);
            transform.LookAt(listToMove[index].transform.position);


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
                //nextContext = null;
                once = false;
                next = false;

                waypoints.listOfWaypoint.Clear();
                ultimateList.Clear();
                inverseList.Clear();
                myDoorList.Clear();
                doorDirection.Clear();

                isValid = true;

                reset = true;
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
                    openDoor = true;

                }
            }

            if (index - 1 > -1)
            {
                if (listToMove[index - 1].name.Contains("Door"))
                {
                    door = listToMove[index - 1].gameObject;
                    closeDoor = true;

                }
            }
            /*if (listToMove[listToMove.Count - 1].name.Contains("Door") && isValid)
            {

                openDoor = true;
            }
            else
            {
                listToMove.Reverse();
                reverse = true;
                baseMovement = true;
            }*/
        }


        /*
        public void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<ScrEnvironment>() != null)
            {

                context = other.GetComponent<ScrEnvironment>();

            }
        }*/
    }
}

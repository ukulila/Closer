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
    public bool add;

    public CinemachineBrain Camera;
    public List<Transform> ultimateList;
    public bool castingRay;
    public CellPlacement cP;
    public Vector3 offset;
    private bool onlyOne;

    void Start()
    {
        add = false;
        reset = true;
        onlyOne = true;

    }

    void Update()
    {
        if (lookCam)
        {
            reset = true;
            transform.LookAt(Camera.transform.position);
            onlyOne = false;
        }


        if (castingRay)
        {
            for (int u = 0; u < context.doorWayPoints.Count; u++)
            {
                Debug.Log("Je me tire premier ray");
                RaycastHit hit;
                int layerMaskDoor = LayerMask.GetMask("Door");


                if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.right, out hit, 10, layerMaskDoor) && hit.transform.parent.parent.gameObject != context.gameObject/* && hit.transform != context.doorWayPoints[u].GetChild(0)*/)
                {
                    Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.right, Color.green, 50);
                    cP.okToSetup = true;


                    Debug.Log(hit.transform.name);


                    if (hit.transform.parent.parent.GetComponent<CellMovement>())
                    {
                        Debug.Log("parentparentconatinsCellmovement");

                        CellMovement cellMove = hit.transform.parent.parent.GetComponent<CellMovement>();

                        if (cellMove.isOpen == false)
                        {
                            Debug.Log("here is a door i could take" + hit.transform.parent.parent.name);
                            cellMove.isOpen = true;
                        }
                        else if (cellMove.isOpen == true)
                        {
                            Debug.Log("close");
                            cellMove.isOpen = false;
                        }
                    }

                    castingRay = false;
                }
                else
                {
                    Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.right, Color.red, 50);
                    castingRay = false;

                }

                if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.right, out hit, 10, layerMaskDoor) && hit.transform.parent.parent.gameObject != context.gameObject)
                {
                    Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.right, Color.green, 50);
                    cP.okToSetup = true;


                    Debug.Log(hit.transform.parent.parent.name);

                    if (hit.transform.parent.parent.GetComponent<CellMovement>())
                    {
                        Debug.Log("parentparentconatinsCellmovement");

                        CellMovement cellMove = hit.transform.parent.parent.GetComponent<CellMovement>();

                        if (cellMove.isOpen == false)
                        {
                            Debug.Log("here is a door i could take" + hit.transform.parent.parent.name);
                            cellMove.isOpen = true;
                        }
                        else if (cellMove.isOpen == true)
                        {
                            Debug.Log("close");
                            cellMove.isOpen = false;
                        }
                    }
                    castingRay = false;

                }
                else
                {
                    Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.right, Color.red, 50);
                    castingRay = false;

                }


            }
        }

        if (add)
        {
            //myDoorList = context.doorWayPoints;
            Debug.Log("Je me tire Second ray");

            for (int u = 0; u < context.doorWayPoints.Count; u++)
            {

                RaycastHit hit;
                int layerMaskDoor = LayerMask.GetMask("Door");


                if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.right, out hit, 10, layerMaskDoor))
                {
                    Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.right, Color.green, 050);
                    //cP.okToSetup = true;

                    myDoor = context.doorWayPoints[u];
                    doorDirection = (hit.collider.transform.parent.transform);
                    castingRay = false;
                }
                else
                {
                    Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.right, Color.red, 50);
                    castingRay = false;

                }

                if (Physics.Raycast(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.right, out hit, 10, layerMaskDoor))
                {
                    Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, -context.doorWayPoints[u].transform.right, Color.green, 50);
                    myDoor = context.doorWayPoints[u];
                    doorDirection = (hit.collider.transform.parent.transform);
                    castingRay = false;

                }
                else
                {
                    Debug.DrawRay(context.doorWayPoints[u].GetChild(0).transform.position + offset, context.doorWayPoints[u].transform.right, Color.red, 50);
                    castingRay = false;

                }


            }

            if (doorDirection != null && myDoor != null && nextContext != null)
            {
                CheckList();
            }

            add = false;


        }







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
            
            
        }


        if (onlyOne)
        {
            RaycastHit hit;
            int layerMaskPlane = LayerMask.GetMask("Planes");

            if (Physics.Raycast(transform.position, transform.forward, out hit, 10, layerMaskPlane))
            {
                Debug.LogError("RaycastPerso");
                Debug.DrawRay(transform.position, transform.forward, Color.green, 100);
                hit.transform.parent.GetComponent<CellMovement>().isSpawn = true;
                //reset = true;
                
            }
            
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



        }


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
        if (Vector3.Distance(transform.position, context.basePos.position) >= 0.01f && !movement)
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
            Debug.Log("Distanceok");
            next = true;
        }

    }


    public void CheckList()
    {


        for (int i = 0; i < context.paths.list.Count; i++)
        {
            if (context.paths.list[i].listOfWaypoint.Contains(myDoor))
            {
                for (int u = 0; u < context.paths.list[i].listOfWaypoint.Count; u++)
                {
                    ultimateList.Add(context.paths.list[i].listOfWaypoint[u]);
                }
            }
        }

        for (int h = 0; h < nextContext.paths.list.Count; h++)
        {
            if (nextContext.paths.list[h].listOfWaypoint.Contains(doorDirection))
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
                nextContext = null;
                once = false;
                next = false;

                waypoints.listOfWaypoint.Clear();
                ultimateList.Clear();
                inverseList.Clear();
                myDoor = null;
                doorDirection = null;

                isValid = true;

                onlyOne = true;
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

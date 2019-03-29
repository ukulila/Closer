using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool reset;
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
    public int animspeed;
    public bool openDoor;
    public float myRotY;
    public bool baseMovement;
    private bool firstPath;
    public GameObject door;
    public bool isValid;
    public List<Transform> inverseList;
    public bool reverse;

    void Start()
    {
        minDist = 0.1f;
    }

    void Update()
    {
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

                if (distance <= minDist && index != waypoints.listOfWaypoint.Count - 2 && next)
                {
                    //once = false;
                    //movement = false;
                    //reset = true;
                    index++;
                    next = false;
                }
                else if (distance <= minDist && index == waypoints.listOfWaypoint.Count - 2)
                {
                    index = 0;
                    movement = false;

                    door = waypoints.listOfWaypoint[waypoints.listOfWaypoint.Count - 1].gameObject;




                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(transform.position, -transform.forward, 100.0F);
                    Debug.DrawRay(transform.position, -transform.forward, Color.red,  100.0F);
                    for (int i = 0; i < hits.Length; i++)
                    {
                        RaycastHit hit = hits[i];

                        if (hit.transform.tag == "Cell" && hit.transform.name != context.transform.name)
                        {
                            isValid = true;
                            context = hit.transform.GetComponent<ScrEnvironment>();
                        }
                       /* else
                        {
                            waypoints.listOfWaypoint.Reverse();
                            baseMovement = true;
                        }*/

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
                    }*/

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
            }
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

                if (distance <= minDist && index != waypoints.listOfWaypoint.Count-1 && next)
                {
                    //once = false;
                    //movement = false;
                    //reset = true;
                    index++;
                    next = false;
                    

                }
                else if (distance <= minDist && index == waypoints.listOfWaypoint.Count-1)
                {
                    index = 0;
                    movement = false;

                    if (reverse)
                    {
                        reset = true;
                        waypoints.listOfWaypoint.Clear();
                        reverse = false;
                    }

                    if (nextContext != null)
                    {
                        context = nextContext;
                        nextContext = null;
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
            

            if (door.transform.rotation.y < 0.75f)
            {
                door.transform.rotation = door.transform.rotation * new Quaternion(transform.rotation.x, myRotY + animspeed * Time.deltaTime, transform.rotation.z, transform.rotation.w);
            }
            else
            {
                firstPath = true;
                isValid = false;
                openDoor = false;
            }




        }


        if (firstPath)
        {
            reset = true;

            /*if (door.transform.rotation.y > 0.0f)
            {
                myRotY--;
            }
            else
            {*/
                firstPath = false;
                door = null;
                return;
            //}

            
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
            }
            else
            {
                reset = false;

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
        /*
        public void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<ScrEnvironment>() != null)
            {

                context = other.GetComponent<ScrEnvironment>();

            }
        }*/
    }

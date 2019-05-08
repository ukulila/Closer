using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CellMovement : MonoBehaviour
{
    [Header("   Bool Manager")]
    public bool click;
    public bool over;
    public bool once;
    public bool movement;
    public bool selected;
    public bool isOpen;
    public bool reverse;
    [Space(10)]
    public bool hasEnded;
    public bool isSpawn;
    [Space(10)]
    public bool moveHorizontal;
    public bool moveVerticalZ;
    public bool moveVerticalX;
    [Space(10)]
    public bool freezePosValue;
    public bool clickDirection;
    public bool raycastAutor;
    public bool ready;
    [Space(10)]

    [Header("   Mouse/Finger Drag Distance Checker")]
    public Vector3 originPos;
    public Vector3 thenPos;
    public Vector3 distanceMove;
    [Space(10)]

    [Space(10)]
    [Header("   InGame Values")]

    [Tooltip("Don't touch me")]
    public List<Transform> toRotate;
    public Vector3 myPosFreeze;
    private int timer;

    [Space(10)]
    [Header("   ***** Values : Need Assignment *****")]

    [Tooltip("Le Cube complet")]
    public CellPlacement cP;
    public PlayerBehaviour player;


    [Header("   ***** Values : Don't Need Assignment *****")]
    [Tooltip("Toutes les autres Cells (A_..., B_...)")]
    public List<Transform> brothers;
    [Tooltip("Sujet à changement UNIQUEMENT selon la taille du cube")]
    [Range(0.0001f, 5)]
    public float resetPosValue = 3.05f;
    [Tooltip("Vitesse à laquelle se font les translations")]
    [Range(0.01f, 1)]
    public float translateSpeed;

    #region Init

    public void Start()
    {
        over = true;

        // Fills List of other Cells
        for (int x = 0; x < transform.parent.childCount; x++)
        {
            brothers.Add(transform.parent.GetChild(x));

        }

        ready = false;
        hasEnded = true;
        freezePosValue = true;

    }
    #endregion


    void Update()
    {
        
        if(player.context.gameObject == gameObject)
        {
            isSpawn = true;

            ROOM_Manager.Instance.currentRoom = transform.GetComponent<RoomInteraction>();

            if (transform.GetComponent<RoomInteraction>().isInteraction == true)
            {
                ObjectManager.Instance.currentObjet = transform.GetComponent<RoomInteraction>().objet;
            }
            else
            {
                ObjectManager.Instance.currentObjet = null;
            }

            if (transform.GetComponent<RoomInteraction>().isDialogue == true)
            {
                NPC_Manager.Instance.currentNPC = transform.GetComponent<RoomInteraction>().npc;
            }
            else
            {
                NPC_Manager.Instance.currentNPC = null;
            }

            transform.GetComponent<RoomInteraction>().UiTextUpdate();
        }
        else
        {
            if(isSpawn == true)
            {
                selected = false;
                isSpawn = false;

            }
        }



        if (isSpawn)
        {
            player.transform.SetParent(transform);
            isOpen = false;
            selected = true;

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
                player.castingRay = true;
                raycastAutor = false;

            }
        }


        if (clickDirection)
        {
            player.nextContext = gameObject.GetComponent<ScrEnvironment>();
            player.add = true;
            clickDirection = false;
        }


        #region ---- RenameByPLace ----


        brothers = brothers.OrderBy(go => go.name).ToList();




        #endregion


        #region ---- RESET ----

        //Deselects everything when click other
        if (Input.GetMouseButtonUp(0))
        {
            once = false;

            click = false;
            timer = 0;

        }

        /* if (toRotate.Count < 4)
         {

             toRotate.Clear();

         }*/


        #endregion

        #region ---- Selection ----

        if (click && GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
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
            if (transform.position.x < 0 && (transform.position.x != resetPosValue || transform.position.x != -resetPosValue))
            {
                transform.position = new Vector3(-resetPosValue, transform.position.y, transform.position.z);

            }
            else if (transform.position.x > 0 && (transform.position.x != resetPosValue || transform.position.x != -resetPosValue))
            {
                transform.position = new Vector3(resetPosValue, transform.position.y, transform.position.z);

            }

            if (transform.position.y < 0 && (transform.position.y != resetPosValue || transform.position.y != -resetPosValue))
            {
                transform.position = new Vector3(transform.position.x, -resetPosValue, transform.position.z);

            }
            else if (transform.position.y > 0 && (transform.position.y != resetPosValue || transform.position.y != -resetPosValue))
            {
                transform.position = new Vector3(transform.position.x, resetPosValue, transform.position.z);
            }

            if (transform.position.z < 0 && (transform.position.z != resetPosValue || transform.position.z != -resetPosValue))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -resetPosValue);

            }
            else if (transform.position.z > 0 && (transform.position.z != resetPosValue || transform.position.z != -resetPosValue))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, resetPosValue);
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
        if (movement && hasEnded /*&& ready*/)
        {
            ready = false;
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
                    toRotate[v].transform.position = new Vector3((Mathf.Lerp(toRotate[v].transform.position.x, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.x, translateSpeed)), toRotate[v].transform.position.y, (Mathf.Lerp(toRotate[v].transform.position.z, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.z, translateSpeed)));

                }
                else
                {
                    toRotate[v].transform.position = new Vector3((Mathf.Lerp(toRotate[v].transform.position.x, toRotate[0].GetComponent<CellMovement>().myPosFreeze.x, translateSpeed)), toRotate[v].transform.position.y, (Mathf.Lerp(toRotate[v].transform.position.z, toRotate[0].GetComponent<CellMovement>().myPosFreeze.z, translateSpeed)));

                }

            }

            ///There May Be A Delay Between Two Movement with this way to check
            ///
            if (toRotate[0].position == toRotate[1].GetComponent<CellMovement>().myPosFreeze)
            {
                Debug.LogWarning("__HAS__ENDED__");

                movement = false;

                for (int o = 0; o < brothers.Count; o++)
                {
                    brothers[o].GetComponent<CellMovement>().DebugPos();
                    brothers[o].GetComponent<CellMovement>().hasEnded = true;
                    player.check = true;
                }

                toRotate[0].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[1].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[2].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[3].GetComponent<CellMovement>().freezePosValue = true;



                once = false;
                selected = false;
                click = false;
                timer = 0;

                moveHorizontal = false;
            }

        }


        #region [SUITE DES MOUVEMENTS]

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
                    toRotate[v].transform.position = new Vector3((Mathf.Lerp(toRotate[v].transform.position.x, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.x, translateSpeed)), (Mathf.Lerp(toRotate[v].transform.position.y, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.y, translateSpeed)), toRotate[v].transform.position.z);

                }
                else
                {
                    toRotate[v].transform.position = new Vector3((Mathf.Lerp(toRotate[v].transform.position.x, toRotate[0].GetComponent<CellMovement>().myPosFreeze.x, translateSpeed)), (Mathf.Lerp(toRotate[v].transform.position.y, toRotate[0].GetComponent<CellMovement>().myPosFreeze.y, translateSpeed)), toRotate[v].transform.position.z);

                }
            }

            ///There May Be A Delay Between Two Movement with this way to check
            ///
            if (toRotate[0].position == toRotate[1].GetComponent<CellMovement>().myPosFreeze)
            {
                Debug.LogWarning("__HAS__ENDED__");
                movement = false;

                for (int o = 0; o < brothers.Count; o++)
                {
                    brothers[o].GetComponent<CellMovement>().DebugPos();
                    brothers[o].GetComponent<CellMovement>().hasEnded = true;
                    player.check = true;

                }

                toRotate[0].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[1].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[2].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[3].GetComponent<CellMovement>().freezePosValue = true;





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
                    toRotate[v].transform.position = new Vector3(toRotate[v].transform.position.x, (Mathf.Lerp(toRotate[v].transform.position.y, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.y, translateSpeed)), (Mathf.Lerp(toRotate[v].transform.position.z, toRotate[v + 1].GetComponent<CellMovement>().myPosFreeze.z, translateSpeed)));

                }
                else
                {
                    toRotate[v].transform.position = new Vector3(toRotate[v].transform.position.x, (Mathf.Lerp(toRotate[v].transform.position.y, toRotate[0].GetComponent<CellMovement>().myPosFreeze.y, translateSpeed)), (Mathf.Lerp(toRotate[v].transform.position.z, toRotate[0].GetComponent<CellMovement>().myPosFreeze.z, translateSpeed)));

                }
            }

            ///There May Be A Delay Between Two Movement with this way to check
            ///
            if (toRotate[0].position == toRotate[1].GetComponent<CellMovement>().myPosFreeze)
            {
                Debug.LogWarning("__HAS__ENDED__");
                movement = false;

                for (int o = 0; o < brothers.Count; o++)
                {
                    brothers[o].GetComponent<CellMovement>().DebugPos();
                    brothers[o].GetComponent<CellMovement>().hasEnded = true;
                    player.check = true;

                }


                toRotate[0].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[1].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[2].GetComponent<CellMovement>().freezePosValue = true;
                toRotate[3].GetComponent<CellMovement>().freezePosValue = true;



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
            //Debug.Log("Right");
            movement = false;

            if (!once)
            {
                toRotate.Clear();
                HorizontalRotateSide(1);
            }
        }
        else if (distanceMove.x <= -100)
        {
           // Debug.Log("Left");
            movement = false;
            if (!once)
            {
                toRotate.Clear();
                HorizontalRotateSide(2);
            }
        }
        else if (distanceMove.y <= -100)
        {
         //   Debug.Log("Down");
            movement = false;
            if (!once)
            {
                toRotate.Clear();
                HorizontalRotateSide(3);
            }
        }
        else if (distanceMove.y >= 100)
        {
          //  Debug.Log("Up");
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

                    if (brothers[u].transform.position.y > 0 && transform.position.y > 0)
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

                    if (brothers[u].transform.position.y > 0 && transform.position.y > 0)
                    {
                        if (toRotate.Count <= 4)
                        {
                            toRotate.Add(brothers[u]);

                        }
                        moveHorizontal = true;
                        once = true;

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

    /*

    public void OnTriggerEnter(Collider other)                                                                                                          //////////////////////////////////
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
    */
    #endregion


    public void DebugPos()
    {
        if (transform.position.x < 0 && (transform.position.x != resetPosValue || transform.position.x != -resetPosValue))
        {
            transform.position = new Vector3(-resetPosValue, transform.position.y, transform.position.z);
            return;
        }
        else if (transform.position.x > 0 && (transform.position.x != resetPosValue || transform.position.x != -resetPosValue))
        {
            transform.position = new Vector3(resetPosValue, transform.position.y, transform.position.z);
            return;
        }

        if (transform.position.y < 0 && (transform.position.y != resetPosValue || transform.position.y != -resetPosValue))
        {
            transform.position = new Vector3(transform.position.x, -resetPosValue, transform.position.z);
            return;
        }
        else if (transform.position.y > 0 && (transform.position.y != resetPosValue || transform.position.y != -resetPosValue))
        {
            transform.position = new Vector3(transform.position.x, resetPosValue, transform.position.z);
            return;
        }

        if (transform.position.z < 0 && (transform.position.z != resetPosValue || transform.position.z != -resetPosValue))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -resetPosValue);
            return;
        }
        else if (transform.position.z > 0 && (transform.position.z != resetPosValue || transform.position.z != -resetPosValue))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, resetPosValue);
            return;
        }
    }
}


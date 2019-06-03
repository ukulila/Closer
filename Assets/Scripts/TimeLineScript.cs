using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class TimeLineScript : MonoBehaviour
{

    public GraphicRaycaster m_Raycaster;

    public bool mouseDown;

    public PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;

    public Camera cam;
    public Transform currentPolaroid;
    public bool polaroidInteraction;
    private Vector3 transPos;
    public Vector3 offset;
    public List<Transform> TargetRangement;
    public List<Transform> TargetClues;

    public Vector3 unTouchedOffset;
    public AnimationCurve LerpCurve;
    public float LerpSpeed;
    public Vector3 originPos;

    public List<GameObject> Deduction;
    public List<GameObject> Rangement;
    public List<GameObject> truth;

    public List<GameObject> reality;
    private Vector3 lerpPos;

    [Space]

    public GameObject submitButton;
    public Transform pola;
    public bool lerpAnim;
    public Vector3 targetOriginScale;
    private bool once;
    public bool autorisation;
    public Transform originTarget;
    public TMP_Text signal;
    public int totalNumberOfClues;
    public Transform previewTarget;

    private float DownCheckValue = -225;
    public bool NothingInTranzit;
    private int timer;
   

    private void Start()
    {
        NothingInTranzit = true;
    }

    void Update()
    {
        //Reset everything on mouse Up
        if (Input.GetMouseButtonUp(0) && polaroidInteraction && previewTarget == null)
        {
            if (autorisation == false)
            {
                Cursor.visible = true;
                currentPolaroid.position = originPos;
                currentPolaroid.SetParent(originTarget);
                currentPolaroid = null;
                previewTarget = null;
                signal.gameObject.SetActive(false);
                NothingInTranzit = true;

                polaroidInteraction = false;
            }
        }

        //Graph Raycaster To detect Polaroids
        if (Input.GetMouseButtonDown(0) && NothingInTranzit)
        {

            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            if (m_Raycaster != null)
            {
                m_Raycaster.Raycast(m_PointerEventData, results);

            }

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<ThisIsPolaroid>() != null && result.gameObject.GetComponent<ThisIsPolaroid>().isUnlocked == true)
                {
                    Cursor.visible = false;

                    originTarget = result.gameObject.transform.parent;
                    lerpAnim = false;
                    currentPolaroid = result.gameObject.transform;
                    originPos = currentPolaroid.position;
                    polaroidInteraction = true;
                    autorisation = false;
                    NothingInTranzit = false;

                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            autorisation = true;
        }

        //Lerp to Exchange pos between 2 polaroids
        if (lerpAnim)
        {
            if (pola != null)
            {

                lerpPos = Vector3.Lerp(pola.transform.position, originPos, 0.2f);
                timer++;
                pola.position = lerpPos;
                pola.transform.SetParent(originTarget);
                if(timer>25)
                {
                    NothingInTranzit = true;
                    timer = 0;
                    signal.gameObject.SetActive(false);
                }
                // pola = null;
                polaroidInteraction = false;

            }
        }
        /*   else
           {
               if (pola != null)
               {
                   pola.position = originPos;
                   pola.SetParent(originTarget);
                   pola = null;
                   polaroidInteraction = false;

               }

           }*/

        ///Start Function when polaroid on hand
        if (polaroidInteraction)
        {
            ClueMove();
        }

        //All checks for polaroid assignation
        if (currentPolaroid != null)
        {

            for (int i = 0; i < TargetClues.Count; i++)
            {

                if (TargetClues[i].transform != originTarget && currentPolaroid != null /*&& pola == null*/)
                {
                    if (currentPolaroid.position.x - TargetClues[i].position.x < 180 && currentPolaroid.position.x - TargetClues[i].position.x > -70 && currentPolaroid.position.y - TargetClues[i].position.y > DownCheckValue && currentPolaroid.position.y - TargetClues[i].position.y < 70)
                    {
                        signal.gameObject.SetActive(true);
                        signal.text = TargetClues[i].GetChild(0).GetComponent<TMP_Text>().text;

                        previewTarget = TargetClues[i];

                        //if there is nothing on the pin
                        if (TargetClues[i].childCount == 1 && Input.GetMouseButtonUp(0))
                        {
                            pola = null;
                            currentPolaroid.SetParent(TargetClues[i]);
                            currentPolaroid.position = TargetClues[i].position + unTouchedOffset;
                            Cursor.visible = true;
                            currentPolaroid = null;
                            autorisation = false;
                            NothingInTranzit = true;
                            polaroidInteraction = false;

                        }

                        //if there is already something
                        if (TargetClues[i].childCount == 2 && Input.GetMouseButtonUp(0))
                        {
                            pola = TargetClues[i].GetChild(1).transform;
                            lerpAnim = true;

                            currentPolaroid.SetParent(TargetClues[i]);
                            currentPolaroid.position = TargetClues[i].position + unTouchedOffset;
                            Cursor.visible = true;
                            currentPolaroid = null;
                            autorisation = false;

                        }

                    }
                    else
                    {
                        if (previewTarget != null && previewTarget == TargetClues[i])
                        {
                            signal.text = "00:00";
                            signal.gameObject.SetActive(false);
                            previewTarget = null;
                        }
                    }

                }

            }

        }

        //Cannot submit when all clues aren't unlocked
        if (Deduction.Count >= totalNumberOfClues)
        {
            submitButton.SetActive(true);
        }
        else
        {
            submitButton.SetActive(false);

        }



    }



    //Polaroid follows mouse position on screen
    public void ClueMove()
    {
        if (Input.GetMouseButton(0))
        {
            transPos = cam.ScreenToWorldPoint(Input.mousePosition + offset);

            currentPolaroid.position = transPos;
        }
    }


    //Compares to lists : deduction and truth. Check if win
    public void submitCheck()
    {
        truth.Clear();

        for (int i = 0; i < TargetClues.Count; i++)
        {

            truth.Add(TargetClues[i].GetChild(1).gameObject);

        }

        bool CheckMatch()
        {
            if (truth.Count != reality.Count)
                return false;
            for (int u = 0; u < truth.Count; u++)
            {
                if (truth[u] != reality[u])
                    return false;
            }
            return true;
        }

        if (CheckMatch() == true)
        {
            signal.gameObject.SetActive(true);

            signal.text = "Win";
            Debug.Log("win");
        }

    }




    /* bool isInListRangement()
     {
         for (int y = 0; y < Rangement.Count; y++)
         {
             if (currentPolaroid.gameObject == Rangement[y])
                 return true;
         }
         return false;
     }*/

    /* bool isInListDeduction()
     {
         for (int y = 0; y < Deduction.Count; y++)
         {
             if (currentPolaroid.gameObject == Deduction[y])
                 return true;
         }
         return false;
     }
     */

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TimeLineScript : MonoBehaviour
{

    public GraphicRaycaster m_Raycaster;
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

    [Space]

    public GameObject submitButton;


    void Start()
    {

    }

    void Update()
    {

        if (Input.GetMouseButtonUp(0) && polaroidInteraction)
        {
            Cursor.visible = true;
            currentPolaroid.position = originPos;
            polaroidInteraction = false;

        }

        if (Input.GetMouseButtonDown(0))
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



                    currentPolaroid = result.gameObject.transform;
                    originPos = currentPolaroid.position;
                    polaroidInteraction = true;

                }
            }
        }


        if (polaroidInteraction)
        {
            ClueMove();



        }

        if (currentPolaroid != null)
        {
            for (int i = 0; i < TargetRangement.Count; i++)
            {
                if (currentPolaroid.position.x - TargetRangement[i].position.x < 180 && currentPolaroid.position.x - TargetRangement[i].position.x > -70 && currentPolaroid.position.y - TargetRangement[i].position.y > -70 && currentPolaroid.position.y - TargetRangement[i].position.y < 70)
                {

                    if (TargetRangement[i].childCount == 0)
                    {
                        LerpSpeed = LerpCurve.Evaluate(Time.deltaTime);

                        polaroidInteraction = false;
                        Deduction.Remove(currentPolaroid.gameObject);

                        isInListRangement();

                        if (!isInListRangement())
                        {
                            Rangement.Add(currentPolaroid.gameObject);
                        }

                        currentPolaroid.SetParent(TargetRangement[i]);
                        currentPolaroid.position = TargetRangement[i].position + unTouchedOffset;
                        Cursor.visible = true;
                    }


                }

                if (currentPolaroid.position.x - TargetClues[i].position.x < 180 && currentPolaroid.position.x - TargetClues[i].position.x > -70 && currentPolaroid.position.y - TargetClues[i].position.y > -70 && currentPolaroid.position.y - TargetClues[i].position.y < 70)
                {

                    if (TargetClues[i].childCount == 0)
                    {
                        LerpSpeed = LerpCurve.Evaluate(Time.deltaTime);

                        polaroidInteraction = false;

                        isInListDeduction();

                        if(!isInListDeduction())
                        {
                            Deduction.Add(currentPolaroid.gameObject);
                        }




                        Rangement.Remove(currentPolaroid.gameObject);
                        currentPolaroid.SetParent(TargetClues[i]);
                        currentPolaroid.position = TargetClues[i].position + unTouchedOffset;
                        Cursor.visible = true;
                    }


                }
            }

        }


        if (Deduction.Count >= 4)
        {
            submitButton.SetActive(true);
        }
        else
        {
            submitButton.SetActive(false);

        }



    }

    public void ClueMove()
    {
        transPos = cam.ScreenToWorldPoint(Input.mousePosition + offset);

        currentPolaroid.position = transPos;
    }
    public void submitCheck()
    {
        truth.Clear();

        for (int i = 0; i < TargetClues.Count; i++)
        {

            truth.Add(TargetClues[i].GetChild(0).gameObject);

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
            Debug.Log("win");
        }

    }



    bool isInListRangement()
    {
        for (int y = 0; y < Rangement.Count; y++)
        {
            if (currentPolaroid.gameObject == Rangement[y])
                return true;
        }
        return false;
    }

    bool isInListDeduction()
    {
        for (int y = 0; y < Deduction.Count; y++)
        {
            if (currentPolaroid.gameObject == Deduction[y])
                return true;
        }
        return false;
    }



}

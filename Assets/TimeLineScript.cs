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
    public Transform Target;
    public Vector3 unTouchedOffset;
    public AnimationCurve LerpCurve;
    public float LerpSpeed;
    public Vector3 originPos;


    void Start()
    {

    }

    void Update()
    {
     
        if(Input.GetMouseButtonUp(0) && polaroidInteraction)
        {

            currentPolaroid.position = originPos;
            polaroidInteraction = false;

        }

        if(Input.GetMouseButtonDown(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            if(m_Raycaster!= null)
            {
                m_Raycaster.Raycast(m_PointerEventData, results);

            }

            foreach (RaycastResult result in results)
            {
                if(result.gameObject.GetComponent<ThisIsPolaroid>() != null)
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

        if(currentPolaroid != null)
        {

            if(currentPolaroid.position.x - Target.position.x < 180 && currentPolaroid.position.x - Target.position.x > -70 && currentPolaroid.position.y - Target.position.y > -70 && currentPolaroid.position.y - Target.position.y < 70)
            {

                if(Target.childCount == 0 )
                {
                    LerpSpeed = LerpCurve.Evaluate(currentPolaroid.position.x - Target.position.x);

                    polaroidInteraction = false;

                    currentPolaroid.SetParent(Target);
                    currentPolaroid.position = Vector3.Lerp(currentPolaroid.position, Target.position + unTouchedOffset, LerpSpeed);
                    Cursor.visible = true;
                }
                

            }
        }


    }

    public void ClueMove()
    {
        transPos = cam.ScreenToWorldPoint(Input.mousePosition + offset);

        currentPolaroid.position = transPos;
    }
}

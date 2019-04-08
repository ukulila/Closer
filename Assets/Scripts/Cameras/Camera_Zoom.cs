using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class Camera_Zoom : MonoBehaviour
{
    [Header("Récupération de components")]
    public CinemachineVirtualCamera virtualCamera;


    [Header("Zoom Parameters")]
    public float minFOV;
    public float maxFOV;

    public enum ZoomDirection { inside, outside, none }
    public ZoomDirection zDirection;
    private bool onZoom;



    private Touch touchOne;
    private Touch touchTwo;

    private Vector2 currentOnePos;
    private Vector2 previousOnePos;
    private Vector2 currentTwoPos;
    private Vector2 previousTwoPos;
    private float touchesDistance;
    private float touchesDistanceTest;

    public float currentZoomPos;
    public float nextZoomPos;
    public float smoothTime = 1f;
    public float zoomRatio;
    public float minimumDistanceNecessary;

    private bool areFingersMoving;

    [Header("Slow Zoom Parameters")]
    public AnimationCurve slowDownZoomCurve;
    private float currentSlowTime;
    private float maxSlowTime = 1f;
    private float slowDownPercent;

    public float zoomSlowTimeRatio;
    public float zoomSlowValueRatio;
    private float zoomSlowValue;


    [Header("Debug Zoom")]
    public List<TextMeshProUGUI> debugTextes;
    public List<Slider> debugSliders;


    public static Camera_Zoom Instance;





    void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        if (Input.touchCount == 2)
        {
            touchOne = Input.GetTouch(0);
            currentOnePos = touchOne.position;
            currentTwoPos = touchTwo.position;

            previousOnePos = currentOnePos - touchOne.deltaPosition;
            previousTwoPos = currentTwoPos - touchTwo.deltaPosition;

            float prevTouchDeltaMag = (previousOnePos - previousTwoPos).magnitude;
            float touchDeltaMag = (currentOnePos - currentTwoPos).magnitude;

            touchesDistance = prevTouchDeltaMag - touchDeltaMag;


            switch (touchTwo.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    zDirection = ZoomDirection.none;
                    onZoom = true;
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    break;

                case TouchPhase.Stationary:
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    areFingersMoving = false;
                    //touchesDistance = 0;
                    break;
            }

            if (touchesDistance > minimumDistanceNecessary)
                areFingersMoving = true;
            else
                areFingersMoving = false;


            if (touchesDistance < 0)
            {
                zDirection = ZoomDirection.outside;
                nextZoomPos = currentZoomPos + touchesDistance * zoomRatio;
            }
            else
            {
                zDirection = ZoomDirection.inside;
                nextZoomPos = currentZoomPos + touchesDistance * zoomRatio;
            }


            currentZoomPos = virtualCamera.m_Lens.OrthographicSize;
        }



        if (onZoom)
        {
            Zoom();
        }

        #region DEBUG TEXT

        debugTextes[2].text = ("zoomRatio : " + zoomRatio);
        debugTextes[3].text = ("touchesDistanceTest : " + touchesDistanceTest);
        debugTextes[4].text = ("areFingersMoving : " + areFingersMoving);
        debugTextes[5].text = ("currentZoomPos : " + currentZoomPos);
        debugTextes[6].text = ("nextZoomPos : " + nextZoomPos);
        debugTextes[10].text = ("touchesDistance : " + touchesDistance);
        debugTextes[11].text = ("minimumDistanceNecessary : " + minimumDistanceNecessary);
        debugTextes[7].text = ("onZoom : " + onZoom);
        debugTextes[8].text = ("zDirection : " + zDirection);

        zoomRatio = debugSliders[0].value;
        minimumDistanceNecessary = debugSliders[1].value;
        smoothTime = debugSliders[2].value;


        #endregion

    }

    /// <summary>
    /// Son nom parle pour lui ... enfin peut etre pas dans toutes les langues
    /// </summary>
    void Zoom()
    {
        if (areFingersMoving)
        {
            zoomSlowValue = currentZoomPos - nextZoomPos;
            currentSlowTime = 0;
        }
        else
        {

        }

        if (virtualCamera.m_Lens.OrthographicSize > minFOV || virtualCamera.m_Lens.OrthographicSize < maxFOV)
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentZoomPos, nextZoomPos, smoothTime);

        if (virtualCamera.m_Lens.OrthographicSize < minFOV || virtualCamera.m_Lens.OrthographicSize > maxFOV)
            virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize, minFOV, maxFOV);
    }



    /// <summary>
    /// Ralentissement du Zoom
    /// </summary>
    void SmoothZoom()
    {
        if (currentSlowTime < maxSlowTime)
        {
            currentSlowTime += Time.deltaTime * zoomSlowTimeRatio;
        }
        else
        {
            onZoom = false;
        }

        slowDownPercent = slowDownZoomCurve.Evaluate(currentSlowTime);



        if (zDirection == ZoomDirection.inside)
        {

        }

        if (zDirection == ZoomDirection.outside)
        {

        }
    }
}

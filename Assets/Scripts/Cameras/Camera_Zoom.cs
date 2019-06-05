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

    [Space]
    public bool onZoom;
    public enum ZoomDirection { inside, outside, none }
    public ZoomDirection zDirection;


    [Space]
    private Touch touchOne;
    private Touch touchTwo;

    [Space]
    private Vector2 currentOnePos;
    private Vector2 previousOnePos;
    private Vector2 currentTwoPos;
    private Vector2 previousTwoPos;
    private float prevTouchDeltaMag;
    private float touchDeltaMag;
    public float touchesDistance;

    [Space]
    public float currentZoomPos;
    public float nextZoomPos;
    public float smoothTime = 0.5f;
    public float zoomRatio = 0.009f;
    public float minimumDistanceNecessary = 1f;

    [Space]
    public bool areFingersMoving;

    [Header("Slow Zoom Parameters")]
    public AnimationCurve slowDownZoomCurve;
    private float currentSlowTime;
    public float maxSlowTime = 1f;
    public float slowDownPercent;

    [Space]
    public float zoomSlowTimeRatio = 0.012f;
    public float zoomSlowValueRatio = 0.025f;
    private float zoomSlowValue;

    public bool isSmoothing;

    [Header("Trail Renderer Limit")]
    public float trailInvisibleAt;



    public static Camera_Zoom Instance;





    void Awake()
    {
        Instance = this;

        currentZoomPos = virtualCamera.m_Lens.OrthographicSize;
    }

   /* private void Start()
    {
        if (virtualCamera.m_Lens.OrthographicSize < trailInvisibleAt)
        {
            trail_Behaviour.Instance.isTrailStillVisible = false;
            trail_Behaviour.Instance.DeactivateTrail();
        }
        else
            trail_Behaviour.Instance.isTrailStillVisible = true;
    }
    */
    
    void Update()
    {
        if (Input.touchCount == 2 && GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
            {
                touchOne = Input.GetTouch(0);
                touchTwo = Input.GetTouch(1);

                currentOnePos = touchOne.position;
                currentTwoPos = touchTwo.position;

                previousOnePos = currentOnePos - touchOne.deltaPosition;
                previousTwoPos = currentTwoPos - touchTwo.deltaPosition;

                prevTouchDeltaMag = (previousOnePos - previousTwoPos).magnitude;
                touchDeltaMag = (currentOnePos - currentTwoPos).magnitude;

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
                        areFingersMoving = true;
                        onZoom = true;
                        break;

                    case TouchPhase.Stationary:
                        areFingersMoving = false;
                        break;

                    case TouchPhase.Ended:
                        // Report that the touch has ended when it ends
                        areFingersMoving = false;
                        if (virtualCamera.m_Lens.OrthographicSize < trailInvisibleAt)
                        {
                            //trail_Behaviour.Instance.isTrailStillVisible = false;
                           // trail_Behaviour.Instance.DeactivateTrail();
                        }
                        //else
                           // trail_Behaviour.Instance.isTrailStillVisible = true;
                        break;
                }



                currentZoomPos = virtualCamera.m_Lens.OrthographicSize;

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
            }
            else
            {
                areFingersMoving = false;
            }


        if (onZoom)
        {
            Zoom();
        }
        
    }
    
    /// <summary>
    /// Son nom parle pour lui ... enfin peut etre pas dans toutes les langues
    /// </summary>
    void Zoom()
    {
        if (areFingersMoving)
        {
            if (virtualCamera.m_Lens.OrthographicSize < maxFOV)
                if (nextZoomPos > currentZoomPos)
                    virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentZoomPos, nextZoomPos, smoothTime);

            if (virtualCamera.m_Lens.OrthographicSize > minFOV)
                if (nextZoomPos < currentZoomPos)
                    virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentZoomPos, nextZoomPos, smoothTime);

            currentSlowTime = 0;

            zoomSlowValue = currentZoomPos - nextZoomPos;
        }
        else
        {
            onZoom = false;
        }
    }


    #region smooth
    /// <summary>
    /// Ralentissement du Zoom
    /// </summary>
    void SmoothZoom()
    {
        if (currentSlowTime < maxSlowTime)
        {
            currentSlowTime += Time.deltaTime;
            isSmoothing = true;
        }
        else
        {
            onZoom = false;
            isSmoothing = false;
            zDirection = ZoomDirection.none;
        }

        slowDownPercent = slowDownZoomCurve.Evaluate(currentSlowTime);


        if (zDirection == ZoomDirection.inside)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentZoomPos, nextZoomPos, smoothTime) - ((zoomSlowValue * zoomSlowValueRatio) * slowDownPercent);
        }

        if (zDirection == ZoomDirection.outside)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentZoomPos, nextZoomPos, smoothTime) - ((zoomSlowValue * zoomSlowValueRatio) * slowDownPercent);
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class CameraMovement : MonoBehaviour
{

    [Header("Récupération des components")]
    public CinemachineDollyCart dollyCart; //Récupération de la vitesse de rotation du script DollyCart utilisant le Dolly Horizontal
    public CinemachineVirtualCamera virtualCamera; //Récupération de l'offset du target
    public Transform dollyTransform; //Récupération de la vitesse de rotation du script DollyCart utilisant le Dolly Vertical
    public CinemachineBrain brain;

    [Header("Camera Rotation Parameters")]

    public bool aboutCamera;
    private bool isRotating;
    private bool isZooming;
    private bool onZoom;

    private bool isFingerMoving;
    private bool isOrientationSet;

    private float pathOffset = 0.5f;
    private float slowingSpeed;
    private float pathSpeed;
    private float fieldOfView = 40;

    private float speed;


    public enum VerticalDirection { up, down, center }
    public VerticalDirection yDirection;
    private bool onVertical;
    public float maxOffset;
    public float minOffset;
    public enum HorizontalDirection { left, right, none };
    public HorizontalDirection xDirection;
    private bool onHorizontal;

    public float minFOV;
    public float maxFOV;

    public float timeRatio;
    public float zoomRatio;
    public float horizontalRatio;
    public float verticalRatio;

    private float currentSlowTime;
    public AnimationCurve slowDownCurve;

    private Touch touchOne;
    private Touch touchTwo;

    private Vector2 currentOnePos;
    private Vector2 currentTwoPos;

    private Vector2 previousOnePos;
    private Vector2 previousTwoPos;

    private float currentX;
    private float currentY;
    public float minimum;

    private float currentTouchesDistance;
    private float newTouchesDistance;
    private float distanceDiff;
    private float oneTouchDistance;

    /*
    [Header("Debug texts")]
    public List<TextMeshProUGUI> debugTexts;
    */

    public static CameraMovement Instance;







    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        //Si le joueur n'a pas sélectionné le CUBE
        if (aboutCamera)
        {
            if (Input.touchCount == 1 && isZooming == false)
            {
                touchOne = Input.GetTouch(0);
                currentOnePos = touchOne.position;

                previousOnePos = currentOnePos - touchOne.deltaPosition;

                currentX = currentOnePos.x - previousOnePos.x;
                currentY = currentOnePos.y - previousOnePos.y;

                oneTouchDistance = (currentOnePos - previousOnePos).magnitude;

                //Si le sens d'orientation n'a pas encore été set
                if (!isOrientationSet)
                {
                    //Si un mouvement a été initié
                    if (oneTouchDistance != 0)
                    {
                        if (currentX > minimum || currentX < -minimum)
                        {
                            onHorizontal = true;
                            isOrientationSet = true;
                            isRotating = true;
                            onVertical = false;
                            yDirection = VerticalDirection.center;
                        }

                        if (currentY > minimum || currentY < -minimum)
                        {
                            onVertical = true;
                            isOrientationSet = true;
                            isRotating = true;
                            onHorizontal = false;
                            xDirection = HorizontalDirection.none;
                        }
                    }
                }

                //Si l'Orientation est Horizontal
                if (onHorizontal)
                {
                    if (currentX > 0)
                    {
                        xDirection = HorizontalDirection.right;
                    }
                    else
                    {
                        xDirection = HorizontalDirection.left;
                    }
                }

                //Si l'Orientation est Vertical
                if (onVertical)
                {
                    if (currentY > 0)
                    {
                        yDirection = VerticalDirection.up;
                    }
                    else
                    {
                        yDirection = VerticalDirection.down;
                    }
                }


                switch (touchOne.phase)
                {
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        // Record initial touch position.
                        isRotating = false;
                        onZoom = false;
                        isZooming = false;
                        break;

                    //Determine if the touch is a moving touch
                    case TouchPhase.Moved:
                        // Determine direction by comparing the current touch position with the initial one
                        isFingerMoving = true;
                        break;

                    case TouchPhase.Stationary:
                        isFingerMoving = false;
                        break;

                    case TouchPhase.Ended:
                        // Report that the touch has ended when it ends
                        isFingerMoving = false;
                        isOrientationSet = false;
                        oneTouchDistance = 0;
                        break;
                }
            }

            if (Input.touchCount == 2 && isRotating == false)
            {
                onZoom = true;

                touchOne = Input.GetTouch(0);
                touchTwo = Input.GetTouch(1);

                currentOnePos = touchOne.position;
                currentTwoPos = touchTwo.position;

                previousOnePos = currentOnePos - touchOne.deltaPosition;
                previousTwoPos = currentTwoPos - touchTwo.deltaPosition;

                currentTouchesDistance = (previousOnePos - previousTwoPos).magnitude;
                newTouchesDistance = (currentOnePos - currentTwoPos).magnitude;

                distanceDiff = currentTouchesDistance - newTouchesDistance;


                switch (touchOne.phase)
                {
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        // Record initial touch position.
                        isZooming = false;
                        isRotating = false;
                        isOrientationSet = true;
                        break;

                    //Determine if the touch is a moving touch
                    case TouchPhase.Moved:
                        // Determine direction by comparing the current touch position with the initial one
                        isFingerMoving = true;
                        isZooming = true;
                        break;

                    case TouchPhase.Stationary:
                        isFingerMoving = false;
                        break;

                    case TouchPhase.Ended:
                        // Report that the touch has ended when it ends
                        isFingerMoving = false;
                        currentTouchesDistance = newTouchesDistance;
                        isOrientationSet = false;
                        distanceDiff = 0;
                        break;
                }

            }


            //Update des valeurs concernées
            dollyCart.m_Speed = pathSpeed; //Vitesse de rotation horizontale
            dollyTransform.position = new Vector3(dollyTransform.position.x, pathOffset, dollyTransform.position.z); //Position vertical du dolly de la camera
            //pathOffset = Mathf.Clamp(pathOffset, minOffset, maxOffset); //Limites de la position vertical
            virtualCamera.m_Lens.FieldOfView = fieldOfView; //Focale de la camera
            //fieldOfView = Mathf.Clamp(fieldOfView, minFOV, maxFOV); //Limites de la focale

            if (isRotating && !isZooming)
            {
                CameraTracking();
                AdjustHeight();
            }
            else
            {
                pathSpeed = 0;
            }

            if (isZooming && !isRotating)
            {
                Zoom();
            }
        }

        /*
        #region DEBUG TEXT
        debugTexts[0].text = ("onZoom : " + onZoom);
        debugTexts[1].text = ("distanceDiff : " + distanceDiff);
        debugTexts[2].text = ("isOrientationSet : " + isOrientationSet);
        debugTexts[3].text = ("isZooming : " + isZooming);
        debugTexts[4].text = ("onHorizontal : " + onHorizontal);
        debugTexts[5].text = ("onVertical : " + onVertical);
        debugTexts[6].text = ("isRotating : " + isRotating);
        debugTexts[7].text = ("speed : " + speed);
        debugTexts[8].text = ("fieldOfView : " + fieldOfView);
        debugTexts[9].text = ("currentSlowTime : " + currentSlowTime);
        debugTexts[10].text = ("aboutCamera : " + aboutCamera);
        debugTexts[11].text = ("currentSlowTime : " + currentSlowTime);
        debugTexts[12].text = ("pathOffset : " + pathOffset);
        debugTexts[13].text = ("pathSpeed : " + pathSpeed);

        #endregion
        */

        CheckComponentValues();
    }

    private void CheckComponentValues()
    {
        if (fieldOfView < minFOV)
        {
            fieldOfView = minFOV;
            currentSlowTime = 1;
            isZooming = false;
        }

        if (fieldOfView > maxFOV)
        {
            fieldOfView = maxFOV;
            currentSlowTime = 1;
            isZooming = false;
        }

        if (pathOffset < minOffset)
        {
            pathOffset = minOffset;
            currentSlowTime = 1;
            isRotating = false;
        }

        if (pathOffset > maxOffset)
        {
            pathOffset = maxOffset;
            currentSlowTime = 1;
            isRotating = false;
        }
    }

    /// <summary>
    /// Déplacement de la caméra sur le Dolly Horizontal
    /// </summary>
    void CameraTracking()
    {
        Debug.Log("Tracking");

        if (isFingerMoving && oneTouchDistance > 1)
        {
            if(onHorizontal)
            {
                speed = currentX * horizontalRatio;
                pathSpeed = speed;
                currentSlowTime = 0;
            }
        }
        else
        {
            SlowDown();
        }
    }

    /// <summary>
    /// Déplacement de la caméra sur le Dolly Vertical
    /// </summary>
    void AdjustHeight()
    {
        Debug.Log("Adjusting");

        if (isFingerMoving && oneTouchDistance > 1)
        {
            if(onVertical)
            {
                speed = currentY * verticalRatio * (Time.deltaTime * timeRatio);

                pathOffset -= speed;

                currentSlowTime = 0;
            }
        }
        else
        {
            SlowDown();
        }

    }


    void Zoom()
    {
        Debug.Log("Zooming");

        if (isFingerMoving && distanceDiff != 0)
        {
            speed = distanceDiff * zoomRatio * (Time.deltaTime * timeRatio);

            fieldOfView += speed;
            
            currentSlowTime = 0;
        }
        else
        {
            SlowDown();
        }
    }


    /// <summary>
    /// Ralentissement du mouvement de camera
    /// </summary>
    void SlowDown()
    {
        if (currentSlowTime < 1)
        {
            currentSlowTime += Time.deltaTime * timeRatio;
        }

        float remainingSpeed = slowDownCurve.Evaluate(currentSlowTime);

        if (onHorizontal)
        {
            slowingSpeed = speed * remainingSpeed;
            pathSpeed = slowingSpeed;
        }

        if(onVertical)
        {
            slowingSpeed = speed * remainingSpeed;
            float currentSpeed = pathOffset;
            pathOffset = currentSpeed - slowingSpeed;
        }

        if (onZoom)
        {
            slowingSpeed = speed * remainingSpeed;
            float currentSpeed = fieldOfView;
            fieldOfView = currentSpeed + slowingSpeed;
        }


        if (slowingSpeed == 0)
        {
            isRotating = false;
            isZooming = false;
        }
    }
}

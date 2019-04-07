using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class Camera_Rotation : MonoBehaviour
{
    [Header("Récupération des components")]
    public CinemachineDollyCart dollyCart; //Récupération de la vitesse de rotation du script DollyCart utilisant le Dolly Horizontal
    public CinemachineVirtualCamera virtualCamera; //Récupération de l'offset du target
    public Transform dollyTransform; //Récupération de la vitesse de rotation du script DollyCart utilisant le Dolly Vertical
    public CinemachineBrain brain; //Récupération du brain


    [Header("Camera Rotation Parameters")]
    public bool aboutCamera;
    private bool isRotating;
    private bool isFingerMoving;
    private bool isOrientationSet;

    public enum VerticalDirection { up, down, center }
    public VerticalDirection yDirection;
    private bool onVertical;
    public enum HorizontalDirection { left, right, none };
    public HorizontalDirection xDirection;
    private bool onHorizontal;


    public float currentHorizontalPos;
    public float currentVerticalPos;
    public float nextHorizontalPos;
    public float nextVerticalPos;

    public float rotationRatio;
    public float slowValueRatio;

    public AnimationCurve slowTimeCurve;
    public float slowPercent;
    public float slowTimeRatio;
    public float currentSlowTime;
    public float maxSlowTime;
    public float slowValue;

    public float floatRef;
    public float smoothTime;
    public float smoothSpeed;


    public Slider debugSlider;
    public Slider debugSlider00;
    public Slider debugSlider01;

    private Touch touchOne;

    private Vector2 currentOnePos;
    private Vector2 previousOnePos;
    private float oneTouchDistance;

    private float currentX;
    private float currentY;
    public float minimumMoveNecessary;


    [Header("Debug texts")]
    public List<TextMeshProUGUI> debugTexts;


    public static Camera_Rotation Instance;


    private void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        //Si le joueur n'a pas sélectionné le CUBE
        if (aboutCamera)
        {
            if (Input.touchCount == 1)
            {
                touchOne = Input.GetTouch(0);
                currentOnePos = touchOne.position;

                previousOnePos = currentOnePos - touchOne.deltaPosition;

                currentX = currentOnePos.x - previousOnePos.x;
                currentY = currentOnePos.y - previousOnePos.y;

                oneTouchDistance = (currentOnePos - previousOnePos).magnitude;

                switch (touchOne.phase)
                {
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        // Record initial touch position.
                        break;

                    //Determine if the touch is a moving touch
                    case TouchPhase.Moved:
                        // Determine direction by comparing the current touch position with the initial one
                        if (oneTouchDistance > minimumMoveNecessary)
                            isFingerMoving = true;

                        break;

                    case TouchPhase.Stationary:
                        if (oneTouchDistance < minimumMoveNecessary)
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


            if (currentX > 0)
            {
                xDirection = HorizontalDirection.right;
            }

            if (currentX < 0)
            {
                xDirection = HorizontalDirection.left;
            }



            currentHorizontalPos = dollyCart.m_Position;
        }


        if (xDirection == HorizontalDirection.left)
        {
            nextHorizontalPos = currentHorizontalPos - oneTouchDistance * rotationRatio;
        }

        if (xDirection == HorizontalDirection.right)
        {
            nextHorizontalPos = currentHorizontalPos + oneTouchDistance * rotationRatio;
        }

        if (currentHorizontalPos == nextHorizontalPos)
        {
            //xDirection = HorizontalDirection.none;
        }

        #region DEBUG TEXT

        debugTexts[2].text = ("xDirection : " + xDirection);
        debugTexts[4].text = ("isFingerMoving : " + isFingerMoving);
        debugTexts[5].text = ("nextHorizontalPos : " + nextHorizontalPos);
        debugTexts[6].text = ("currentSlowTime : " + currentSlowTime);
        debugTexts[10].text = ("smoothTime : " + smoothTime);
        debugTexts[11].text = ("slowTimeRatio : " + slowTimeRatio);
        debugTexts[7].text = ("slowValueRatio : " + slowValueRatio);
        debugTexts[8].text = ("slowValue : " + slowValue);

        #endregion

        //slowValue = debugSlider.value;
        //slowValueRatio = debugSlider00.value;
        //slowTimeRatio = debugSlider01.value;

        CameraTracking();


    }


    /// <summary>
    /// Déplacement de la caméra sur le Dolly Horizontal
    /// </summary>
    void CameraTracking()
    {
        Debug.Log("Tracking");

        if (isFingerMoving)
        {
            dollyCart.m_Position = Mathf.Lerp(currentHorizontalPos, nextHorizontalPos, smoothTime);
            slowValue = currentHorizontalPos - nextHorizontalPos;
            currentSlowTime = 0;
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
            if (onVertical)
            {

            }
        }
        else
        {

        }

    }


    void SlowDown()
    {
        if (currentSlowTime < maxSlowTime)
        {
            currentSlowTime += Time.deltaTime * slowTimeRatio;
        }

        slowPercent = slowTimeCurve.Evaluate(currentSlowTime);

        if (xDirection == HorizontalDirection.left)
        {
            dollyCart.m_Position = Mathf.Lerp(dollyCart.m_Position, nextHorizontalPos, smoothTime) - ((slowValue * slowValueRatio) * slowPercent);
        }

        if (xDirection == HorizontalDirection.right)
        {
            dollyCart.m_Position = Mathf.Lerp(dollyCart.m_Position, nextHorizontalPos, smoothTime) - ((slowValue * slowValueRatio) * slowPercent);
        }
    }
}


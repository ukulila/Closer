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
    public bool isFingerMoving;
    private bool isOrientationSet;

    private Touch touchZero;

    private Vector2 currentZeroPos;
    private Vector2 previousZeroPos;
    private float oneTouchDistance;

    private float currentX;
    private float currentY;
    public float minimumMoveNecessary = 8f;

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

    public float horizontalRotationRatio = 0.075f;
    public float verticalRotationRatio = 0.015f;

    public float maxHeight;
    public float minHeight = 0;

    [Header("Slow Parameters")]

    public AnimationCurve slowTimeCurve;
    public float slowPercent;

    public float currentSlowTime;
    public float maxSlowTime;

    public float horizontalSlowTimeRatio = 2f;
    private float horizontalSlowValue;
    public float horizontalSlowValueRatio = 0.5f;
    public float horizontalSmoothTime = 1f;

    public float verticalSlowTimeRatio = 0.17f;
    private float verticalSlowValue;
    public float verticalSlowValueRatio = 0.012f;
    public float verticalSmoothTime = 1f;


    public static Camera_Rotation Instance;





    private void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        //Si le joueur n'a pas sélectionné le CUBE
        if (aboutCamera && GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
        {
            //Si un doigt est sur l'écran
            if (Input.touchCount == 1)
            {
                //Get finger's positions related to the screen, and update related values from it
                touchZero = Input.GetTouch(0);
                currentZeroPos = touchZero.position;

                previousZeroPos = currentZeroPos - touchZero.deltaPosition;

                currentX = currentZeroPos.x - previousZeroPos.x;
                currentY = currentZeroPos.y - previousZeroPos.y;

                oneTouchDistance = (currentZeroPos - previousZeroPos).magnitude;

                //Do stuff depending on touch phase et bla bla bla ...
                switch (touchZero.phase)
                {
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        // Record initial touch position.
                        isOrientationSet = false;
                        onVertical = false;
                        onHorizontal = false;
                        yDirection = VerticalDirection.center;
                        xDirection = HorizontalDirection.none;
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

            //Set orientation
            if (!isOrientationSet && isFingerMoving)
            {
                if (currentX < 0)
                {
                    if (currentY > 0)
                    {
                        if ((currentX * -1) > currentY)
                        {
                            onHorizontal = true;
                            onVertical = false;

                            isOrientationSet = true;
                        }
                        else
                        {
                            onHorizontal = false;
                            onVertical = true;

                            isOrientationSet = true;
                        }
                    }
                    else
                    {
                        if (currentX > currentY)
                        {
                            onHorizontal = true;
                            onVertical = false;

                            isOrientationSet = true;
                        }
                        else
                        {
                            onHorizontal = false;
                            onVertical = true;

                            isOrientationSet = true;
                        }
                    }
                }
                else
                {
                    if (currentY > 0)
                    {
                        if ((currentX * -1) > currentY)
                        {
                            onHorizontal = true;
                            onVertical = false;

                            isOrientationSet = true;
                        }
                        else
                        {
                            onHorizontal = false;
                            onVertical = true;

                            isOrientationSet = true;
                        }
                    }
                    else
                    {
                        if (currentX > currentY)
                        {
                            onHorizontal = true;
                            onVertical = false;

                            isOrientationSet = true;
                        }
                        else
                        {
                            onHorizontal = false;
                            onVertical = true;

                            isOrientationSet = true;
                        }
                    }
                }

                if (currentX > minimumMoveNecessary || currentX < -minimumMoveNecessary)
                {
                    onHorizontal = true;
                    isOrientationSet = true;
                    onVertical = false;
                    yDirection = VerticalDirection.center;
                }

                if (currentY > minimumMoveNecessary || currentY < -minimumMoveNecessary)
                {
                    onVertical = true;
                    isOrientationSet = true;
                    onHorizontal = false;
                    xDirection = HorizontalDirection.none;
                }
            }

            //Update orientation depending on the direction
            if (isOrientationSet)
            {
                if (onHorizontal)
                {
                    if (currentX > 0)
                    {
                        xDirection = HorizontalDirection.right;
                    }

                    if (currentX < 0)
                    {
                        xDirection = HorizontalDirection.left;
                    }
                }

                if (onVertical)
                {
                    if (currentY > 0)
                    {
                        yDirection = VerticalDirection.down;
                    }

                    if (currentY < 0)
                    {
                        yDirection = VerticalDirection.up;
                    }
                }
            }

            //Update les valeurs "current"
            currentHorizontalPos = dollyCart.m_Position;
            currentVerticalPos = dollyTransform.position.y;
        }

        //Si on est sur l'axe horizontal
        if (onHorizontal)
        {
            if (xDirection == HorizontalDirection.left)
            {
                nextHorizontalPos = currentHorizontalPos - oneTouchDistance * horizontalRotationRatio;
            }

            if (xDirection == HorizontalDirection.right)
            {
                nextHorizontalPos = currentHorizontalPos + oneTouchDistance * horizontalRotationRatio;
            }

            CameraTracking();
        }

        //Si on est sur l'axe vertical
        if (onVertical)
        {
            if (yDirection == VerticalDirection.down)
            {
                nextVerticalPos = currentVerticalPos - oneTouchDistance * verticalRotationRatio;
            }

            if (yDirection == VerticalDirection.up)
            {
                nextVerticalPos = currentVerticalPos + oneTouchDistance * verticalRotationRatio;
            }

            AdjustHeight();
        }
    }


    /// <summary>
    /// Déplacement de la caméra sur le Dolly Horizontal
    /// </summary>
    void CameraTracking()
    {
        if (isFingerMoving)
        {
            dollyCart.m_Position = Mathf.Lerp(currentHorizontalPos, nextHorizontalPos, horizontalSmoothTime);
            horizontalSlowValue = currentHorizontalPos - nextHorizontalPos;
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
        if (isFingerMoving)
        {
            verticalSlowValue = currentVerticalPos = nextVerticalPos;
            currentSlowTime = 0;

            if (dollyTransform.position.y < maxHeight || dollyTransform.position.y > minHeight)
                dollyTransform.position = new Vector3(dollyTransform.position.x, Mathf.Lerp(currentVerticalPos, nextVerticalPos, verticalSmoothTime), dollyTransform.position.z);
        }
        else
        {
            SlowDown();
        }

        if (dollyTransform.position.y > maxHeight || dollyTransform.position.y < minHeight)
            dollyTransform.position = new Vector3(dollyTransform.position.x, Mathf.Clamp(dollyTransform.position.y, minHeight, maxHeight), dollyTransform.position.z);
    }


    /// <summary>
    /// Ralentissement de la rotation CAMERA
    /// </summary>
    void SlowDown()
    {
        if (currentSlowTime < maxSlowTime)
        {
            currentSlowTime += Time.deltaTime * horizontalSlowTimeRatio;
        }

        slowPercent = slowTimeCurve.Evaluate(currentSlowTime);



        if (xDirection == HorizontalDirection.left)
        {
            dollyCart.m_Position = Mathf.Lerp(dollyCart.m_Position, nextHorizontalPos, horizontalSmoothTime) - ((horizontalSlowValue * horizontalSlowValueRatio) * slowPercent);
        }

        if (xDirection == HorizontalDirection.right)
        {
            dollyCart.m_Position = Mathf.Lerp(dollyCart.m_Position, nextHorizontalPos, horizontalSmoothTime) - ((horizontalSlowValue * horizontalSlowValueRatio) * slowPercent);
        }

        if (yDirection == VerticalDirection.down)
        {
            dollyTransform.position = new Vector3(dollyTransform.position.x, Mathf.Lerp(currentVerticalPos, nextVerticalPos, verticalSmoothTime) - (verticalSlowValue * verticalSlowValueRatio) * slowPercent,
                dollyTransform.position.z);
        }

        if (yDirection == VerticalDirection.up)
        {
            dollyTransform.position = new Vector3(dollyTransform.position.x, Mathf.Lerp(currentVerticalPos, nextVerticalPos, verticalSmoothTime) + (verticalSlowValue * verticalSlowValueRatio) * slowPercent,
                dollyTransform.position.z);
        }
    }
}


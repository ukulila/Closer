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
    public float minimumMoveNecessary;

    private float currentTouchesDistance;
    private float newTouchesDistance;
    private float distanceDiff;
    private float oneTouchDistance;

    
    [Header("Debug texts")]
    public List<TextMeshProUGUI> debugTexts;
    

    [Header("Switching Interface Parameters")]

    public List<Vector3> targetOffsets;
    public List<float> uiPathPosition;
    public Vector3 uiDollyScale;
    public Vector3 gameDollyScale;
    public float uiFOV;
    public float gameFOV;


    public Image selectionTimingImage;
    public string currentSelectedCell;
    public float timingOfSelection;
    public bool switchToUI;

    public AnimationCurve cameraRepositioningCurve;
    public AnimationCurve targetRepositioningCurve;
    public float animationCurveTimingMax;
    public float currentRepositionTime;
    private float repoPercent;
    private float retarPercent;

    [Range(0.1f, 1)]
    public float switchDurationRatioModifier;

    private float currentPathPos;
    private float continuePosDifference;
    private float reversePosDifference;
    private float animationPosDifference;
    private float positionMax;

    public float currentFOV;
    public float fovDiff;

    public Vector3 currentDollyScale;
    public Vector3 dollyDiff;

    public Vector3 currentTargetOffset;
    public Vector3 targetOffsetDiff;

    public bool cameraReposition = true;

    public static CameraMovement Instance;







    private void Awake()
    {
        Instance = this;

        positionMax = dollyTransform.gameObject.GetComponent<CinemachineSmoothPath>().MaxUnit(CinemachinePathBase.PositionUnits.Distance);
        dollyDiff = uiDollyScale - dollyTransform.localScale;

    }


    void RepositionCamera()
    {
        if (currentRepositionTime < animationCurveTimingMax)
        {
            currentRepositionTime += Time.deltaTime;
        }
        else
        {
            cameraReposition = true;
        }

        repoPercent = cameraRepositioningCurve.Evaluate(currentRepositionTime / animationCurveTimingMax);

        retarPercent = targetRepositioningCurve.Evaluate(currentRepositionTime / animationCurveTimingMax);

        if (switchToUI)
        {
            if(!cameraReposition)
            {
                //Changement de la position
                dollyCart.m_Position = currentPathPos + animationPosDifference * repoPercent;


                //Changement de la Target Offset
                virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(currentTargetOffset.x + targetOffsetDiff.x * retarPercent,
                    currentTargetOffset.y + targetOffsetDiff.y * retarPercent, currentTargetOffset.z + targetOffsetDiff.z * retarPercent);

                //Changement du Scale du dolly
                dollyTransform.localScale = new Vector3(currentDollyScale.x + dollyDiff.x * repoPercent, currentDollyScale.y + dollyDiff.y * repoPercent, currentDollyScale.z + dollyDiff.z * repoPercent);

                //Changement de la focale
                virtualCamera.m_Lens.FieldOfView = currentFOV + fovDiff * repoPercent;
            }
        }
        else
        {
            //Changement de la Target Offset
            virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(currentTargetOffset.x - targetOffsetDiff.x * retarPercent,
                currentTargetOffset.y - targetOffsetDiff.y * retarPercent, currentTargetOffset.z - targetOffsetDiff.z * retarPercent);

            //Changement du Scale du dolly
            dollyTransform.localScale = new Vector3(currentDollyScale.x - dollyDiff.x * repoPercent, currentDollyScale.y - dollyDiff.y * repoPercent, currentDollyScale.z - dollyDiff.z * repoPercent);

            //Changement de la focale
            virtualCamera.m_Lens.FieldOfView = currentFOV - fovDiff * repoPercent;
        }

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
                        if (currentX > minimumMoveNecessary || currentX < -minimumMoveNecessary)
                        {
                            onHorizontal = true;
                            isOrientationSet = true;
                            isRotating = true;
                            onVertical = false;
                            yDirection = VerticalDirection.center;
                        }

                        if (currentY > minimumMoveNecessary || currentY < -minimumMoveNecessary)
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

        if (switchToUI)
        {
            if(cameraReposition)
            {
                animationCurveTimingMax = 1.5f;

                if (Input.GetMouseButtonDown(0) && Input.touchCount < 2)
                {
                    RaycastHit selectedCube;

                    currentPathPos = dollyCart.m_Position;
                    currentDollyScale = dollyTransform.localScale;
                    currentTargetOffset = virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
                    currentFOV = virtualCamera.m_Lens.FieldOfView;
                    currentRepositionTime = 0;


                    if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube) && cameraReposition)
                    {
                        if (selectedCube.collider)
                        {
                            switchToUI = false;
                            cameraReposition = false;
                        }
                    }
                }
            }   
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && Input.touchCount < 2 && !aboutCamera && cameraReposition)
            {
                RaycastHit selectedCube;

                currentPathPos = dollyCart.m_Position;
                currentDollyScale = dollyTransform.localScale;
                currentTargetOffset = virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
                currentFOV = virtualCamera.m_Lens.FieldOfView;
                currentRepositionTime = 0;


                if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube) && cameraReposition)
                {

                    currentSelectedCell = selectedCube.collider.gameObject.name;
                    fovDiff = uiFOV - currentFOV;

                    if (currentSelectedCell == "E_u1_Cell_Up_BackLeft" || currentSelectedCell == "D_d4_Cell_Down_BackLeft")
                    {
                        Debug.Log("1");
                        continuePosDifference = (uiPathPosition[1] - currentPathPos);


                        if (continuePosDifference < 0)
                        {
                            reversePosDifference = positionMax - (continuePosDifference * -1);

                            if ((continuePosDifference * -1) > reversePosDifference)
                            {
                                animationPosDifference = reversePosDifference;
                            }
                            else
                            {
                                animationPosDifference = continuePosDifference;
                            }
                        }
                        else
                        {
                            reversePosDifference = positionMax - continuePosDifference;

                            if (continuePosDifference > reversePosDifference)
                            {
                                animationPosDifference = reversePosDifference;
                            }
                            else
                            {
                                animationPosDifference = continuePosDifference;
                            }
                        }

                        if (animationPosDifference < 0)
                        {
                            animationCurveTimingMax = (animationPosDifference * -1) * switchDurationRatioModifier;
                        }
                        else
                        {
                            animationCurveTimingMax = animationPosDifference * switchDurationRatioModifier;
                        }

                        targetOffsetDiff = targetOffsets[1] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "A_d1_Cell_Down_FrontLeft" || currentSelectedCell == "H_u4_Cell _Up_FrontLeft")
                    {
                        Debug.Log("0");
                        continuePosDifference = (uiPathPosition[0] - currentPathPos);

                        if (continuePosDifference < 0)
                        {
                            reversePosDifference = positionMax - (continuePosDifference * -1);

                            if ((continuePosDifference * -1) > reversePosDifference)
                            {
                                animationPosDifference = reversePosDifference;
                            }
                            else
                            {
                                animationPosDifference = continuePosDifference;
                            }
                        }
                        else
                        {
                            reversePosDifference = positionMax - continuePosDifference;

                            if (continuePosDifference > reversePosDifference)
                            {
                                animationPosDifference = reversePosDifference;
                            }
                            else
                            {
                                animationPosDifference = continuePosDifference;
                            }
                        }

                        if (animationPosDifference < 0)
                        {
                            animationCurveTimingMax = (animationPosDifference * -1) * switchDurationRatioModifier;
                        }
                        else
                        {
                            animationCurveTimingMax = animationPosDifference * switchDurationRatioModifier;
                        }

                        targetOffsetDiff = targetOffsets[0] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "B_d2_Cell_Down_FrontRight" || currentSelectedCell == "G_u3_Cell_Up_FrontRight")
                    {
                        //Debug.Log("3");
                        continuePosDifference = (uiPathPosition[3] - currentPathPos);

                        if (continuePosDifference < 0)
                        {
                            reversePosDifference = positionMax - (continuePosDifference * -1);

                            if ((continuePosDifference * -1) > reversePosDifference)
                            {
                                animationPosDifference = reversePosDifference;
                            }
                            else
                            {
                                animationPosDifference = continuePosDifference;
                            }
                        }
                        else
                        {
                            reversePosDifference = positionMax - continuePosDifference;

                            if (continuePosDifference > reversePosDifference)
                            {
                                animationPosDifference = reversePosDifference;
                            }
                            else
                            {
                                animationPosDifference = continuePosDifference;
                            }
                        }

                        if (animationPosDifference < 0)
                        {
                            animationCurveTimingMax = (animationPosDifference * -1) * switchDurationRatioModifier;
                        }
                        else
                        {
                            animationCurveTimingMax = animationPosDifference * switchDurationRatioModifier;
                        }

                        targetOffsetDiff = targetOffsets[3] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "F_u2_Cell_Up_BackRight" || currentSelectedCell == "C_d3_Cell_Down_BackRight")
                    {
                        Debug.Log("2");
                        continuePosDifference = (uiPathPosition[2] - currentPathPos);

                        if (continuePosDifference < 0)
                        {
                            reversePosDifference = positionMax - (continuePosDifference * -1);

                            if ((continuePosDifference * -1) > reversePosDifference)
                            {
                                animationPosDifference = reversePosDifference;
                            }
                            else
                            {
                                animationPosDifference = continuePosDifference;
                            }
                        }
                        else
                        {
                            reversePosDifference = positionMax - continuePosDifference;

                            if (continuePosDifference > reversePosDifference)
                            {
                                animationPosDifference = reversePosDifference;
                            }
                            else
                            {
                                animationPosDifference = continuePosDifference;
                            }
                        }

                        if (animationPosDifference < 0)
                        {
                            animationCurveTimingMax = (animationPosDifference * -1) * switchDurationRatioModifier;
                        }
                        else
                        {
                            animationCurveTimingMax = animationPosDifference * switchDurationRatioModifier;
                        }

                        targetOffsetDiff = targetOffsets[2] - currentTargetOffset;
                    }
                }
            }
        }



        if (Input.GetMouseButton(0) && Input.touchCount < 2 && !aboutCamera && cameraReposition && !switchToUI)
        {
            //Debug raycast d'ouverture UI (actions contextuelles)

            RaycastHit selectedCube;

            if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube))
            {
                Debug.DrawRay(brain.OutputCamera.ScreenPointToRay(Input.mousePosition).origin, brain.OutputCamera.ScreenPointToRay(Input.mousePosition).direction * 8, Color.blue, 5);

                if (currentSelectedCell == selectedCube.collider.gameObject.name && selectedCube.collider.gameObject.GetComponent<CellMovement>().once == false && cameraReposition == true)
                {
                    selectionTimingImage.fillAmount += Time.deltaTime * timingOfSelection;

                    if (selectionTimingImage.fillAmount == 1)
                    {
                        switchToUI = true;
                        cameraReposition = false;
                    }
                }
                else
                {
                    Debug.LogError("No selection valid");
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionTimingImage.fillAmount = 0;
        }


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
        debugTexts[10].text = ("currentX : " + currentX);
        debugTexts[11].text = ("currentY : " + currentY);
        debugTexts[12].text = ("pathOffset : " + pathOffset);
        debugTexts[13].text = ("pathSpeed : " + pathSpeed);
        
        #endregion


        if (!cameraReposition)
            RepositionCamera();

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
            if (onHorizontal)
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
            if (onVertical)
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

        if (onVertical)
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

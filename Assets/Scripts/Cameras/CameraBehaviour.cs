using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;


public class CameraBehaviour : MonoBehaviour
{
    [Header("Ralentissement de rotation CUBE")]
    public Camera fingerCamera; //Camera fixe pour la récupération des positions du doigt pendant une rotation CUBE/Camera
    public CinemachineDollyCart dollyCart; //Récupération de la vitesse de rotation du script DollyCart utilisant le Dolly Horizontal
    public CinemachineVirtualCamera virtualCamera; //Récupération de l'offset du target
    public Transform dollyTransform; //Récupération de la vitesse de rotation du script DollyCart utilisant le Dolly Vertical
    public CinemachineBrain brain;

    //Assignation des valeurs pour les rotations CUBE
    [SerializeField]
    private float pathSpeed;
    [SerializeField]
    private float pathOffset;


    public bool aboutCamera;


    public bool isFingerMoving;


    private bool isRotating;


    [Header("Direction pour la rotation CUBE")]
    private float currentHorizontalSpeed;
    public float defaultHorizontalSpeed;
    public enum HorizontalDirection { left, right, none };
    public HorizontalDirection xDirection;
    [SerializeField]
    private bool onHorizontal;
    [SerializeField]
    private float currentX;
    [SerializeField]
    private float currentY;

    [SerializeField]
    public float currentVerticalSpeed;
    public float maxHeigh;
    public float minHeigh;
    public enum VerticalDirection { up, down, center }
    public VerticalDirection yDirection;
    [SerializeField]
    private bool onVertical;

    //Distance minimum avant de la prise en compte du mouvement (éviter les   glissements de doigt/rotations de camera   inopinés)
    public float minimumMove = 0.05f;
    private bool isOrientationSet;

    [Header("Ralentissement de la Rotation Horizontale")]
    public AnimationCurve descreasingCurve;
    private float currentSlowTime;
    private float remainingSpeed;
    public float slowTimeRatio;
    public float verticalSlowRatio;
    public float horizontalSlowRatio;

    public float lastHorizontalSpeedRecord;
    public float lastVerticalSpeedRecord;
    public bool setRecord;


    private Vector3 currentFingerPosition;
    private Vector3 moveRecorder;
    private Vector3 lastPosition;

    public float currentDistance;

    private bool lastPositionUpdated;

    [Header("Set Up de la Rotation Verticale")]
    [Range(0, 1)]
    public float verticalSpeedRatioModifier;

    private float targetOffset;
    public AnimationCurve targetOffsetCurve;
    private Vector3 targetVectorOffset;


    private float fieldOfView;
    public float maxFOV;
    public float minFOV;
    private bool twoFingers;
    private float fingersDistance;
    private float minDistance;
    public enum Zoom { into, outo, none }
    public Zoom iZoom;


    [Header("Switching Interface Parameters")]

    public List<Vector3> targetOffsets;
    public List<float> uiPathPosition;
    public Vector3 uiDollyScale;
    public Vector3 gameDollyScale;
    public float uiFOV;
    public float gameFOV;
    public Vector3 uiUpDollyPos;
    public Vector3 uiDownDollyPos;
    public Vector3 gameDollyPos;

    public Image selectionTimingImage;
    public float timeBeforeSearch;
    public float maxTimeBeforeSearch;
    public string currentSelectedCell;
    public float timingOfSelection;
    public bool switchToUI;
    public bool isPlayerHere;
    public AnimationCurve cameraRepositioningCurve;
    public AnimationCurve targetRepositioningCurve;
    public float animationCurveTimingMax;
    public float animationTimingMin;
    public float currentRepositionTime;
    private float repoPercent;
    private float retarPercent;

    [Range(0.1f, 1)]
    public float switchDurationRatioModifier;

    private float currentPathPos;
    public float continuePosDifference;
    private float reversePosDifference;
    private float animationPosDifference;
    private float positionMax;

    public float currentFOV;
    public float fovDiff;

    public Vector3 currentDollyScale;
    public Vector3 dollyDiff;

    public Vector3 currentTargetOffset;
    public Vector3 targetOffsetDiff;

    public Vector3 currentDollyPosition;
    public Vector3 dollyPositionDiff;

    public bool cameraReposition = true;



    public static CameraBehaviour Instance;






    private void Awake()
    {
        Instance = this;

        positionMax = dollyTransform.gameObject.GetComponent<CinemachineSmoothPath>().MaxUnit(CinemachinePathBase.PositionUnits.Distance);
        dollyDiff = uiDollyScale - dollyTransform.localScale;
    }

    /// <summary>
    /// Récupère la position de la souris en perspective
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Vector3 GetWorldPositionOnPlane(float z)
    {
        Ray ray = fingerCamera.ScreenPointToRay(Input.mousePosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }



    void Update()
    {

        if (switchToUI)
        {
            if (cameraReposition)
            {
                animationCurveTimingMax = 1.5f;

                Debug.Log("Room set");


                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit selectedCube;

                    currentPathPos = dollyCart.m_Position;
                    currentDollyScale = dollyTransform.localScale;
                    currentDollyPosition = dollyTransform.localPosition;
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
            Debug.Log("Room unset");

            ROOM_Manager.Instance.DeactivateUI();


            if (Input.GetMouseButtonDown(0) && !aboutCamera && cameraReposition)
            {
                RaycastHit selectedCube;

                currentPathPos = dollyCart.m_Position;
                currentDollyScale = dollyTransform.localScale;
                currentDollyPosition = dollyTransform.localPosition;
                currentTargetOffset = virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
                currentFOV = virtualCamera.m_Lens.FieldOfView;
                currentRepositionTime = 0;

                if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube) && cameraReposition)
                {

                    currentSelectedCell = selectedCube.collider.gameObject.name;
                    isPlayerHere = selectedCube.collider.gameObject.GetComponent<CellMovement>().isSpawn;
                    fovDiff = uiFOV - currentFOV;


                    //Debug.Log("We are touching something ... and it's cubic");

                    if (currentSelectedCell == "B_d2_Cell_Down_FrontRight")
                    {
                        Debug.Log("1");
                        continuePosDifference = (uiPathPosition[1] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - gameDollyPos;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[1] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "G_u3_Cell_Up_FrontRight")
                    {
                        Debug.Log("0");
                        continuePosDifference = (uiPathPosition[0] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - gameDollyPos;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[0] - currentTargetOffset;
                    }


                    if (currentSelectedCell == "A_d1_Cell_Down_FrontLeft")
                    {
                        Debug.Log("3");
                        continuePosDifference = (uiPathPosition[3] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - gameDollyPos;

                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[3] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "H_u4_Cell _Up_FrontLeft")
                    {
                        Debug.Log("2");
                        continuePosDifference = (uiPathPosition[2] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - gameDollyPos;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[2] - currentTargetOffset;
                    }


                    if (currentSelectedCell == "E_u1_Cell_Up_BackLeft")
                    {
                        Debug.Log("4");
                        continuePosDifference = (uiPathPosition[4] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - gameDollyPos;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[4] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "D_d4_Cell_Down_BackLeft")
                    {
                        Debug.Log("5");
                        continuePosDifference = (uiPathPosition[5] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - gameDollyPos;

                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[5] - currentTargetOffset;
                    }


                    if (currentSelectedCell == "C_d3_Cell_Down_BackRight")
                    {
                        Debug.Log("7");
                        continuePosDifference = (uiPathPosition[7] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - gameDollyPos;

                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[7] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "F_u2_Cell_Up_BackRight")
                    {
                        Debug.Log("6");
                        continuePosDifference = (uiPathPosition[6] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - gameDollyPos;

                        CheckRepositionWay();

                        targetOffsetDiff = targetOffsets[6] - currentTargetOffset;
                    }

                }
            }
        }


        if (Input.GetMouseButton(0) && !aboutCamera && cameraReposition && !switchToUI && isPlayerHere)
        {

            RaycastHit selectedCube;

            if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube))
            {
                //Debug raycast d'ouverture UI (actions contextuelles)
                Debug.DrawRay(brain.OutputCamera.ScreenPointToRay(Input.mousePosition).origin, brain.OutputCamera.ScreenPointToRay(Input.mousePosition).direction * 8, Color.blue, 5);

                if (currentSelectedCell == selectedCube.collider.gameObject.name && selectedCube.collider.gameObject.GetComponent<CellMovement>().once == false && cameraReposition == true && !isFingerMoving && isPlayerHere)
                {
                    if (timeBeforeSearch < maxTimeBeforeSearch)
                        timeBeforeSearch += Time.deltaTime;
                    else
                        selectionTimingImage.fillAmount += Time.deltaTime * timingOfSelection;

                    if (selectionTimingImage.fillAmount == 1 && currentSelectedCell == selectedCube.collider.gameObject.name && !isFingerMoving && isPlayerHere)
                    {
                        switchToUI = true;
                        cameraReposition = false;
                        StartCoroutine(UIapparitionTime());
                    }
                }
                else
                {
                    Debug.LogError("No selection valid");
                }
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            currentFingerPosition = GetWorldPositionOnPlane(1);
            lastPosition = currentFingerPosition;
        }


        if (Input.GetMouseButtonUp(0))
        {
            isOrientationSet = false;
            isFingerMoving = false;
            setRecord = true;
            onHorizontal = false;
            onVertical = false;

            selectionTimingImage.fillAmount = 0;
            timeBeforeSearch = 0;
        }


        if (Input.GetMouseButton(0))
        {
            currentFingerPosition = GetWorldPositionOnPlane(1);

            currentX = currentFingerPosition.x - lastPosition.x;
            currentY = currentFingerPosition.y - lastPosition.y;


            //Si le sens d'orientation n'a pas encore été set
            if (!isOrientationSet)
            {
                //Si un mouvement a été initié
                if (currentDistance != 0)
                {
                    if (currentX > minimumMove || currentX < -minimumMove)
                    {
                        onHorizontal = true;
                        isOrientationSet = true;
                    }

                    if (currentY > minimumMove || currentY < -minimumMove)
                    {
                        onVertical = true;
                        isOrientationSet = true;
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


            if (aboutCamera)
            {
                Move();
            }
            else
            {
                pathSpeed = 0;
            }



            //Check si le doight du joueur est en mouvement
            if (moveRecorder == currentFingerPosition)
                isFingerMoving = false;
            else
                isFingerMoving = true;


            //Si le doight ne bouge plus, début du ralentissement du déplacement
            if (!isFingerMoving)
            {
                lastPosition = Vector3.MoveTowards(lastPosition, currentFingerPosition, 15);
            }
            else
            {
                currentSlowTime = 0;
                setRecord = true;
            }
        }

        if (!cameraReposition)
            RepositionCamera();

        if (cameraReposition && !switchToUI)
            UpdateComponentValueForRotation();

        //Si le doigt ne bouge pas
        if (!isFingerMoving)
        {
            SlowDownSpeed();
        }

        if (setRecord)
        {
            lastHorizontalSpeedRecord = pathSpeed;
            lastVerticalSpeedRecord = pathOffset;
            setRecord = false;
        }
    }

    /// <summary>
    /// Set le sens de rotation camera pour la reposition
    /// </summary>
    private void CheckRepositionWay()
    {
        if (continuePosDifference < 0)
        {
            Debug.Log("Continue est négatif");

            reversePosDifference = positionMax - (continuePosDifference * -1);

            if ((continuePosDifference * -1) > reversePosDifference)
            {
                //Debug.Log("reversePosDifference");

                animationPosDifference = reversePosDifference;
            }
            else
            {
                animationPosDifference = continuePosDifference;
            }
        }
        else
        {
            Debug.Log("Continue est positif");

            reversePosDifference = positionMax - continuePosDifference;

            if (continuePosDifference > reversePosDifference)
            {
                //Debug.Log("reversePosDifference");
                animationPosDifference = reversePosDifference;
            }
            else
            {
                //Debug.Log("continuePosDifference");
                animationPosDifference = continuePosDifference;
            }
        }

        if (animationPosDifference < 0)
        {
            animationCurveTimingMax = -animationTimingMin + (animationPosDifference * -1) * switchDurationRatioModifier;
        }
        else
        {
            animationCurveTimingMax = animationTimingMin + animationPosDifference * switchDurationRatioModifier;
        }
    }

    private void UpdateComponentValueForRotation()
    {
        //Distance la distance entre le vecteur du doight et sa dernière position enregistré jusqu'au moment où son doigt a bougé
        currentDistance = Vector3.Distance(lastPosition, currentFingerPosition);

        //Update des valeurs concernées
        dollyCart.m_Speed = pathSpeed;
        dollyTransform.position = new Vector3(dollyTransform.position.x, pathOffset, dollyTransform.position.z);
        virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = targetVectorOffset;

        targetOffset = targetOffsetCurve.Evaluate(pathOffset);

        targetVectorOffset = new Vector3(targetVectorOffset.x, targetOffset, targetVectorOffset.z);
    }

    public void LateUpdate()
    {
        moveRecorder = currentFingerPosition;
    }


    /// <summary>
    /// Lance l'animation de cam UI
    /// </summary>
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
            if (!cameraReposition)
            {
                targetOffset = targetOffsetDiff.y * retarPercent;

                //Changement de la position
                dollyCart.m_Position = currentPathPos + animationPosDifference * repoPercent;


                //Changement de la Target Offset
                virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(currentTargetOffset.x + targetOffsetDiff.x * retarPercent,
                    currentTargetOffset.y + targetOffsetDiff.y * retarPercent, currentTargetOffset.z + targetOffsetDiff.z * retarPercent);

                //Changement du Scale du dolly
                //dollyTransform.localScale = new Vector3(currentDollyScale.x + dollyDiff.x * repoPercent, currentDollyScale.y + dollyDiff.y * repoPercent, currentDollyScale.z + dollyDiff.z * repoPercent);

                //Changement du Transform du dolly
                dollyTransform.localPosition = new Vector3(currentDollyPosition.x, currentDollyPosition.y + dollyPositionDiff.y * repoPercent, currentDollyPosition.z);

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
            //dollyTransform.localScale = new Vector3(currentDollyScale.x - dollyDiff.x * repoPercent, currentDollyScale.y - dollyDiff.y * repoPercent, currentDollyScale.z - dollyDiff.z * repoPercent);

            //Changement du Transform du dolly
            dollyTransform.localPosition = new Vector3(currentDollyPosition.x, currentDollyPosition.y - dollyPositionDiff.y * repoPercent, currentDollyPosition.z);

            //Changement de la focale
            virtualCamera.m_Lens.FieldOfView = currentFOV - fovDiff * repoPercent;
        }

    }

    void Move()
    {
        if (onHorizontal)
        {
            CameraTracking();
        }

        if (onVertical)
        {
            AdjustHeight();
        }
    }

    IEnumerator UIapparitionTime()
    {
        yield return new WaitForSeconds(0.5f);

        ROOM_Manager.Instance.LaunchUI();
    }

    /// <summary>
    /// Déplacement de la caméra sur le Dolly Horizontal
    /// </summary>
    void CameraTracking()
    {
        Debug.Log("Tracking");


        if (isFingerMoving)
        {
            currentHorizontalSpeed = defaultHorizontalSpeed + 1 * currentDistance;

            if (xDirection == HorizontalDirection.right && currentDistance > 0)
            {
                pathSpeed = currentHorizontalSpeed;
            }

            if (xDirection == HorizontalDirection.left && currentDistance > 0)
            {
                pathSpeed = -currentHorizontalSpeed;
            }

        }
    }

    /// <summary>
    /// Déplacement de la caméra sur le Dolly Vertical
    /// </summary>
    void AdjustHeight()
    {
        Debug.Log("Adjusting");

        if (isFingerMoving)
        {
            currentVerticalSpeed = Time.deltaTime + currentDistance * verticalSpeedRatioModifier;

            if (yDirection == VerticalDirection.up && currentDistance > 0 && pathOffset > minHeigh)
            {
                pathOffset = -currentVerticalSpeed + lastVerticalSpeedRecord;
            }

            if (yDirection == VerticalDirection.down && currentDistance > 0 && pathOffset < maxHeigh)
            {
                pathOffset = currentVerticalSpeed + lastVerticalSpeedRecord;
            }
        }
        else
        {
            pathOffset = lastVerticalSpeedRecord + currentDistance;
        }
    }


    /// <summary>
    /// Ralenti la Speed de rotation
    /// </summary>
    void SlowDownSpeed()
    {

        if (currentSlowTime < 1)
        {
            currentSlowTime += Time.deltaTime * slowTimeRatio;
        }

        remainingSpeed = descreasingCurve.Evaluate(currentSlowTime);

        pathSpeed = remainingSpeed * lastHorizontalSpeedRecord;
    }

    void FieldOfView()
    {

    }
}

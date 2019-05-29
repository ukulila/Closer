using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class Camera_UI : MonoBehaviour
{

    [Header("Récupération des components")]
    public CinemachineDollyCart dollyCart; //Récupération de la vitesse de rotation du script DollyCart utilisant le Dolly Horizontal
    public CinemachineVirtualCamera virtualCamera; //Récupération de l'offset du target
    public Transform dollyTransform; //Récupération de la vitesse de rotation du script DollyCart utilisant le Dolly Vertical
    public CinemachineBrain brain;

    [Header("Switching Interface Parameters")]
    public bool switchToUI;
    public bool isPlayerHere;

    public CellMovement cellMove;
    public PatchCells cellPatch;
    public string currentSelectedCell;
    public LayerMask cellMask;

    [Header("******* TUTO specific")]
    public bool isLookingForPathCells;

    [Header("   Target Offset")]
    public List<Vector3> targetOffsets;
    public Vector3 currentTargetOffset;
    public Vector3 targetOffsetDiff;

    [Header("   Path Position")]
    public List<float> uiPathPosition;
    private float currentPathPos;
    public float continuePosDifference;
    public float reversePosDifference;
    public float animationPosDifference;
    public float positionMax;

    [Header("   FOV")]
    public float uiFOV;
    public float gameFOV;
    public float currentFOV;
    public float fovDiff;

    [Header("   Dolly Position")]
    public Vector3 uiUpDollyPos;
    public Vector3 uiDownDollyPos;
    public Vector3 gameDollyPos;
    public Vector3 currentDollyPosition;
    public Vector3 dollyPositionDiff;

    [Header("   Animation Parameters")]
    public Image selectionTimingImage;
    public float timeBeforeSearch;
    public float maxTimeBeforeSearch;

    public float timingOfSelection;

    [Space]
    public AnimationCurve cameraRepositioningCurve;
    public float animationCurveTimingMax;
    public float animationTimingMin;
    public float currentRepositionTime;
    public float repoPercent;

    [Range(0, 0.3f)]
    public float switchDurationRatioModifier;

    [Space]
    public bool cameraReposition = true;

    [Header("Debug")]
    public List<TextMeshProUGUI> debugText;



    public static Camera_UI Instance;




    void Awake()
    {
        Instance = this;

        positionMax = dollyCart.m_Path.PathLength;

        debugText[0].text = "animationPosDiff = " + animationPosDifference;
        debugText[1].text = "currentFOV = " + currentFOV;
        debugText[2].text = "fovDiff = " + fovDiff;
        debugText[3].text = "targetOffsetDiff = " + targetOffsetDiff;
        debugText[4].text = "m_Position = " + dollyCart.m_Position;
        debugText[5].text = "animationPosDiff = " + animationPosDifference;
    }

    public void SwitchToNoUi()
    {
        if (GameManager.Instance.currentGameMode == GameManager.GameMode.InvestigationMode || GameManager.Instance.currentGameMode == GameManager.GameMode.Dialogue)
        {
            currentPathPos = dollyCart.m_Position;
            currentDollyPosition = dollyTransform.localPosition;
            currentTargetOffset = virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
            currentFOV = virtualCamera.m_Lens.OrthographicSize;
            currentRepositionTime = 0;

            switchToUI = false;

            ROOM_Manager.Instance.DeactivateUI();
            cameraReposition = false;
        }
    }

    void Update()
    {
        if (switchToUI)
        {
            if (cameraReposition)
            {
                animationCurveTimingMax = animationTimingMin;
            }
        }
        else
        {
            InventorySystem.Instance.canBeDisplayed = true;

            if (Input.GetMouseButtonDown(0) && Camera_Rotation.Instance.aboutCamera == false && cameraReposition && GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
            {
                RaycastHit selectedCube;


                SetInitialCameraValues();


                if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube, Mathf.Infinity, cellMask) && cameraReposition)
                {
                    currentSelectedCell = selectedCube.collider.gameObject.name;

                    if (isLookingForPathCells)
                    {
                        cellPatch = selectedCube.collider.gameObject.GetComponent<PatchCells>();

                        if (cellPatch != null)
                            isPlayerHere = cellPatch.isSpawn;
                    }
                    else
                    {
                        cellMove = selectedCube.collider.gameObject.GetComponent<CellMovement>();

                        if (cellMove != null)
                            isPlayerHere = cellMove.isSpawn;
                    }


                    if (isPlayerHere)
                    {
                        SetCameraNextLocation(currentSelectedCell);
                    }
                }
            }
        }


        if (Input.GetMouseButton(0) && Camera_Rotation.Instance.aboutCamera == false && cameraReposition && !switchToUI && isPlayerHere && GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
        {
            RaycastHit selectedCube;


            if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube, Mathf.Infinity, cellMask))
            {
                //currentSelectedCell = selectedCube.collider.gameObject.name;

                if (isLookingForPathCells)
                {
                    cellPatch = selectedCube.collider.gameObject.GetComponent<PatchCells>();

                    if (cellPatch != null)
                        isPlayerHere = cellPatch.isSpawn;
                }
                else
                {
                    cellMove = selectedCube.collider.gameObject.GetComponent<CellMovement>();

                    if (cellMove != null)
                        isPlayerHere = cellMove.isSpawn;
                }


                if (currentSelectedCell != null && currentSelectedCell == selectedCube.collider.gameObject.name && cameraReposition == true && isPlayerHere)
                {
                    if (!isLookingForPathCells)
                    {
                        if (Camera_Rotation.Instance.isFingerMoving == false)
                        {
                            if (timeBeforeSearch < maxTimeBeforeSearch)
                                timeBeforeSearch += Time.deltaTime;
                            else
                                selectionTimingImage.fillAmount += Time.deltaTime * timingOfSelection;

                            if (selectionTimingImage.fillAmount == 1 && currentSelectedCell == selectedCube.collider.gameObject.name && isPlayerHere)
                            {
                                switchToUI = true;
                                cameraReposition = false;

                                ROOM_Manager.Instance.LaunchUI((animationCurveTimingMax - animationTimingMin) * switchDurationRatioModifier);
                            }
                        }
                    }
                    else
                    {

                        if (timeBeforeSearch < maxTimeBeforeSearch)
                            timeBeforeSearch += Time.deltaTime;
                        else
                            selectionTimingImage.fillAmount += Time.deltaTime * timingOfSelection;


                        if (selectionTimingImage.fillAmount == 1 /*&& currentSelectedCell == selectedCube.collider.gameObject.transform.parent.name*/ && isPlayerHere)
                        {
                            switchToUI = true;
                            cameraReposition = false;

                            Camera_Rotation.Instance.xDirection = Camera_Rotation.HorizontalDirection.none;
                            Camera_Rotation.Instance.yDirection = Camera_Rotation.VerticalDirection.center;

                            Camera_Rotation.Instance.onHorizontal = false;
                            Camera_Rotation.Instance.onVertical = false;

                            Camera_Zoom.Instance.zDirection = Camera_Zoom.ZoomDirection.none;
                            Camera_Zoom.Instance.onZoom = false;

                            ROOM_Manager.Instance.LaunchUI((animationCurveTimingMax - animationTimingMin) * switchDurationRatioModifier);
                        }
                    }
                }
                else
                {
                    selectionTimingImage.fillAmount = 0;
                    timeBeforeSearch = 0;
                    isPlayerHere = false;
                }
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            selectionTimingImage.fillAmount = 0;
            timeBeforeSearch = 0;
            isPlayerHere = false;
        }


        if (!cameraReposition)
            RepositionCamera();

        debugText[0].text = "animationPosDiff = " + animationPosDifference;
        debugText[1].text = "currentFOV = " + virtualCamera.m_Lens.OrthographicSize;
        debugText[2].text = "fovDiff = " + fovDiff;
        debugText[3].text = "targetOffsetDiff = " + targetOffsetDiff;
        debugText[4].text = "m_Position = " + dollyCart.m_Position;
        debugText[5].text = "animationPosDiff = " + animationPosDifference;

    }

    public void SetInitialCameraValues()
    {
        currentPathPos = dollyCart.m_Position;
        currentDollyPosition = dollyTransform.localPosition;
        currentTargetOffset = virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
        currentFOV = virtualCamera.m_Lens.OrthographicSize;
        currentRepositionTime = 0;
    }

    public void SetCameraNextLocation(string nextLocationName)
    {
        if (nextLocationName == "B_d2_Cell_Down_FrontRight")
        {

            continuePosDifference = (uiPathPosition[1] - currentPathPos);
            dollyPositionDiff = uiDownDollyPos - currentDollyPosition;
            fovDiff = uiFOV - currentFOV;


            CheckRepositionWay();


            targetOffsetDiff = targetOffsets[1] - currentTargetOffset;
        }

        if (nextLocationName == "G_u3_Cell_Up_FrontRight")
        {
            continuePosDifference = (uiPathPosition[0] - currentPathPos);
            dollyPositionDiff = uiUpDollyPos - currentDollyPosition;
            fovDiff = uiFOV - currentFOV;


            CheckRepositionWay();


            targetOffsetDiff = targetOffsets[0] - currentTargetOffset;
        }


        if (nextLocationName == "A_d1_Cell_Down_FrontLeft")
        {
            continuePosDifference = (uiPathPosition[3] - currentPathPos);
            dollyPositionDiff = uiDownDollyPos - currentDollyPosition;
            fovDiff = uiFOV - currentFOV;

            CheckRepositionWay();


            targetOffsetDiff = targetOffsets[3] - currentTargetOffset;
        }

        if (nextLocationName == "H_u4_Cell _Up_FrontLeft")
        {
            continuePosDifference = (uiPathPosition[2] - currentPathPos);
            dollyPositionDiff = uiUpDollyPos - currentDollyPosition;
            fovDiff = uiFOV - currentFOV;


            CheckRepositionWay();


            targetOffsetDiff = targetOffsets[2] - currentTargetOffset;
        }


        if (nextLocationName == "E_u1_Cell_Up_BackLeft")
        {
            continuePosDifference = (uiPathPosition[4] - currentPathPos);
            dollyPositionDiff = uiUpDollyPos - currentDollyPosition;
            fovDiff = uiFOV - currentFOV;


            CheckRepositionWay();


            targetOffsetDiff = targetOffsets[4] - currentTargetOffset;
        }

        if (nextLocationName == "D_d4_Cell_Down_BackLeft")
        {
            continuePosDifference = (uiPathPosition[5] - currentPathPos);
            dollyPositionDiff = uiDownDollyPos - currentDollyPosition;
            fovDiff = uiFOV - currentFOV;

            CheckRepositionWay();


            targetOffsetDiff = targetOffsets[5] - currentTargetOffset;
        }


        if (nextLocationName == "C_d3_Cell_Down_BackRight")
        {
            continuePosDifference = (uiPathPosition[7] - currentPathPos);
            dollyPositionDiff = uiDownDollyPos - currentDollyPosition;
            fovDiff = uiFOV - currentFOV;

            CheckRepositionWay();


            targetOffsetDiff = targetOffsets[7] - currentTargetOffset;
        }

        if (nextLocationName == "F_u2_Cell_Up_BackRight")
        {
            continuePosDifference = (uiPathPosition[6] - currentPathPos);
            dollyPositionDiff = uiUpDollyPos - currentDollyPosition;
            fovDiff = uiFOV - currentFOV;

            CheckRepositionWay();

            targetOffsetDiff = targetOffsets[6] - currentTargetOffset;
        }
    }

    /// <summary>
    /// Passe la camera en mode Investigation (zoom sur la room du joueur)
    /// </summary>
    public void RepositionCamera()
    {
        if (currentRepositionTime < animationCurveTimingMax)
        {
            currentRepositionTime += Time.deltaTime;
        }
        else
        {
            if (switchToUI)
            {
                InventorySystem.Instance.canBeDisplayed = false;
            }
            else
            {
                InventorySystem.Instance.canBeDisplayed = true;
            }

            cameraReposition = true;
        }

        repoPercent = cameraRepositioningCurve.Evaluate(currentRepositionTime / animationCurveTimingMax);


        if (!cameraReposition)
        {
            if (switchToUI)
            {
                //Changement de la focale
                virtualCamera.m_Lens.OrthographicSize = currentFOV + (fovDiff * repoPercent);

                //Changement de la Target Offset
                virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(currentTargetOffset.x + targetOffsetDiff.x * repoPercent,
                    currentTargetOffset.y + targetOffsetDiff.y * repoPercent, currentTargetOffset.z + (targetOffsetDiff.z * repoPercent));

                //Changement de la position
                dollyCart.m_Position = currentPathPos + (animationPosDifference * repoPercent);

                //Changement du Transform du dolly
                dollyTransform.position = new Vector3(dollyTransform.position.x, currentDollyPosition.y + (dollyPositionDiff.y * repoPercent),
                    dollyTransform.position.z);
            }
            else
            {
                //Changement de la Target Offset
                virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(currentTargetOffset.x - targetOffsetDiff.x * repoPercent,
                currentTargetOffset.y - targetOffsetDiff.y * repoPercent, currentTargetOffset.z - targetOffsetDiff.z * repoPercent);

                //Changement du Transform du dolly
                dollyTransform.position = new Vector3(dollyTransform.position.x, currentDollyPosition.y - dollyPositionDiff.y * repoPercent,
                        dollyTransform.position.z);

                //Changement de la focale
                virtualCamera.m_Lens.OrthographicSize = currentFOV - fovDiff * repoPercent;
            }
        }
    }

    /// <summary>
    /// Set le sens de rotation camera pour la reposition
    /// </summary>
    private void CheckRepositionWay()
    {
        if (continuePosDifference < 0)
        {
            reversePosDifference = positionMax + continuePosDifference;

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
                animationPosDifference = (reversePosDifference * -1);
            }
            else
            {
                animationPosDifference = continuePosDifference;
            }
        }

        if (continuePosDifference == 0)
        {
            animationCurveTimingMax = animationTimingMin;
        }


        if (animationPosDifference < 0)
        {
            animationCurveTimingMax = animationTimingMin + (animationPosDifference * -1) * switchDurationRatioModifier;
        }
        else
        {
            animationCurveTimingMax = animationTimingMin + animationPosDifference * switchDurationRatioModifier;
        }
    }
}

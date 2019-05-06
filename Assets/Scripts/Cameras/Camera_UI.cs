using System.Collections;
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
    public string currentSelectedCell;

    public List<Image> RTargetImageContextuelle;
    public List<TextMeshProUGUI> RTargetTextContextuelle;


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

    public AnimationCurve cameraRepositioningCurve;
    public AnimationCurve targetRepositioningCurve;
    public float animationCurveTimingMax;
    public float animationTimingMin;
    public float currentRepositionTime;
    private float repoPercent;
    private float retarPercent;

    [Range(0, 0.3f)]
    public float switchDurationRatioModifier;




    public bool cameraReposition = true;

    public List<TextMeshProUGUI> debugTexts;
    public List<Slider> sliders;



    public static Camera_UI Instance;




    void Awake()
    {
        Instance = this;

        positionMax = dollyCart.m_Path.PathLength;
    }


    void Update()
    {
        if (switchToUI)
        {
            ActivateUIRaycastTarget();

            if (cameraReposition)
            {
                animationCurveTimingMax = 1.5f;

                //Debug.Log("Room set");

                if (Input.GetMouseButtonDown(0) && GameManager.Instance.currentGameMode == GameManager.GameMode.InvestigationMode)
                {
                    RaycastHit selectedCube;

                    currentPathPos = dollyCart.m_Position;
                    currentDollyPosition = dollyTransform.localPosition;
                    currentTargetOffset = virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
                    currentFOV = virtualCamera.m_Lens.OrthographicSize;
                    currentRepositionTime = 0;


                    if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube) && cameraReposition)
                    {
                        if (selectedCube.collider)
                        {
                            //Debug.Log("Deactivate UI nOW");
                            switchToUI = false;
                            ROOM_Manager.Instance.DeactivateUI();
                            cameraReposition = false;
                        }
                    }
                }
            }
        }
        else
        {
            //Debug.Log("Room unset");

            InventorySystem.Instance.canBeDisplayed = true;
            DeactivateUIRaycastTarget();

            if (Input.GetMouseButtonDown(0) && Camera_Rotation.Instance.aboutCamera == false && cameraReposition && GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
            {

                RaycastHit selectedCube;
                LayerMask cellMask = LayerMask.GetMask("Cell");

                currentPathPos = dollyCart.m_Position;
                currentDollyPosition = dollyTransform.localPosition;
                currentTargetOffset = virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
                currentFOV = virtualCamera.m_Lens.OrthographicSize;
                currentRepositionTime = 0;

                if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube, cellMask) && cameraReposition)
                {

                    //Debug.DrawRay(brain.OutputCamera.ScreenPointToRay(Input.mousePosition).origin, brain.OutputCamera.ScreenPointToRay(Input.mousePosition).direction * 25, Color.red, 1);

                    //Debug.Log("Target cell");

                    currentSelectedCell = selectedCube.collider.gameObject.transform.parent.name;
                    cellMove = selectedCube.collider.gameObject.transform.parent.GetComponent<CellMovement>();


                    if (cellMove != null)
                        isPlayerHere = cellMove.isSpawn;

                    fovDiff = uiFOV - currentFOV;


                    //Debug.Log("We are touching something ... and it's cubic");

                    if (currentSelectedCell == "B_d2_Cell_Down_FrontRight")
                    {
                        //Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[1] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - currentDollyPosition;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[1] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "G_u3_Cell_Up_FrontRight")
                    {
                        //Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[0] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - currentDollyPosition;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[0] - currentTargetOffset;
                    }


                    if (currentSelectedCell == "A_d1_Cell_Down_FrontLeft")
                    {
                        //Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[3] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - currentDollyPosition;

                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[3] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "H_u4_Cell _Up_FrontLeft")
                    {
                        //Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[2] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - currentDollyPosition;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[2] - currentTargetOffset;
                    }


                    if (currentSelectedCell == "E_u1_Cell_Up_BackLeft")
                    {
                        //Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[4] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - currentDollyPosition;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[4] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "D_d4_Cell_Down_BackLeft")
                    {
                        //Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[5] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - currentDollyPosition;

                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[5] - currentTargetOffset;
                    }


                    if (currentSelectedCell == "C_d3_Cell_Down_BackRight")
                    {
                        //Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[7] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - currentDollyPosition;

                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[7] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "F_u2_Cell_Up_BackRight")
                    {
                        //Debug.Log("continuePosDifference" + continuePosDifference);
                        continuePosDifference = (uiPathPosition[6] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - currentDollyPosition;

                        CheckRepositionWay();

                        targetOffsetDiff = targetOffsets[6] - currentTargetOffset;
                    }

                }
            }
        }


        if (Input.GetMouseButton(0) && Camera_Rotation.Instance.aboutCamera == false && cameraReposition && !switchToUI && isPlayerHere && GameManager.Instance.currentGameMode == GameManager.GameMode.PuzzleMode)
        {

            RaycastHit selectedCube;
            LayerMask cellMask = LayerMask.GetMask("Cell");

            if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube, cellMask))
            {
                //Debug.Log("Drawing Raycast");

                //Debug raycast d'ouverture UI (actions contextuelles)
                //Debug.DrawRay(brain.OutputCamera.ScreenPointToRay(Input.mousePosition).origin, brain.OutputCamera.ScreenPointToRay(Input.mousePosition).direction * 8, Color.blue, 5);

                currentSelectedCell = selectedCube.collider.gameObject.transform.parent.name;
                cellMove = selectedCube.collider.gameObject.transform.parent.GetComponent<CellMovement>();

                if (cellMove != null && currentSelectedCell != null && currentSelectedCell == selectedCube.collider.gameObject.transform.parent.name && selectedCube.collider.gameObject.transform.parent.GetComponent<CellMovement>().once == false
                    && cameraReposition == true && isPlayerHere && cellMove != null)
                {
                    //Debug.Log("COME ON !");

                    if (timeBeforeSearch < maxTimeBeforeSearch)
                        timeBeforeSearch += Time.deltaTime;
                    else
                        selectionTimingImage.fillAmount += Time.deltaTime * timingOfSelection;

                    if (selectionTimingImage.fillAmount == 1 && currentSelectedCell == selectedCube.collider.gameObject.transform.parent.name && isPlayerHere)
                    {
                        switchToUI = true;
                        cameraReposition = false;
                        StartCoroutine(UIapparitionTime(animationCurveTimingMax));
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
            timeBeforeSearch = 0;
        }


        #region DEBUG TEXT

        //debugTexts[0].text = ("cameraReposition : " + cameraReposition);
        //debugTexts[1].text = ("selectionTimingImage.fillAmount : " + selectionTimingImage.fillAmount);
        //debugTexts[2].text = ("switchToUI : " + switchToUI);
        //debugTexts[3].text = ("isZooming : " + timeBeforeSearch);
        //debugTexts[4].text = ("isPlayerHere : " + isPlayerHere);
        //debugTexts[5].text = ("animationCurveTimingMax : " + animationCurveTimingMax);
        //debugTexts[6].text = ("currentRepositionTime : " + currentRepositionTime);
        //debugTexts[7].text = ("speed : " + curr);
        //debugTexts[8].text = ("fieldOfView : " + fieldOfView);
        //debugTexts[9].text = ("currentSlowTime : " + currentSlowTime);
        //debugTexts[10].text = ("currentX : " + currentX);
        //debugTexts[11].text = ("currentY : " + currentY);
        //debugTexts[12].text = ("pathOffset : " + pathOffset);
        //debugTexts[13].text = ("pathSpeed : " + pathSpeed);

        #endregion


        if (!cameraReposition)
            RepositionCamera();
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

                if (InventorySystem.Instance.isInventoryDisplayed)
                    InventorySystem.Instance.inventoryButton.onClick.Invoke();

            }
            else
            {
                InventorySystem.Instance.canBeDisplayed = true;
            }

            cameraReposition = true;
        }

        repoPercent = cameraRepositioningCurve.Evaluate(currentRepositionTime / animationCurveTimingMax);

        retarPercent = targetRepositioningCurve.Evaluate(currentRepositionTime / animationCurveTimingMax);

        if (switchToUI)
        {
            if (!cameraReposition)
            {
                //Changement de la position
                dollyCart.m_Position = currentPathPos + animationPosDifference * repoPercent;


                //Changement de la Target Offset
                virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(currentTargetOffset.x + targetOffsetDiff.x * retarPercent,
                    currentTargetOffset.y + targetOffsetDiff.y * retarPercent, currentTargetOffset.z + targetOffsetDiff.z * retarPercent);

                //Changement du Transform du dolly
                dollyTransform.position = new Vector3(currentDollyPosition.x + dollyPositionDiff.x * repoPercent, currentDollyPosition.y + dollyPositionDiff.y * repoPercent,
                    currentDollyPosition.z + dollyPositionDiff.z * repoPercent);

                //Changement de la focale
                virtualCamera.m_Lens.OrthographicSize = currentFOV + fovDiff * repoPercent;
            }
        }
        else
        {
            //Changement de la Target Offset
            virtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(currentTargetOffset.x - targetOffsetDiff.x * retarPercent,
                currentTargetOffset.y - targetOffsetDiff.y * retarPercent, currentTargetOffset.z - targetOffsetDiff.z * retarPercent);


            //Changement du Transform du dolly
            dollyTransform.position = new Vector3(currentDollyPosition.x - dollyPositionDiff.x * repoPercent, currentDollyPosition.y - dollyPositionDiff.y * repoPercent,
                    currentDollyPosition.z - dollyPositionDiff.z * repoPercent);


            //Changement de la focale
            virtualCamera.m_Lens.OrthographicSize = currentFOV - fovDiff * repoPercent;
        }

    }

    /// <summary>
    /// Set le sens de rotation camera pour la reposition
    /// </summary>
    private void CheckRepositionWay()
    {
        if (continuePosDifference < 0)
        {
            //Debug.Log("Continue est négatif");

            reversePosDifference = positionMax + continuePosDifference;

            if ((continuePosDifference * -1) > reversePosDifference)
            {
                //Debug.Log("Continue 1");

                animationPosDifference = reversePosDifference;
            }
            else
            {
                //Debug.Log("Continue 2");

                animationPosDifference = continuePosDifference;
            }
        }
        else
        {
            //Debug.Log("Continue est positif");

            reversePosDifference = positionMax - continuePosDifference;

            if (continuePosDifference > reversePosDifference)
            {
                //Debug.Log("Continue 3");
                animationPosDifference = reversePosDifference;
            }
            else
            {
                //Debug.Log("Continue 4");

                animationPosDifference = continuePosDifference;
            }
        }

        if (continuePosDifference == 0)
        {
            animationCurveTimingMax = animationTimingMin;
        }


        if (animationPosDifference < 0)
        {
            //Debug.Log("Result 1");

            animationCurveTimingMax = animationTimingMin + (animationPosDifference * -1) * switchDurationRatioModifier;
        }
        else
        {
            //Debug.Log("Result 2");

            animationCurveTimingMax = animationTimingMin + animationPosDifference * switchDurationRatioModifier;
        }


    }

    /// <summary>
    /// Active les Raycast Target de l'UI Contextuelle
    /// </summary>
    public void ActivateUIRaycastTarget()
    {
        for (int i = 0; i < RTargetImageContextuelle.Count; i++)
        {
            RTargetImageContextuelle[i].raycastTarget = true;
            RTargetTextContextuelle[i].raycastTarget = true;
        }
    }

    /// <summary>
    /// Désactive les Raycast Target de l'UI Contextuelle
    /// </summary>
    public void DeactivateUIRaycastTarget()
    {
        for (int i = 0; i < RTargetImageContextuelle.Count; i++)
        {
            RTargetImageContextuelle[i].raycastTarget = false;
            RTargetTextContextuelle[i].raycastTarget = false;
        }
    }

    /// <summary>
    /// Delay avant l'apparition de L'UI d'actions contextuelles
    /// </summary>
    /// <returns></returns>
    IEnumerator UIapparitionTime(float time)
    {
        //Debug.Log("UI will appear");

        yield return new WaitForSeconds(time);

        //Debug.Log("BOOM");

        ROOM_Manager.Instance.LaunchUI();
    }
}

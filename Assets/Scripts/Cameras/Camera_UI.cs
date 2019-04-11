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
    public CellMovement cellMove;
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

    public List<TextMeshProUGUI> debugTexts;
    public List<Slider> sliders;



    public static Camera_UI Instance;




    void Awake()
    {
        Instance = this;
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
                            ROOM_Manager.Instance.DeactivateUI();
                            cameraReposition = false;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Room unset");

            if (Input.GetMouseButtonDown(0) && Camera_Rotation.Instance.aboutCamera == false && cameraReposition)
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
                    cellMove = selectedCube.collider.gameObject.GetComponent<CellMovement>();
                    if (cellMove != null)
                        isPlayerHere = cellMove.isSpawn;
                    fovDiff = uiFOV - currentFOV;


                    //Debug.Log("We are touching something ... and it's cubic");

                    if (currentSelectedCell == "B_d2_Cell_Down_FrontRight")
                    {
                        Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[1] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - gameDollyPos;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[1] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "G_u3_Cell_Up_FrontRight")
                    {
                        Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[0] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - gameDollyPos;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[0] - currentTargetOffset;
                    }


                    if (currentSelectedCell == "A_d1_Cell_Down_FrontLeft")
                    {
                        Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[3] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - gameDollyPos;

                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[3] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "H_u4_Cell _Up_FrontLeft")
                    {
                        Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[2] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - gameDollyPos;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[2] - currentTargetOffset;
                    }


                    if (currentSelectedCell == "E_u1_Cell_Up_BackLeft")
                    {
                        Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[4] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - gameDollyPos;


                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[4] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "D_d4_Cell_Down_BackLeft")
                    {
                        Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[5] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - gameDollyPos;

                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[5] - currentTargetOffset;
                    }


                    if (currentSelectedCell == "C_d3_Cell_Down_BackRight")
                    {
                        Debug.Log("continuePosDifference" + continuePosDifference);

                        continuePosDifference = (uiPathPosition[7] - currentPathPos);
                        dollyPositionDiff = uiDownDollyPos - gameDollyPos;

                        CheckRepositionWay();


                        targetOffsetDiff = targetOffsets[7] - currentTargetOffset;
                    }

                    if (currentSelectedCell == "F_u2_Cell_Up_BackRight")
                    {
                        Debug.Log("continuePosDifference" + continuePosDifference);
                        continuePosDifference = (uiPathPosition[6] - currentPathPos);
                        dollyPositionDiff = uiUpDollyPos - gameDollyPos;

                        CheckRepositionWay();

                        targetOffsetDiff = targetOffsets[6] - currentTargetOffset;
                    }

                }
            }
        }

        
        if (Input.GetMouseButton(0) && Camera_Rotation.Instance.aboutCamera == false && cameraReposition && !switchToUI && isPlayerHere)
        {

            RaycastHit selectedCube;

            if (Physics.Raycast(brain.OutputCamera.ScreenPointToRay(Input.mousePosition), out selectedCube))
            {
                //Debug raycast d'ouverture UI (actions contextuelles)
                Debug.DrawRay(brain.OutputCamera.ScreenPointToRay(Input.mousePosition).origin, brain.OutputCamera.ScreenPointToRay(Input.mousePosition).direction * 8, Color.blue, 5);

                if (currentSelectedCell == selectedCube.collider.gameObject.name && selectedCube.collider.gameObject.GetComponent<CellMovement>().once == false && cameraReposition == true && isPlayerHere && cellMove != null)
                {
                    if (timeBeforeSearch < maxTimeBeforeSearch)
                        timeBeforeSearch += Time.deltaTime;
                    else
                        selectionTimingImage.fillAmount += Time.deltaTime * timingOfSelection;

                    if (selectionTimingImage.fillAmount == 1 && currentSelectedCell == selectedCube.collider.gameObject.name && isPlayerHere)
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


        if (Input.GetMouseButtonUp(0))
        {
            selectionTimingImage.fillAmount = 0;
            timeBeforeSearch = 0;
        }


        #region DEBUG TEXT

        debugTexts[0].text = ("cameraReposition : " + cameraReposition);
        debugTexts[1].text = ("selectionTimingImage.fillAmount : " + selectionTimingImage.fillAmount);
        debugTexts[2].text = ("switchToUI : " + switchToUI);
        debugTexts[3].text = ("isZooming : " + timeBeforeSearch);
        debugTexts[4].text = ("isPlayerHere : " + isPlayerHere);
        debugTexts[5].text = ("animationCurveTimingMax : " + animationCurveTimingMax);
        debugTexts[6].text = ("currentRepositionTime : " + currentRepositionTime);
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

    /// <summary>
    /// Set le sens de rotation camera pour la reposition
    /// </summary>
    private void CheckRepositionWay()
    {
        if (continuePosDifference < 0)
        {
            //Debug.Log("Continue est négatif");

            reversePosDifference = positionMax - (continuePosDifference * -1);

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
    /// Delay avant l'apparition de L'UI d'actions contextuelles
    /// </summary>
    /// <returns></returns>
    IEnumerator UIapparitionTime()
    {
        yield return new WaitForSeconds(0.5f);

        ROOM_Manager.Instance.LaunchUI();
    }
}

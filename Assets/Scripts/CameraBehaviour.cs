using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraBehaviour : MonoBehaviour
{
    [Header("Ralentissement de rotation CUBE")]
    public Camera fingerCamera; //Camera fixe pour la récupération des positions du doigt pendant une rotation CUBE/Camera
    public CinemachineDollyCart camDollyHorizontal; //Récupération de la vitesse de rotation du script DollyCart utilisant le Dolly Horizontal
    public CinemachineVirtualCamera camTargetingOffset; //Récupération de l'offset du target
    public Transform camVertical; //Récupération de la vitesse de rotation du script DollyCart utilisant le Dolly Vertical

    //Assignation des valeurs pour les rotations CUBE
    private float pathSpeed;
    private float pathOffset;


    public bool rotateAroundCube = true;


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
    public float testing = 0.05f;
    private bool isOrientationSet;

    [Header("Ralentissement de la Rotation Horizontale")]
    public AnimationCurve descreasingCurve;
    private float currentSlowTime;
    private float remainingSpeed;


    private float lastHorizontalSpeedRecord;
    private float lastVerticalSpeedRecord;
    private bool setRecord;


    private Vector3 currentFingerPosition;
    private Vector3 moveRecorder;
    private Vector3 lastPosition;

    private float currentDistance;

    private bool lastPositionUpdated;

    [Header("Set Up de la Rotation Verticale")]
    [Range(0,1)]
    public float verticalSpeedRatioModifier;

    private float targetOffset;
    public AnimationCurve targetOffsetCurve;
    private Vector3 targetVectorOffset;

    [Header("Set Up de la Rotation Verticale")]
    private float fieldOfView;
    public float maxFOV;
    public float minFOV;
    private bool twoFingers;
    private float fingersDistance;
    private float minDistance;
    public enum Zoom { into, outo, none }
    public Zoom iZoom;

    public static CameraBehaviour Instance;






    private void Awake()
    {
        Instance = this;
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
        }


        if (Input.GetMouseButton(0))
        {
            currentFingerPosition = GetWorldPositionOnPlane(1);

            currentX = currentFingerPosition.x - lastPosition.x;
            currentY = currentFingerPosition.y - lastPosition.y;


            //Si le sens d'orientation n'a pas encore été set
            if(!isOrientationSet)
            {
                //Si un mouvement a été initié
                if (currentDistance != 0)
                {
                    if (currentX > testing || currentX < -testing)
                    {
                        onHorizontal = true;
                        isOrientationSet = true;
                    }

                    if (currentY > testing || currentY < -testing)
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


            if (rotateAroundCube)
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
        



        //Distance la distance entre le vecteur du doight et sa dernière position enregistré jusqu'au moment où son doigt a bougé
        currentDistance = Vector3.Distance(lastPosition, currentFingerPosition);

        //Update des valeurs concernées
        camDollyHorizontal.m_Speed = pathSpeed;
        camVertical.position = new Vector3(camVertical.position.x, pathOffset, camVertical.position.z);
        camTargetingOffset.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = targetVectorOffset;

        targetOffset = targetOffsetCurve.Evaluate(pathOffset);

        targetVectorOffset = new Vector3(targetVectorOffset.x, targetOffset, targetVectorOffset.z);

        //Si le doigt ne bouge pas
        if (!isFingerMoving)
        {
            SlowDownSpeed();
            /*
            onHorizontal = false;
            onVertical = false;
            */
        }


        if (setRecord)
        {
            lastHorizontalSpeedRecord = pathSpeed;
            lastVerticalSpeedRecord = pathOffset;
            setRecord = false;
        }
    }




    public void LateUpdate()
    {
        moveRecorder = currentFingerPosition;
    }




    void Move()
    {
        if(onHorizontal)
        {
            CameraTracking();
        }
        
        if(onVertical)
        {
            AdjustHeight();
        }
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
            currentVerticalSpeed = Time.deltaTime * 0.01f + currentDistance * verticalSpeedRatioModifier;

            if (yDirection == VerticalDirection.up && currentDistance > 0 && pathOffset > minHeigh)
            {
                pathOffset = - currentVerticalSpeed + lastVerticalSpeedRecord;
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
            currentSlowTime += Time.deltaTime;
        }

        remainingSpeed = descreasingCurve.Evaluate(currentSlowTime);

        pathSpeed = remainingSpeed * lastHorizontalSpeedRecord;
    }

    void FieldOfView()
    {

    }
}

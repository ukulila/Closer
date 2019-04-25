using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CellScript : MonoBehaviour
{
    [Header("   Tweaking Values")]
    [Tooltip("Checker Avec le prog Gameplay pour le tweak de cette value")]
    [Range(0.1f, 5)]
    public float speed = 3;
    [Space(10)]
    public float fin;
    [Space(10)]
    public int timeBetweenTwoTouches = 30;

    [Space(5)]
    [Header("   InGame Values [Don't touch it]")]
    public bool isInRotation;
    public int timer;
    private bool first;
    private int nbrTouch;
    private bool set;
    private Quaternion myRot;
    private int timeRot = 0;

    [Space(10)]
    [Header("   ***** Values : Need Assignment [See Tooltip] *****")]
    [Space(5)]
    [Tooltip("Toutes les autres cells (A_..., B_...)")]
    public List<CellMovement> brothers;
    [Space(10)]
    [Tooltip("Le cube en lui même")]
    public CellPlacement cP;
    [Space(10)]
    [Tooltip("La Camera")]
    public CinemachineBrain myBrain;

    


    void Start()
    {

        first = false;

    }

    void Update()
    {

        if (first)
        {
            timer++;

            if (timer >= timeBetweenTwoTouches)
            {
                nbrTouch = 0;
                timer = 0;
                first = false;
            }
        }

        if (isInRotation)
        {
            transform.Rotate(new Vector3(0, 1, 0), speed);
            cP.isInRotation = true;
            timeRot++;

            if (timeRot > fin)
            {
                timeRot = 0;
                isInRotation = false;
                cP.isInRotation = false;

                for (int r = 0; r < brothers.Count; r++)
                {
                    brothers[r].hasEnded = true;

                }
            }
        }



        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            int LayerMaskCells = LayerMask.GetMask("Cell");

            if (Physics.Raycast(myBrain.OutputCamera.ScreenPointToRay(Input.mousePosition), out hit, 250, LayerMaskCells))
            {
                if(hit.transform.gameObject == gameObject)
                { 
                first = true;
                nbrTouch += 1;


                    if (first && nbrTouch > 1)
                    {
                        for (int r = 0; r < brothers.Count; r++)
                        {
                            brothers[r].hasEnded = false;

                        }

                        set = true;
                        if (set)
                        {
                            myRot = transform.rotation;
                            set = false;
                        }
                        isInRotation = true;
                        nbrTouch = 0;
                    }
                }

            }
        }


    }
}

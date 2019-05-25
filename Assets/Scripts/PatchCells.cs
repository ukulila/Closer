using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PatchCells : MonoBehaviour
{

    public CameraBehaviour cB;
    public Camera_Rotation cR;
    public CinemachineBrain myBrain;
    public bool click;
    public Camera_UI Cui;

    public bool isSpawn;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;

            int LayerMaskCells = LayerMask.GetMask("Cell");

            if (Physics.Raycast(myBrain.OutputCamera.ScreenPointToRay(Input.mousePosition), out hit, 250, LayerMaskCells))
            {
                click = true;

                if (cB != null)
                    cB.aboutCamera = false;
                else
                    cR.aboutCamera = false;
            }
            else
            {
                if (cB != null)
                    cB.aboutCamera = true;
                else
                    cR.aboutCamera = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class MainMenuCamControl : MonoBehaviour
{
    public CinemachineVirtualCamera MainMenu;
    public CinemachineVirtualCamera Options;
    public CinemachineVirtualCamera SelectionNiveaux;
    public CinemachineVirtualCamera Timeline;

    public List<CinemachineVirtualCamera> cameras;
    public CinemachineVirtualCamera CamToDeactivate;

    private bool mainMenu;
    private bool options;
    private bool Selection;
    private bool timeLine;


    private void Awake()
    {
        cameras = new List<CinemachineVirtualCamera>() { MainMenu, Options, SelectionNiveaux, Timeline };
    }

    // Start is called before the first frame update
    void Start()
    {
        MainMenuActif();
        
        for (int i = 0; i < cameras.Count - 1; i++)
        {
            if (cameras[i].gameObject.activeInHierarchy)
            {
                CamToDeactivate = cameras[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MainMenu.gameObject.SetActive(true);

        /*void ActivationOptions()
        {
            // ce qu'il se passe dans le menu

        }*/
    }

    void MainMenuActif()
    {
        


    }

    public void SwitchCam(CinemachineVirtualCamera CamToActivate)
    {
        
        CamToDeactivate.gameObject.SetActive(false);
        CamToActivate.gameObject.SetActive(true);

        CamToDeactivate = CamToActivate;
    }
}

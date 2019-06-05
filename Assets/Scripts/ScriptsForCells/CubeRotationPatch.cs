using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CubeRotationPatch : MonoBehaviour
{

    public PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    public GraphicRaycaster m_Raycaster;
    public GameObject canvasRotation;
    public static CubeRotationPatch Instance;

    public bool hitUIButton;

    public void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            if (m_Raycaster != null)
            {
                m_Raycaster.Raycast(m_PointerEventData, results);

            }
            
             if(results != null)
            {
           // Debug.Log(results);

            }
             
            for (int i = 0; i < results.Count; i++)
            {
                //Debug.Log(results[i]);
                //Debug.Log(results);
                //canvasRotation.SetActive(false);
                hitUIButton = true;
            }
            /*
           foreach (RaycastResult result in results)
           {
            Debug.Log(results);
               
           }*/
        }

        if(Input.GetMouseButtonUp(0))
        {
            hitUIButton = false;
        }
    }


}

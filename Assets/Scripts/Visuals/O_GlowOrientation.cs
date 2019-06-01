using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class O_GlowOrientation : MonoBehaviour
{
    public Color isSpawnColor;
    public Color isSelectedColor;
    public Color isOpenColor;

    [Space (10)]

    public AnimationCurve orientation;
    public float CurrentY;
    public CinemachineDollyCart dolly;

    [Space(10)]

    public List<SpriteRenderer> Childs;

    //isSpawn = "isSpawn";
    //isSelected = "isSelected";
    //isOpen = "isOpen";


    private void OnEnable()
    {
        if(Childs.Count == 0)
        {
            GetChilds();
        }
    }

   

    void Update()
    {
        CurrentY = orientation.Evaluate(dolly.m_Position);
        transform.eulerAngles = new Vector3(0, CurrentY, 0);

    }





    public void ReColor(String state)
    {
        switch (state)
        {
            case "isSpawn":

                for (int i = 0; i < Childs.Count; i++)
                {
                    Childs[i].color = isSpawnColor;
                }

                break;
            case "isSelected":

                for (int i = 0; i < Childs.Count; i++)
                {
                    Childs[i].color = isSelectedColor;
                }

                break;
            case "isOpen":

                for (int i = 0; i < Childs.Count; i++)
                {
                    Childs[i].color = isOpenColor;
                }

                break;
            case "isBlack":

                for (int i = 0; i < Childs.Count; i++)
                {
                    Childs[i].color = Color.black;
                }

                break;

            default:
                for (int i = 0; i < Childs.Count; i++)
                {
                    Childs[i].color = Color.white;
                }
                break;

        }

    }






    private void OnDrawGizmos()
    {
        if (Childs.Count == 0)
        {
            GetChilds();
        }

        if(dolly == null)
        {
           
        }
    }

    private void GetChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Childs.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
    }
}

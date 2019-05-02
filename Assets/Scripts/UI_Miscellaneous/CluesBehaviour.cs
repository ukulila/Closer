using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CluesBehaviour : MonoBehaviour
{
    public List<bool> clues;
    public Transform cluesBoard;
    public Material material;
    public float amount;
    public bool pop;


    public void Update()
    {

        
        for (int i = 0; i < clues.Count; i++)
        {
            

            if (clues[i])
            {
                material = transform.GetChild(i + 1).GetComponent<Renderer>().material;
                amount = material.GetFloat("_Amount");
                material.SetFloat("_Amount", amount);

             //   Debug.Log(transform.GetChild(i + 1));

                pop = true;
            }
          /*  else
            {
                amount = 1;/*
                if (amount >= 0 && amount <= 1)
                {

                    amount += 0.01f;

                }
            }*/
        }


        if(pop)
        {
            if (amount >=0)
            {
                amount -= 0.005f;
                material.SetFloat("_Amount", amount);

            }
            /*else
            {
                pop = false;
            }*/
        }

        
    }

   
}

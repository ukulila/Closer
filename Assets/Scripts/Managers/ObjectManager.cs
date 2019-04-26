using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    public Objet_Interaction currentObjet;


    public static ObjectManager Instance;



    private void Awake()
    {
        Instance = this;
    }

    public void ExamineCurrentObject()
    {

    }
}

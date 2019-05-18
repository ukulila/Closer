using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Renamer : MonoBehaviour
{

    public string PositionName;

    //public List<bool> ExposedPlanes;
    public OpacityKiller ConcernedCell;

    public int CellType;

    //0 = FrontRight
    //1 = FrontLeft
    //2 = BackLeft
    //3 = Backright


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<OpacityKiller>() != null)
        {
            other.gameObject.name = PositionName;

            ConcernedCell = other.GetComponent<OpacityKiller>();


           /* for (int i = 0; i < ExposedPlanes.Count; i++)
            {
                if (ExposedPlanes[i] == true)
                {*/
                    ConcernedCell.OpacityCheck( CellType);
               // }

         //   }

        }


    }

}

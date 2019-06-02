using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnim : MonoBehaviour
{

    public Animator UILevelSelectionAnimator;

    public void Set_myAnimation()
    {

        UILevelSelectionAnimator.SetTrigger("In");

    }

}

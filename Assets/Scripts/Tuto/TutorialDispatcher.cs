using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDispatcher : MonoBehaviour
{
    #region tuto01

    public GameObject camTuto01;
    public GameObject camTuto02;
    public GameObject investigationTuto;
    public GameObject returnTuto;
    public GameObject TalkToTuto;

    public float waitCamTuto01;
    public float waitcamTuto02;
    public float waitinvestigationTuto;
    public float waitreturnTuto;
    public float waitTalkToTuto;

    private bool passed02;
    private bool once;



    public void ThrowCamTuto01Fctn()
    {
        StartCoroutine("ThrowCamTuto01");
    }

    public void ThrowCamTuto02Fctn()
    {
        StartCoroutine("ThrowCamTuto02");
    }

    public void ThrowInvestigationFctn()
    {
        StartCoroutine("ThrowInvestigationTuto");
    }

    public void ThrowTalkToTutoFctn()
    {
        if(once == false)
        {
            StartCoroutine("ThrowTalkToTuto");
            once = true;
        }
    }

    public void offTalkToTutoFctn()
    {
            TalkToTuto.SetActive(false);
    }

    public void ThrowReturnTutoFctn()
    {
        StartCoroutine("ThrowReturnTuto");
    }


    IEnumerator ThrowCamTuto01()
    {
        yield return new WaitForSeconds(waitCamTuto01);

        camTuto01.SetActive(true);
    }

    IEnumerator ThrowCamTuto02()
    {
        yield return new WaitForSeconds(waitcamTuto02-1.5f);

        camTuto01.SetActive(false);

        //yield return new WaitForSeconds(waitcamTuto02);

        yield return new WaitForSeconds(waitcamTuto02-0.5f);
        camTuto02.SetActive(true);
        passed02 = true;
    }

    IEnumerator ThrowInvestigationTuto()
    {
        yield return new WaitForSeconds(waitinvestigationTuto + 3);

        camTuto02.SetActive(false);

        yield return new WaitForSeconds(waitinvestigationTuto-2);

        investigationTuto.SetActive(true);
        StopAllCoroutines();
    }

    IEnumerator ThrowTalkToTuto()
    {
        yield return new WaitForSeconds(0.2f);

        investigationTuto.SetActive(false);



        yield return new WaitForSeconds(waitTalkToTuto);

        TalkToTuto.SetActive(true);
    }

    IEnumerator ThrowReturnTuto()
    {
        yield return new WaitForSeconds(waitreturnTuto);

        returnTuto.SetActive(true);
    }

    #endregion






    public void Update()
    {
        #region tuto01 Update

        if(Camera_Rotation.Instance.aboutCamera == true)
        {

            if(camTuto01.activeInHierarchy == true)
            {
                
                StartCoroutine("ThrowCamTuto02");

            }

            if (camTuto02.activeInHierarchy == true)
            {

                StartCoroutine("ThrowInvestigationTuto");

            }

        }
        #endregion

    }
}
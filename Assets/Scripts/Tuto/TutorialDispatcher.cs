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
        if (once == false)
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
        yield return new WaitForSeconds(1.5f);

        camTuto01.SetActive(true);

        yield return new WaitForSeconds(3);

        camTuto01.SetActive(false);
    }

    IEnumerator ThrowCamTuto02()
    {
        yield return new WaitForSeconds(waitcamTuto02 - 1.5f);

        camTuto01.SetActive(false);

        //yield return new WaitForSeconds(waitcamTuto02);

        yield return new WaitForSeconds(waitcamTuto02 - 0.5f);
        camTuto02.SetActive(true);
        passed02 = true;
    }

    IEnumerator ThrowInvestigationTuto()
    {
        if (DoorsAlignedTuto != null)
            DoorsAlignedTuto.SetActive(false);

            yield return new WaitForSeconds(waitinvestigationTuto + 3);

        if (camTuto02 != null)
            camTuto02.SetActive(false);

        yield return new WaitForSeconds(waitinvestigationTuto - 2);

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


    public GameObject SlideTuto;
    public GameObject DoorsAlignedTuto;

    public float waitSlideTuto;
    public float waitDoorsAlignedTuto;
    public CellMovement targetRoom;
    public bool isTuto01;
    public bool isTuto02;

    public static TutorialDispatcher Instance;
    private bool oneTime;

    public void Awake()
    {
        Instance = this;
    }

    public void ThrowSlideTutoFctn()
    {

        StartCoroutine("ThrowSlideTuto");

    }

    public void ThrowDoorsAlignFctn()
    {

        StartCoroutine("ThrowDoorsAlignedTuto");

    }

    public void InverseThrowDoorsAlignFctn()
    {

        StartCoroutine("InverseThrowDoorsAlignedTuto");

    }

    IEnumerator ThrowSlideTuto()
    {
        yield return new WaitForSeconds(waitSlideTuto);

        SlideTuto.SetActive(true);
    }

    IEnumerator ThrowDoorsAlignedTuto()
    {
        yield return new WaitForSeconds(waitSlideTuto - 1.5f);

        SlideTuto.SetActive(false);

        DoorsAlignedTuto.SetActive(true);
    }

    IEnumerator InverseThrowDoorsAlignedTuto()
    {
        yield return new WaitForSeconds(waitSlideTuto - 1);

        DoorsAlignedTuto.SetActive(false);
        investigationTuto.SetActive(true);

    }

    IEnumerator InvestTuto02()
    {
        if (DoorsAlignedTuto != null)
            DoorsAlignedTuto.SetActive(false);

        yield return new WaitForSeconds(waitinvestigationTuto - 2);

        investigationTuto.SetActive(true);
        StopAllCoroutines();
    }

    public void Update()
    {
        #region tuto01 Update
        if (isTuto01)
        {
            if (Camera_Rotation.Instance.aboutCamera == true)
            {

                if (camTuto01.activeInHierarchy == true)
                {

                    StartCoroutine("ThrowCamTuto02");

                }

                if (camTuto02.activeInHierarchy == true)
                {

                    StartCoroutine("ThrowInvestigationTuto");

                }

            }
        }

        #endregion

        if (isTuto02)
        {

            if (targetRoom.isSpawn == true)
            {
                if(!oneTime)
                {
                    DoorsAlignedTuto.SetActive(false);

                    StartCoroutine("InvestTuto02");

                    oneTime = true;
                }
            }

        }
    }
}
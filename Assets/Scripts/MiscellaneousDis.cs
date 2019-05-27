using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscellaneousDis : MonoBehaviour
{
    public MeshRenderer mR;
    public bool invisibleOnSpawn;

    public void Start()
    {
        mR = GetComponent<MeshRenderer>();

        if(invisibleOnSpawn)
        {
            mR.enabled = false;

            if (transform.childCount != 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.name == "OpacityToZero")
        {
            mR.enabled = true;

            if (transform.childCount != 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (mR.enabled == true)
            {
                if (transform.childCount != 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                mR.enabled = false;

            }
        }


    }

    public void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name == "OpacityToZero")
        {
            mR.enabled = false;
            if (transform.childCount != 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

    }
}

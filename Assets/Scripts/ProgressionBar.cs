using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBar : MonoBehaviour
{
    [Header("Récupération des components")]
    public List<Image> image;
    public List<LineRenderer> linesEmpty;
    public List<LineRenderer> linesFull;
    public List<float> PosXToReach; 
    public List<bool> trigger;
    public List<ParticleSystem> PartSys;
    public Animator progressionAnim;

    [Header("Paramètres")]
    public float timeToStop;
    public float timer;
    public bool timing;
    public bool isHorizontal;
    public bool isVertical;




    public static ProgressionBar Instance;



    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        if (progressionAnim == null)
            progressionAnim = GetComponent<Animator>();

        if (isHorizontal)
        {
            for (int i = 0; i < linesFull.Count; i++)
            {
                //linesFull[i].SetPosition(0, linesFull[i].GetPosition(1));
               // linesFull[i].enabled = false;

                image[0].color = Color.green;

                PartSys[i].Stop();
            }
        }

        #region Vertical
        /*
        if (isVertical)
        {

            for (int i = 0; i < linesFull.Count; i++)
            {

                linesFull[i].SetPosition(1, linesFull[i].GetPosition(0));
                linesFull[i].enabled = false;
                image[0].color = Color.green;
                PartSys[i].Stop();

            }
        }
        */
        #endregion
    }

    public void Update()
    {
        if (isHorizontal)
        {
            if (trigger[0])
            {
               /* if (linesFull[0].GetPosition(0).x < PosXToReach[0])
                {
                  //  linesFull[0].enabled = true;
                  //  linesFull[0].SetPosition(0, new Vector3(linesFull[0].GetPosition(0).x + 0.08f, linesFull[0].GetPosition(0).y, linesFull[0].GetPosition(0).z));
                }
                else
                {*/
                    PartSys[0].Play();
                    image[1].color = Color.green;
                   // linesEmpty[0].enabled = false;


                    if (timer < timeToStop)
                    {
                        timing = true;
                    }

                    if (timer >= timeToStop)
                    {
                        timing = false;
                        timer = 0;
                        PartSys[0].Stop();
                        trigger[0] = false;
                    }
              //  }
            }

            if (trigger[1])
            {
                /*if (linesFull[1].GetPosition(0).x < PosXToReach[1])
                {
                    linesFull[1].enabled = true;
                    linesFull[1].SetPosition(0, new Vector3(linesFull[1].GetPosition(0).x + 0.08f, linesFull[1].GetPosition(0).y, linesFull[1].GetPosition(0).z));
                }
                else
                {*/
                    PartSys[1].Play();
                    image[2].color = Color.green;
                    //linesEmpty[1].enabled = false;


                    if (timer < timeToStop)
                    {
                        timing = true;
                    }

                    if (timer >= timeToStop)
                    {
                        timing = false;
                        timer = 0;
                        PartSys[1].Stop();
                        trigger[1] = false;
                    }
               // }
            }

            if (trigger[2])
            {
                /*if (linesFull[2].GetPosition(0).x < PosXToReach[2])
                {
                    linesFull[2].enabled = true;
                    linesFull[2].SetPosition(0, new Vector3(linesFull[2].GetPosition(0).x + 0.08f, linesFull[2].GetPosition(0).y, linesFull[2].GetPosition(0).z));
                }
                else
                {*/
                    PartSys[2].Play();
                    image[3].color = Color.green;
                   // linesEmpty[2].enabled = false;


                    if (timer < timeToStop)
                    {
                        timing = true;
                    }

                    if (timer >= timeToStop)
                    {
                        timing = false;
                        timer = 0;
                        PartSys[2].Stop();
                        trigger[2] = false;

                    }
             // }
            }

            if (timing)
            {
                timer += 0.1f;
            }
        }

        #region Vertical
        //if (isVertical)
        //{
        //    if (trigger[0])
        //    {
        //        if (linesFull[0].GetPosition(1).y < PosXToReach[0])
        //        {
        //            linesFull[0].enabled = true;
        //            linesFull[0].SetPosition(1, new Vector3(linesFull[0].GetPosition(1).x , linesFull[0].GetPosition(1).y + 0.08f, linesFull[0].GetPosition(1).z));

        //        }
        //        else
        //        {
        //            PartSys[0].Play();
        //            image[1].color = Color.green;
        //            linesEmpty[0].enabled = false;

        //            if (timer < timeToStop)
        //            {
        //                timing = true;
        //            }
        //            if (timer >= timeToStop)
        //            {
        //                timing = false;
        //                timer = 0;
        //                PartSys[0].Stop();
        //                trigger[0] = false;
        //            }
        //        }
        //    }
        //    /*   else
        //       {
        //           linesFull[0].SetPosition(0, linesFull[0].GetPosition(1));
        //           linesFull[0].enabled = false;
        //       }*/

        //    if (trigger[1])
        //    {
        //        if (linesFull[1].GetPosition(1).y < PosXToReach[1])
        //        {
        //            linesFull[1].enabled = true;
        //            linesFull[1].SetPosition(1, new Vector3(linesFull[1].GetPosition(1).x , linesFull[1].GetPosition(1).y + 0.08f, linesFull[1].GetPosition(1).z));

        //        }
        //        else
        //        {
        //            PartSys[1].Play();
        //            image[2].color = Color.green;
        //            linesEmpty[1].enabled = false;

        //            if (timer < timeToStop)
        //            {
        //                timing = true;
        //            }
        //            if (timer >= timeToStop)
        //            {
        //                timing = false;
        //                timer = 0;
        //                PartSys[1].Stop();
        //                trigger[1] = false;

        //            }
        //        }
        //    }
        //    /*  else
        //      {
        //          linesFull[1].SetPosition(0, linesFull[1].GetPosition(1));
        //          linesFull[1].enabled = false;
        //      }*/

        //    if (trigger[2])
        //    {
        //        if (linesFull[2].GetPosition(1).y < PosXToReach[2])
        //        {
        //            linesFull[2].enabled = true;

        //            linesFull[2].SetPosition(1, new Vector3(linesFull[2].GetPosition(1).x , linesFull[2].GetPosition(1).y + 0.08f, linesFull[2].GetPosition(1).z));

        //        }
        //        else
        //        {
        //            PartSys[2].Play();
        //            image[3].color = Color.green;
        //            linesEmpty[2].enabled = false;

        //            if (timer < timeToStop)
        //            {
        //                timing = true;
        //            }
        //            if (timer >= timeToStop)
        //            {
        //                timing = false;
        //                timer = 0;
        //                PartSys[2].Stop();
        //                trigger[2] = false;

        //            }
        //        }
        //    }
        //    /*   else
        //       {
        //           linesFull[2].SetPosition(0, linesFull[2].GetPosition(1));
        //           linesFull[2].enabled = false;
        //       }*/



        //    if (timing)
        //    {
        //        timer += 0.1f;
        //    }




        //}
        #endregion
    }




    /// <summary>
    /// Trigger l'animation de progression
    /// </summary>
    /// <param name="progressionToTrigger"></param>
    public void TriggerProgression(int progressionToTrigger)
    {
        trigger[progressionToTrigger] = true;
    }

    /// <summary>
    /// Trigger l'apparition
    /// </summary>
    public void Apparition()
    {
        progressionAnim.ResetTrigger("FadeOut");
        progressionAnim.ResetTrigger("FadeIn");

        progressionAnim.SetTrigger("FadeIn");
    }

    /// <summary>
    /// Trigger la disparition
    /// </summary>
    public void Disparition()
    {
        progressionAnim.ResetTrigger("FadeOut");
        progressionAnim.ResetTrigger("FadeIn");

        progressionAnim.SetTrigger("FadeOut");
    }

}

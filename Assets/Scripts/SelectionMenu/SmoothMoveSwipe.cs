using System.Collections.Generic;
using UnityEngine;

public class SmoothMoveSwipe : MonoBehaviour
{
    [Header("Swipping parmaeters")]
    public bool isSwipping = true;

    [Space]

    private Vector2 startTouchPosition, endTouchPosition;
    private Vector3 startSwipePosition, endSwipePosition;
    public float swipeTime;
    public float swipeDuration = 0.1f;
<<<<<<< Updated upstream
    //public ElargSelectionLevelSystem largScript;
=======
>>>>>>> Stashed changes
    public int levelMax = 4;
    public int currentLevel;
    public RectTransform line;

    [Space]

    public List<Vector2> levelCranRef;
    public float cranRef;

    [Space]

    [Range(0.1f, 1)]
    public float moveRatio;
    public Vector2 moveStart;
    public Vector2 moveValue;

    [Header("Animation Curve")]
    public AnimationCurve cranAnimationCurve;
    public float currentCranTime;
    public float cranMaxTime;
    public float cranPercent;

    [Space]

    public Vector2 lastPos;
    public Vector2 cranPos;
    public Vector2 diffPos;

    [Space]

    public bool isCranAnimationOver;



    //public Animator[] BAnims;

    public static SmoothMoveSwipe Instance;



    private void Awake()
    {
        Instance = this;

        levelCranRef = new List<Vector2>();

        //for (int i = 0; i < LevelManager.Instance.levels.Count; i++)
        //{
        //    levelCranRef.Add(new Vector2(cranRef - (280 * i), line.localPosition.y));
        //}

        isSwipping = true;
    }

    private void Start()
    {
        for (int i = 0; i < LevelManager.Instance.levels.Count; i++)
        {
            levelCranRef.Add(new Vector2(cranRef - (280 * i), line.localPosition.y));
        }
    }

    private void Update()
    {
        if (isCranAnimationOver == false)
            CrantAnimation();

        if (Input.GetMouseButtonDown(0) && isSwipping)
        {
            isCranAnimationOver = true;
            currentCranTime = 0;

            //Valeurs pour le SWIPE
            moveStart = line.localPosition;
            moveValue = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && isSwipping)
        {
            line.localPosition = new Vector2(Mathf.Clamp(moveStart.x + (Input.mousePosition.x - moveValue.x) * moveRatio, (cranRef - (280 * (LevelManager.Instance.levels.Count - 1))), cranRef), line.localPosition.y);
        }

        if (Input.GetMouseButtonUp(0) && isSwipping)
        {
            lastPos = line.localPosition;

            for (int i = 0; i < levelCranRef.Count; i++)
            {
                Vector2 rightLimit = new Vector2(0, 0);
                Vector2 leftLimit = new Vector2(0, 0);


                if (lastPos.x < levelCranRef[levelCranRef.Count - 1 - i].x)
                {
                    rightLimit = levelCranRef[levelCranRef.Count - 1 - i + 1];
                    leftLimit = levelCranRef[levelCranRef.Count - 1 - i];

                    //Debug.Log("Right " + rightLimit);
                    //Debug.Log("Left " + leftLimit);

                    //Debug.Log("Right comparaison " + (rightLimit.x - lastPos.x));
                    //Debug.Log("Left comparaison " + ((leftLimit.x - lastPos.x) * -1));

                    if ((rightLimit.x - lastPos.x) > ((leftLimit.x - lastPos.x) * -1))
                    {
                        //Debug.Log("GO RIGHT");
                        diffPos = rightLimit - lastPos;
                    }
                    else
                    {
                        //Debug.Log("GO LEFT");
                        diffPos = leftLimit - lastPos;
                    }

                    //Debug.Log(diffPos);

                    isCranAnimationOver = false;
                    return;
                }
            }
        }
    }


    private void CrantAnimation()
    {
        if (currentCranTime < cranMaxTime)
        {
            currentCranTime += Time.deltaTime;
        }
        else
        {
            isCranAnimationOver = true;
        }

        cranPercent = cranAnimationCurve.Evaluate(currentCranTime / cranMaxTime);

        line.localPosition = new Vector2(lastPos.x + diffPos.x * cranPercent, lastPos.y);
    }

    /*
    private IEnumerator Swipe(string whereToGo)
    {
        switch (whereToGo)
        {
            case "left":
                if (currentLevel > 0)
                {
                    BAnims[currentLevel].SetBool("Open", false);
                    //largScript.currentLevel--;
                    BAnims[currentLevel].SetBool("Open", true);
                }

<<<<<<< Updated upstream
                /*swipeTime = 0f;
                startSwipePosition = transform.position;
                endSwipePosition = new Vector3
                    (startSwipePosition.x - 1.75f, transform.position.y, transform.position.z);

                while (swipeTime < swipeDuration)
                {
                    swipeTime += Time.deltaTime;
                    transform.position = Vector2.Lerp
                        (startSwipePosition, endSwipePosition, swipeTime / swipeDuration);
                    yield return null;
                }
                break;

            case "right":
                if (currentLevel < levelMax)
                {
                    BAnims[currentLevel].SetBool("Open", false);
                    //largScript.currentLevel++;
                    BAnims[currentLevel].SetBool("Open", true);
                }

                /*
                swipeTime = 0f;
                startSwipePosition = transform.position;
                endSwipePosition = new Vector3
                    (startSwipePosition.x + 1.75f, transform.position.y, transform.position.z);

                while (swipeTime < swipeDuration)
                {
                    swipeTime += Time.deltaTime;
                    transform.position = Vector2.Lerp
                        (startSwipePosition, endSwipePosition, swipeTime / swipeDuration);
                    yield return null;
                }
                break;
        }

        float lerpValue = 0;
        while (lerpValue < 1)
        {
            print("WHILE");
            lerpValue += Time.deltaTime;
            line.anchoredPosition = Vector3.Lerp(startSwipePosition, endSwipePosition, lerpValue);
            yield return new WaitForEndOfFrame();
        }


        yield return null;
    }
    */
}

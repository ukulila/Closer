using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothMoveSwipe : MonoBehaviour {

    private Vector2 startTouchPosition, endTouchPosition;
    private Vector3 startSwipePosition, endSwipePosition;
    public float swipeTime;
    public float swipeDuration = 0.1f;
    public ElargSelectionLevelSystem largScript;
    public int levelMax = 4;
	public int currentLevel;
	public RectTransform line;


    public Animator[] BAnims;

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            startTouchPosition = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;

            if ((endTouchPosition.x < startTouchPosition.x) && transform.position.x > -1.75f)
                StartCoroutine(Swipe("left"));

            if ((endTouchPosition.x > startTouchPosition.x) && transform.position.x < 1.75f)
                StartCoroutine(Swipe("right"));
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(Swipe("left"));
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Swipe("right"));
        }
    }

    private IEnumerator Swipe(string whereToGo)
    {
        switch (whereToGo)
        {
            case "left":
                if (currentLevel > 0)
                {
                    BAnims[currentLevel].SetBool("Open", false);
                    largScript.currentLevel--;
                    BAnims[currentLevel].SetBool("Open", true);
                }

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
                }*/
                break;

            case "right":
                if (currentLevel < levelMax)
                {
                    BAnims[currentLevel].SetBool("Open", false);
                    largScript.currentLevel++;
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
                }*/
                break;
        }

		float lerpValue = 0;
		while(lerpValue < 1)
		{
			print("WHILE");
			lerpValue += Time.deltaTime;
			line.anchoredPosition = Vector3.Lerp(startSwipePosition, endSwipePosition, lerpValue);
			yield return new WaitForEndOfFrame();
		}
        

        yield return null;
    }
}

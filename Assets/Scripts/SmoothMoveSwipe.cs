using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMoveSwipe : MonoBehaviour {

    private Vector2 startTouchPosition, endTouchPosition;
    private Vector3 startSwipePosition, endSwipePosition;
    public float swipeTime;
    public float swipeDuration = 0.1f;
    public ElargSelectionLevelSystem largScript;
    public int levelMax = 4;

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

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(Swipe("left"));
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(Swipe("right"));
        }
    }

    private IEnumerator Swipe(string whereToGo)
    {
        switch (whereToGo)
        {
            case "left":
                if (largScript.currentLevel > 0)
                {
                    BAnims[largScript.currentLevel].SetBool("Open", false);
                    largScript.currentLevel--;
                    BAnims[largScript.currentLevel].SetBool("Open", true);
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
                if (largScript.currentLevel < levelMax)
                {
                    BAnims[largScript.currentLevel].SetBool("Open", false);
                    largScript.currentLevel++;
                    BAnims[largScript.currentLevel].SetBool("Open", true);
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

        // MOVE PARENT FROM ANIMATORS

        yield return null;
    }
}

using System;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    private Vector2 initialFingerPosition;
    private Vector2 finalFingerPosition;
    public float throwForce;
    public GameObject grenadePrefab;
    private float countdown = 0f;// countdown of the grenade

    [SerializeField]
    private float minDistanceForSwipe = 5f;
    private bool detectSwipeAfterRelease = false;

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;// each frame delete 1s
        foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)// take the initial finger position
                {
                    initialFingerPosition = touch.position;
                }

                if (touch.phase == TouchPhase.Ended)// take the final finger position
                {
                    finalFingerPosition = touch.position;
                    DetectionSwipe();
                }
            }
        }

    void throwGrenade()
    {

        throwForcecalc(VerticalMovementDistance());
        GameObject gr = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = gr.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward*throwForce, ForceMode.VelocityChange);
    }

    private void DetectionSwipe()
    {

            if (SwipeDistanceCheckMet())
            {
                if (isSwipeStraight())
                {
                if (countdown <= 0f)
                {
                    throwGrenade();
                    countdown = 3f;// make the countdown 3s to not spam grenades
                }
                }
            }
        }

    private bool isSwipeStraight()// checking is the swipe is straight
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()// checking if the swipe met the condition
    {
        return VerticalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(finalFingerPosition.y - initialFingerPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(finalFingerPosition.x - initialFingerPosition.x);
    }

    void throwForcecalc(float swipeDist)// to calculate the throw force
    {
        throwForce = 0.09f * swipeDist;
    }
}

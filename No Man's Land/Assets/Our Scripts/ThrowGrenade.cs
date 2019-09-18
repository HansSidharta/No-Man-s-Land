using System;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    private Vector2 initialFingerPosition;
    private Vector2 finalFingerPosition;
    public float throwForce;
    public GameObject grenadePrefab;
    private float countdown = 3f;

    [SerializeField]
    private float minDistanceForSwipe = 5f;
    private bool detectSwipeAfterRelease = false;

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    initialFingerPosition = touch.position;
                }

                //if (!detectSwipeAfterRelease && touch.phase == TouchPhase.Moved)
                //{
                //    finalFingerPosition = touch.position;
                //    DetectionSwipe();
                //}

                if (touch.phase == TouchPhase.Ended)
                {
                    finalFingerPosition = touch.position;
                    DetectionSwipe();
                }
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
        if(SwipeDistanceCheckMet())
        {
            if(isSwipeStraight())
            {
                throwGrenade();
            }
        }
    }

    private bool isSwipeStraight()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
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

    void throwForcecalc(float swipeDist)
    {
        throwForce = 0.09f * swipeDist;
    }
}

using UnityEngine;
using System;

public class AI : MonoBehaviour
{
    private float startPointX;
    public GameObject AIPlayer;
    private float speed = 0.1f;
    private float AITime = 1f;
    private float stopMovingTime = 1f;
    private float movingTime = 1f;
    private bool stopTrackMove;
    private Boolean moveRight = true;
    public ThrowGrenade throwGrenade;
    // Start is called before the first frame update
    void Start()
    {
        startPointX = AIPlayer.transform.position.x;
        throwGrenade = FindObjectOfType<ThrowGrenade>();
    }

    // Update is called once per frame
    void Update()
    {
        AITime -= Time.deltaTime;
        stopMovingTime -= Time.deltaTime;
        movingTime -= Time.deltaTime;

        System.Random r = new System.Random();

        //int algorithm = r.Next(51);

        if(AITime <= 0f)
        {
            throwGrenade.throwGrenade();
            StopMoving();
            stopMovingTime = 3f;
            AITime = 5f;
        }
        else if(stopMovingTime<=0f)
        {
            if(startPointX - 40 > AIPlayer.transform.position.x)
            {
                MoveRight();
                moveRight = true;
            }
            else if(startPointX + 40 < AIPlayer.transform.position.x)
            {
                MoveLeft();
                moveRight = false;

            }
            else
            {
                int a = r.Next(51);
                if(a < 24 && movingTime <= 0f)
                {
                    MoveLeft();
                    movingTime = 4f;
                    moveRight = true;
                }
                else if(a >= 24 && movingTime <= 0f)
                {
                    MoveRight();
                    movingTime = 4f;
                    moveRight = false;
                }
                else
                {
                    if(moveRight)
                    {
                        MoveRight();
                    }
                    else
                    {
                        MoveLeft();
                    }
                }
            }
        }
    }

    //private void LeftWall()
    //{
    //    if (startPointX - 40 > AIPlayer.transform.position.x)
    //    {
    //        MoveRight();
    //        moveRight = true;
    //    }
    //    else
    //    {
    //        MoveLeft();
    //        moveRight = false;
    //    }
    //}

    //private void RightWall()
    //{
    //    if (startPointX + 40 < AIPlayer.transform.position.x)
    //    {
    //        MoveLeft();
    //        moveRight = false;
    //    }
    //    else
    //    {
    //        MoveRight();
    //        moveRight = true;
    //    }
    //}

    private void MoveLeft()
    {
        AIPlayer.transform.Translate(-speed, 0f, 0f);
    }
    private void MoveRight()
    {
        AIPlayer.transform.Translate(speed, 0f, 0f);
    }

    private void StopMoving()
    {
        AIPlayer.transform.Translate(0f, 0f, 0f);
    }
}

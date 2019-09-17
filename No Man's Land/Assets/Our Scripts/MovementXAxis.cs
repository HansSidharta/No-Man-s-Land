using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementXAxis : MonoBehaviour
{
    //variables
    public float moveSpeed = 0.5f;
    public GameObject character;
    public Rigidbody characterBody;
    private float screenwidth;
    private float screenHeight;
    public bool facingLeft = true;


    // Start is called before the first frame update
    void Start()
    {
        screenwidth = Screen.width;//gets the screen width
        screenHeight = Screen.height;//gets the screen height
        characterBody = character.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //controls for the left/right touchscreen
        if (Input.touchCount > 0)
        {
            //get touch position
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            //clamp the position so the sprite cannot move off the screen
            var pos = Camera.main.WorldToViewportPoint(transform.position);
            pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
            transform.position = Camera.main.ViewportToWorldPoint(pos);

            // if (touchPosition.x < 0  && touchPosition.y < -60)//make the sprite move left
            // {
            //     if (facingLeft)
            //     {
            //         Flip();
            //     }
            //     MoveLeft();
            // }
            // else if (touchPosition.x > 0 && touchPosition.y < -60)//make the character move right
            // {
            //     if (!facingLeft)
            //     {
            //         Flip();
            //     }
            //     MoveRight();
            // }
        }
        else
        {
            StopMoving();
        }
    }
    public void MoveLeft()//method to move left
    {
        characterBody.velocity = new Vector2(-moveSpeed, characterBody.velocity.y);
    }
    public void MoveRight()//method to move right
    {
        characterBody.velocity = new Vector2(moveSpeed, characterBody.velocity.y);
    }
    public void StopMoving()//stop the sprite moving
    {
        characterBody.velocity = new Vector2(0f, characterBody.velocity.y);
    }
    public void Flip()//to flip the sprite depending on pressing left or right
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

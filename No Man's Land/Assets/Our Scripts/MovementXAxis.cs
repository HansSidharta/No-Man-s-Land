using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementXAxis : MonoBehaviour
{
    //variables
    public float moveSpeed = 300;
    public GameObject character;
    private Rigidbody2D characterBody;
    private float screenwidth;


    // Start is called before the first frame update
    void Start()
    {
        screenwidth = Screen.width;
        characterBody = character.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if(touchPosition.x > 0)
            {
                MoveRight();
            }
            else if(touchPosition.x < 0)
            {
                MoveLeft();
            }
        }
        else
        {
            StopMoving();
        }
    }
    public void MoveLeft()
    {
        characterBody.velocity = new Vector2(-moveSpeed, characterBody.velocity.y);
    }
    public void MoveRight()
    {
        characterBody.velocity = new Vector2(moveSpeed, characterBody.velocity.y);
    }
    public void StopMoving()
    {
        characterBody.velocity = new Vector2(0f, characterBody.velocity.y);
    }
    //method to be able to be tested in Unity Editor
   /* void FixedUpdate()
    {
        #if UNITY_EDITOR
        RunCharacter(Input.GetAxis("Horizontal"));
        #endif
    }*/

}

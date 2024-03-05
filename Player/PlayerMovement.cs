using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terresquall;

public class PlayerMovement : MonoBehaviour
{

    public const int DEFAULT_MOVESPEED = 2;

    // Movement
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 lastMovedVector;

    //References
    Rigidbody2D rb;
    
    PlayerStats player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
        
    }
    private void FixedUpdate()
    {
        Move();
    }

    void InputManagement()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        float moveX, moveY;
        if (VirtualJoystick.CountActiveInstances() > 0 )
        {
            moveX = VirtualJoystick.GetAxisRaw("Horizontal");
            moveY = VirtualJoystick.GetAxisRaw("Vertical");
        }
        else
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
        }
        

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0);
        }
        else if (moveDir.y != 0) 
        { 
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
        }
        
        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
        }
        
    }

    void Move()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        } 
        rb.velocity = moveDir * DEFAULT_MOVESPEED * player.Stats.moveSpeed;
    }
}

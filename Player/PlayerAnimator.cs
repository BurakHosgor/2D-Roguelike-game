using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator anim;
    PlayerMovement pm;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pm = FindObjectOfType<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.moveDir.x != 0 || pm.moveDir.y != 0)
        {
            anim.SetBool("Move", true);
            SpriteDirectonChecker();
        }
        else
        {
            anim.SetBool("Move" , false);
        }
        
    }
    void SpriteDirectonChecker()
    {
        if (pm.lastHorizontalVector < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}

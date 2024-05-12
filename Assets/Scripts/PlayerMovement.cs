using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

  [SerializeField] float playerspeed = 4f;
  [SerializeField] float jumpSpeed = 10f;
  [SerializeField] float ClimbingSpeed = 2f;

  bool isAlive = true;
  Animator PlayerAnimator;
  Rigidbody2D myRigidbody;
  Vector2 moveInput;

  BoxCollider2D myBodyCollider;


  [SerializeField] float ladderGravity = 0f;

  string IsJumping = "Jumping";
  bool TouchingGround;
  void Start()
  {
    PlayerAnimator = GetComponent<Animator>();
    myRigidbody = GetComponent<Rigidbody2D>();
    myBodyCollider = GetComponent<BoxCollider2D>();
  }


  void Update()
  {
    if (!isAlive) return;
    Run();
    FlipSprite();
    AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
    if (stateInfo.IsName(IsJumping) && stateInfo.normalizedTime >= 1)
    {
      Debug.Log("Is not jujing dog");
      PlayerAnimator.SetBool("IsJumping", false);

    }
    /*if (Mathf.Abs(myRigidbody.velocity.y) <= Mathf.Epsilon)
    {
      PlayerAnimator.SetBool("IsJumping", false);
      // Debug.Log("Is not jumping");
    } */
    ClimbLadder();
    Die();
  }

  void OnMove(InputValue value)
  {
    if (!isAlive) return;
    moveInput = value.Get<Vector2>();
    Debug.Log(moveInput);


  }
  /*
  //ALTERNATIVE SOLUTION 
   void OnCollisionEnter2D(Collision2D collision)
   {
     if (collision.gameObject.tag == "Level")
     {
       TouchingGround = true;
       Debug.Log("I am touching the ground");
     }
   }

   void OnCollisionExit2D(Collision2D collision)
   {
     if (collision.gameObject.tag == "Level")
     {
       Debug.Log("NOT touching dog");
       TouchingGround = false;
     }
   }

   */



  void OnJump(InputValue value)
  {
    if (!isAlive) return;
    LayerMask Groundmask = LayerMask.GetMask("Ground");
    if (value.isPressed && myBodyCollider.IsTouchingLayers(Groundmask))
    {
      myRigidbody.velocity += new Vector2(0f, jumpSpeed);
      PlayerAnimator.SetBool("IsJumping", true);
      Debug.Log("Is jumping");
    }
    //) /* && TouchingGround == true*/)

  }


  void ClimbLadder()
  {

    LayerMask Climbingmask = LayerMask.GetMask("Climbing");

    if (GetComponent<CapsuleCollider2D>().IsTouchingLayers(Climbingmask))
    {
      myRigidbody.gravityScale = ladderGravity;
      Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * ClimbingSpeed);
      myRigidbody.velocity = climbVelocity;
      PlayerAnimator.SetBool("isclimbing", true);
    }
    else
    {
      myRigidbody.gravityScale = 1.5f;
      PlayerAnimator.SetBool("isclimbing", false);
    }
  }

  void Run()
  {
    Vector2 playerVelocity = new Vector2(moveInput.x * playerspeed, myRigidbody.velocity.y);
    myRigidbody.velocity = playerVelocity;
    if (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon)
    {
      PlayerAnimator.SetBool("isRunning", true);
    }
    else
    {
      PlayerAnimator.SetBool("isRunning", false);
    }
  }

  void FlipSprite()
  {
    bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

    if (playerHasHorizontalSpeed)
    {
      transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);

    }
  }

  void Die()
  {
    LayerMask EnemyMask = LayerMask.GetMask("Enemy");
    if (myRigidbody.IsTouchingLayers(EnemyMask))
    {
      isAlive = false;
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  Rigidbody2D Groberbody;
  BoxCollider2D GroberCollider;
  [SerializeField] float moveSpeed = 1f;

  void Start()
  {
    Groberbody = GetComponent<Rigidbody2D>();
    GroberCollider = GetComponent<BoxCollider2D>();
  }


  void Update()
  {
    Groberbody.velocity = new Vector2(moveSpeed, 0f);
  }

  void OnTriggerExit2D(Collider2D other)
  {
    moveSpeed = -moveSpeed;
    FlipEnemyFacing();
  }

  void FlipEnemyFacing()
  {

    transform.localScale = new Vector2(-(Mathf.Sign(Groberbody.velocity.x)), 1f);
  }

}

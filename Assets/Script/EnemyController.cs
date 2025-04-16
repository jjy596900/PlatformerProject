using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private bool isMovingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

    }

    private void OnTriggerEnter2D(Collider2D collistion)
    {
        if (collistion.CompareTag("Boundary")) 
        {
            isMovingRight = !isMovingRight;
        }

    }




}

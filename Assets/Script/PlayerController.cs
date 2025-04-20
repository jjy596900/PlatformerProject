using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Vector2 movement;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Animator pAni;
    private bool isGrounded;

    public bool isInvincible = false;
    public float invincibilityDuration = 5f;
    private float invincibilityTimer = 0f;

    private bool isSpeedBoosted = false;
    private float speedBoostTimer = 0f;
    private float originalSpeed;

    private bool isJumpBoosted = false;
    private float jumpBoostTimer = 0f;
    private float originalJumpForce;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed;

        rb = GetComponent<Rigidbody2D>();
        originalJumpForce = jumpForce;
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (isJumpBoosted)
        {
            jumpBoostTimer -= Time.deltaTime;
            if (jumpBoostTimer <= 0f)
            {
                jumpForce = originalJumpForce;
                isJumpBoosted = false;
                Debug.Log("점프 증가 효과 종료");
            }
        }

        bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        if (moveInput > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("JumpAction");
        }

        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                Debug.Log("무적 상태 해제");
            }

            if (isSpeedBoosted)
            {
                speedBoostTimer -= Time.deltaTime;
                if (speedBoostTimer <= 0f)
                {
                    moveSpeed = originalSpeed;
                    isSpeedBoosted = false;
                    Debug.Log("속도 증가 종료");
                }
            }

        }



    }


    public void ActivateInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        Debug.Log("무적 상태 시작!");

        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    public void ActivateSpeedBoost(float speedMultiplier, float duration)
    {
        if (!isSpeedBoosted)
            originalSpeed = moveSpeed;

        moveSpeed = originalSpeed * speedMultiplier;
        speedBoostTimer = duration;
        isSpeedBoosted = true;
        Debug.Log("속도 증가 발동!");
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            Debug.Log("무적 상태! 데미지 무시");
            return;
        }

        Debug.Log($"플레이어가 {damage} 데미지를 입었습니다.");
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.CompareTag("Finish")) 
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


    }
    public void ActivateJumpBoost(float multiplier, float duration)
    {
        if (!isJumpBoosted)
            originalJumpForce = jumpForce;

        jumpForce = originalJumpForce * multiplier;
        jumpBoostTimer = duration;
        isJumpBoosted = true;
        Debug.Log("점프 증가 발동!");
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

}
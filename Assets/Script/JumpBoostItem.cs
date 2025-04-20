using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoostItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float jumpMultiplier = 1.5f;  // 1.5배 점프
    public float boostDuration = 5f;     // 5초 지속

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivateJumpBoost(jumpMultiplier, boostDuration);
                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 change;
    public float speed;
    private Animator animator;

    private Rigidbody2D physicsBody;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        physicsBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        change.Normalize();
        
    }

    private void FixedUpdate()
    {
        if (change != Vector2.zero)
        {
            Vector2 position = physicsBody.position;
            position += change * speed * Time.deltaTime;
            physicsBody.MovePosition(position);
            animator.SetFloat("MoveX", change.x);
            animator.SetFloat("MoveY", change.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }
}

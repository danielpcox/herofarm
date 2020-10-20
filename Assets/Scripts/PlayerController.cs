using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 change;
    private Vector2 lookDirection;
    public float speed;
    private Animator animator;

    private Rigidbody2D physicsBody;

    private ArrayList inventory = new ArrayList();

    void Start()
    {
        animator = GetComponent<Animator>();
        physicsBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        change.Normalize();
        if (change != Vector2.zero)
        {
            lookDirection = change;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                physicsBody.position + Vector2.up * 0.2f,
                lookDirection,
                1.5f,
                LayerMask.GetMask("Collectibles"));
            if (hit.collider != null)
            {
                GameObject go = hit.collider.gameObject;
                Debug.Log(go.name);
                inventory.Add(go);
                go.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (inventory.Count > 0)
            {
                GameObject go = (GameObject)inventory[0];
                go.SetActive(true);
                inventory.RemoveAt(0);
            }
        }
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

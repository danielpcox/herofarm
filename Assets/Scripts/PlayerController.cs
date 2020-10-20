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
    public int inventoryCapacity;

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

        // Slurp into inventory
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                physicsBody.position,
                lookDirection,
                1.5f,
                LayerMask.GetMask("Collectibles"));
            if (hit.collider!=null && inventory.Count < inventoryCapacity)
            {
                GameObject go = hit.collider.gameObject;
                Debug.Log(go.name);
                go.SetActive(false);
                inventory.Add(go);
            }
        }

        // Dump from inventory
        if (Input.GetKeyDown(KeyCode.C))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                   physicsBody.position,
                   lookDirection,
                   1.5f,
                   ~LayerMask.GetMask("Player"));
            if (hit.collider==null && inventory.Count > 0)
            {
                GameObject go = (GameObject)inventory[0];
                go.transform.position = physicsBody.position + lookDirection * 1.5f;
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

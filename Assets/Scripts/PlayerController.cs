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

    public InventoryDisplay inventoryDisplay;
    public static int inventoryCapacity = 10;
    private GameObject[] inventory; // = new GameObject[inventoryCapacity];
    private int inventoryCursor = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        physicsBody = GetComponent<Rigidbody2D>();
        inventory = new GameObject[inventoryDisplay.images.Length];
    }

    void Update()
    {
        // capture input
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        change.Normalize();
        if (change != Vector2.zero)
        {
            lookDirection = change;
        }

        // Slurp into inventory
        if (Input.GetKeyDown(KeyCode.X) && inventory[inventoryCursor]==null)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                physicsBody.position,
                lookDirection,
                1.5f,
                LayerMask.GetMask("Collectibles"));
            if (hit.collider!=null)
            {
                GameObject go = hit.collider.gameObject;
                Debug.Log($"Adding to inventory: {go.name}");
                go.SetActive(false);
                inventory[inventoryCursor] = go;
                inventoryDisplay.PlaceAt(inventoryCursor, go);
            }
        }

        // Dump from inventory
        if (Input.GetKeyDown(KeyCode.C) && inventory[inventoryCursor]!=null)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                   physicsBody.position,
                   lookDirection,
                   1.5f,
                   ~LayerMask.GetMask("Player"));
            if (hit.collider==null)
            {
                GameObject go = inventory[inventoryCursor];
                go.transform.position = physicsBody.position + lookDirection * 1.5f;
                go.SetActive(true);
                inventory[inventoryCursor] = null;
                inventoryDisplay.ClearAt(inventoryCursor);
            }
        }

        // Move cursor
        if (Input.GetKeyDown(KeyCode.E))
            DecrementInventoryCursor();
        if (Input.GetKeyDown(KeyCode.R))
            IncrementInventoryCursor();
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


    // TODO move the whole inventory system into a ScriptableObject
    private void IncrementInventoryCursor()
    {
        if (inventoryCursor < inventoryCapacity - 1)
            inventoryCursor++;
        Debug.Log($"inventoryCursor: {inventoryCursor}");
    }

    private void DecrementInventoryCursor()
    {
        if (inventoryCursor > 0)
            inventoryCursor--;
        Debug.Log($"inventoryCursor: {inventoryCursor}");
    }
}

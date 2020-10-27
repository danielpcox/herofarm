using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Vector2 change;
    private Vector2 lookDirection;
    public float speed;
    private Animator animator;

    private Rigidbody2D physicsBody;

    public InventoryDisplay inventoryDisplay;
    public static int inventoryCapacity = 10;
    private GameObject[] inventory;
    private int inventoryCursor = 0;

    public float maxEnergy;
    private float energy;
    public Slider energyDisplay;

    void Start()
    {
        animator = GetComponent<Animator>();
        physicsBody = GetComponent<Rigidbody2D>();
        inventory = new GameObject[inventoryDisplay.images.Length];

        energy = maxEnergy / 2;
        energyDisplay.value = energy / maxEnergy;
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                   physicsBody.position,
                   lookDirection,
                   1.5f,
                   LayerMask.GetMask("Collectibles"));
            if (hit.collider != null)
            {

                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] == null)
                    {
                        GameObject go = hit.collider.gameObject;
                        go.SetActive(false);
                        inventory[i] = go;
                        inventoryDisplay.PlaceAt(i, go);
                        break;
                    }
                }
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

        // Eat food
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameObject item = inventory[inventoryCursor];
            Food food = item.GetComponent<Food>();
            if (food != null)
            {
                food.Eat(this);
                inventoryDisplay.ClearAt(inventoryCursor);
                Destroy(item);
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


    // TODO move the whole inventory system into a ScriptableObject
    private void IncrementInventoryCursor()
    {
        if (inventoryCursor < inventoryCapacity - 1)
        {
            inventoryDisplay.CursorAt(++inventoryCursor);
        }
        Debug.Log($"inventoryCursor: {inventoryCursor}");
    }

    private void DecrementInventoryCursor()
    {
        if (inventoryCursor > 0)
        {
            inventoryDisplay.CursorAt(--inventoryCursor);

        }
    }



    public void ChangeEnergy(float amount)
    {
        energy = Mathf.Clamp(energy+amount, 0, maxEnergy);
        energyDisplay.value = energy / maxEnergy;
    }
}

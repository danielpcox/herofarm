using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public Image[] images = new Image[10];

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceAt(int position, GameObject go)
    {
        Image itemImage = images[position];
        Text itemText = images[position].GetComponentInChildren<Text>();
        itemImage.gameObject.SetActive(true);
        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
        itemImage.sprite = spriteRenderer.sprite;
        itemText.text = go.name;
    }

    public void ClearAt(int position)
    {
        images[position].gameObject.SetActive(false);
    }
}

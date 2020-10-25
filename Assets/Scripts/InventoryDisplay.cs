using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public Image[] images = new Image[10];
    private Sprite emptySlot;
    private int previousCursorPosition = 0;

    private void Start()
    {
        emptySlot = images[0].sprite;
        images[0].color = Color.red;
    }

    public void PlaceAt(int position, GameObject go)
    {
        Image image = images[position];
        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
        image.sprite = spriteRenderer.sprite;

        TextMeshProUGUI itemCaption = images[position].GetComponentInChildren<TextMeshProUGUI>();
        itemCaption.enabled = true;
        itemCaption.SetText(go.name);
    }

    public void ClearAt(int position)
    {
        Image image = images[position];
        image.sprite = emptySlot;
        TextMeshProUGUI itemCaption = image.GetComponentInChildren<TextMeshProUGUI>();
        itemCaption.enabled = false;
    }

    public void CursorAt(int position)
    {
        Image image = images[position];
        image.color = Color.red;

        Image previousImage = images[previousCursorPosition];
        previousImage.color = Color.white;

        previousCursorPosition = position;
    }
}

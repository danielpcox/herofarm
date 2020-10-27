using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public void Eat(PlayerController player)
    {
        player.ChangeEnergy(1);
    }
}

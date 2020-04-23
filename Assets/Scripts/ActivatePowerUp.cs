using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePowerUp : MonoBehaviour
{
    //when the player enters the power ups box collider
    void OnTriggerEnter2D(Collider2D player)
    {
        //it gets the game status 
        GameObject go = GameObject.Find("GameStatus");
        GameStatus gs = go.GetComponent<GameStatus>();

        //sets the powerup in the game status to true so the path finding will happen
        //and disables the powerup sprite
        if (player.gameObject.CompareTag("Player"))
        {
            gs.powerUp = true;
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph_creation : MonoBehaviour
{
    public GameStatus gs;
    
    //attched to positions in with Exit_tag in level scene
    //used to create graph to find path form power up
    private void OnTriggerStay2D(Collider2D collision)
    {
        //checks if the positions with exit_tag are colliding with a tile
        //if they are destroy them
        if (collision.gameObject.tag == "Tile" && this.gameObject.tag == "Exit_tag" && gs.sceneLoaded == true)
        {
            //print("delete " + gameObject.tag + " name = " + gameObject.name);
            Destroy(gameObject);
        }
    }
}

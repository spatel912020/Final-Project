using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTiles : MonoBehaviour
{

    //used to destroy tiles that might collide with the start and exit locations
    //or power up spawn location
    //set to prefab platformposition
    //platform position is just a box collider that is created to delete the tiles
    //then deystoyed so start and exit platforms can be instantiated
    private void OnTriggerStay2D(Collider2D collision)
    {
        //if the collision is with a tile
        if (collision.tag == "Tile")
        {
            //destroy the tile
            Destroy(collision.gameObject);
        }
        //then destroy the platform position
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomPosition : MonoBehaviour
{
    /* Attached to the prefab PowerUp
     * Takes a random position in PowerUp
     * and creates an instance of the power up object
     */



    public GameObject[] positions;              //positions in PowerUp
    public GameObject go;                       //used to create power up
    public GameObject positionCollider;         //prefab of collider to destroy tile
    public GameObject pu;                       //refernce of power up used for testing
    GameStatus gs;                              //get the GameStatus to check if the level is full
    public bool powerUpCreated = false;         //if the power up object has been created
  
    void Start()
    {
        //get the GameStatus
        GameObject gameO = GameObject.Find("GameStatus");
        gs = gameO.GetComponent<GameStatus>();
    }

    //used to spawnPower up in a random position
    void Update()
    {
        //if the level is complete and the power up has not been created
        if (gs.full && !powerUpCreated) {
            //get the SpawnPowerUp instance, sets the powerups parent to be the center of the room
            //this is done so the bfs can go off of the center and not a random point
            GameObject spawnPowerUpInstance = GameObject.Find("SpawnPowerUp");
            SpawnPowerUp spu = spawnPowerUpInstance.GetComponent<SpawnPowerUp>();

            //get a random location for the powerup
            int randomPosition = Random.Range(0, positions.Length);
            Vector2 spawnPos = new Vector2(positions[randomPosition].transform.position.x, positions[randomPosition].transform.position.y);

            //create the powerUp collider to get rid of any tiles in the way
            //this is destroyed after a collistion
            GameObject puCol = Instantiate(positionCollider,
                                              positions[randomPosition].transform.position,
                                              Quaternion.identity);
            //create the power up
            pu = Instantiate(go,
                         positions[randomPosition].transform.position,
                         Quaternion.identity);
            //set its parent to the center 
            pu.transform.parent = spu.spawnParentTransform;

            powerUpCreated = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerUp : MonoBehaviour
{
    public GameObject[] roomCenters;            //room positions on the map

    public LevelGeneration lg;                  //instance of the levelGeneration
    public GameStatus gs;                       //instance of the GameStatus
    public GameObject powerUpPrefab;            //the powerUp prefab to be created
    public Transform spawnParentTransform;      //used to set the transform of the powerup


    void Update()
    {
        //if the powerUp has not been spawned and the level is full
        if (gs.powerUpSpawned == false && gs.full)
        {
            //get a random position on the map
            int randomPowerUpPosition = Random.Range(0, roomCenters.Length);
            GameObject r = roomCenters[randomPowerUpPosition];

            //while the position is not the starting location 
            while(r.transform.position.x == lg.startPosX && r.transform.position.y == lg.startPosY)
            {
                //get a good spawn location
                randomPowerUpPosition = Random.Range(0, roomCenters.Length);
                r = roomCenters[randomPowerUpPosition];
            }

            //make the prefabs transform the same as the parent
            spawnParentTransform = r.transform;
            //spawn the powerup 
            Instantiate(powerUpPrefab, r.transform.position, Quaternion.identity);

            gs.powerUpSpawned = true;
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Used to spawn the pixel of the prefabed room */
public class SpawnObject : MonoBehaviour
{
    //array of sprites to be picked at random, set in unity
    public GameObject[] objects;

    void Start()
    {
        //pick a random sprite                 !!Curently only one!!
        int rand = Random.Range(0, objects.Length);
        //create the sprite in the empty game objects location
        //set in prefab
        GameObject instance = Instantiate(objects[rand], transform.position, Quaternion.identity);

        //make the pixel a child to its prefab room parent
        instance.transform.parent = transform;
    }

}

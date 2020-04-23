using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour
{
    public string Scene;           //set in hierarchy
                                   //name of the level to be loaded      
    //attached to door prefab

    //attached to exit looks if exit and player box colliders touch
    void OnTriggerEnter2D(Collider2D player)
    {
        //get the game object
        GameObject go = GameObject.Find("GameStatus");
        GameStatus gs = go.GetComponent<GameStatus>();

        //if the exit touches the player
        if (player.gameObject.CompareTag("Player"))
        {
            //reset the scene
            gs.sceneLoaded = false;
            SceneManager.LoadScene(Scene);
     
        }
    }
}

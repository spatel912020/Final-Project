using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //atached to camera in hierarchy
    //refernced in game status in hierarchy
    //when the player has been created
    //allows the camera to follow the set camPos


    private Func<Vector3> camPosFunc;           //function that returns player position
                                                //or any attached gameObject position
                                                //set in GameStatus to the player.transform.position

    public GameStatus gs;                       //used to check if the player has been created
                                                //set GameStatus instance in hierarchy

    //called in GameStatus to get the player.transform.position
    public void Setup(Func<Vector3> camPosFunc)
    {
        this.camPosFunc = camPosFunc;
    }

    // Update is called once per frame
    void Update()
    {
        //if the player has been created
        //statment here to prevent moving camera before camPos is set
        if(gs.playerMoved == true)
        {
            //set the camera.postion
            //because attached to the camera transform.postion is the camera.position

            //updates the camera position to the player position with regular z value
            Vector3 camPos = camPosFunc();
            camPos.z = transform.position.z;
            transform.position = camPos;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//used in High Score scene 
public class HighScoreUpdate : MonoBehaviour
{
    //set to the canvas -> HighScoreText in hiegharcy
    void Start()
    {
        //gets the gamestatus and display the highscore
        GameObject go = GameObject.Find("GameStatus");
        GameStatus gs = go.GetComponent<GameStatus>();

        //display the highscore from the game status
        //updates the highscore displayed to the current highscore
        GetComponent<Text>().text = "" + gs.highscore;
    }
}

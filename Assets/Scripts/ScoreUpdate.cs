using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour
{
    //this script is atached to playerinfo -> score panel -> text in the hierarchy
    private void Update()
    {
        //gets the game status for the highscore
        GameObject go = GameObject.Find("GameStatus");
        GameStatus gs = go.GetComponent<GameStatus>();

        //game status for time and score is not required becasue the are static public variables
        //if the GameStatus is in the scene then they can be retrieved
        //however highscore requires the above code to display because it is not static
        GetComponent<Text>().text = "Time: " + GameStatus.time + "\n" + "Depth: " + GameStatus.score + "\n" + "High Score: " + gs.highscore; // + "     Health: " + GameStatus.health + "     Lives: " + GameStatus.lives;
    }
}

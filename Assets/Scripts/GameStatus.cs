using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

/* This is a main driving part of the game
 * It stores values and is set as a prefab to pass data from one scene to the next
 * It is used to check if a certain function has been preformed
 * It is also used to spawn the player and exit instances
 *      the player prefab holds the player gameObject
 *      and is only created after the level has been created
 *      the exit prefab holds the exit door which itself
 *      has the code to change scenes
 */


public class GameStatus: MonoBehaviour
{
    static public float time = 30;                  //current time in level, each level has 30 sec
    static public float score = 0;                  //current scroe in the level
    public int highscore = 0;                       //total highscore of the game
  

    public bool levelLoaded = false;                //if the level scene has been loaded

    public GameObject platformCol;                  //used to destroy tiles in the way of the start and exit
                       
    public LevelGeneration roomGen;                 //instance of the level generation
    public string StartScene;                       //used to call startScene, set in hiearchy

    //public float radius;            
    public GameObject startingPlatform;             //starting platform prefab
    public bool playerMoved = false;                //if the player has been created
    //private Transform PlayerTransform;           
    public float playerY;                           //set in the hiearchy creates player just above starting platform
    public GameObject player;                       //set in hiearchy player instance
    public GameObject exit;                         //prefab of the exit 

    public bool sceneLoaded = false;                //if the scene has been loaded
    public bool graphCreated = false;               //if the graph for bfs has been created
    public bool powerUp = false;                    //if the powerUp has been activated
    public bool powerUpSpawned = false;             //if the powerUp has spawned
    public bool full;                               //if the level is full of rooms

    public CameraFollow cf;                         //used to make camera follow the player

    //set to buttons in the game to call the scene at the sceneIndex
    //scene index set in build settings
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
        
    }

    //loads the level asynchronously, this is a little overkill as the level is not that large
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            yield return new WaitForSeconds(10f);
        }
    }

    //quits the application if button pressed
    public void quit()
    {
        Application.Quit();
    }

    //counts the time down, if the time goes below 0 the game ends
    public void Countdown()
    {
        time -= Time.deltaTime;
        if(time < 0)
        {
            GameOver();
        }
    }

    //gameOver instance
    public void GameOver()
    {
        //set the sceneLoaded to false, check the highscore, set the score, and go to the StartScene
        sceneLoaded = false;
        if((int)score - 1 > highscore) {
            highscore = (int)score - 1;
        }
        score = 0;
        SceneManager.LoadScene(StartScene);
    }

    //used to keep instance of highscore after one run is over
    void OnDestroy()
    {
        PlayerPrefs.SetInt("highscore", highscore);
    }

    //used to create player and exit
    public void CreatePlayerExit()
    {
     
        if (full == true && playerMoved == false)
        {
            //get the start, end, and player positions form the level generation
            Vector2 endPos = new Vector2(roomGen.endPosX, roomGen.endPosY);
            Vector2 startPos = new Vector2(roomGen.startPosX, roomGen.startPosY);
            Vector2 playerPos = new Vector2(roomGen.startPosX, roomGen.startPosY + playerY);

            //create colliders to get rid of tiles that might be in the way
            GameObject startcol =Instantiate(platformCol, startPos, Quaternion.identity);
            GameObject endcol =Instantiate(platformCol, endPos, Quaternion.identity);

            //create a starting prebab and player instance
            GameObject instance = (GameObject)Instantiate(startingPlatform, startPos, Quaternion.identity);
            GameObject playerInstance = (GameObject)Instantiate(player, playerPos, Quaternion.identity);

            //used to set camera to follow playerInstance
            cf.Setup( () => playerInstance.transform.position);

            //create a exit prefab at endPos
            GameObject exitInstance = (GameObject)Instantiate(exit, endPos, Quaternion.identity);

            //set the time, score and playerMoved
            playerMoved = true;
            time = 30;
            score = score + 1;
        }
            

    }

    //called on first frame
    void Start()
    {
        //set the highscore from the PlayerPrefs
        highscore = PlayerPrefs.GetInt("highscore", 0);
    }

    //called on every frame
    void Update()
    {
        //if teh scene has loaded, it is full, and the player has been created, start the countdown
        if (sceneLoaded == true && full && playerMoved)
        {
            Countdown();
        }
        //if the player has not been created but the room is full and complete
        if (full == true && playerMoved == false)
        {
            CreatePlayerExit();
        } 
    }
}

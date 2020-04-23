using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Attached to an empty gameObject called LevelGenertion in the level scene
 * This is used to create the rooms in the level scene
 * Each level calls the prefabRooms and creates a path to an exit
 * Move is called until the exit is determined
 * Once move is finished fill level is called
 * all the other positions in the level are set to a random room
 */

public class LevelGeneration : MonoBehaviour
{
    public bool stopMove;                          //set to false when no more rooms can be moved  
    

    public LayerMask roomType;

    private int countDown;                          //used to make sure when the room moves down the room above has a bottom exit
    public float moveDistance;                      //how much room goes in certain direction, currently 10 (width of room)
    private int direction;                          //number to be randomly generated to find direction the room should be generated
    public Transform[] startingPositions;           //set in unity, top row where the first room can be created 
    //public GameObject[] startingPlatform;
    public GameObject[] prefabRooms;                //array of rooms with different oppenings
    /* Rooms are in unity as a prefab
     * they are set into the array in unity
     * Element 0 = left/right exits
     * Element 1 = left/right/bottom exits
     * Element 2 = left/right/top exits
     * Eleemnt 3 = Left/right/top/bottom exits
     */

    public GameObject prevRoom;                     //set to the previous room created
    public GameObject[] roomPos;                    //array of posible room locations
                                                    //set in hiearchy 

    //used to give room time to generate so next generation location can be determined
    private float timeBetweenRoom;
    public float startTimeBetweenRoom = 0.25f;

    //all set in unity, all are how far a room can in any dirrection, rooms dont generate up
    public float minX;
    public float maxX;
    public float minY;

    //these are made when the first room is made in move
    //and the last room is made in move
    //used to determine the start and end platfrom spawn locations
    public float startPosX;
    public float startPosY;
    public float endPosX;
    public float endPosY;

    //public GameObject spawnPoint;
    public GameStatus gs;

    public LayerMask findRoom;                           //used to find prevoius room when moving down

    /* Called on first frame, before update is called
     */
    private void Start()
    {
        //makes sure the level is set to not full
        gs.full = false;         

        //create the first room and set its position to the starting position for the platform and player
        int randomStartingPosition = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randomStartingPosition].position;
        startPosX = transform.position.x;
        startPosY = transform.position.y;

        //used in the power_up to find start
        this.name = "Start";

        //creates a room with left/right exits at the one of the starting locations
        GameObject instance = (GameObject)Instantiate(prefabRooms[0], transform.position, Quaternion.identity);
        prevRoom = instance;

        //gets a new direction to go in
        direction = Random.Range(0, 5);

        while (!stopMove)
        {
            //direction = Random.Range(0, 5);
            if (timeBetweenRoom <= 0)
            {
                //if possible move and create the new room
                Move();
                timeBetweenRoom = startTimeBetweenRoom;
            }
            else
            {
                timeBetweenRoom -= Time.deltaTime;              //time between current and previous frame
                                                                //decreases timeBetweenRoom till it is less than 0
            }
        }

        //if the level has stopped moving and it is not full fill the level with rooms
        if (stopMove == true && gs.full == false)
        {
            FillLevel();
        }


    }

    //used to fill the level with rooms in positions where are room in not currently present
    private void FillLevel()
    {
        //look at each position in roomPos
        foreach(GameObject r in roomPos)
        {
            //detect if a gameObject with roomType layer is present
            Collider2D roomDetection = Physics2D.OverlapCircle(r.transform.position, 1, roomType);

            //if not create a random room
            if(roomDetection == null)
            {
                int rand = Random.Range(0, prefabRooms.Length);
                Instantiate(prefabRooms[rand], r.transform.position, Quaternion.identity);
            }
        }
        //set the room to full and the scene is now loaded
        gs.full = true;
        gs.sceneLoaded = true;
    }

    //moves the rooms randomly down the level with exits that create a path
    //prevRoom is changed every time a room is created
    //this is to check if the previous rooms type so a path can be created
    private void Move()
    {
        //move in the right direction
        if(direction == 0 || direction == 1) {
            //if the position is not all the way right
            if(transform.position.x < maxX) {
                countDown = 0;                  //resets time moved down 

                //move to right and set the transform position
                Vector2 newPos = new Vector2(transform.position.x + moveDistance, transform.position.y);
                transform.position = newPos;

                //pick a random room and create it
                int rand = Random.Range(0, prefabRooms.Length);
                prevRoom = Instantiate(prefabRooms[rand], transform.position, Quaternion.identity);

                //pick a random direction
                direction = Random.Range(0, 5);

                /* these two statements are to keep the room from going right then left
                 * if either left numbers are picked they are changed
                 */
                if (direction == 2) {               //if left position 2 move right
                    direction = 1;
                }
                else if(direction == 3) {           //if left positon 3 move down
                    direction = 4;
                }
            }
            //if the room is all the way right it must go down
            else {
                direction = 4;
            }
          //move in the left direction 
        } else if(direction == 2 || direction == 3) {
            //if its not all the way left
            if(transform.position.x > minX) {
                countDown = 0;                          //resets time moved down 

                //move to the left and set new transform position
                Vector2 newPos = new Vector2(transform.position.x - moveDistance, transform.position.y);
                transform.position = newPos;

                //pick a random room and create it
                int rand = Random.Range(0, prefabRooms.Length);
                prevRoom = Instantiate(prefabRooms[rand], transform.position, Quaternion.identity);

                //randomize a new direction thats not right
                direction = Random.Range(2, 5);
            }
            //if it cant move farther left go down
            else {
                direction = 4;
            }
          //move down
        } else if(direction == 4) {
            countDown++;            //increment times moved down

            //if the current position is not at the bottom
            if(transform.position.y > minY){
                
                //if the current rooms type is does not have a bottom exit
                //--------------------------room types are set in the RoomType script---------------------------
                //left/right == 0   | left/right/bottom == 1  | left/right/top == 2 | left/right/top/bottom == 3
                if(prevRoom.GetComponent<RoomType>().type != 1 || prevRoom.GetComponent<RoomType>().type != 3){
                    //if the 2nd to last room placed was a in the down direction
                    if(countDown >= 2){
                        //change the current room to a left/right/top/bottom room then
                        prevRoom.GetComponent<RoomType>().RoomDestruction();
                        prevRoom = Instantiate(prefabRooms[3], transform.position, Quaternion.identity);
                    }
                    else{
                        //change the current room to a room with a bottom 
                        prevRoom.GetComponent<RoomType>().RoomDestruction();
                        int randBottomRoom = Random.Range(1, 4);
                        if (randBottomRoom == 2){
                            randBottomRoom = 1;
                        }
                        //create in the current position a room with a bottom exit
                        prevRoom = Instantiate(prefabRooms[randBottomRoom], transform.position, Quaternion.identity);
                    }               
                }
                //get a new position below the current room position, and change that to the new position
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveDistance);
                transform.position = newPos;

                //pick a random room that has a top and create it
                int rand = Random.Range(2, 4);
                prevRoom = Instantiate(prefabRooms[rand], transform.position, Quaternion.identity);

                //pick a new direction
                direction = Random.Range(0, 5);
            }
            //if the path can not move anyfarther down, stop the Move function
            else{

                endPosX = transform.position.x;
                endPosY = transform.position.y;
                this.name = "Exit";
                this.tag = "Room_Center";
                stopMove = true;
            }
        }

        


    }
}
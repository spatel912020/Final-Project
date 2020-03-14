using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    private bool stopMove;                          //set to false when no more rooms can be moved                   
    private int countDown;                          //used to make sure when the room moves down the room above has a bottom exit
    public float moveDistance;                      //how much room goes in certain direction, currently 10 (width of room)
    private int direction;                          //number to be randomly generated to find direction the room should be generated
    public Transform[] startingPositions;           //set in unity, top row where the first room can be created 
    public GameObject[] prefabRooms;                //array of rooms with different oppenings
    /* Rooms are in unity as a prefab
     * they are set into the array in unity
     * Element 0 = left/right exits
     * Element 1 = left/right/bottom exits
     * Element 2 = left/right/top exits
     * Eleemnt 3 = Left/right/top/bottom exits
     */

    //used to give room time to generate so next generation location can be determined
    private float timeBetweenRoom;
    public float startTimeBetweenRoom = 0.25f;

    //all set in unity, all are how far a room can in any dirrection, rooms dont generate up
    public float minX;
    public float maxX;
    public float minY;

    public LayerMask findRoom;                           //used to find prevoius room when moving down

    /* Called on first frame, before update is called
     */
    private void Start()
    {
        int randomStartingPosition = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randomStartingPosition].position;
        //Debug.Log("Starting Position");
        //Debug.Log(transform.position.x);

        //creates a room with left/right exits at the one of the starting locations
        GameObject instance = (GameObject)Instantiate(prefabRooms[0], transform.position, Quaternion.identity);

        //gets a new direction to go in
        direction = Random.Range(0, 5);     
    }

    /* Called every frame
     * Move is only called after the previous room has been generated
     */
    private void Update()
    {
        //called when new room can be generated
        if(timeBetweenRoom <= 0 && !stopMove) {
            Move();
            timeBetweenRoom = startTimeBetweenRoom;
        }
        else {
            timeBetweenRoom -= Time.deltaTime;              //time between current and previous frame
                                                            //decreases timeBetweenRoom till it is less than 0 
        }
    }

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
                Instantiate(prefabRooms[rand], transform.position, Quaternion.identity);

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
                Instantiate(prefabRooms[rand], transform.position, Quaternion.identity);

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
                //holds the current room before the room moves down
                Collider2D currentRoom = Physics2D.OverlapCircle(transform.position, 1, findRoom);
                //if the current rooms type is does not have a bottom exit
                //--------------------------room types are set in the RoomType script---------------------------
                //left/right == 0   | left/right/bottom == 1  | left/right/top == 2 | left/right/top/bottom == 3
                if(currentRoom.GetComponent<RoomType>().type != 1 && currentRoom.GetComponent<RoomType>().type != 3){
                    //if the 2nd to last room placed was a in the down direction
                    if(countDown >= 2){
                        //change the current room to a left/right/top/bottom room then
                        currentRoom.GetComponent<RoomType>().RoomDestruction();
                        Instantiate(prefabRooms[3], transform.position, Quaternion.identity);
                    }
                    else{
                        //change the current room to a room with a bottom 
                        currentRoom.GetComponent<RoomType>().RoomDestruction();
                        int randBottomRoom = Random.Range(1, 4);
                        if (randBottomRoom == 2){
                            randBottomRoom = 1;
                        }
                        //create in the current position a room with a bottom exit
                        Instantiate(prefabRooms[randBottomRoom], transform.position, Quaternion.identity);
                    }               
                }
                //get a new position below the current room position, and change that to the new position
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveDistance);
                transform.position = newPos;

                //pick a random room that has a top and create it
                int rand = Random.Range(2, 4);
                Instantiate(prefabRooms[rand], transform.position, Quaternion.identity);

                //pick a new direction
                direction = Random.Range(0, 5);
            }
            //if the path can not move anyfarther down, stop the Move function
            else{
                stopMove = true;
            }
        }
    }
}
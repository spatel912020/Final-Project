using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public int type;            //type of room set in unity
    //left/right == 0   | left/right/bottom == 1  | left/right/top == 2 | left/right/top/bottom == 3

    //destroy a game obeject given to the function, used in room generation script
    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
}

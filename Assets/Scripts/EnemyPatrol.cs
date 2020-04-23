using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script not currently used
//it is attached to the skeleton prefab


public class EnemyPatrol : MonoBehaviour
{
    public float speed;
    public float sightDistance;

    //private bool moveRight = true;
    bool right = true;

    public Transform detectTileR;
    public Transform detectTileL;

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        int layer_mask = LayerMask.GetMask("Tile");
        

        RaycastHit2D tileRight = Physics2D.Raycast(detectTileR.position, Vector2.right, sightDistance, layer_mask);
        if (tileRight.collider == true)
        {
            
            if (right == true)
            {
                Debug.Log("flip right");
                transform.eulerAngles = new Vector3(0, -180, 0);
                right = false;
            }
            else
            {
                Debug.Log("flip left");
                transform.eulerAngles = new Vector3(0, 0, 0);
                right = true;
            }
               
        }      
    }
}

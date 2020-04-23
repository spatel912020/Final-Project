using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class power_up : MonoBehaviour
{
    public Sprite right;
    public Sprite left;
    public Sprite up;
    public Sprite down;

    GameObject[] temp_position;
    List<GameObject> position_objects = new List<GameObject>();
    public class Graph_nodes
    {
        public float x;
        public float y;
        public GameObject gameObject;
        public List<Graph_nodes> adj_list = new List<Graph_nodes>();
        public Graph_nodes previous;
    }
    public class Graph
    {
        public
            //graph_nodes instead of GameObject
            List<Graph_nodes> nodes = new List<Graph_nodes>();
    }
    public Graph my_graph = new Graph();

    public bool BFS()
    {
        List<Graph_nodes> queue = new List<Graph_nodes>();
        List<Graph_nodes> visited = new List<Graph_nodes>();
        //GameObject start = GameObject.Find("SpawnPoint(Clone)");
        GameObject start = GameObject.Find("PowerUp(Clone)");
        float start_x = start.transform.position.x;
        float start_y = start.transform.position.y;

        queue.Add(my_graph.nodes.Find(r => r.x == start_x && r.y == start_y));
        my_graph.nodes.Find(r => r.x == start_x && r.y == start_y).previous = queue[0];
        GameObject exit = GameObject.Find("Exit");
        float exit_x = exit.transform.position.x;
        float exit_y = exit.transform.position.y;
        
        while (queue.Count != 0)
        {
            Graph_nodes curr = new Graph_nodes();
            curr = queue[0];
            //print("curr = " + curr.gameObject.name);
            queue.RemoveAt(0);

            if (exit_x == curr.x && exit_y == curr.y)
            {
                my_graph.nodes.Find(r => r.gameObject.name == "Exit").previous = curr.previous;
                //print("eXIT = " + curr.gameObject.name);
                return true;
            }
            for (int i= 0; i < curr.adj_list.Count; i++)
            {
                //print("curr = " + curr.gameObject.name +"neighbor = " + curr.adj_list[i].gameObject.name);
                if (visited.Contains(my_graph.nodes.Find(r => r.x == curr.adj_list[i].x && r.y == curr.adj_list[i].y)) == false)
                {
                    my_graph.nodes.Find(r => r.x == curr.adj_list[i].x && r.y == curr.adj_list[i].y).previous = curr;
                    visited.Add(my_graph.nodes.Find(r => r.x == curr.adj_list[i].x && r.y == curr.adj_list[i].y));
                    queue.Add(my_graph.nodes.Find(r => r.x == curr.adj_list[i].x && r.y == curr.adj_list[i].y));
                }
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject go = GameObject.Find("GameStatus");
        GameStatus gs = go.GetComponent<GameStatus>();

        if (gs.sceneLoaded && !gs.graphCreated && gs.powerUp)
        {
            position_objects.Clear();
            temp_position = GameObject.FindGameObjectsWithTag("Room_Center");
            foreach (var gameobject in temp_position)
            {
                position_objects.Add(gameobject);
            }

            temp_position = GameObject.FindGameObjectsWithTag("Exit_tag");
            foreach (var gameobject in temp_position)
            {
                position_objects.Add(gameobject);
            }

            temp_position = GameObject.FindGameObjectsWithTag("POWERUP");
            foreach (var gameobject in temp_position)
            {
                position_objects.Add(gameobject);
            }


            //add nodes to graph list
            foreach (var position in position_objects)
            {
                Graph_nodes cur_node = new Graph_nodes();
                cur_node.x = position.transform.position.x;
                cur_node.y = position.transform.position.y;
                cur_node.gameObject = position;

                foreach (var neighbor in position_objects)
                {
                    float x = neighbor.transform.position.x;
                    float y = neighbor.transform.position.y;
                    Graph_nodes neighbor_node = new Graph_nodes();
                    //Check left node
                    if (x == cur_node.x - 5 && y == cur_node.y)
                    {
                        neighbor_node.x = x;
                        neighbor_node.y = y;
                        neighbor_node.gameObject = neighbor;
                        //print(position.name + " is neighbor with " + neighbor.name);
                        if (cur_node.adj_list.Contains(neighbor_node) == false)
                        {
                            cur_node.adj_list.Add(neighbor_node);
                        }
                        neighbor_node.adj_list.Add(cur_node);
                    }
                    //Check right node
                    else if (x == cur_node.x + 5 && y == cur_node.y)
                    {
                        neighbor_node.x = x;
                        neighbor_node.y = y;
                        neighbor_node.gameObject = neighbor;
                        //print(position.name + " is neighbor with " + neighbor.name);
                        if (cur_node.adj_list.Contains(neighbor_node) == false)
                        {
                            cur_node.adj_list.Add(neighbor_node);
                        }
                        neighbor_node.adj_list.Add(cur_node);
                    }
                    //Check bottom node
                    else if (x == cur_node.x && y == cur_node.y - 5)
                    {
                        neighbor_node.x = x;
                        neighbor_node.y = y;
                        neighbor_node.gameObject = neighbor;
                        //print(position.name + " is neighbor with " + neighbor.name);
                        if (cur_node.adj_list.Contains(neighbor_node) == false)
                        {
                            cur_node.adj_list.Add(neighbor_node);
                        }
                        neighbor_node.adj_list.Add(cur_node);
                    }
                    //Check above node
                    else if (x == cur_node.x && y == cur_node.y + 5)
                    {
                        neighbor_node.x = x;
                        neighbor_node.y = y;
                        neighbor_node.gameObject = neighbor;
                        //print(position.name + " is neighbor with " + neighbor.name);
                        if (cur_node.adj_list.Contains(neighbor_node) == false)
                        {
                            cur_node.adj_list.Add(neighbor_node);
                        }
                        neighbor_node.adj_list.Add(cur_node);
                    }
                }
                my_graph.nodes.Add(cur_node);
            }
            BFS();
            //print("isPath = " + BFS());
            gs.graphCreated = true;
        }
        //Render spirit if power up is ture
        else if(gs.sceneLoaded && gs.graphCreated && gs.powerUp)
        {
            GameObject start = GameObject.Find("PowerUp(Clone)");
            float start_x = start.transform.position.x;
            float start_y = start.transform.position.y;

            Graph_nodes start_node = my_graph.nodes.Find(r => r.x == start_x && r.y == start_y);

            Graph_nodes temp = my_graph.nodes.Find(r => r.gameObject.name == "Exit");

            while(temp != start_node)
            {
               //print("Name = " + temp.gameObject.name +" Prev = " + temp.previous.gameObject.name);
               if(temp.x == temp.previous.x){
                    if(temp.y == temp.previous.y + 5)
                    {
                        temp.previous.gameObject.GetComponent<SpriteRenderer>().sprite = up;
                    }
                    else if(temp.y == temp.previous.y - 5)
                    {  
                        temp.previous.gameObject.GetComponent<SpriteRenderer>().sprite = down;
                    }
               }
               else if (temp.y == temp.previous.y)
                {
                    if (temp.x == temp.previous.x + 5)
                    {
                        temp.previous.gameObject.GetComponent<SpriteRenderer>().sprite = right;
                    }
                    else if (temp.x == temp.previous.x - 5)
                    {                       
                        temp.previous.gameObject.GetComponent<SpriteRenderer>().sprite = left;
                    }
                }
               temp = temp.previous;
            }
            //gs.powerUp = false;
        }
    }
}

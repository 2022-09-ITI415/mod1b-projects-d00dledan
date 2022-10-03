using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Determine which direction snake is going to go
    public PlayerDirection direction;

    // Movement interval 
    public float step_Length = 0.2f;

    // Move it .1 in a second rather than 60 times in a second, how many times we can move in a second
    public float movement_Frequency = 0.1f;
    private float counter;
    private bool move;

    //Add to snake when we eat fruit
    [SerializeField]
    private GameObject tailPrefab;

    // Store previous movement of player
    private List<Vector3> delta_Position;

    // Snake heads, tails, nodes
    private List<Rigidbody> nodes;

    private Rigidbody main_Body;
    private Rigidbody head_Body;
    private Transform tr;

   
    private bool create_Node_At_Tail;


    // Start is called before the first frame update
    void Awake()
    {
        tr = transform;
        //The Snake
        main_Body = GetComponent<Rigidbody>();

        //Get rigid bodies of every snake child
        InitSnakeNodes();
        InitPlayer();

        delta_Position = new List<Vector3>() {

            new Vector3(-step_Length,0f),       
            new Vector3(0f, step_Length),       
            new Vector3(step_Length,0f),        
            new Vector3(0f, -step_Length)       
        };

    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementFrequency();
    }

    void FixedUpdate()
    {
        if (move)
        {
            move = false;
            Move();
        }
    }

    void InitSnakeNodes()
    {

        // Get rigidbody of every part of the snake 
        nodes = new List<Rigidbody>();

        //Get the head of the snake
        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        //Get node of the snake
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        //Get the node/tail of the snake
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

        //Rigidbody of the head of snake
        head_Body = nodes[0];
    }

    void SetDirectionRandom()
    {
       
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom;
    }

    void InitPlayer()
    {
        // Set initial direction of player 
        SetDirectionRandom();

        switch (direction)
        {

            case PlayerDirection.RIGHT:

                //Shift comoponents to follow the head depending on what position you start at 
                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE * 2f, 0f, 0f);

                break;

            case PlayerDirection.LEFT:

                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);

                break;

            case PlayerDirection.UP:

                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.NODE * 2f, 0f);

                break;

            case PlayerDirection.DOWN:

                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.NODE * 2f, 0f);

                break;

        }
    }


    void Move()
    {
        
        Vector3 dPosition = delta_Position[(int)direction];

        
        Vector3 parentPos = head_Body.position;
        Vector3 prevPosition;

        //Start moving head of snake by setting position to it current position + direction we are going 
        main_Body.position = main_Body.position + dPosition;
        head_Body.position = head_Body.position + dPosition;

        
        for (int i = 1; i < nodes.Count; i++)
        {

            prevPosition = nodes[i].position;
            
            nodes[i].position = parentPos;
            
            parentPos = prevPosition;
        }

        
        if (create_Node_At_Tail)
        {

            
            create_Node_At_Tail = false;

           
            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);


             
            newNode.transform.SetParent(transform, true);
            
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }




    void CheckMovementFrequency()
    {

        counter += Time.deltaTime;

        if (counter >= movement_Frequency)
        {

            // move snake whenever counter is grater than 0.2
            counter = 0f;
            move = true;
        }

    }

    public void SetInputDirection(PlayerDirection dir)
    {
        // prevent movement in opposite direction of where you are moving currently
        if (dir == PlayerDirection.UP && direction == PlayerDirection.DOWN || dir == PlayerDirection.DOWN && direction == PlayerDirection.UP ||
            dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT || dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT)
        {
            return;
        }

        direction = dir;
        //move right away rather than wait for threshold of movement frequency
        ForceMove();
    }

    void ForceMove()
    {
        counter = 0;
        move = false;
        Move();
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.FRUIT)
        {
            target.gameObject.SetActive(false);
            create_Node_At_Tail = true;

           //GameplayController.instance.IncreaseScore();
          // AudioManager.instance.Play_PickUpSound();
        }

        if (target.tag == Tags.WALL || target.tag == Tags.BOMB || target.tag == Tags.TAIL)
        {
            Time.timeScale = 0f;
            //AudioManager.instance.Play_DeadSound();
        }
    }
}

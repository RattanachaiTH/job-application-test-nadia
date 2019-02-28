using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Public valiable
    public static int size = 40;
    public static float speedRate = 0.05f;
    public static int highScore = 0;
    public bool hard;
    public GameObject background;
    public GameObject cam;
    public GameObject uiStart;
    public GameObject uiGameOver;
    public GameObject uiScoreText;
    public GameObject uiHighScoreText;
    public GameObject uiOption;
    public Color colorSnake;
    public Color colorFood;
    public Sprite spriteAsset;

    // Private valiable
    private int score;
    private int maxSize;
    private int minSize;
    private int length;
    private int option;
    private float sizeLength;
    private float sizeHaft;
    private GameObject objectSnake;
    private GameObject objectFood;
    private GameObject objectTailStack;
    private List<GameObject> listTailObject;
    /*
    private Node[,] state;
    private Node nodeSnake;
    private Node nodeFood;
    private List<Node> listCreateFood;
    private List<Node> listTail;
    */
    private State state;
    private bool status;
    private Controller controller;

    // move when speedRate pass
    float timer;
    bool move;

    public void Awake()
    {
        // initial value
        print("speed: " + speedRate);
        maxSize = 200;
        minSize = 10;
        length = 100;
        status = false;
        uiStart.SetActive(true);
    }

    public void FixedUpdate()
    {
        if (status == true)
        {
            controller.GetInput();
            Timer();
            if (move == true)
            {
                state.direction = controller.GetDirection();
                print("Head1: " + state.GetHeadPosition());
                print("Food1: " + state.GetFoodPosition());
                State nextState = state.GetNextState();
                print("Head2: "+ nextState.GetHeadPosition());
                print("Food2: "+ nextState.GetFoodPosition());
                // Game Over
                if (nextState.gameover == true)
                {
                    print("game over");
                    status = false;
                    GameOver();
                }
                // Eat food
                if (nextState.eat == true)
                {
                    score++;
                    nextState.AddFood();
                    CreateTailObject(nextState.tailList[nextState.tailList.Count - 1]);
                }

                // Update state
                state = nextState;
                controller.SetState(state);
                UpdateObject();
                move = false;
            }
        }
    }

    public void Timer()
    {
        timer += Time.deltaTime;
        if (timer > speedRate)
        {
            timer = 0;
            move = true;
        }
    }

    void GameSetup()
    {
        // Game setup
        score = 0;
        CreateMap();
        CreateState();
        //CreateSnakeNode();
        //CreateFoodNode();
        CreateSnakeObject();
        CreateFoodObject();
        if (option == 0)    // Human
        {
            controller = new HumanController(state);
            print("Human!!!!!!!!!!!!!!!!!!");
        }
        else if (option == 1)    // Random
        {
            controller = new RandomController(state);
        }
        else if (option == 2)    // AI (Future work)
        {
            controller = new AIController(state);
        }
        move = false;
    }

    void CreateMap()
    {
        if (size > maxSize)
        {
            size = maxSize;
        }
        else if (size < minSize)
        {
            size = minSize;
        }
        sizeLength = size * length;
        sizeHaft = size / 2;

        // background setting
        background.transform.localScale = new Vector3(sizeLength, sizeLength, 0);
        background.transform.position = new Vector3(sizeHaft - 0.5f, sizeHaft - 0.5f, 0);

        // camera setting
        cam.transform.GetComponent<Camera>().orthographicSize = sizeHaft+2;
        cam.transform.position = new Vector3(sizeHaft - 0.5f, sizeHaft - 0.5f, -10);
    }

    void CreateState()
    {
        int start_x = (int)sizeHaft - 5;
        int start_y = (int)sizeHaft;
        Vector2 head = new Vector2(start_x, start_y);
        Vector2 food = new Vector2(-1, -1);
        Vector2 direction = new Vector2(1, 0);
        List<Vector2> tailList = new List<Vector2>();
        state = new State(size, head, food, direction, tailList, true);
        state.AddFood();
    }

    void CreateSnakeObject()
    {
        objectSnake = new GameObject("Snake_Object");
        objectTailStack = new GameObject("Tail_Stack");
        listTailObject = new List<GameObject>();
        objectSnake.transform.position = state.GetHeadPosition();
        objectSnake.transform.parent = transform;
        objectTailStack.transform.parent = transform;
        CreateColor(objectSnake, colorSnake);
        /*
        Snake snake = objectSnake.AddComponent<Snake>();
        snake.gameManager = transform.GetComponent<GameManager>();
        snake.SetPosition(state.GetHeadPosition());
        */
    }

    void CreateFoodObject()
    {
        objectFood = new GameObject("Food_Object");
        objectFood.transform.position = state.GetFoodPosition();
        objectFood.transform.parent = transform;
        CreateColor(objectFood, colorFood);
    }
    
    void CreateTailObject(Vector2 position)
    {
        int tailLength = state.tailList.Count;
        GameObject newTail = new GameObject("Tail_Object" + (tailLength));
        newTail.transform.parent = objectTailStack.transform;
        CreateColor(newTail, colorSnake);
        newTail.transform.localScale = new Vector3(length, length, 1);
        newTail.transform.position = position;
        listTailObject.Add(newTail);
    }

    // Function for updating Snake nodes
    public void UpdateObject()
    {
        if (state.gameover == false)
        {
            // Update head object
            objectSnake.transform.position = state.GetHeadPosition();

            // Update food object
            objectFood.transform.position = state.GetFoodPosition();

            // Update tail objects
            for (int i = 0; i < state.tailList.Count; i++)
            {
                listTailObject[i].transform.position = state.tailList[i];
            }
        }
        else
        {
            // gameover = true;
        }

        
        /*
        //  When eat food
        if (addTail == true)
        {
            // Create new Tail
            CreateTailNode(tempCurrent);

            // Create new Food
            nodeFood = null;
            CreateFoodNode();

            // Add Score
            score++;
            if (hard == true)
            {
                speedRate -= 0.01f;
            }
        }
        else
        {
            // Add space at ended tail for Food
            listCreateFood.Add(tempCurrent);
        }
        

        // Update positions
        // Update SnakeHead and Food positions
        if (nodeFood != null && nodeSnake != null)
        {
            objectSnake.transform.position = nodeSnake.position;
            objectFood.transform.position = nodeFood.position;
        }
        // Update Tail positions
        for (int i = 0; i < listTail.Count; i++)
        {
            listTailObject[i].transform.position = listTail[i].position;
        }
        */
    }

    // Function for creating block
    void CreateColor(GameObject obj, Color color)
    {
        SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteAsset;
        spriteRenderer.color = color;
        obj.transform.localScale = new Vector3(length, length, 1);
    }

    #region UI_SECTION
    public void GameStart()
    {
        option = uiOption.GetComponent<TMP_Dropdown>().value;
        GameSetup();
        status = true;
        uiStart.SetActive(false);
    }

    public void GameOver()
    {
        if (score > highScore)
        {
            highScore = score;
        }
        UpdateScore();
        uiGameOver.SetActive(true);
    }

    public void PlayAgain()
    {
        Destroy(objectSnake);
        Destroy(objectFood);
        Destroy(objectTailStack);
        GameSetup();
        status = true;
        uiGameOver.SetActive(false);
    }

    public void UpdateScore()
    {
        uiScoreText.GetComponent<TextMeshProUGUI>().SetText("Score: " + score);
        uiHighScoreText.GetComponent<TextMeshProUGUI>().SetText("High score: " + highScore);
    }
    #endregion

    // Set and Get functions
    #region GET_SET_FUNCTION
    public void SetStatus(bool status)
    {
        this.status = status;
    }

    #endregion
}

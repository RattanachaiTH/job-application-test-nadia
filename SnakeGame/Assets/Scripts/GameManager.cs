using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Public valiable
    public static int size = 80;
    public static float speedRate = 1f;
    public static int highScore = 0;
    public bool hard;
    public GameObject background;
    public GameObject cam;
    public GameObject uiStart;
    public GameObject uiGameOver;
    public GameObject scoreText;
    public GameObject highScoreText;
    public GameObject uiScoreText;
    public GameObject uiHighScoreText;
    public GameObject uiOptionText;
    public GameObject uiOption;
    public GameObject uiSizeScrollbar;
    public GameObject uiSpeedScrollbar;
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
    private float timer;
    private bool move;
    private bool status;
    private State state;
    private GameObject objectSnake;
    private GameObject objectFood;
    private GameObject objectTailStack;
    private List<GameObject> listTailObject;
    private Controller controller;

    public void Awake()
    {
        // initial value
        maxSize = 200;
        minSize = 10;
        length = 100;
        status = false;
        uiStart.SetActive(true);
    }

    public void FixedUpdate()
    {
        // check status game
        if (status == true)
        {
            // Waiting for next move
            controller.GetInput();
            Timer();
            if (move == true)
            {
                state.direction = controller.GetDirection();
                State nextState = state.GetNextState();
                // Game Over
                if (nextState.gameover == true)
                {
                    status = false;
                    GameOver();
                }
                // When get food
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
                UpdateScore();
                move = false;
            }
        }
    }

    void GameSetup()
    {
        // Game setup
        score = 0;
        CreateMap();
        CreateState();
        CreateObject();
        controller = GetController();
        UpdateScore();
        scoreText.SetActive(true);
        highScoreText.SetActive(true);
        move = false;
    }

    // Get controller
    Controller GetController()
    {
        Controller controller;
        // Controller setting
        if (option == 0)                                // Human
        {
            controller = new HumanController(state);
        }
        else if (option == 1)                           // Random
        {
            controller = new RandomController(state);
        }
        else if (option == 2)                           // AI (Future work)
        {
            controller = new AIController(state);
        }
        else
        {
            controller = new HumanController(state);
        }

        return controller;
    }

    // Set map follow size of arena
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

    // Create intial state for new game
    void CreateState()
    {
        int start_x = (int)sizeHaft - 5;
        int start_y = (int)sizeHaft;
        Vector2 head = new Vector2(start_x, start_y);
        Vector2 food = new Vector2(-1, -1);
        Vector2 direction = new Vector2(1, 0);
        List<Vector2> tailList = new List<Vector2>();
        state = new State(size, head, food, direction, tailList, true, 0);
        state.AddFood();
    }
    
    // Create intial object in scene
    void CreateObject()
    {
        // Create snake object
        objectSnake = new GameObject("Snake_Object");
        objectTailStack = new GameObject("Tail_Stack");
        listTailObject = new List<GameObject>();
        objectSnake.transform.position = state.GetHeadPosition();
        objectSnake.transform.parent = transform;
        objectTailStack.transform.parent = transform;
        CreateColor(objectSnake, colorSnake);

        // Create food object
        objectFood = new GameObject("Food_Object");
        objectFood.transform.position = state.GetFoodPosition();
        objectFood.transform.parent = transform;
        CreateColor(objectFood, colorFood);
    }
    
    // Create new tail object in scene
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

    // Function for checking next move
    public void Timer()
    {
        timer += Time.deltaTime;
        if (timer > speedRate)
        {
            timer = 0;
            move = true;
        }
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
        size = (int)(size * uiSizeScrollbar.GetComponent<Scrollbar>().value);
        speedRate = 1 - (speedRate / 10) - uiSpeedScrollbar.GetComponent<Scrollbar>().value;
        uiOption.transform.parent = uiGameOver.transform;
        uiOptionText.transform.parent = uiGameOver.transform;
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
        scoreText.SetActive(false);
        highScoreText.SetActive(false);
        uiGameOver.SetActive(true);
    }

    public void PlayAgain()
    {
        Destroy(objectSnake);
        Destroy(objectFood);
        Destroy(objectTailStack);
        option = uiOption.GetComponent<TMP_Dropdown>().value;
        GameSetup();
        status = true;
        uiGameOver.SetActive(false);
    }

    public void UpdateScore()
    {
        scoreText.GetComponent<TextMeshProUGUI>().SetText("Score: " + score);
        highScoreText.GetComponent<TextMeshProUGUI>().SetText("High score: " + highScore);
        uiScoreText.GetComponent<TextMeshProUGUI>().SetText("Score: " + score);
        uiHighScoreText.GetComponent<TextMeshProUGUI>().SetText("High score: " + highScore);
    }
    #endregion
    
}

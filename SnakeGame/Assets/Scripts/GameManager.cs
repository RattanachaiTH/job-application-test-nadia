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
    private Node[,] state;
    private Node nodeSnake;
    private Node nodeFood;
    private List<Node> listCreateFood;
    private List<Node> listTail;
    private List<GameObject> listTailObject;
    private bool status;

    public void Awake()
    {
        // initial value
        maxSize = 200;
        minSize = 10;
        length = 100;
        status = false;
        uiStart.SetActive(true);
        GameSetup();
    }

    void GameSetup()
    {
        // Game setup
        score = 0;
        CreateMap();
        CreateState();
        CreateSnakeNode();
        CreateFoodNode();
        CreateSnakeObject();
        CreateFoodObject();
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
        listCreateFood = new List<Node>();
        listTail = new List<Node>();
        listTailObject = new List<GameObject>();
        state = new Node[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                state[i, j] = new Node(new Vector2(i, j));
                listCreateFood.Add(state[i, j]);
            }
        }
    }
    
    void CreateSnakeNode()
    {
        int index_x = (int)sizeHaft - 4;
        int index_y = (int)sizeHaft;
        nodeSnake = state[index_x, index_y];
        listCreateFood.Remove(nodeSnake);
    }

    void CreateFoodNode()
    {
        int random = Random.Range(0, listCreateFood.Count);
        nodeFood = listCreateFood[random];
        listCreateFood.Remove(nodeFood);
    }
    
    void CreateTailNode(Node node)
    {
        listTail.Add(node);
        CreateTailObject(node.position);
    }

    void CreateSnakeObject()
    {
        objectSnake = new GameObject("Snake_Object");
        objectTailStack = new GameObject("Tail_Stack");
        objectSnake.transform.position = new Vector3(-10f, -10f, 10f);
        objectSnake.transform.parent = transform;
        objectTailStack.transform.parent = transform;
        CreateColor(objectSnake, colorSnake);
        Snake snake = objectSnake.AddComponent<Snake>();
        snake.gameManager = transform.GetComponent<GameManager>();
        snake.SetPosition(nodeSnake.position);
    }

    void CreateFoodObject()
    {
        objectFood = new GameObject("Food_Object");
        objectFood.transform.position = new Vector3(-10f, -10f, 10f);
        objectFood.transform.parent = transform;
        CreateColor(objectFood, colorFood);
    }

    void CreateTailObject(Vector2 position)
    {
        GameObject newTail = new GameObject("Tail_Object" + (listTail.Count + 1));
        newTail.transform.parent = objectTailStack.transform;
        CreateColor(newTail, colorSnake);
        newTail.transform.localScale = new Vector3(length, length, 1);
        newTail.transform.position = position;
        listTailObject.Add(newTail);
    }

    // Function for updating Snake nodes
    public void UpdateState(Vector2 position, Node nextNode, bool addTail)
    {
        // Move SnakeHead
        objectSnake.transform.position = position;
        Node tempNext = nextNode;
        Node tempCurrent = nodeSnake;
        listCreateFood.Remove(nextNode);
        nodeSnake = nextNode;

        // Move SnakeTail
        for (int i = 0; i < listTail.Count; i++)
        {
            tempNext = tempCurrent;
            tempCurrent = listTail[i];
            listTail[i] = tempNext;
        }

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
    public Node[,] GetState()
    {
        return state;
    }

    public Node GetSnakeNode()
    {
        return nodeSnake;
    }

    public Node GetFoodNode()
    {
        return nodeFood;
    }

    public List<Node> GetTailList()
    {
        return listTail;
    }

    public bool GetStatus()
    {
        return status;
    }

    public void SetStatus(bool status)
    {
        this.status = status;
    }

    #endregion
}

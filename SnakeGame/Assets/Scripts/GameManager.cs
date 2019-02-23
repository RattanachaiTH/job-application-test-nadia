using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Public valiable
    public int size = 40;
    public float speedRate = 0.05f;
    public int score;
    public GameObject background;
    public GameObject cam;
    public GameObject objectSnake;
    public GameObject objectFood;
    public Node[,] state;
    public Node nodeSnake;
    public Node nodeFood;
    public Color colorSnake;
    public Color colorFood;
    public bool status;
    public Sprite spriteAsset;

    // Private valiable
    private int maxSize;
    private int minSize;
    private int length;
    private float sizeLength;
    private float sizeHaft;

    public void Start()
    {
        // initial value
        maxSize = 200;
        minSize = 10;
        length = 100;
        score = 0;

        // object setup
        CreateMap();
        CreateState();
        CreateSnakeNode();
        CreateFoodNode();
        CreateSnakeObject();
        CreateFoodObject();
        status = true;
    }
    void FixedUpdate()
    {
        UpdateState();
    }


    void CreateMap()
    {
        // initial value
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
        state = new Node[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                state[i, j] = new Node(Node.Type.None, new Vector2(i, j));
            }
        }
    }
    
    void CreateSnakeNode()
    {
        if (nodeSnake == null)
        {
            int index = (int)sizeHaft - 1;
            state[index, index] = new Node(Node.Type.Head, new Vector2(index, index));
            nodeSnake = state[index, index];
        }
    }

    void CreateFoodNode()
    {
        if (nodeFood == null)
        {
            List<Node> listNone = CheckNoneNode();
            int number = Random.Range(0, listNone.Count);
            nodeFood = listNone[number];
            nodeFood.type = Node.Type.Food;
        }
    }

    void CreateSnakeObject()
    {
        objectSnake = new GameObject("Snake_Object");
        objectSnake.transform.parent = transform;
        CreateColor(objectSnake, colorSnake);
        objectSnake.transform.localScale = new Vector3(length, length, 1);
        Snake snake = objectSnake.AddComponent<Snake>();
        snake.gameManager = transform.GetComponent<GameManager>();
        snake.position = nodeSnake.position;
    }

    void CreateFoodObject()
    {
        objectFood = new GameObject("Food_Object");
        objectFood.transform.parent = transform;
        CreateColor(objectFood, colorFood);
        objectFood.transform.localScale = new Vector3(length, length, 1);
        Food food = objectSnake.AddComponent<Food>();
    }

    List<Node> CheckNoneNode()
    {
        List<Node> listNone = new List<Node>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (state[i, j].type == Node.Type.None)
                {
                    listNone.Add(state[i, j]);
                }
            }
        }
        return listNone;
    }

    public void UpdateSnake(Vector2 position, bool addTail)
    {
        Node tempNode = nodeSnake;
        Vector2 tempPosition = nodeSnake.position;
        nodeSnake.position = position;
    }
    
    public void UpdateState()
    {
        if (nodeFood != null && nodeSnake != null)
        {
            objectSnake.transform.position = nodeSnake.position;
            objectFood.transform.position = nodeFood.position;
        }
    }

    void CreateColor(GameObject obj, Color color)
    {
        SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteAsset;
        spriteRenderer.color = color;
    }

}

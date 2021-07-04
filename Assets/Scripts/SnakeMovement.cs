using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeMovement : MonoBehaviour
{
    public Transform wall, snake, snakeHolder, food;
    Transform canvas;
    Text scoreText, maxScoreText;
    int width = 18;
    int height = 18;
    int score = 0, maxScore = 0;
    public int dir = 0;
    float curX = 11.5f, curY = 10.5f;
    float td = 0, speed = 0.08f;
    public List<Vector3> snakePos = new List<Vector3>();
    bool dirlocked = false, foodexist = false, paused = false;
    Transform foodIns;
    void Start()
    {
        Find();
        FillPos();
        SetSnake();
        RespawnSnake();
        SpawnFood();
    }

    void Find()
    {
        canvas = GameObject.Find("Canvas").transform;
        scoreText = canvas.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
        maxScoreText = canvas.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
    }

    void FillPos()
    {
        transform.position = new Vector3(-width / 2, 0, -height / 2);
        for (int i = -1; i < width + 1; i++)
        {
            for (int j = -1; j < height + 1; j++)
            {
                if (i == -1 || i == width || j == -1 || j == height)
                {
                    Transform wallIns = Instantiate(wall, transform);
                    wallIns.localPosition = new Vector3(i + 0.5f, 0.5f, j + 0.5f);
                }
            }
        }
    }

    void SetSnake()
    {
        score = 0;
        scoreText.text = "" + score;
        snakePos.Clear();

        snakePos.Add(new Vector3(4.5f, 1, 5.5f));
        snakePos.Add(new Vector3(5.5f, 1, 5.5f));
        snakePos.Add(new Vector3(6.5f, 1, 5.5f));
        snakePos.Add(new Vector3(7.5f, 1, 5.5f));
        snakePos.Add(new Vector3(8.5f, 1, 5.5f));

        curX = 8.5f;
        curY = 5.5f;
    }

    void RespawnSnake()
    {
        foreach (Transform child in snakeHolder)
        {
            Destroy(child.gameObject);
        }

        for (int k = 0; k < snakePos.Count; k++)
        {
            Transform snakeIns = Instantiate(snake, snakeHolder.transform);
            snakeIns.localPosition = snakePos[k];
        }
    }

    void Update()
    {
        Pause();
        if (!paused)
        {
            MovementArrow();
            Move();
        }
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            paused = !paused;
        }
    }

    void MovementArrow()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && dir != 2 && !dirlocked)
        {
            dir = 0;
            dirlocked = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && dir != 3 && !dirlocked)
        {
            dir = 1;
            dirlocked = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && dir != 0 && !dirlocked)
        {
            dir = 2;
            dirlocked = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && dir != 1 && !dirlocked)
        {
            dir = 3;
            dirlocked = true;
        }
    }

    void Move()
    {
        td += Time.deltaTime;
        Vector3 newPos = new Vector3(20, 1, 20);
        if (td > speed)
        {
            if (dir == 0)
            {
                curX = (curX + 1) % width;
            }
            if (dir == 1)
            {
                curY = (curY + height - 1) % height;
            }
            if (dir == 2)
            {
                curX = (curX + width - 1) % width;
            }
            if (dir == 3)
            {
                curY = (curY + 1) % height;
            }

            newPos = new Vector3(curX, 1, curY);

            if (snakePos.Contains(newPos))
            {
                SetSnake();
            }
            else
            {
                snakePos.Add(newPos);
            }

            if (newPos == foodIns.localPosition)
            {
                score++;
                if (maxScore < score) maxScore = score;
                scoreText.text = "" + score;
                maxScoreText.text = "" + maxScore;
                snakePos.Add(newPos);
                SpawnFood();
            }

            dirlocked = false;
            snakePos.RemoveAt(0);
            td = 0;
        }

        RespawnSnake();
    }



    void SpawnFood()
    {
        if (foodIns != null)
        {
            Destroy(foodIns.gameObject);
        }
        foodIns = Instantiate(food, transform);
        foodIns.localPosition = new Vector3((int)Random.Range(1, width - 1) + 0.5f, 1, (int)Random.Range(1, height - 1) + 0.5f);
        foodexist = true;
    }
}

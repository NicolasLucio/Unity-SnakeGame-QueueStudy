using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class exempleGame : MonoBehaviour
{
    public Queue<GameObject> gameGrid = new Queue<GameObject>();
    public Queue<GameObject> gameEnemyGrid = new Queue<GameObject>();

    public GameObject gameSquare;
    public GameObject gameEnemy;
    public GameObject scoreTextObject;
    private Vector2 lastPosition;
    private Vector2 tempPosition;
    private Vector2 enemyPosition;
    private bool firstObject;
    private bool alreadyExist;
    private bool playerDied;
    private int died;
    private int lastScore;
    private Text scoreText;
    
    void Start()
    {
        firstObject = true;
        playerDied = false; 
        lastPosition = new Vector2(0.0f, 0.0f);
        died = 0;
        scoreText = scoreTextObject.GetComponent<Text>();

        InstantiateFirstObject();
    }
    
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CalculateInput("Right");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CalculateInput("Down");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CalculateInput("Left");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CalculateInput("Up");
        }
    }

    void InstantiateFirstObject()
    {
        GameObject tempObject = Instantiate(gameSquare, lastPosition, Quaternion.identity);
        gameGrid.Enqueue(tempObject);
        InstantiateRandomEnemy();
        playerDied = false;
    }

    void InstantiateRandomEnemy()
    {
        int posX = Random.Range(-6, 17);
        int posY = Random.Range(-6, 7);
        if (posX % 2 != 0)
        {
            posX--;
        }
        if (posY % 2 != 0)
        {
            posY--;
        }
        alreadyExist = false;
        enemyPosition = new Vector2(posX, posY);

        foreach (GameObject auxPosition in gameGrid)
        {
            Vector2 tempCheckPosition = auxPosition.transform.position;
            if (tempCheckPosition == enemyPosition)
            {
                alreadyExist = true;
            }
        }

        if (alreadyExist == true)
        {
            InstantiateRandomEnemy();
        }
        else
        {
            GameObject tempEnemy = Instantiate(gameEnemy, enemyPosition, Quaternion.identity);
            gameEnemyGrid.Enqueue(tempEnemy);
        }
    }

    void CalculateInput(string whichInput)
    {
        int moved = 0;
        if (whichInput == "Right")
        {
            if (lastPosition.x < 16)
            {
                tempPosition = new Vector2((lastPosition.x + 2), lastPosition.y);
                GameObject tempObject = Instantiate(gameSquare, tempPosition, Quaternion.identity);
                gameGrid.Enqueue(tempObject);
                moved++;
            }            
        }
        else if (whichInput == "Down")
        {
            if (lastPosition.y > -6)
            {
                tempPosition = new Vector2(lastPosition.x, (lastPosition.y - 2));
                GameObject tempObject = Instantiate(gameSquare, tempPosition, Quaternion.identity);
                gameGrid.Enqueue(tempObject);
                moved++;
            }                      
        }
        else if (whichInput == "Left")
        {
            if (lastPosition.x > -6)
            {
                tempPosition = new Vector2((lastPosition.x - 2), lastPosition.y);
                GameObject tempObject = Instantiate(gameSquare, tempPosition, Quaternion.identity);
                gameGrid.Enqueue(tempObject);
                moved++;
            }            
        }
        else if (whichInput == "Up")
        {
            if (lastPosition.y < 6)
            {
                tempPosition = new Vector2(lastPosition.x, (lastPosition.y + 2));
                GameObject tempObject = Instantiate(gameSquare, tempPosition, Quaternion.identity);
                gameGrid.Enqueue(tempObject);
                moved++;
            }                 
        }
        if (firstObject == true && moved == 1)
        {
            firstObject = false;
        }
        else
        {
            if (moved == 1)
            {
                GameObject tempDeletedObject = gameGrid.Dequeue();
                tempDeletedObject.SetActive(false);
                Destroy(tempDeletedObject);
            }            
        }
        lastPosition = tempPosition;
        CheckEnemy();
        CheckSelfPosition();
        moved = 0;        
    }

    void CheckEnemy()
    {
        if (lastPosition == enemyPosition)
        {
            firstObject = true;
            GameObject tempDeletedEnemy = gameEnemyGrid.Dequeue();
            tempDeletedEnemy.SetActive(false);
            Destroy(tempDeletedEnemy);
            InstantiateRandomEnemy();
        }
    }

    void CheckSelfPosition()
    {
        foreach (GameObject auxSelfPosition in gameGrid)
        {
            Vector2 tempSelfPosition = auxSelfPosition.transform.position;
            if (lastPosition == tempSelfPosition)
            {
                died++;
            }
        }
        if (died > 1)
        {
            playerDied = true;
        }

        if (playerDied == true)
        {
            foreach (GameObject auxReset in gameGrid)
            {
                auxReset.SetActive(false);
                Destroy(auxReset);
            }
            foreach (GameObject auxReset2 in gameEnemyGrid)
            {
                auxReset2.SetActive(false);
                Destroy(auxReset2);
            }
            lastScore = gameGrid.Count;
            gameEnemyGrid.Clear();
            gameGrid.Clear();
            lastPosition = new Vector2(0.0f, 0.0f);
            CheckScore();
            InstantiateFirstObject();
        }
        died = 0;        
    }

    void CheckScore()
    {
        scoreText.text = "Last Score: " + lastScore.ToString(); 
    }
}

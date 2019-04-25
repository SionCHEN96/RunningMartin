using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TileManager : MonoBehaviour
{

    public GameObject[] tilePrefabs;

    public GameObject currentTile;

    private static TileManager instance;

    //Create two stacks
    private Stack<GameObject> leftTiles = new Stack<GameObject>();
    private Stack<GameObject> topTiles = new Stack<GameObject>();


    public static TileManager Instance
    {
        get
        {
            if (instance == null) //Finds the instance if it doesn't exist
            {
                instance = GameObject.FindObjectOfType<TileManager>();
            }

            return instance;

        }

    }

    //property
    public Stack<GameObject> LeftTiles
    {
        get
        {
            return leftTiles;
        }

        set
        {
            leftTiles = value;
        }
    }

    //property
    public Stack<GameObject> TopTiles
    {
        get
        {
            return topTiles;
        }

        set
        {
            topTiles = value;
        }
    }


    // Use this for initialization
    void Start()
    {

        CreateTiles(50);
        //Spawns 50 tiles when the game starts
        for (int i = 0; i < 20; i++)
        {
            SpawnTile();
        }

    }

    public  void CreateTiles(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            LeftTiles.Push(Instantiate(tilePrefabs[0]));
            TopTiles.Push(Instantiate(tilePrefabs[1]));

            TopTiles.Peek().name = "TopTile";
            TopTiles.Peek().SetActive(false);

            LeftTiles.Peek().name = "LeftTile";
            LeftTiles.Peek().SetActive(false);
        }

    }
    public void SpawnTile()
    {
        if (LeftTiles.Count <= 0 || TopTiles.Count <= 0)
        {
            CreateTiles(10);
        }
        //Generating a random number between 0 and 1
        int randomIndex = Random.Range(0, 2);
        if (randomIndex == 0)
        {
            GameObject tmp = LeftTiles.Pop();
            tmp.SetActive(true);
            tmp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = tmp;
        }
        else if(randomIndex==1)
        {
            GameObject tmp = TopTiles.Pop();
            tmp.SetActive(true);
            tmp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = tmp;
        }

        int spawnPickup = Random.Range(0, 5);
        if (spawnPickup == 0)
        {
            currentTile.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("scene");
    }


}

using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    private float fallDelay=.2f;
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    void OnTriggerExit(Collider other)
    {
        //If the player exits the tile
        if (other.tag == "Player")
        {   
            //Spawns a new tile
            TileManager.Instance.SpawnTile();
            StartCoroutine(FallDown());
        }
    }

    IEnumerator FallDown()
    {
        yield return new WaitForSeconds(fallDelay);
        GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(2);
        switch (gameObject.name)
        {
            case "LeftTile":
                TileManager.Instance.LeftTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;
            case "TopTile":
                TileManager.Instance.TopTiles.Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    private void Update()
    {

    }
}

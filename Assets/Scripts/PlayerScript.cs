using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    public float speed;
    private Vector3 dir;

    private bool isDead;

    public AudioClip[] sounds;

    public GameObject ps;
    public GameObject resetButtom;
    public Animator gameOverAnim;

    private int score = 0;
    public Text scoreText;
    public Text currentScoreText;
    public Text bestScoreText;

    public bool IsDead
    {
        get
        {
            return isDead;
        }

        set
        {
            isDead = value;
        }
    }

    // Use this for initialization

    void Start()
    {
        isDead = false;
        //Sets the player's direction to 0, so that the player doesn't move when the game start
        dir = Vector3.zero;
        this.GetComponent<AudioSource>().clip = sounds[0];
        //PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        //If we click on the screen or the first mouse button
        if (Input.GetMouseButtonDown(0) && !isDead)
        {
            score++;
            //Switches the players direction every time we click ont he screen or mouse
            if (dir == Vector3.forward)
            {
                dir = Vector3.left;
                //transform.Rotate(0, -90, 0);
            }
            else
            {
                dir = Vector3.forward;
               // transform.Rotate(0,90,0);
            }
        }

        //Calculates the player's movement
        float amoutToMove = speed * Time.deltaTime;

        //Makes the player move
        transform.Translate(dir * amoutToMove);
        speed += 0.1f * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickup")
        {
            other.gameObject.SetActive(false);
            Instantiate(ps, transform.position, Quaternion.identity);
            this.GetComponent<AudioSource>().Play();
            score += Random.Range(3, 6);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tile")
        {
            RaycastHit hit;

            Ray downRay = new Ray(transform.position, -Vector3.up);

            if (!Physics.Raycast(downRay, out hit))
            {
                isDead = true;
                GameOver();
                resetButtom.SetActive(true);
            }
        }
    }

    private void GameOver()
    {
        gameOverAnim.SetTrigger("GameOver");
        currentScoreText.text = score.ToString();
        scoreText.gameObject.SetActive(false);

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
        this.GetComponent<AudioSource>().clip = sounds[1];
        this.GetComponent<AudioSource>().Play();
    }



}

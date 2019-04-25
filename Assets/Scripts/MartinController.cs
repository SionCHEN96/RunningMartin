using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MartinController : MonoBehaviour
{

    //outside game obejects
    public GameObject resetButtom;
    public Animator gameOverAnim;

    //Text
    public Text scoreText;
    public Text currentScoreText;
    public Text bestScoreText;
    int score;

    //Physical Attributes
    public float forwardVel = 8f;
    public float jumpVel = 25f;
    public float distToGrounded = 0.1f;
    public float downAccel = 0.75f;
    public LayerMask ground;
    Vector3 velocity = Vector3.zero;

    //Components
    Animator animator;
    Rigidbody rBody;
    public AudioClip[] sounds;

    //States
    private bool isDead = false;
    bool isBegin = false;
    bool isGrounded;

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

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGrounded,ground);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody>();
        this.GetComponent<AudioSource>().clip = sounds[0];

        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKey)
        {
            isBegin = true;
            animator.SetBool("IsBegin", isBegin);
        }

        Turn();
        Jump();


    }

    private void FixedUpdate()
    {
        scoreText.text = score.ToString();
        forwardVel += 0.005f;

        CheckDie();

        Run();
        rBody.velocity = transform.TransformDirection(velocity);

    }

    void Run()
    {
        if (isBegin && !isDead)
        {
            velocity.z = forwardVel;
            //Debug.Log(velocity.z);
        }
    }

    void Turn()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Rotate(transform.up * 90);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Rotate(transform.up * -90);
        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space)&&Grounded())
        {
            velocity.y = jumpVel;
            animator.SetBool("IsJump", true);
        }
        else if(Grounded()&&!Input.GetKey(KeyCode.Space))
        {
            velocity.y = 0;
            animator.SetBool("IsJump", false);
        }
        else
        {
            velocity.y -= downAccel;
        }

    }

    void CheckDie()
    {
        if (transform.position.y < -10)
        {
            IsDead = true;

            GameOver();
            resetButtom.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            score += other.gameObject.GetComponent<PickupInfo>().point;
            GetComponent<AudioSource>().Play();
        }
    }

    private void GameOver()
    {
        gameOverAnim.SetTrigger("GameOver");
        gameOverAnim.gameObject.GetComponent<AudioSource>().Play();

        currentScoreText.text = score.ToString();
        scoreText.gameObject.SetActive(false);

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
    }




}

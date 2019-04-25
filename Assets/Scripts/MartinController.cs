using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MartinController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isDead = false;
    public float speed = 5f;

    Animator animator;

    private float gravity = -10f;

    bool isBegin = false;
    bool isJump = false;

    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            isBegin = true;
            animator.SetBool("IsBegin", isBegin);
        }

        
    }

    private void FixedUpdate()
    {
        if (isBegin)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }


}

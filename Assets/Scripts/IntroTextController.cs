using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTextController : MonoBehaviour
{


    private void FixedUpdate()
    {
        if (Input.anyKey)
        {
            gameObject.SetActive(false);
        }
    }
}

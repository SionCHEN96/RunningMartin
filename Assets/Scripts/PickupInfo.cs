using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInfo : MonoBehaviour
{
    public int point=3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            Invoke("Reset", 3f);
        }
    }

    private void Reset()
    {
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
    }
}

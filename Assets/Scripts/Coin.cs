using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<BirdStatus>().CollectCoin(this.gameObject); // Using "this" is actually unnecessary here, I just think it improves readability
            other.GetComponent<BirdAgent>().HandleCollectCoin();
        }
    }
}

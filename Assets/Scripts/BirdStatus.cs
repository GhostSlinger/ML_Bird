using UnityEngine;

public class BirdStatus : MonoBehaviour
{
    private int coinsCollected;
    
    // Use this for initialization
    void Start()
    {
        coinsCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CollectCoin(GameObject coin)
    {
        coinsCollected++;
        //Destroy(coin);
        coin.gameObject.SetActive(false);
    }
}

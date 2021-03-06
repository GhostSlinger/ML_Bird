using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class BirdAgent : Agent
{
    public float speedMultiplier;

    // Store a reference to the Rigidbody2D component required to use 2D Physics.
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;

    private Vector3 birdStartPos;
    public Transform coinsParent;
    public Transform bombsParent;

    private int coinsCollected = 0;

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;

        // Get and store a references to the Rigidbody2D and SpriteRenderer components.
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        birdStartPos = transform.position;                      // Gets the bird original starting position

    }

    public void HandleCollectCoin()
    {
        Debug.Log("Got a reward by coin!");
        AddReward(1.0f);

        coinsCollected++;

        if (coinsParent.childCount == coinsCollected)
        {
            Debug.Log("No more coins, ending episode");
            EndEpisode();
        }
    }

    public void HandleHitBomb()
    {
        Debug.Log("Ouch! You hit a bomb!");
        if(StepCount > 30000)
        {
            AddReward(-1.0f);
        }
        else
        {
            AddReward(-0.25f);
        }
        
    }


    // mlagents-learn config/ppo/config.yaml --run-id=BirdTest_01 
    // Resets the agent everytime it finishes getting all coins or max steps
    public override void OnEpisodeBegin()
    {
        coinsCollected = 0;
        Debug.Log("An episode began now");

        // TODO: Implement logic for resetting the game

        transform.position = birdStartPos;                      // basically resets the bird to its original position

        // Optional 1: Make bird start at random position
        //float x = Random.RandomRange(-8f, 8f);
        //float y = Random.RandomRange(-4f, 2.5f);
        //transform.position = new Vector3(x, y, transform.position.z);


        foreach (Transform coin in coinsParent)                 // When an episode begin, this goes to coins parent, loop through all children and set active
        {
            coin.gameObject.SetActive(true);
        }

        foreach (Transform bomb in bombsParent)                 
        {
            bomb.gameObject.SetActive(true);
        }
    }

    // Tells the agent what the current game state
    public override void CollectObservations(VectorSensor sensor)
    {
        //base.CollectObservations(sensor);

        //// Target and Agent positions
        //sensor.AddObservation(Target.localPosition);
        //sensor.AddObservation(this.transform.localPosition);

        //// Agent velocity
        //sensor.AddObservation(rBody.velocity.x);
        //sensor.AddObservation(rBody.velocity.z);

        sensor.AddObservation(0.0f);
    }

    // Actions movement given to the agent
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //base.OnActionReceived(actions);

        int action_chosen = actionBuffers.DiscreteActions[0];           // we are taking only the 1st discrete branch here

        float moveHorizontal = 0.0f;
        float moveVertical = 0.0f;

        switch(action_chosen)
        {
            case 0: // Don't move
                break;
            case 1:
                moveHorizontal = -1.0f;
                break;
            case 2:
                moveVertical = 1.0f;
                break;
            case 3:
                moveHorizontal = 1.0f;
                break;
            case 4:
                moveVertical = -1.0f;
                break;

        }

        // Flip the bird to face the horizontal direction chosen.
        if (moveHorizontal > 0)
        {
            sr.flipX = true;
        }
        else if (moveHorizontal < 0)
        {
            sr.flipX = false;
        }
        // Use the two floats to create a vector.
        Vector2 forceDirection = new Vector2(moveHorizontal, moveVertical);

        // Accelerate the player in the chosen direction.
        rb2d.AddForce(forceDirection * speedMultiplier);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

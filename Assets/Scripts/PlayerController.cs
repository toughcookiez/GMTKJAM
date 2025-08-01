using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public LayerMask groundLayer;

    //List of all Objects that were Spawned by the player
    private List<GameObject> spawnedObjects = new List<GameObject>();

    public bool isAutoRunner = false;
    public float moveSpeed = 100;
    public float jumpForce = 300;
    public float superJumpForce = 500;

    public float health = 3;
    public float maxHealth = 3;

    public float points = 0;

    public int lives = 3;

    //Height unter which player dies because he fell into pit
    public float deathHeight = -20;

    public GameObject[] prefabs = new GameObject[3];

    public GameObject nextPrefab;

    public GameObject destroyJumpPrefab;
    public GameObject emptyJumpPrefab;

    private Vector2 groundCheckPosition;
    private bool isGrounded = true;
    private bool canDoubleJump = false;
    private bool canDash = true;
    private bool isDashing = false;

    //Destroy jump... while jumping the player destroys the objects they touch
    private bool hasDestroyJump = false;
    //private bool destroyJumpActive = false;

    //Empty jump... player can just jump without spawning an object or doing any ability
    private bool hasEmptyJump = false;

    //Cooldown, between jumps (also between jump and double jump)
    private float jumpCooldownTime = 0.2f;
    private bool isJumpCooldown = false;

    //Checks if player has already jumped in this loop. if player can beat loop without jumping
    //then probability for randomDelete will be increased
    private bool hasJumpedInThisLoop = false;



    public float destroyJumpProbability;
    public float emptyJumpProbability;

    //Probability that some objects get destroyed when player reaches the goal
    public float goalDestroysObjectsProbability;

    //the probability that was set at the beginning
    public float originalGoalDestroysObjectsProbability;

    public int deleteObjectNumber = 4;

    private Vector2 startPos;

    public Rigidbody2D rb;

    private PrefabImage prefabImage;

    //Radius in which objects are destroyed during DestroyJump
    private BoxCollider2D destroyRadius;

    public int pointsPerDestroyedObject = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startPos = rb.position;

        prefabImage = GameObject.Find("PrefabImage").GetComponent<PrefabImage>();


        destroyRadius = transform.Find("DestroyRadius").GetComponent<BoxCollider2D>();

        destroyRadius.enabled = false;

        originalGoalDestroysObjectsProbability = goalDestroysObjectsProbability;


        ChooseNextCard();


    }



    // Update is called once per frame
    void Update()
    {



        if (!isAutoRunner)
        {

            //Move Left/Right
            if (Input.GetKey(KeyCode.A))
            {
                rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);

            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

            }



        }
        else
        {
            //If is autorunner move right automatically
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);



        }


        //Jump

        if (IsGrounded() && !isJumpCooldown && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //Double Jump
        if (canDoubleJump && !IsGrounded() && !isJumpCooldown && Input.GetKeyDown(KeyCode.Space))
        {
            canDoubleJump = false;


            Jump();
        }


        //Respawn 
        if (Input.GetKeyDown(KeyCode.R))
        {

            PlayerHit();
            RespawnPlayer();
        }

        //if player falls into pit they die
        if (rb.position.y < deathHeight)
        {
            PlayerDies();
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Block"))
        {
            isGrounded = true;
            canDoubleJump = false;
            destroyRadius.enabled = false;
            //destroyJumpActive = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Block"))
        {
            isGrounded = false;
            canDoubleJump = true;
        }
    }

    //Get hit by obstacle
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Only get hit if not using DestroyJump
        if (other.gameObject.CompareTag("Harmful") && !destroyRadius.enabled)
        {
            PlayerHit();
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            GoalReached();
        }
    }

    private void Jump()
    {
        UseCurrentCard();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        hasJumpedInThisLoop = true;

        isJumpCooldown = true;
        StartCoroutine(JumpCooldown());


    }

    private bool HasSpecialAbility()
    {
        return hasDestroyJump || hasEmptyJump;
    }

    public void PlayerDies()
    {

        //Reset player at start position if they still have lives

        if (lives > 0)
        {
            lives--;
            health = maxHealth;
            RespawnPlayer();
        }
        else
        {
            //GAME OVER
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

    }

    //Player gets hit and loses one health
    public void PlayerHit()
    {
        if (health > 0) { health--; }
        else
        {
            PlayerDies();
        }

    }

    public void GoalReached()
    {
        RespawnPlayer();
        points += 10;

        //If player finished this loop without jumping, probability for random destroy is increased
        if (!hasJumpedInThisLoop)
        {
            goalDestroysObjectsProbability += 5;
        }
        else
        {
            goalDestroysObjectsProbability = originalGoalDestroysObjectsProbability;
        }


        //There is a chance that some objects get destroyed
        int rand = Random.Range(0, 101);
        if (rand < goalDestroysObjectsProbability)
        {
            RandomDeleteObjects();
        }

        //set back to false
        hasJumpedInThisLoop = false;
    }


    //Choose some random spawnedObjects and Destroy them
    public void RandomDeleteObjects()
    {
        for (int i = 0; i < Mathf.Min(deleteObjectNumber, spawnedObjects.Count); i++)
        {
            int rand = Random.Range(0, spawnedObjects.Count);
            GameObject obj = spawnedObjects[rand];
            spawnedObjects.RemoveAt(rand);
            Destroy(obj);

        }
    }

    //Uses the current card (spawns object or use special ability)
    private void UseCurrentCard()
    {

        destroyRadius.enabled = false;
        //destroyJumpActive = false;

        if (hasEmptyJump)
        {

            hasEmptyJump = false;
        }
        else if (hasDestroyJump)
        {
            //destroyJumpActive = true;

            destroyRadius.enabled = true;

            hasDestroyJump = false;
        }
        else
        {
            SpawnPrefab();
        }

        ChooseNextCard();
    }

    private void ChooseNextCard()
    {

        //Choose randomly if next card is spawn prefab or ability
        int rand = Random.Range(0, 101);


        if (rand < destroyJumpProbability)
        {
            hasDestroyJump = true;

            nextPrefab = destroyJumpPrefab;
        }
        else if (rand < emptyJumpProbability)
        {
            hasEmptyJump = true;
            nextPrefab = emptyJumpPrefab;
        }
        else
        {
            ChooseNextPrefab();
        }
        prefabImage.UpdateImage();
    }


    //Spawn prefab at player position
    private void SpawnPrefab()
    {
        GameObject instance = Instantiate(nextPrefab);
        instance.transform.position = rb.position;

        spawnedObjects.Add(instance);


    }

    private void ChooseNextPrefab()
    {
        nextPrefab = prefabs[Random.Range(0, prefabs.Length)];


    }

    private void RespawnPlayer()
    {
        rb.position = startPos;
    }

    private bool IsGrounded()
    {

        if (Physics2D.Raycast(transform.position, Vector2.down, 0.8f, groundLayer.value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Very high jump that is performed on jumppad
    public void SuperJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, superJumpForce);


        //Double jump possible also after jumppad
        canDoubleJump = true;
    }


    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldownTime);

        isJumpCooldown = false;
    }
}
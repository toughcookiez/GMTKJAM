using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public LayerMask groundLayer;

    //List of all Objects that were Spawned by the player
    public List<GameObject> spawnedObjects = new List<GameObject>();

    public bool isAutoRunner = false;
    public float moveSpeed = 100;
    public float jumpForce = 300;
    public float superJumpForce = 500;

    public float health = 3;
    public float maxHealth = 3;

    public float points = 0;

    public GameObject BlockBreakEffect;
    public GameObject StarEffect;

    public int lives = 3;

    //Height unter which player dies because he fell into pit
    public float deathHeight = -20;

    public GameObject[] prefabs = new GameObject[3];

    public GameObject nextPrefab;
    public GameObject nextNextPrefab;

    public GameObject destroyJumpPrefab;
    public GameObject emptyJumpPrefab;

    private Vector2 groundCheckPosition;
    private bool isGrounded = true;
    private bool canDoubleJump = false;
    private bool canDash = true;
    private bool isDashing = false;

    //During respawning the player destroys objects at the respawn point
    private bool isRespawnDestroy = false;

    private bool _isDead = false;

    [SerializeField] private GameObject LoseScreen;

    [SerializeField] private TextMeshProUGUI ScoreText;

    [SerializeField] private TextMeshProUGUI HighScoreText;

    [SerializeField] private HighScore _levelHighScore;


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

    private GameObject starObject;

    public float destroyJumpProbability;
    public float emptyJumpProbability;

    //Probability that some objects get destroyed when player reaches the goal
    public float goalDestroysObjectsProbability;

    //the probability that was set at the beginning
    public float originalGoalDestroysObjectsProbability;

    public int deleteObjectNumber = 4;

    private Vector2 startPos;

    public Rigidbody2D rb;

    private PrefabImage prefabFirstImage;
    private PrefabImage prefabSecondImage;

    //Radius in which objects are destroyed during DestroyJump
    private BoxCollider2D destroyRadius;

    public int pointsPerDestroyedObject = 2;

    private ItemsManager itemsManager;

    public GameObject ItemHolder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoseScreen.SetActive(false);
        rb = GetComponent<Rigidbody2D>();

        itemsManager = ItemHolder.GetComponent<ItemsManager>();

        startPos = rb.position;

        prefabFirstImage = GameObject.Find("FirstCardImage").GetComponent<PrefabImage>();
        prefabSecondImage = GameObject.Find("SecondCardImage").GetComponent<PrefabImage>();


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
            if (Input.GetKey(KeyCode.A) && !_isDead)
            {
                rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);

            }
            if (Input.GetKey(KeyCode.D) && !_isDead)
            {
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

            }



        }
        else
        {
            if (_isDead)
            {
                return;
            }

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
        //if (Input.GetKeyDown(KeyCode.R))
        //{

        //    PlayerHit();
        //    RespawnPlayer();
        //}

        //if player falls into pit they die
        if (rb.position.y < deathHeight)
        {
            PlayerDies();
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || (other.gameObject.CompareTag("Block") && !destroyRadius.enabled ))
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
        if (_isDead)
        {
            return;
        }
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
        if (_isDead)
        {
            return;
        }

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
        lives--;
        if (lives > 0)
        {
            health = maxHealth;
            RespawnPlayer();
        }
        else
        {
            lives = 0;
            //GAME OVER
            _isDead = true;
            if (_levelHighScore.score < points)
            {
                _levelHighScore.score = points;
            }
            LoseScreen.SetActive(true);
            string PointsString = points.ToSafeString();
            ScoreText.text = "Score: " + PointsString;

            string HighScoreString = _levelHighScore.score.ToSafeString();
            HighScoreText.text = "HighScore: " + HighScoreString;
        }

    }

    //Player gets hit and loses one health
    public void PlayerHit()
    {
        health--;
        if (health <= 0)
        {
            PlayerDies();
        }

    }

    public void GoalReached()
    {
        RespawnPlayer();
        points += 10;

        float ranValue = Random.Range(1, 4);
        if (ranValue == 3)
        {
            foreach (var obj in itemsManager.Holders)
            {
                if (obj.transform.childCount == 0)
                {
                    Instantiate(itemsManager.Items[Random.Range(0, itemsManager.Items.Length)], obj.transform.position, Quaternion.identity);
                }
                else 
                {
                    Destroy(obj.transform.GetChild(0).gameObject);
                    Instantiate(itemsManager.Items[Random.Range(0, itemsManager.Items.Length)], obj.transform.position, Quaternion.identity);
                }
            }
        }

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
            if (BlockBreakEffect != null)
            {
                GameObject breakEffect = Instantiate(BlockBreakEffect, obj.transform.position, Quaternion.identity);
                breakEffect.GetComponent<ParticleSystem>().startColor = obj.GetComponent<BlockColor>()._breakColor;
            }
            spawnedObjects.Remove(obj);
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

            starObject = Instantiate(StarEffect, transform.position, Quaternion.identity, transform);

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
        ChooseNextPrefab();

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
       
        prefabFirstImage.UpdateImage();
        prefabSecondImage.UpdateImage();
    }


    //Spawn prefab at player position
    private void SpawnPrefab()
    {
        GameObject instance = Instantiate(nextPrefab);
        instance.transform.position = rb.position;
        StartCoroutine(ScalePrefab(instance,.2f));

        spawnedObjects.Add(instance);


    }

    private IEnumerator ScalePrefab(GameObject instance, float duration)
    {
        float currentTime = 0;

        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float scale = Mathf.Lerp(0, 1, currentTime / duration);
            instance.transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
        instance.transform.localScale = new Vector3(1, 1, 1);
    }
    

    private void ChooseNextPrefab()
    {
        nextPrefab = nextNextPrefab != null? nextNextPrefab : prefabs[Random.Range(0, prefabs.Length)];
        nextNextPrefab = prefabs[Random.Range(0, prefabs.Length)];
    }

    private void RespawnPlayer()
    {
        rb.position = startPos;

        //After reaching goal for a short time player destroys objects at respawn point
        isRespawnDestroy = true;
        destroyRadius.enabled = true;

        StartCoroutine(StopRespawnDestroy());
    }

    private bool IsGrounded()
    {

        if (Physics2D.Raycast(transform.position, Vector2.down, 0.8f, groundLayer.value))
        {
            if (starObject != null)
            {
                Destroy(starObject);
            }
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

    IEnumerator StopRespawnDestroy()
    {
        yield return new WaitForSeconds(0.1f);

        isRespawnDestroy = false;
        destroyRadius.enabled = false;
    }
}
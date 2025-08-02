using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    //Direction into which cannon shoots bullets
    public Vector2 dir;

    //wait time to shoot next bullet
    public float timeBetweenShots;

    //speed of shot bullet
    public float bulletSpeed;

    public GameObject bulletPrefab;

    bool isWaitingForNextShot = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitForNextShot());
    }

    // Update is called once per frame
    void Update()
    {
        if(!isWaitingForNextShot) {
            Shoot();
        }
        
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.position = new Vector2 (transform.position.x + dir.x, transform.position.y + dir.y);
        bulletRb.linearVelocity = new Vector2(dir.x *bulletSpeed, dir.y *bulletSpeed);

        StartCoroutine(WaitForNextShot());
    }

    IEnumerator WaitForNextShot()
    {
        isWaitingForNextShot = true;

        yield return new WaitForSeconds(timeBetweenShots);
        isWaitingForNextShot = false;
    }
}

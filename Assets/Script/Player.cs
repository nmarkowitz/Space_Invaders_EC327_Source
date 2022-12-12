using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;

    public float speed = 5.0f;

    private bool _laserActive;

    private void Update()
    {
        // GetKey vs GetKeyDown
        // GetKey returns true for every frame where the key is pressed down
        // GetKeyDown returns true only the first frame where key is pressed

        // player moving left or right
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            // position updates to direction times speed times frame time
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
            this.transform.position += Vector3.up * this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
            this.transform.position += Vector3.down * this.speed * Time.deltaTime;
        }

        // shooting laser
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!_laserActive){
            // instantiate(prefab, position, rotation)
            // identity: no rotation
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            
            // call LaserDestroyed when delegate destroyed is invoked
            projectile.destroyed += LaserDestroyed;
            _laserActive = true;
        }

    }

    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") || 
            other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

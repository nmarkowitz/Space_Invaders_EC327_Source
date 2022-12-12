using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    // arrays of prefabs that corresponds to each rows
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public float spacing = 2.0f;

    // AnimationCurve: x y graph, x is the percentage and y is speed
    public AnimationCurve speed;

    // missile prefab
    public Projectile missilePrefab;

    // speed of missile launch
    public float missileAttackRate = 1.0f;

    public int amountKilled {get; private set; }
    public int amountAlive => this.totalInvaders - amountKilled;
    public int totalInvaders => this.rows * this.columns;
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;

    // direction of which the enemies are moving
    private Vector3 _direction = Vector2.right;
    private void Awake()
    {
        for (int row = 0; row < this.rows; row++)
        {
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * spacing), 0.0f);
            for (int col = 0; col < this.columns; col++)
            {
                // this.prefabs[row]: prefab that we are instantiating from
                // provide the transform of the invader we are parenting the new invader to
                // transform of an object is used to store and manipulate the position
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                // call function when killed is invoked
                invader.killed += InvaderKilled;


                Vector3 position = rowPosition;
                position.x += col * spacing;
                // set to localPosition to set it relative to the parent class (invaders)
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void Update()
    {
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        //getting left and right edges of the screen
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        // looping through each child object
        foreach (Transform invader in this.transform)
        {
            // activeInHierarchy checks for if invader is active in the current hierarchy
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            float padding = 1.0f;
            // if moving to the right and the position of the invader is greater than the right edge of the screen
            if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - padding))
            {
                AdvanceRow();
            } else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + padding))
            {
                AdvanceRow();
            }
        }
    }

    // change the direction of the enemines
    private void AdvanceRow()
    {
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void MissileAttack()
    {
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            // chances of invaders launch a missile increases as the number of invaders decrease
            if (Random.value < (1.0f / (float) this.amountAlive)) 
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                // one shot at a time
                break;
            }
        }
    }

    private void InvaderKilled()
    {
        this.amountKilled++;
        if (this.amountKilled >= this.totalInvaders)
        {
            // reload screen
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

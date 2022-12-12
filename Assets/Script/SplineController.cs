// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplineController : MonoBehaviour
{
    // shows private variable's value on inspecter without allowing other scripts to access it
    // [SerializeField] private Transform pointA;
    // [SerializeField] private Transform pointB;
    // [SerializeField] private Transform pointC;
    // [SerializeField] private Transform pointD;
    // [SerializeField] private Transform pointABCD;

    public Invader prefab;
    // public int totalInvaders = 4;
    public float speed = 1.0f;
    
    public Vector3 PointA {get; private set; }
    public Vector3 PointB {get; private set; }
    public Vector3 PointC {get; private set; }
    public Vector3 PointD {get; private set; }

    private float xPosB;
    private float yPosB;
    private float xPosC;
    private float yPosC;

    // // missile prefab
    // public Projectile missilePrefab;

    // // invader prefab
    // public Invader[] prefabs;

    // // // speed of missile launch
    // // public float missileAttackRate = 1.0f;

    // // public for get, but private for changing value
    // public int amountKilled {get; private set; }
    // public int amountAlive => this.totalInvaders - amountKilled;
    // public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;

    private float timer = 0.0f;
    private float timeToMove = 2.5f;
    private float timerSpeed = 0.2f;
    private float interpolateAmount;

    // private void Start()
    // {
    //     InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    // }

    private void Awake()
    {

        // for(int i = 0; i < totalInvaders; i++)
        // {
            Invader invader = Instantiate(this.prefab, this.transform);
            invader.killed += InvaderKilled;
        // }
        
        xPosB = Random.Range(-15.0f, 15.0f);
        yPosB = Random.Range(-15.0f, 15.0f);

        xPosC = Random.Range(-15.0f, 15.0f);
        yPosC = Random.Range(-15.0f, 15.0f);

        PointA = GetEdgePoint();
        PointB = new Vector3(xPosB, yPosB, 0.0f);
        PointC = new Vector3(xPosC, yPosC, 0.0f);
        PointD = GetEdgePoint();
    
    }

    private void Update() {

        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            timer += Time.deltaTime * timerSpeed;
            if (timer >= timeToMove)
            {
                xPosB = Random.Range(-15.0f, 15.0f);
                yPosB = Random.Range(-15.0f, 15.0f);

                xPosC = Random.Range(-15.0f, 15.0f);
                yPosC = Random.Range(-15.0f, 15.0f);

                PointA = GetEdgePoint();
                PointD = GetEdgePoint();
                PointB = new Vector3(xPosB, yPosB, 0.0f);
                PointC = new Vector3(xPosC, yPosC, 0.0f);
                timer = 0.0f;
            }
                interpolateAmount = (interpolateAmount + (this.speed / 50.0f) * Time.deltaTime) % 1f;
                invader.position = CubicLerp(PointA, PointB, PointC, PointD, interpolateAmount);
        
                // Debug.Log("inside loop: " + Vector3.Distance(transform.position, PointD));
                // if (Vector3.Distance(transform.position, PointD) <= 0.5f)
                // {
                    
                //     // timer = 0.0f;
                // }
                
            // }
            
        }

        /*
        // Lerp:
        // when interAmount = 0, AB is at pointA, when 1, AB is at pointB
        // when interAmount between 0 and 1, pointAB is between point A and point B
        pointAB.position = Vector3.Lerp(pointA.position, pointB.position, interpolateAmount);
        */
        //     // increase as time goes
        //     // % 1f allows the value to reset to 0 when reaches 1
        //     interpolateAmount = (interpolateAmount + (this.speed / 10.0f) * Time.deltaTime) % 1f;
        //     invader.position = CubicLerp(pointA.position, pointB.position, pointC.position, pointD.position, interpolateAmount);

        // }
        // invader.position = CubicLerp(pointA.position, pointB.position, pointC.position, pointD.position, interpolateAmount);
    }


    private Vector3 GetEdgePoint() {
        float xPos = Random.Range(-15.0f, 15.0f);
        float yPos = Random.Range(-15.0f, 15.0f);

        float diff_xy = System.Math.Abs(xPos) - System.Math.Abs(yPos);
        if (diff_xy >= 0)
        {
            if(xPos >= 0){
                Vector3 point = new Vector3(16.0f, yPos, 0.0f);
                return point;
            } else {
                Vector3 point = new Vector3(-16.0f, yPos, 0.0f);
                return point;
            }
        } 
        else
        {
            if(yPos >= 0){
                Vector3 point = new Vector3(xPos, 16.0f, 0.0f);
                return point;
            } else {
                Vector3 point = new Vector3(xPos, -16.0f, 0.0f);
                return point;
            }
        }
    }

    // if not make sense:
    // https://www.youtube.com/watch?v=7j_BNf9s0jM
    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t) {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        
        return Vector3.Lerp(ab, bc, this.interpolateAmount);
    }

    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t) {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);

        return Vector3.Lerp(ab_bc, bc_cd, this.interpolateAmount);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Boundry")){

            PointA = GetEdgePoint();
            PointD = GetEdgePoint();
            Debug.Log("Boundry Triggered");
    //         // triggered when invader is killed so invaders can get updated
    //         // this.killed.Invoke();
    //         // this.gameObject.SetActive(false);
    //         this.gameObject.SetActive(false);
        }
        
    }

    private void InvaderKilled()
    {
        // reload screen
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

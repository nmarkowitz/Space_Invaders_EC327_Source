using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Requirements
- Spawn semi-randomly
- Rotate while moving through space (Transform)
- Damage on hit
*/

public class PowerUp : MonoBehaviour
{

    public Vector3 direction;
    private bool _powerupActive = false;
    public float speed = 5.0f;
    // some sort of c sharp event, or not, no idea what it is
    // c sharp delegate
    public System.Action destroyed;

    // Update is called once per frame
    void Update()
    {
        //getting left and right edges of the screen
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // Create a random value
        //int rand_num = Range.Range(0,20);

        // If the number is 15, spawn a object
        // if (rand_num == 15)
        // {
            
        // }

        //this.transform.position += this.direction * this.speed * Time.deltaTime;
        //this.transform.rotation.z += this.speed * Time.deltaTime;
    }
}

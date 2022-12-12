using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;

    public float speed;
    // some sort of c sharp event, or not, no idea what it is
    // c sharp delegate
    public System.Action destroyed;

    private void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    // unity auto calls this function
    // when projectile collides with something it gets destroyed
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.destroyed != null){
            // event invoked when collision happens
            // allow other scripts to register when an event has happened
            this.destroyed.Invoke();
        }
        Destroy(this.gameObject);
    }
}

// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;


// for invader animation
public class Invader : MonoBehaviour
{
    public Sprite[]  animationSprites;
    public float animationTime = 1.0f;

    // delegate to keep track of the killing
    public System.Action killed;

    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;

    //first function unity calls
    private void Awake(){
        // looks for the component specified under the same gameobject that Invader is attached to
        // in this case, it is looking for SpriteRenderer
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // once program is awake, this function is called
    private void Start()
    {
        // invoke a program after x amount of time
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        _animationFrame++;

        // prevent frame from exceeding the number of frames provided
        if(_animationFrame >= this.animationSprites.Length){
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")){

            // triggered when invader is killed so invaders can get updated
            this.killed.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}

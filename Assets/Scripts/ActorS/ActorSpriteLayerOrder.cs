using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class ActorSpriteLayerOrder : MonoBehaviour
{
    Collider2D spriteCollider;
    SpriteRenderer sprite;


    Collider2D otherCollider;
    SpriteRenderer otherSprite;


    void Awake()
    {
        spriteCollider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        SpriteRenderer _otherSprite;
        if (other.TryGetComponent<SpriteRenderer>(out _otherSprite))
        {
            otherSprite = _otherSprite;
            otherCollider = other;
        }
    }

    void Update()
    {
        if (otherSprite != null && otherCollider != null)
        {
            if (transform.position.y > otherCollider.transform.position.y)
            {
                if (sprite.sortingOrder > otherSprite.sortingOrder)
                {
                    sprite.sortingOrder = otherSprite.sortingOrder - 1;
                }
            }
            else
            {
                if (sprite.sortingOrder <= otherSprite.sortingOrder)
                {
                    sprite.sortingOrder = otherSprite.sortingOrder + 1;
                }
            }
        }
    }
}

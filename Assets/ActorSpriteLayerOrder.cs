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

    /* Collider2D tilemapCollider;
    TilemapRenderer tileMap; */
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
            //Debug.Log(other.name);
            otherSprite = _otherSprite;
            otherCollider = other;
        }

        /* 
        TilemapRenderer _tileMap;
        if (other.TryGetComponent<TilemapRenderer>(out _tileMap))
        {
            tilemapCollider = other;
            tileMap = _tileMap;
        } */
    }

    void Update()
    {
        if (otherSprite != null && otherCollider != null)
        {
            //Debug.Log("debug");
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
        /*
        if (tileMap != null && tilemapCollider != null)
        {
            if (transform.position.y > tilemapCollider.transform.position.y && sprite.sortingOrder > tileMap.sortingOrder)
            {
                sprite.sortingOrder = tileMap.sortingOrder - 1;
            }
            else
            {
                sprite.sortingOrder = tileMap.sortingOrder + 1;
            }
        }
        */
    }
}

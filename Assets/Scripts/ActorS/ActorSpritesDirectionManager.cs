using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSpritesDirectionManager : MonoBehaviour
{
    public Animator animator;
    public enum spriteDirection
    {
        none,
        N,
        W,
        S,
        E,
        NW,
        NE,
        SW,
        SE
    }
    public spriteDirection currentSpriteDirection = spriteDirection.N;
    List<Vector2> cardinalDirections = new List<Vector2>();

    public enum spriteAction
    {
        idle,
        walking
    }
    public spriteAction currentSpriteAction = spriteAction.idle;
    private void OnEnable()
    {
        EventDirector.somebody_UpdateSpriteDirection += UpdateDirection;
    }
    private void OnDisable()
    {
        EventDirector.somebody_UpdateSpriteDirection -= UpdateDirection;
    }
    private void Start()
    {
        cardinalDirections.Add(Vector2.up);
        cardinalDirections.Add(Vector2.left);
        cardinalDirections.Add(Vector2.down);
        cardinalDirections.Add(Vector2.right);
        cardinalDirections.Add((Vector2.up + Vector2.left).normalized);
        cardinalDirections.Add((Vector2.up + Vector2.right).normalized);
        cardinalDirections.Add((Vector2.down + Vector2.left).normalized);
        cardinalDirections.Add((Vector2.down + Vector2.right).normalized);
    }

    private void Update()
    {
        SelectAnimation();
    }

    public void UpdateDirection(Transform who, Vector2 direction, spriteAction action)
    {
        if (who == transform)
        {
            currentSpriteAction = action;

            if (direction != Vector2.zero)
            {
                int whichDir = 0;
                float minimalAngle = 360f;

                int i = 0;
                foreach (Vector2 dir in cardinalDirections)
                {
                    float angle = Mathf.Abs(Vector2.Angle(direction, dir));
                    if (angle < minimalAngle)
                    {
                        whichDir = i;
                        minimalAngle = angle;
                    }
                    ++i;
                }

                switch (whichDir)
                {
                    default:
                        break;
                    case 0:
                        currentSpriteDirection = spriteDirection.N;
                        break;
                    case 1:
                        currentSpriteDirection = spriteDirection.W;
                        break;
                    case 2:
                        currentSpriteDirection = spriteDirection.S;
                        break;
                    case 3:
                        currentSpriteDirection = spriteDirection.E;
                        break;
                    case 4:
                        currentSpriteDirection = spriteDirection.NW;
                        break;
                    case 5:
                        currentSpriteDirection = spriteDirection.NE;
                        break;
                    case 6:
                        currentSpriteDirection = spriteDirection.SW;
                        break;
                    case 7:
                        currentSpriteDirection = spriteDirection.SE;
                        break;
                }

            }
        }
    }

    void SelectAnimation()
    {
        if (currentSpriteDirection != spriteDirection.none)
        {
            string stateName = "Player_Idle_" + currentSpriteDirection.ToString();
            if (currentSpriteAction == spriteAction.walking)
            {
                stateName = "Player_Walk_" + currentSpriteDirection.ToString();
            }
            if (currentSpriteAction == spriteAction.idle)
            {
                stateName = "Player_Idle_" + currentSpriteDirection.ToString();
            }

            animator.Play(stateName);
        }
    }    
    

}

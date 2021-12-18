using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimationManager : MonoBehaviour
{
    public AnimatorOverrideController thisActorAnim;
    public Animator animator;

    public enum spriteDirection
    {
        N,
        W,
        S,
        E,
        NW,
        NE,
        SW,
        SE
    }
    public spriteDirection currentSpriteDirection = spriteDirection.S;
    List<Vector2> cardinalVectors = new List<Vector2>();

    public enum spriteAction
    {
        idle,
        walking,
        dead
    }
    public spriteAction currentSpriteAction;// = spriteAction.idle;

    private void OnEnable()
    {
        EventDirector.somebody_UpdateSpriteVector += UpdateVector;
        EventDirector.somebody_UpdateSpriteDirection += UpdateDirection;
        EventDirector.somebody_UpdateSpriteAction += UpdateAction;

    }
    private void OnDisable()
    {
        EventDirector.somebody_UpdateSpriteVector -= UpdateVector;
        EventDirector.somebody_UpdateSpriteDirection -= UpdateDirection;
        EventDirector.somebody_UpdateSpriteAction -= UpdateAction;
    }

    private void Start()
    {
        animator.runtimeAnimatorController = thisActorAnim;

        cardinalVectors.Add(Vector2.up);
        cardinalVectors.Add(Vector2.left);
        cardinalVectors.Add(Vector2.down);
        cardinalVectors.Add(Vector2.right);
        cardinalVectors.Add((Vector2.up + Vector2.left).normalized);
        cardinalVectors.Add((Vector2.up + Vector2.right).normalized);
        cardinalVectors.Add((Vector2.down + Vector2.left).normalized);
        cardinalVectors.Add((Vector2.down + Vector2.right).normalized);
    }
    private void Update()
    {
        UpdateAnimation();
    }
    public void UpdateAction(Transform who, spriteAction action)
    {
        if (who == transform)
        {
            currentSpriteAction = action;
        }
    }
    public void UpdateDirection(Transform who, spriteDirection direction)
    {
        if (who == transform)
        {
            currentSpriteDirection = direction;
        }
    }
    public void UpdateVector(Transform who, Vector2 direction)
    {
        if (who == transform)
        {
            if (direction != Vector2.zero)
            {
                int whichDir = 0;
                float minimalAngle = 360f;

                int i = 0;
                foreach (Vector2 dir in cardinalVectors)
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

    void UpdateAnimation()
    {
        string stateName = "Actor_Idle_" + currentSpriteDirection.ToString();
        if (currentSpriteAction == spriteAction.walking)
        {
            stateName = "Actor_Walk_" + currentSpriteDirection.ToString();
        }
        if (currentSpriteAction == spriteAction.idle)
        {
            stateName = "Actor_Idle_" + currentSpriteDirection.ToString();
        }
        if (currentSpriteAction == spriteAction.dead)
        {
            stateName = "Actor_Dead_" + currentSpriteDirection.ToString();
        }

        animator.Play(stateName);
    }
}

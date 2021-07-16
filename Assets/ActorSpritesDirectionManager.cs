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
    List<Vector3> cardinalDirections = new List<Vector3>();
    private void Start()
    {
        cardinalDirections.Add(Vector3.up);
        cardinalDirections.Add(Vector3.left);
        cardinalDirections.Add(Vector3.down);
        cardinalDirections.Add(Vector3.right);
        cardinalDirections.Add((Vector3.up + Vector3.left).normalized);
        cardinalDirections.Add((Vector3.up + Vector3.right).normalized);
        cardinalDirections.Add((Vector3.down + Vector3.left).normalized);
        cardinalDirections.Add((Vector3.down + Vector3.right).normalized);
    }

    private void Update()
    {
        SelectAnimation();
    }

    public void UpdateDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Vector3 closestDirection = Vector3.zero;
            int whichDir = 0;
            float minimalAngle = 360f;

            int i = 0;
            foreach (Vector3 dir in cardinalDirections)
            {
                float angle = Mathf.Abs(Vector3.Angle(direction, dir));
                if (angle < minimalAngle)
                {
                    whichDir = i;
                    minimalAngle = angle;
                    print(closestDirection);
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

    void SelectAnimation()
    {
        if (currentSpriteDirection != spriteDirection.none)
        {
            bool isCharacterWalking = true;
            string stateName = "Player_Walk_" + currentSpriteDirection.ToString();


            if (isCharacterWalking)
            {
                animator.Play(stateName);
            }
        }
    }    
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ActorAnimManager : MonoBehaviour
{
    #region DEPENDENCIES
    ActorMovement actorMovement;

    #endregion

    #region References
    AnimLib animLib;
    AnimPlayer animPlayer;
    public SpriteRenderer actorSprite;
    #endregion

    [SerializeField]
    string playOnEnable = "";
    [SerializeField] bool isLooping = true;



    List<Vector2> cardinalVectors = new List<Vector2>();
    public enum spriteDirection
    {
        S, SW, W, NW, N, NE, E, SE
    }
    spriteDirection currentSpriteDirection = spriteDirection.S;

    string currentSpriteAction;


    private void Awake()
    {
        animLib = actorSprite.GetComponent<AnimLib>();
        animPlayer = actorSprite.GetComponent<AnimPlayer>();
        actorMovement = GetComponent<ActorMovement>();


        cardinalVectors.Add(Vector2.up);
        cardinalVectors.Add(Vector2.left);
        cardinalVectors.Add(Vector2.down);
        cardinalVectors.Add(Vector2.right);
        cardinalVectors.Add((Vector2.up + Vector2.left).normalized);
        cardinalVectors.Add((Vector2.up + Vector2.right).normalized);
        cardinalVectors.Add((Vector2.down + Vector2.left).normalized);
        cardinalVectors.Add((Vector2.down + Vector2.right).normalized);

    }
    void OnEnable()
    {
        if (playOnEnable != "")
        {
            UpdateAnimation(playOnEnable, _looping: isLooping);
        }

    }

    public void UpdateAnimation(string _action, Vector2? _direction = null, bool _looping = true, bool waitTillSeqEnd = false)
    {
        UpdateDirection(_direction);
        UpdateAction(_action);
        isLooping = _looping;

        List<AnimFrame> sequence = animLib.FindSequence(currentSpriteAction, currentSpriteDirection.ToString());
        if (sequence != null)
        {
            animPlayer.UpdateData(
                sequence,
                animLib.FindStateFrameLength(currentSpriteAction),
                isLooping,
                waitTillSeqEnd);
        }

    }
    void UpdateAction(string action)
    {
        action = action.ToLower();
        currentSpriteAction = action;
    }
    void UpdateDirection(Vector2? _direction)
    {
        if (_direction == null)
        {
            currentSpriteDirection = spriteDirection.S;
        }
        else
        {
            int x = cardinalVectors.Count();

            if (_direction.Value != Vector2.zero)
            {
                int whichDir = 0;
                float minimalAngle = 180f;

                int i = 0;
                foreach (Vector2 dir in cardinalVectors)
                {
                    float angle = Vector2.Angle(_direction.Value, dir);
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

}

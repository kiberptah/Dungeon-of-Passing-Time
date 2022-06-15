using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ActorAnimManager : MonoBehaviour
{
    #region References
    AnimLib animLib;
    AnimPlayer animPlayer;
    public SpriteRenderer actorSprite;
    #endregion

    [SerializeField] string playOnEnable = "";
    [SerializeField] bool isLooping = true;

    List<Vector3> cardinalVectors = new List<Vector3>();

    public enum spriteDirection
    {
        S, SW, W, NW, N, NE, E, SE
    }
    spriteDirection currentSpriteDirection = spriteDirection.S;

    string currentSpriteAction;

    #region Initialization
    private void Awake()
    {
        animLib = actorSprite.GetComponent<AnimLib>();
        animPlayer = actorSprite.GetComponent<AnimPlayer>();

        cardinalVectors.Add(Vector3.forward);
        cardinalVectors.Add(Vector3.left);
        cardinalVectors.Add(Vector3.back);
        cardinalVectors.Add(Vector3.right);
        cardinalVectors.Add((Vector3.forward + Vector3.left).normalized);
        cardinalVectors.Add((Vector3.forward + Vector3.right).normalized);
        cardinalVectors.Add((Vector3.back + Vector3.left).normalized);
        cardinalVectors.Add((Vector3.back + Vector3.right).normalized);
    }
    void OnEnable()
    {
        if (playOnEnable != "")
        {
            UpdateAnimation(playOnEnable, _looping: isLooping);
        }
    }
    #endregion



    public void UpdateAnimation(string _action, Vector3? _direction = null, bool _looping = true, bool waitTillSeqEnd = false)
    {
        UpdateDirection(_direction);
        UpdateAction(_action);
        isLooping = _looping;

        List<AnimFrame> sequence = animLib.FindSequence(currentSpriteAction, currentSpriteDirection.ToString());
        if (sequence != null)
        {
            animPlayer.UpdateData(
                sequence,
                animLib.FindStateEvents(currentSpriteAction),
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
    void UpdateDirection(Vector3? _direction)
    {
        if (_direction == null)
        {
            currentSpriteDirection = spriteDirection.S;
        }
        else
        {
            int x = cardinalVectors.Count();

            if (_direction.Value != Vector3.zero)
            {
                int whichDir = 0;
                float minimalAngle = 180f;

                int i = 0;
                foreach (Vector3 dir in cardinalVectors)
                {
                    float angle = Vector3.Angle(_direction.Value, dir);
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

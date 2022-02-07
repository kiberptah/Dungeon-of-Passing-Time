using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStuffAssigning : MonoBehaviour
{
    public PlayerInput playerInput;
    public CustomMouseCursorScript cursor;

    void Start()
    {
        cursor.player = playerInput.actorControllerConnector.transform;
    }
}

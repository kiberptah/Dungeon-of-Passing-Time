using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStuffAssigning : MonoBehaviour
{
    public PlayerController playerController;
    public CustomMouseCursorScript cursor;
    //public Camera characterCamera;
    void Awake()
    {
        cursor.player = playerController.actorConnector;
        //cursor.player = characterCamera.transform;
        playerController.cursorScript = cursor;
    }

    private void OnEnable()
    {
        //EventDirector.somebody_TakeDamage += SlowTimeOnHit;
        EventDirector.somebody_TookDamage += PlayerTakesDamage;
        EventDirector.somebody_TookDamage += PlayerDealsDamage;
    }
    private void OnDisable()
    {
        //EventDirector.somebody_TakeDamage -= SlowTimeOnHit;
        EventDirector.somebody_TookDamage -= PlayerTakesDamage;
        EventDirector.somebody_TookDamage -= PlayerDealsDamage;
    }

    void PlayerTakesDamage(Transform who, float amount, Transform fromWhom)
    {
        if (who == playerController.actorConnector.transform)
        {
            EventDirector.player_TookDamage?.Invoke();
        }
    }
    void PlayerDealsDamage(Transform who, float amount, Transform fromWhom)
    {
        if (fromWhom == playerController.actorConnector.transform)
        {
            EventDirector.player_DealtDamage?.Invoke();
        }

    }

}

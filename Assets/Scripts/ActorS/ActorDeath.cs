using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorDeath : MonoBehaviour
{
    #region DEPENDENCIES
    ActorHealth actorHealth;
    #endregion
    [SerializeField] GameObject corpse;
    void Awake()
    {
        actorHealth = GetComponent<ActorHealth>();
    }
    void OnEnable()
    {
        actorHealth.noHealth += DeathAndKnockback;
    }
    void OnDisable()
    {
        actorHealth.noHealth -= DeathAndKnockback;
    }

    void DeathAndKnockback(float dmg, float kb, Transform killer)
    {
        gameObject.SetActive(false);
        GameObject _corpse = Instantiate(corpse, transform.position, Quaternion.identity);

        Corpse corpseScript = _corpse.GetComponent<Corpse>();

        corpseScript.animLib.libData = GetComponent<ActorAnimManager>().actorSprite.GetComponent<AnimLib>().libData;
        corpseScript.Initialize(transform, dmg, kb, killer);
    }
}

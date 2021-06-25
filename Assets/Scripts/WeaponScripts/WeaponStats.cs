using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [System.Serializable]
    public class Attack
    {
        public string attackName;
        public float damage;
        public float damageTickDelay;
        public float coolDownSeconds;


        public enum typeOfDamage
        {
            physical,
            magical
        }
        public typeOfDamage damageType;
        public enum attackWithType
        {
            slash,
            pierce,
            throwing
        }
        public attackWithType attackType;

        public Collider2D attackCollider;
    }

    public Attack[] attack;

    
    private void Awake()
    {
        foreach (Attack a in attack)
        {
            a.attackCollider.enabled = false;
        }
    }
    
}

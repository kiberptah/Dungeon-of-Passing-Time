using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSlime_Attack : MonoBehaviour
{
    [SerializeField] float flightDuration = .1f;
    [SerializeField] float flightSpeed = 10f;
    [SerializeField] GameObject flyingSlimePrefab;
    Transform target;
    //[SerializeField] Transform particlesSystem;

    ActorAnimManager animManager;
    ActorMovement actorMovement;

    Vector3 jumpDirection = Vector3.zero;
    private void Awake()
    {
        //Debug.Log("awake");
        animManager = GetComponent<ActorAnimManager>();
        actorMovement = GetComponent<ActorMovement>();

    }
    public void StartAttack(Transform _target)
    {
        actorMovement.enabled = false;

        target = _target;
        jumpDirection = target.position - transform.position;

        //Debug.Log("dist " + Vector3.Distance(flyingSlime.transform.position, transform.position));
        animManager.UpdateAnimation("Attack_Jumping", _direction: jumpDirection, _looping: false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            gameObject.SetActive(false);

        }
    }

    public void SpawnFlyingForm()
    {
        /// this func is called from animation event
        /// 

        Vector3 spawnDirection = transform.position + (jumpDirection).normalized;

        GameObject flyingSlime = Instantiate(flyingSlimePrefab, spawnDirection, Quaternion.identity);
        flyingSlime.GetComponent<PSlime_FlyingForm>().Initialize(jumpDirection, flightDuration, flightSpeed, this);
        transform.gameObject.SetActive(false);
        //particlesSystem.parent = null;
        //particlesSystem.gameObject.SetActive(true);

    }

    public void Land(Vector3 direction)
    {
        //particlesSystem.parent = transform;
        //particlesSystem.transform.localPosition = Vector3.zero;
        animManager.UpdateAnimation("Attack_Landing", direction, false);
    }

    public void FinishAttack()
    {
        actorMovement.enabled = true;
    }
}

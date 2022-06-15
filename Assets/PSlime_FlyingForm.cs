using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSlime_FlyingForm : MonoBehaviour
{
    Vector3 direction = Vector3.zero;
    float flightDuration = 1f;
    float flightSpeed = 1f;
    PSlime_Attack originalForm;

    [SerializeField] Rigidbody rb;
    [SerializeField] ActorAnimManager animManager;
    public void Initialize(Vector3 direction, float flightDuration, float flightSpeed, PSlime_Attack originalForm)
    {
        this.direction = direction.normalized;
        this.flightDuration = flightDuration;
        this.flightSpeed = flightSpeed;
        this.originalForm = originalForm;

        rb.AddRelativeForce(this.direction * flightSpeed, ForceMode.VelocityChange);
        animManager.UpdateAnimation("Attack_Flying", direction);

        StartCoroutine(Flight());
    }

    IEnumerator Flight()
    {
        yield return new WaitForSeconds(flightDuration);
        originalForm.transform.position = transform.position + direction;
        originalForm.gameObject.SetActive(true);
        originalForm.Land(direction * 0.5f);

        Destroy(gameObject);
    }
}

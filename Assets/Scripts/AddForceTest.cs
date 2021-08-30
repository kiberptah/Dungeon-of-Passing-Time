using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceTest : MonoBehaviour
{
    public float force = 10f;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            transform.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.right * force, ForceMode2D.Impulse);
            print("addforce " + Vector3.right * force);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            transform.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.left * force, ForceMode2D.Impulse);
            print("addforce " + Vector3.left * force);

        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            transform.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.up * force, ForceMode2D.Impulse);
            print("addforce " + Vector3.up * force);

        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            transform.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.down * force, ForceMode2D.Impulse);
            print("addforce " + Vector3.down * force);

        }

    }
}

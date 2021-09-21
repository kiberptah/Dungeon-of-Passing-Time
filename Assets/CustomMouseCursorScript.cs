using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMouseCursorScript : MonoBehaviour
{

    public Transform cursorObject;
    public Transform player;
    Vector3 previousPlayerPosition;

    Vector3 deltaPlayerPosition;

    public float range = 3f;
    public float sensitivityScale = 1f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, player.position.z);
        //cursorObject.position = player.position;

        previousPlayerPosition = player.position;
    }
    void FixedUpdate()
    {
        cursorObject.position
                = new Vector3(cursorObject.position.x + Input.GetAxis("Mouse X") * sensitivityScale,
                cursorObject.position.y + Input.GetAxis("Mouse Y") * sensitivityScale, 0);
        Debug.Log("Input.GetAxis(Mouse X) " + Input.GetAxis("Mouse X"));
        Debug.Log("Input.GetAxis(Mouse Y) " + Input.GetAxis("Mouse Y"));

        /*
         * use if cursor is player's child
        if (cursorObject.localPosition.magnitude >= range)
        {
            cursorObject.localPosition = cursorObject.localPosition.normalized * range;
            //cursorObject.position = new Vector3(cursorObject.position.x, cursorObject.position.y, 0);
        }
        */

        if (Vector3.Distance(cursorObject.position, player.position) >= range)
        {
            cursorObject.position = player.position + (cursorObject.position - player.position).normalized * range;
            //cursorObject.position = new Vector3(cursorObject.position.x, cursorObject.position.y, 0);
        }

    }

    private void LateUpdate()
    {
        deltaPlayerPosition = player.position - previousPlayerPosition;

        cursorObject.position += deltaPlayerPosition;


        previousPlayerPosition = player.position;
    }
}

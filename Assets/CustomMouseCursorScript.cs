using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMouseCursorScript : MonoBehaviour
{

    public Transform cursorObject;
    public Transform player;

    Vector3 previousMousePosition;
    Vector3 deltaMousePosition;


    Vector3 previousPlayerPosition;
    Vector3 deltaPlayerPosition;

    Vector3 previousCameraPosition;
    Vector3 deltaCameraPosition;

    public float range = 3f;
    public float sensitivityScale = 1f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, player.position.z);
        //cursorObject.position = player.position;

        previousPlayerPosition = player.position;
    }
    void Update()
    {
        GetPlayerDelta();
        GetCameraDelta();
        GetMouseDelta();

        cursorObject.position += deltaMousePosition - deltaCameraPosition + deltaPlayerPosition;
        /*
        cursorObject.position
                = new Vector3(cursorObject.position.x + Input.GetAxis("Mouse X") * sensitivityScale,
                cursorObject.position.y + Input.GetAxis("Mouse Y") * sensitivityScale, 0);
        Debug.Log("Input.GetAxis(Mouse X) " + Input.GetAxis("Mouse X"));
        Debug.Log("Input.GetAxis(Mouse Y) " + Input.GetAxis("Mouse Y"));
        */
        /*
         * use if cursor is player's child
        if (cursorObject.localPosition.magnitude >= range)
        {
            cursorObject.localPosition = cursorObject.localPosition.normalized * range;
            //cursorObject.position = new Vector3(cursorObject.position.x, cursorObject.position.y, 0);
        }
        */ 

        if (Vector3.Distance(cursorObject.position, player.position) > range)
        {
            cursorObject.position = player.position + (cursorObject.position - player.position).normalized * range;
            //cursorObject.position = new Vector3(cursorObject.position.x, cursorObject.position.y, 0);
        }


        
    }

    void GetPlayerDelta()
    {
        deltaPlayerPosition = player.position - previousPlayerPosition;
        previousPlayerPosition = player.position;
    }
    void GetCameraDelta()
    {
        deltaCameraPosition = Camera.main.transform.position - previousCameraPosition;
        previousCameraPosition = Camera.main.transform.position;
    }
    void GetMouseDelta()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        deltaMousePosition = mousePosition - previousMousePosition;

        
        //print(deltaMousePosition);

        previousMousePosition = mousePosition;
    }
}

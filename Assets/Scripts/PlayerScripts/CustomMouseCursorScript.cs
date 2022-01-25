using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CustomMouseCursorScript : MonoBehaviour
{
    public Transform cursorObject;
    public Transform player;


    Vector3 deltaMousePosition;

    Vector3 previousPlayerPosition;
    Vector3 deltaPlayerPosition;

    Vector3 previousCameraPosition;
    Vector3 deltaCameraPosition;


    [Header("Tuning")]
    [SerializeField] bool isCursorLimitedToThePlayer = true;
    public float range = 3f;
    public float sensitivityScale = 1f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        previousPlayerPosition = player.position;
    }



    void Update()
    {
        GetPlayerDelta();
        //GetCameraDelta();
        GetMouseDelta();


        cursorObject.position += deltaMousePosition + deltaPlayerPosition;


        if (isCursorLimitedToThePlayer) // cursor distance limit can be disabled
        {
            if (Vector3.Distance(cursorObject.position, player.position) > range)
            {
                cursorObject.position = player.position + (cursorObject.position - player.position).normalized * range;
            }
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
        deltaMousePosition = sensitivityScale * new Vector2(Input.GetAxis("Mouse X") * Time.deltaTime, Input.GetAxis("Mouse Y") * Time.deltaTime);
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class CustomMouseCursorScript : MonoBehaviour
{
    public PlayerInput playerInput;

    [Header("Assign Manually")]
    public Transform cursorObject;
    public SpriteRenderer cursorSprite;
    float defaultAlpha = 1f;
    [Header("Do Not Assign")]
    public Transform player;


    Vector2 deltaMousePosition;

    Vector3 previousPlayerPosition;
    Vector2 deltaPlayerPosition;


    InputAction cursorDelta;
    Vector2 cursorPosRaw;
    InputAction cursorPosition;


    Vector3 previousCameraPosition;
    Vector3 deltaCameraPosition;


    [Header("Tuning")]
    [SerializeField] bool isCursorLimitedToThePlayer = true;
    public float rangeLimit = 3f;
    public float desiredRangeCoeff = 0.75f;
    public float sensitivity = 1f;
    public float desiredRangeLerp = 0.05f;


    bool gamepadCursorInput = false;
    IEnumerator DisableGamepad()
    {
        yield return new WaitForSecondsRealtime(1f);
        gamepadCursorInput = false;
        yield return null;
    }

    public bool combatMode = false;
    public bool cursorCentered = false;
    void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();

        cursorDelta = playerInput.Character.CursorDelta;
        cursorPosition = playerInput.Character.CursorPosition;

    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        previousPlayerPosition = player.position;
        cursorPosRaw = cursorObject.position;

        defaultAlpha = cursorSprite.color.a;
    }



    void Update()
    {
        if (player != null)
        {

            GetMouseDelta();
            GetPlayerDelta();

            #region Detect Gamepad Input
            if (cursorPosition.ReadValue<Vector2>() != Vector2.zero)
            {
                StopCoroutine("DisableGamepad");
                gamepadCursorInput = true;
            }
            else if (deltaMousePosition != Vector2.zero)
            {
                StartCoroutine("DisableGamepad");
            }
            #endregion

            #region Read Cursor Position
            if (gamepadCursorInput)
            {
                Vector2 pos = cursorPosition.ReadValue<Vector2>();
                if (combatMode)
                {
                    cursorPosRaw = pos.normalized * rangeLimit * desiredRangeCoeff
                    + (Vector2)player.position;
                }
                else
                {
                    cursorPosRaw = pos.normalized * Mathf.Pow(pos.magnitude, 3) * rangeLimit * desiredRangeCoeff
                    + (Vector2)player.position;
                }
                cursorObject.position = cursorPosRaw;

            }
            else
            {
                cursorPosRaw += deltaMousePosition + deltaPlayerPosition;
                cursorObject.position = cursorPosRaw;

                if (combatMode)
                {

                    // Smoothly lerp to some distance from body
                    Vector2 rawPosToLerp = deltaMousePosition + deltaPlayerPosition
                     + (Vector2)player.position
                     + ((Vector2)cursorObject.position - (Vector2)player.position).normalized * rangeLimit * desiredRangeCoeff;

                    cursorPosRaw = Vector3.Lerp(cursorObject.position, rawPosToLerp, desiredRangeLerp);
                    cursorObject.position = Vector2.Lerp(cursorObject.position, cursorPosRaw, desiredRangeLerp);

                }
            }
            #endregion

            #region Color
            cursorSprite.color = new Color(cursorSprite.color.r, cursorSprite.color.g, cursorSprite.color.b,
                defaultAlpha * Vector2.Distance(player.position, cursorObject.position));
            #endregion

            #region Limit Range
            if (isCursorLimitedToThePlayer) // cursor distance limit can be disabled
            {
                if (Vector3.Distance(cursorObject.position, player.position) > rangeLimit)
                {
                    cursorPosRaw = player.position + (cursorObject.position - player.position).normalized * rangeLimit;
                    cursorObject.position = cursorPosRaw;
                }
            }
            #endregion


            #region Check if cursor is dead centered on player
            if (Vector2.Distance(cursorObject.position, (Vector2)player.position) < 0.1f)
            {
                cursorCentered = true;
            }
            else
            {
                cursorCentered = false;
            }
            #endregion
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
        deltaMousePosition = cursorDelta.ReadValue<Vector2>() * sensitivity * Time.deltaTime;
    }




}

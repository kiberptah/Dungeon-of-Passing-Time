using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class CustomMouseCursorScript : MonoBehaviour
{
    public PlayerInput playerInput;
    [HideInInspector] public ActorConnector player;

    [Header("Assign Manually")]
    [SerializeField] Transform camera;
    public SpriteRenderer cursorSprite;
    float defaultAlpha = 1f;


    [Header("Positioning")]
    InputAction cursorDelta;
    InputAction cursorPosition;

    Vector3 deltaMousePosition;
    Vector3 cursorPosRaw;

    Vector3 playerPosition = Vector3.zero;
    Vector3 previousPlayerPosition;
    Vector3 deltaPlayerPosition;

    


    Vector3 previousCameraPosition;
    Vector3 deltaCameraPosition;


    [Header("Tuning")]
    [SerializeField] bool isCursorLimitedToThePlayer = true;
    public float rangeLimit = 3f;
    public float desiredRangeCoeff = 0.75f;
    public float sensitivity = 1f;
    public float desiredRangeLerp = 0.05f;

    [Header("Sprites for States")]
    [SerializeField] Sprite cursor_default;
    [SerializeField] Sprite cursor_combat;
    [SerializeField] Sprite cursor_look;
    [SerializeField] Sprite cursor_interact;
    [SerializeField] Sprite cursor_deny;
    [SerializeField] Sprite cursor_talk;
    public enum cursorState
    {
        original,
        combat,
        look,
        interact,
        deny,
        talk
    }
    cursorState currentCursorState;
    [Header("etc")]

    bool gamepadCursorInput = false;
    IEnumerator DisableGamepad()
    {
        yield return new WaitForSecondsRealtime(1f);
        gamepadCursorInput = false;
        yield return null;
    }

    [HideInInspector] public bool combatMode = false;
    [HideInInspector] public bool cursorCentered = false;

    #region Init
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

        playerPosition = player.transform.position;
        playerPosition = playerPosition.OverrideY(0.5f);
        previousPlayerPosition = playerPosition;

        previousCameraPosition = camera.position;

        cursorPosRaw = transform.position;

        defaultAlpha = cursorSprite.color.a;
    }
    #endregion

      
    void Update()
    {
        if (player != null)
        {
            playerPosition = player.transform.position;
            playerPosition = playerPosition.OverrideY(0.5f);

            GetMouseDelta();
            GetPlayerDelta();
            GetCameraDelta();

            #region Detect Gamepad Input
            if (cursorPosition.ReadValue<Vector2>() != Vector2.zero)
            {
                StopCoroutine("DisableGamepad");
                gamepadCursorInput = true;
            }
            else if (deltaMousePosition != Vector3.zero)
            {
                StartCoroutine("DisableGamepad");
            }
            #endregion

            #region Read Cursor Position
            if (gamepadCursorInput)
            {
                Vector3 pos = new Vector3(cursorPosition.ReadValue<Vector2>().x, 0, cursorPosition.ReadValue<Vector2>().y);
                if (combatMode)
                {
                    cursorPosRaw = pos.normalized * rangeLimit * desiredRangeCoeff
                    + playerPosition;
                }
                else
                {
                    cursorPosRaw = pos.normalized * Mathf.Pow(pos.magnitude, 3) * rangeLimit * desiredRangeCoeff
                    + playerPosition;
                }
                transform.position = cursorPosRaw;

            }
            else
            {
                //cursorPosRaw += deltaMousePosition + deltaPlayerPosition;
                cursorPosRaw += deltaMousePosition + deltaCameraPosition;
                transform.position = cursorPosRaw;

                if (combatMode)
                {
                    // Smoothly lerp to some distance from body
                    Vector3 rawPosToLerp = playerPosition 
                        + deltaMousePosition + deltaPlayerPosition
                     + (transform.position - playerPosition).normalized * rangeLimit * desiredRangeCoeff;

                    cursorPosRaw = Vector3.Lerp(transform.position, rawPosToLerp, desiredRangeLerp);
                    transform.position = Vector3.Lerp(transform.position, cursorPosRaw, desiredRangeLerp);

                }
            }
            #endregion

            #region Color
            cursorSprite.color = new Color(cursorSprite.color.r, cursorSprite.color.g, cursorSprite.color.b,
                Mathf.Clamp(Vector3.Distance(playerPosition, transform.position), 0, defaultAlpha));
            #endregion


            #region Hard Limit Range
            if (isCursorLimitedToThePlayer) // cursor distance limit can be disabled
            {
                if (Vector3.Distance(transform.position, playerPosition) > rangeLimit)
                {
                    cursorPosRaw = playerPosition + (transform.position - playerPosition).normalized * rangeLimit;
                    transform.position = cursorPosRaw;
                }
            }
            #endregion


            #region Check if cursor is dead centered on player
            if (Vector3.Distance(transform.position, playerPosition) < 0.1f)
            {
                cursorCentered = true;
            }
            else
            {
                cursorCentered = false;
            }
            #endregion


            ScanForInteractables();
        }
    }



    void GetPlayerDelta()
    {
        deltaPlayerPosition = playerPosition - previousPlayerPosition;
        previousPlayerPosition = playerPosition;
    }

    void GetCameraDelta()
    {
        deltaCameraPosition = camera.position - previousCameraPosition;
        previousCameraPosition = camera.position;
    }

    void GetMouseDelta()
    {
        deltaMousePosition = new Vector3
            (cursorDelta.ReadValue<Vector2>().x, 0, cursorDelta.ReadValue<Vector2>().y) 
            * sensitivity * Time.deltaTime;
    }
























    public IInteractable potentialInteractable;

    void ScanForInteractables()
    {
        if (combatMode == false)
        {
            Vector3 rayOrigin = transform.position - new Vector3(0, -1, 1);

            Ray ray = new Ray(rayOrigin, new Vector3(0, -1, 1));
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, 10f, LayerMask.GetMask("Interactables"));

            if (hitInfo.transform == null)
            {
                ChangeCursorState(cursorState.original);
                return;
            }
            else// if (potentialInteractable == null)
            {
                if (hitInfo.transform.TryGetComponent(out IInteractable interactable))
                {
                    potentialInteractable = interactable;

                    if (player.interactionDetector.interactables.Contains(interactable))
                    {
                        ChangeCursorState(cursorState.interact);
                    }
                    else
                    {
                        ChangeCursorState(cursorState.deny);
                    }
                }
                else
                {
                    ChangeCursorState(cursorState.original);
                    potentialInteractable = null;
                }
            }
            

        }
    }
    public void ChangeCursorState(cursorState state)
    {
        currentCursorState = state;
        UpdateCursorSprite();

        if (state == cursorState.combat)
        {
            potentialInteractable = null;
        }
    }
    void UpdateCursorSprite()
    {
        switch (currentCursorState)
        {
            case cursorState.original:
                cursorSprite.sprite = cursor_default;
                break;
            case cursorState.combat:
                cursorSprite.sprite = cursor_combat;
                break;
            case cursorState.interact:
                cursorSprite.sprite = cursor_interact;
                break;
            case cursorState.deny:
                cursorSprite.sprite = cursor_deny;
                break;

        }
    }
}

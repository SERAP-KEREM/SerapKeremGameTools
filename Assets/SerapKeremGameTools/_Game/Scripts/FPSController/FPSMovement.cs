using SerapKeremGameTools._Game._AudioSystem;
using SerapKeremGameTools._Game._InputSystem;
using System.Collections;
using UnityEngine;
namespace SerapKeremGameTools._Game._FPSPlayerSystem
{
    public class FPSMovement : MonoBehaviour
{
    // Properties
    public bool CanMove { get; private set; } = true;

    [Header("Functional Options")]
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool useFootSteps = true;
    [SerializeField] private bool canZoom = true;


    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float slopeSpeed = 8f;
    [SerializeField] private bool canJump = true;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 15.0f;
    [SerializeField] private bool WillSlideOnSlopes = true;

    [Header("Zoom Parameters")]
    [SerializeField] private float timeToZoom = 0.3f;
    [SerializeField] private float zoomFOV = 30f;
    private float defaultFOV;
    private Coroutine zoomRoutine;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultipler = 1.5f;
    [SerializeField] private float sprintStepMultipler = 0.6f;

    [Header("Audio Parameters")]
    [SerializeField] private string woodAudioName;
    [SerializeField] private string metalAudioName;
    [SerializeField] private string grassAudioName;
    [SerializeField] private string woodAudioNameSprint;
    [SerializeField] private string metalAudioNameSprint;
    [SerializeField] private string grassAudioNameSprint;
    [SerializeField] private string woodAudioNameJump;
    [SerializeField] private string metalAudioNameJump;
    [SerializeField] private string grassAudioNameJump;
    // Private variables
    private Camera playerCamera;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector2 currentInput;
    private float footstepTimer = 0;
    private float timer = 0;

    private bool isCrouching = false;
    private bool duringCrouchAnimation = false;
    private float defaultYPos;

    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);



    //// Mouse Look Variables
    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseLookSpeedX = 2.0f;  // Dikey dönü? h?z?
    [SerializeField] private float mouseLookSpeedY = 2.0f;  // Yatay dönü? h?z?
    private float currentRotationX = 0f;  // Yukar?-a?a?? dönü? aç?s?

    // Initialization
    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        defaultFOV = playerCamera.fieldOfView;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // SLIDING PARAMETERS
    private Vector3 hitPointNormal;
    private bool IsSliding
    {
        get
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal = slopeHit.normal;

                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }
    // Main Update Loop
    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            if (PlayerFPSInput.IsShouldJump) HandleJump();
            if (PlayerFPSInput.IsShouldCrouch) StartCoroutine(CrouchStand());
            if (canUseHeadbob) HandleHeadbob();
            if (useFootSteps) HandleFootSteps();
            if (canZoom) HandleZoom();

            if (canJump)
                HandleJump();
            ApplyFinalMovements();
        }
    }

    // Handle movement input and calculate direction
    private void HandleMovementInput()
    {

        currentInput = new Vector2((PlayerFPSInput.IsSprinting ? sprintSpeed : isCrouching ? crouchSpeed : walkSpeed) * Input.GetAxis("Vertical"), (PlayerFPSInput.IsSprinting ? sprintSpeed : isCrouching ? crouchSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    // Handle mouse look (camera rotation)
    private void HandleMouseLook()
    {
        // Mouse hareketinden gelen dikey hareket (Y ekseni)
        float rotationX = Input.GetAxis("Mouse Y") * mouseLookSpeedY;  // Dikey hassasiyet
        currentRotationX -= rotationX;  // Yukar?/a?a?? bak???n birikmesini sa?la
        currentRotationX = Mathf.Clamp(currentRotationX, -80f, 80f);  // Y ekseninde s?n?r

        // Kameran?n rotas?n? (yerel dönü?ünü) güncelleme
        playerCamera.transform.localRotation = Quaternion.Euler(currentRotationX, 0, 0);

        // Karakterin yatay rotas?n? (sa?a/sola dönme)
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * mouseLookSpeedX, 0);
    }

    // Handle jump behavior
    private void HandleJump()
    {
        if (PlayerFPSInput.IsShouldJump && characterController.isGrounded && !duringCrouchAnimation)
        {
            moveDirection.y = jumpForce;
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5))
            {
                string jumpAudio = GetFootstepAudioNameJump(hit.collider.tag);
                if (!string.IsNullOrEmpty(jumpAudio))
                {
                    AudioManager.Instance.PlayOneShotAudio(jumpAudio); // Jump sesini çal
                }
            }
        }
    }

    // Handle crouch behavior
    private void HandleCrouch()
    {
        if (PlayerFPSInput.IsShouldCrouch)
            StartCoroutine(CrouchStand());
    }

    // Handle zoom (FOV change)
    private void HandleZoom()
    {
        if (Input.GetKeyDown(PlayerFPSInput.zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
            }
            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }
        if (Input.GetKeyUp(PlayerFPSInput.zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
            }
            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }
    }

   

    // Handle headbob for walking, sprinting, crouching
    private void HandleHeadbob()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(currentInput.x) > 0.1f || Mathf.Abs(currentInput.y) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : PlayerFPSInput.IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : PlayerFPSInput.IsSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    // Handle footstep sounds based on surface type
    private void HandleFootSteps()
    {
        if (!characterController.isGrounded || currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5))
            {
                string footstepAudio = PlayerFPSInput.IsSprinting ? GetFootstepAudioNameSprint(hit.collider.tag) : GetFootstepAudioName(hit.collider.tag);
                if (!string.IsNullOrEmpty(footstepAudio))
                {
                    AudioManager.Instance.PlayOneShotAudio(footstepAudio);
                }
            }

            footstepTimer = GetCurrentOffset();
        }
    }

    private string GetFootstepAudioName(string surfaceTag)
    {
        switch (surfaceTag)
        {
            case "FootSteps/WOOD": return woodAudioName;
            case "FootSteps/METAL": return metalAudioName;
            case "FootSteps/GRASS": return grassAudioName;
            default: return null;
        }
    }

    private string GetFootstepAudioNameSprint(string surfaceTag)
    {
        switch (surfaceTag)
        {
            case "FootSteps/WOOD": return woodAudioNameSprint;
            case "FootSteps/METAL": return metalAudioNameSprint;
            case "FootSteps/GRASS": return grassAudioNameSprint;
            default: return null;
        }
    }
    private string GetFootstepAudioNameJump(string surfaceTag)
    {
        switch (surfaceTag)
        {
            case "FootSteps/WOOD": return woodAudioNameJump;
            case "FootSteps/METAL": return metalAudioNameJump;
            case "FootSteps/GRASS": return grassAudioNameJump;
            default: return null;
        }
    }
    private float GetCurrentOffset()
    {
        return isCrouching ? baseStepSpeed * crouchStepMultipler : PlayerFPSInput.IsSprinting ? baseStepSpeed * sprintStepMultipler : baseStepSpeed;
    }

    // Apply final movement (gravity and slopes)
    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        if (WillSlideOnSlopes && IsSliding)
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    // Crouch stand coroutine (animation)
    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;

    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView;
        float timeElapsed = 0;

        while (timeElapsed < timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;

            playerCamera.fieldOfView = targetFOV;
            zoomRoutine = null;
        }
    }
    }
    }
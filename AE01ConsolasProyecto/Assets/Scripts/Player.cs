using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float movSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float maxPitch = 80f;
    [SerializeField] private Transform pCamera;
    public InputActionAsset InpActions;

    private InputAction Move;
    private InputAction Look;
    private InputAction Jump;

    private Vector2 mMove;
    private Vector2 mLook;

    private Rigidbody rb;
    private BoxCollider col;
    private bool isController = false;
    private float pendingYaw;
    private float pitch;
    private bool jumpRequested;

    private void Awake()
    {
        if (pCamera == null)
        {
            var cam = GetComponentInChildren<Camera>();
            if (cam != null) pCamera = cam.transform;
        }
        
        Move = InputSystem.actions.FindAction("Move");
        Look = InputSystem.actions.FindAction("Look");
        Jump = InputSystem.actions.FindAction("Jump");

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        InpActions.FindActionMap("Player").Enable();
        InputSystem.onActionChange += OnActionChange;
        Cursor.lockState = CursorLockMode.Locked;

    }
    private void OnDestroy()
    {
        InputSystem.onActionChange -= OnActionChange;
    }
    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;

        var device = (obj as InputAction)?.activeControl?.device;
        if (device == null) return;

        isController = !(device is Mouse || device is Keyboard);
    }
    // Update is called once per frame
    void Update()
    {
        mMove = Move.ReadValue<Vector2>();
        mLook = Look.ReadValue<Vector2>();
        if (Jump.WasPressedThisFrame())
        {
            jumpRequested = true;

        }
        Vector2 delta = mLook * rotSpeed * Time.deltaTime;


        pendingYaw += delta.x;

        pitch = Mathf.Clamp(pitch - delta.y, -maxPitch, maxPitch);
        if (pCamera != null)
            pCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
    private void FixedUpdate()
    {
        Walk();
        Rotate();
        Salto();
    }
    public void Walk()
    {
        Vector3 move = (transform.forward * mMove.y + transform.right * mMove.x) * movSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

    }
    public void Rotate()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, pendingYaw, 0f));
        pendingYaw = 0f;
        
    }
    private bool IsGrounded()
    {
        Bounds b = col.bounds;
        Vector3 halfExtents = new Vector3(b.extents.x * 0.9f, 0.05f, b.extents.z * 0.9f);

        Vector3 origin = new Vector3(b.center.x, b.min.y + 0.1f, b.center.z);

        return Physics.BoxCast(origin, halfExtents, Vector3.down, out _,
                               transform.rotation, 0.1f + groundCheckDistance,
                               groundMask, QueryTriggerInteraction.Ignore);
    }
    private void Salto()
    {
        if (jumpRequested)
            Debug.Log($"Jump pedido. Grounded: {IsGrounded()}");
        if (jumpRequested && IsGrounded())
        {
            float jumpVelocity = Mathf.Sqrt(2f * -Physics.gravity.y * jumpHeight);
            Vector3 v = rb.linearVelocity; 
            v.y = jumpVelocity;
            rb.linearVelocity = v;
        }
        jumpRequested = false;
    }
}

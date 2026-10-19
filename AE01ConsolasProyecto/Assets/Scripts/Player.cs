using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float movSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private Camera camera;
    public InputActionAsset InpActions;

    private InputAction Move;
    private InputAction Look;

    private Vector2 mMove;
    private Vector2 mLook;

    private Rigidbody rb;
    private bool isController = false;

    private void Awake()
    {
        Move = InputSystem.actions.FindAction("Move");
        Look = InputSystem.actions.FindAction("Look");

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InpActions.FindActionMap("Player").Enable();
        InputSystem.onActionChange += OnActionChange;

    }
    private void OnDestroy()
    {
        InputSystem.onActionChange -= OnActionChange;
    }
    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            InputAction action = obj as InputAction;
            if (action == null) return;

            InputDevice device = action.activeControl?.device;
            if (device == null) return;

            isController = !(device is Mouse || device is Keyboard);
        }
    }
    // Update is called once per frame
    void Update()
    {
        mMove = Move.ReadValue<Vector2>();
        mLook = Look.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        Walk();
        Rotate();
    }
    public void Walk()
    {
        Vector3 move = (transform.forward * mMove.y + transform.right * mMove.x) * movSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

    }
    public void Rotate()
    {
        if (isController)
        {
            if (mLook.sqrMagnitude > 0.01f)
            {
                Vector3 direct = new Vector3(mLook.x, 0f, mLook.y);
                Quaternion targetRotation = Quaternion.LookRotation(direct);
                Debug.Log($"Rotando hacia: {targetRotation.eulerAngles}");
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime));
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;



public class UIController : MonoBehaviour
{
    public InputActionAsset InpActions;

    private InputAction Navigate;
    private InputAction Click;
    private InputAction Cancel;
    private InputAction Point;

    private Vector2 mNavigate;


    private bool isController = false;


    private void Awake()
    {
        Navigate = InputSystem.actions.FindAction("Navigate");
        Click = InputSystem.actions.FindAction("Click");
        Cancel = InputSystem.actions.FindAction("Cancel");


    }
    void Start()
    {
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
        mNavigate = Navigate.ReadValue<Vector2>();

    }

}

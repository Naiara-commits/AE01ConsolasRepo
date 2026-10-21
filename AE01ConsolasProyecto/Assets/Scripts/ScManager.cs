using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;


public class ScManager : MonoBehaviour
{
    public InputActionAsset InpActions;
    private InputAction Cancel;

    public void Awake()
    {
        Cancel = InputSystem.actions.FindAction("Cancel");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Cancel.WasPressedThisFrame())
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    public void Game()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
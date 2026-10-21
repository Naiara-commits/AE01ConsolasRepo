using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] private string menuScene = "MainScene";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            GoToMenu();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GoToMenu();
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene(menuScene);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameManager gameManager;

    private bool isDead = false;
    private float delay = 1f;
    private string activeScene;
    
    // The compiler would not allow me to have the GetActiveScene method without the Start method.
    // My desk has been abused by my head way too much.
    private void Start()
    {
        activeScene = SceneManager.GetActiveScene().name;
    }

    // Game over when player is dead.
    public void GameOver()
    {
        if (!isDead && playerMovement.isGrounded)
        {
            isDead = true;
            Debug.Log("I'm dead");
            playerMovement.enabled = false;
            Invoke("Restart", delay);
        }
        else if (!isDead && !playerMovement.isGrounded)
        {
            isDead = true;
            Debug.Log("I'm dead but falling");
            Invoke("Restart", delay);
        }
    }

    // Restarts the player after he dies.
    // TODO: fix the unload and load functions in GameManager.
    // Note: Grew frustrated; used the Scene Manager. Get Akshat because I don't know
    // what the everlasting f**k I am doing.
    private void Restart()
    {
        //gameManager.UnloadLevel(activeScene);
        //gameManager.LoadLevel(activeScene);
        SceneManager.LoadScene(activeScene);
    }
}

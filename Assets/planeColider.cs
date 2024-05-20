using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPositionOnCollision : MonoBehaviour
{
    // Przekazany obiekt, którego pozycja zostanie zresetowana
    public GameObject objectToReset;

    // Pozycja początkowa
    private Vector3 initialPosition;

    private void Start()
    {
        // Zapisz pozycję początkową obiektu
        initialPosition = objectToReset.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Sprawdź, czy kolizja dotyczy obiektu, którego pozycja ma być zresetowana
        if (collision.gameObject == objectToReset)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}


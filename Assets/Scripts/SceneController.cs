using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void Load(int index = -1)
    {
        if (index == 0) Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(index == -1 ? SceneManager.GetActiveScene().buildIndex : index);
    }
}

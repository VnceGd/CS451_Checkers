using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveMatch : MonoBehaviour
{
    public void TraverseToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

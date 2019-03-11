using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveMatch : MonoBehaviour
{
    public void TraverseToMainMenu()
    {
        Destroy(GameServerManager.instance.gameObject);
        Destroy(GameManager.instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}

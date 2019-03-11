using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveMatch : MonoBehaviour
{
    public void TraverseToMainMenu()
    {
        if (GameServerManager.instance)
        {
            Destroy(GameServerManager.instance.gameObject);
        }
        if (GameManager.instance)
        {
            Destroy(GameManager.instance.gameObject);
        }
        SceneManager.LoadScene("MainMenu");
    }
}

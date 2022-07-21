
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMananger : MonoBehaviour
{
    private static Camera _ins;
    public static Camera Ins
    {
        get
        {
            if (!_ins)
                _ins = FindObjectOfType<Camera>();

            return _ins;
        }
    }

    public TextMeshPro mouseScore;
    public TextMeshPro controllerScore;
    
    
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    
}
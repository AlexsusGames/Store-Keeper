using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameWindow : MonoBehaviour
{
    [SerializeField] private Player player;
    public void Replay()
    {
        SceneManager.LoadScene(0);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}

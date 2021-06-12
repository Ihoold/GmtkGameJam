using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public void TogglePauseMenu(bool toggle) {
        if (toggle) {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        } else {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Quit() {
        Application.Quit();
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void Start()
    {
        TogglePauseMenu(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePauseMenu(!pauseMenu.activeSelf);
    }
}

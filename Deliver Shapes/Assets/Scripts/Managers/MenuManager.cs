using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField] private Button playButton;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            Play();
        });
    }

    private void Play() {
        SceneManager.LoadScene("Game");
    }
}

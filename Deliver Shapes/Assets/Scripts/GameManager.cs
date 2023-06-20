using UnityEngine;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private Animator winScreenAnimator;

    public void Win() {
        winScreenAnimator.Play("Win");
    }
}

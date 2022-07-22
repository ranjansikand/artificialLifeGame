using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] Text _currentScoreText;
    [SerializeField] Text _highScoreText;

    int _currentScore = 0, _highScore;

    private void Awake() {
        Enemy.enemyDied += AddScore;
        PlayerController.playerDied  += SaveScore;

        _highScore = PlayerPrefs.GetInt("High", 0);

        _highScoreText.text = _highScore.ToString();
        _currentScoreText.text = _currentScore.ToString();
    }

    private void AddScore() {
        _currentScore++;
        _currentScoreText.text = _currentScore.ToString();

        if (_currentScore > _highScore) {
            _highScoreText.text = _currentScoreText.text;
        }
    }

    private void SaveScore() {
        if (_currentScore > _highScore) {
            PlayerPrefs.SetInt("High", _currentScore);
        }
    }
}

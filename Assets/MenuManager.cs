using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    // [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject GameUI;
    // [SerializeField] private GameObject LoseUI;
    // [SerializeField] private GameObject PauseUI;
    // [SerializeField] private TextMeshProUGUI _stateText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    // [SerializeField] private TextMeshProUGUI _highScoreText;

    private void Update()
    {
        _scoreText.text = "Score: " + GameManager.enemiesDefeated.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField]
    private Text[] _textList;

    [SerializeField]
    private Sprite[] _lives;
    [SerializeField]
    private Image _livesImage;

    private bool _gameOver;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _livesImage.enabled = true; //lives display
        _textList[0].enabled = true;
        _textList[0].text = "Enemies Destroyed: " + 0;

    }

    public void OnScoreUpdate(int _enemiesDestroyed)
    {
        _textList[0].text = "Enemies Destroyed: " + _enemiesDestroyed;
    }
    public void OnLivesUpdate(int _currentLives)
    {
        _livesImage.sprite = _lives[_currentLives];
    }
    public void OnPlayerDeath()
    {
        _textList[0].enabled = false; //enemy counter text

        _livesImage.enabled = false; //lives display

        _gameOver = true;

        StartCoroutine(OnGameOver()); //flicker game over text
        _textList[2].enabled = true; //restart text
        _gameManager.OnGameOver();
    }
    IEnumerator OnGameOver()
    {
        while (_gameOver == true)
        {
            _textList[1].enabled = true; //game over text
            yield return new WaitForSeconds(0.75f);
            _textList[1].enabled = false; //game over text
            yield return new WaitForSeconds(0.75f); 
        }
    }

}

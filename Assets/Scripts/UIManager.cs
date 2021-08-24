using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _enemiesDestroyedCount;

    [SerializeField]
    private Sprite[] _lives;
    [SerializeField]
    private Image _livesImage;
    // Start is called before the first frame update
    void Start()
    {
        _enemiesDestroyedCount.text = "Enemies Destroyed: " + 0;
    }

    public void OnScoreUpdate(int _enemiesDestroyed)
    {
        _enemiesDestroyedCount.text = "Enemies Destroyed: " + _enemiesDestroyed;
    }
    public void OnLivesUpdate(int _currentLives)
    {
        _livesImage.sprite = _lives[_currentLives];
    }

}

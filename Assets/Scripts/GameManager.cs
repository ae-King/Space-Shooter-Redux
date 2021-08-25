using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isgameOver;

    public void OnRestart(InputAction.CallbackContext context) //UI needs its own scheme and it needs to be set to it in Player Input
    {
        if (context.performed && _isgameOver == true)
        {
            SceneManager.LoadScene(1); //build settings index of current game scene "balls" is 1
        }
    }

    public void OnGameOver()
    {
        _isgameOver = true;
    }

}

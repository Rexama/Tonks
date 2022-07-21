using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _ins;

    public static GameLogic Ins
    {
        get
        {
            if (!_ins)
                _ins = FindObjectOfType<GameLogic>();

            return _ins;
        }
    }

    public TextMeshProUGUI mouseScore;
    public TextMeshProUGUI controllerScore;

    
    public GameObject Mouse;
    public GameObject Controller;
    public Vector2 mouseStartPos;
    public Vector2 controllerStartPos;

    public List<GameObject> bullets;
    private bool isRestarting = false;
    

    private void Start()
    {
        mouseStartPos = Mouse.transform.position;
        controllerStartPos = Controller.transform.position;
    }

    public void ReloadGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    public void ResetGame()
    {
        if (!isRestarting)
        {
            isRestarting = true;
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(2f).OnComplete(() =>
            {
                Mouse.transform.position = new Vector3(5f, 0f, 0f); //mouseStartPos;
                Controller.transform.position = new Vector3(-5f, 0f, 0f); //controllerStartPos;
                Mouse.SetActive(true);
                Controller.SetActive(true);
                DestroyBullets();
                isRestarting = false;
            });
        }
    }

    public void ControllerWin()
    {
        controllerScore.text = (int.Parse(controllerScore.text) + 1).ToString();
        ResetGame();
    }

    public void MouseWin()
    {
        mouseScore.text = (int.Parse(mouseScore.text) + 1).ToString();
        ResetGame();
    }

    public void DestroyBullets()
    {
        foreach (var bullet in bullets)
        {
            Destroy(bullet);
        }
    }
}
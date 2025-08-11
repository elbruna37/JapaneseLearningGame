using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    bool pagesPass = false;
    bool hasStarted = false;

    public bool isGameOver = true;
    public bool backToMenu = false;
    public bool isInMenu = true;
    public bool canPaint = false;

    public int life = 10;

    //bools de control
    public bool menuIsReady;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 60;

        life = 10;
    }

    private void Update()
    {
        if (AnimatorController.Instance != null)
        {
            pagesPass = AnimatorController.Instance.GetAnimatorBool("pasar_varias");
        }


        if (!isGameOver && !hasStarted)
        {
            hasStarted = true;
            StartCoroutine(WaitToPaint());
        }

        if (life <= 0)
        {
            isGameOver = true;
            hasStarted = false;
            backToMenu = true;
            life = 10;
        }
    }
    public void LoadNextScene()
    {
        // Cargar la siguiente escena en el build settings
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);

        StartCoroutine(WaitForOpenBook());
    }

    IEnumerator WaitForOpenBook()
    {
        yield return new WaitForSeconds(1.5f);

        AnimatorController.Instance.SetAnimatorBool("abrir_libro", true);

        yield return new WaitForSeconds(2f);

        AnimatorController.Instance.SetAnimatorBool("pasar_pagina", true);

        menuIsReady = true;
    }

    IEnumerator WaitToPaint()
    {
        yield return new WaitForSeconds(3f);

        canPaint = true;
    }
}

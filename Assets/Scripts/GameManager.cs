using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    bool pagesPass = false;
    bool hasStarted = false;

    public int life = 10;


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
    }

    private void Update()
    {
        pagesPass = AnimatorController.Instance.GetAnimatorBool("pasar_varias");

        if (pagesPass && !hasStarted)
        {
            hasStarted = true;
            StartCoroutine(WaitToPaint());
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
    }

    IEnumerator WaitToPaint()
    {
        yield return new WaitForSeconds(3f);

        LevelGenerator.canPaint = true;
    }
}

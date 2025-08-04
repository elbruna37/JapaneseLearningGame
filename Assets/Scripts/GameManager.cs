using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    bool pagesPass = false;
    bool hasStarted = false;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

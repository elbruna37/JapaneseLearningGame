using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject pressAnyKey;

    private bool isGameActive = false;
    private bool hasReachedDestination = false;

    public float time;
    public float speed;
    public float speedRotation;

    private Vector3 cameraPosition;
    private Quaternion cameraRotation;

    private Vector3 endPosition = new Vector3(-2, 4.88f, -6.47f);
    private Quaternion endRotation = Quaternion.Euler(60.369f, 0, 0); // Más legible que usar valores directos

    // Referencia al GameManager
    private GameManager gameManager;

    void Start()
    {
        // Buscar el GameManager en la escena
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("No se encontró el GameManager en la escena");
        }

        StartCoroutine(BlinkingText());
    }

    void Update()
    {
        cameraPosition = mainCamera.transform.position;
        cameraRotation = mainCamera.transform.rotation;

        if (Input.anyKey && !isGameActive)
        {
            Debug.Log("Pressed any Key");
            isGameActive = true;
        }

        CameraMotion();
    }

    IEnumerator BlinkingText()
    {
        while (!isGameActive)
        {
            pressAnyKey.SetActive(false);
            yield return new WaitForSeconds(time);
            pressAnyKey.SetActive(true);
            yield return new WaitForSeconds(time);
        }
        pressAnyKey.SetActive(false);
    }

    void CameraMotion()
    {
        if (isGameActive && !hasReachedDestination)
        {
            // Movimiento de posición
            mainCamera.transform.position = Vector3.MoveTowards(cameraPosition, endPosition, speed * Time.deltaTime);

            // Rotación solo cuando llegue a la posición
            if (Vector3.Distance(cameraPosition, endPosition) < 0.001f)
            {
                mainCamera.transform.rotation = Quaternion.RotateTowards(cameraRotation, endRotation, speedRotation * Time.deltaTime);

                // Comprobar si ha completado la rotación
                if (Quaternion.Angle(cameraRotation, endRotation) < 0.1f)
                {
                    hasReachedDestination = true;
                    OnCameraReachedDestination();
                }
            }
        }
    }

    void OnCameraReachedDestination()
    {
        Debug.Log("Cámara ha llegado al destino");
        if (gameManager != null)
        {
            // Iniciar una corrutina para esperar 1 segundo antes de cambiar de escena
            StartCoroutine(DelayedSceneChange());
        }
    }

    IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSeconds(1f); // Esperar 1 segundo
        gameManager.LoadNextScene();
    }
}

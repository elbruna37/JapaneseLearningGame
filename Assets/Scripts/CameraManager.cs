using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject pressAnyKey;

    private bool isGameActive = false;
    private bool hasReachedDestination = false;

    public float blinkTime = 0.5f;
    public float moveDuration = 3f;
    public float delayBeforeScene = 0f;

    private Vector3 endPosition = new Vector3(-2, 4.88f, -6.47f);
    private Quaternion endRotation = Quaternion.Euler(60.369f, 0, 0);

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
            Debug.LogError("No se encontró el GameManager en la escena");

        StartBlinking();
    }

    void Update()
    {
        if (Input.anyKey && !isGameActive)
        {
            isGameActive = true;
            pressAnyKey.SetActive(false);

            StartCameraMotion();
        }
    }

    void StartBlinking()
    {
        pressAnyKey.SetActive(true);

        CanvasGroup cg = pressAnyKey.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = pressAnyKey.AddComponent<CanvasGroup>();

        cg.alpha = 1.5f;

        cg.DOFade(0f, blinkTime)
          .SetLoops(-1, LoopType.Yoyo)
          .SetEase(Ease.InOutSine);
    }

    void StartCameraMotion()
    {
        if (hasReachedDestination) return;

        Transform cam = mainCamera.transform;

        Vector3 startPos = cam.position;

        Vector3 midPoint = (startPos + endPosition) / 2;
        midPoint.y += 1.5f;

        Vector3[] path = new Vector3[]
        {
            startPos,
            midPoint,
            endPosition
        };

        Sequence seq = DOTween.Sequence();

        seq.Append(
            cam.DOPath(path, moveDuration, PathType.CatmullRom)
               .SetEase(Ease.InOutSine)
        );

        seq.Join(
            cam.DORotateQuaternion(endRotation, moveDuration)
               .SetEase(Ease.InOutSine)
        );

        seq.OnComplete(() =>
        {
            hasReachedDestination = true;
            OnCameraReachedDestination();
        });
    }

    void OnCameraReachedDestination()
    {
        Debug.Log("Cámara ha llegado al destino");

        if (gameManager != null)
        {
            DOVirtual.DelayedCall(delayBeforeScene, () =>
            {
                gameManager.LoadNextScene();
            });
        }
    }
}
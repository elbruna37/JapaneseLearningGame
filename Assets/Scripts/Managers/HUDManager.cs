using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    public GameObject canvasMenu;
    public GameObject canvasTutorial;
    public GameObject canvasLife;
    public GameObject canvasLevelSelector;

    public GameObject LifeHUD;
    [SerializeField] private TextMeshProUGUI lifeText;

    public RectMask2D [] masksMainMenu;
    public RectMask2D [] masksStartMenu;
 
    private Vector4 originalPadding = new Vector4(0, 0, 1.3f, 0);

    private void Awake()
    {
        Instance = this;
        UpdateLifeDisplay();
    }

    void Update()
    {

        if (GameManager.Instance.menuIsReady)
        {
            GameManager.Instance.menuIsReady = false;

            AnimateMasks(masksMainMenu, 2f, 0.5f, 1f, false);
        }

        if (GameManager.Instance.canPaint)
        {
            LifeHUD.SetActive(true);
            UpdateLifeDisplay();
        }
        else if (GameManager.Instance.isGameOver) { LifeHUD.SetActive(false); }
            
    }

    public void UpdateLifeDisplay()
    {
        if (lifeText != null)
        {
            lifeText.text = GameManager.Instance.life.ToString();
        }
    }

    void AnimateMasks(RectMask2D[] masks, float delayBeforeStart, float delayBetweenEach, float duration, bool reverse)
    {
        Debug.Log(reverse ? "Comenzando desaparicion menu" : "Comenzando aparicion menu");

        for (int i = 0; i < masks.Length; i++)
        {
            RectMask2D mask = masks[i];
            if (mask == null) continue;

            float totalDelay = delayBeforeStart + (i * delayBetweenEach);

            float startValue = reverse ? 0f : originalPadding.z;
            float endValue = reverse ? originalPadding.z : 0f;

            DOVirtual.Float(startValue, endValue, duration, value =>
            {
                if (mask != null)
                {
                    mask.padding = new Vector4(
                        originalPadding.x,
                        originalPadding.y,
                        value,
                        originalPadding.w
                    );
                }
            })
            .SetDelay(totalDelay)
            .SetEase(Ease.InOutCubic);
        }
    }

    public void StartPressed()
    {
        AnimateMasks(masksStartMenu, 0.5f, 0.5f, 1f, false);
    }

    public void StartTutorial()
    {
        AnimateMasks(masksMainMenu, 0.5f, 0.5f, 0.5f, true);
        AnimateMasks(masksStartMenu, 0.5f, 0.5f, 0.5f, true);

        GameManager.Instance.isGameOver = false;
        GameManager.Instance.isInMenu = false;
        GameManager.Instance.life = 10;

        AnimatorController.Instance.PasarVariasPaginas(() =>
        {
            Debug.Log("Animación terminada");
            canvasMenu.SetActive(false);
            canvasTutorial.SetActive(true);
        });
    }
}

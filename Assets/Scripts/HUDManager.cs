using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    public GameObject LifeHUD;
    [SerializeField] private TextMeshProUGUI lifeText;

    public GameObject LeftPageFvas;
    public GameObject RightPageCanvas;

    public RectMask2D [] masksMainMenu;
    public RectMask2D [] masksStartMenu;
 

    private Vector4 originalPadding = new Vector4(0, 0, 1.3f, 0);

    public GameObject Turorial;
    public GameObject MainMenu;

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

            StartCoroutine(StartStaggeredMaskAnimation(masksMainMenu, 2f, 0.5f, 1f, false));
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

    IEnumerator StartStaggeredMaskAnimation(RectMask2D[] masks,float delayBeforeStart, float delayBetweenEach, float animationDuration, bool reverse)  // reverse = false ---- open    reverse = true --------- close
    {
        if (!reverse) { Debug.Log("Comenzando aparicion menu"); } else { Debug.Log("Comenzando desaparicion menu"); }

            yield return new WaitForSeconds(delayBeforeStart);

        for (int i = 0; i < masks.Length; i++)
        {
            float delay = i * delayBetweenEach;
            StartCoroutine(AnimateMaskPadding(masks[i], delay, animationDuration, reverse));
        }
    }

    IEnumerator AnimateMaskPadding(RectMask2D mask, float delay, float duration, bool reverse)
    {
        yield return new WaitForSeconds(delay);

        float tiempoInicio = Time.time;

        float startValue = reverse ? 0f : originalPadding.z;
        float endValue = reverse ? originalPadding.z : 0f;

        Vector4 paddingFinal = new Vector4(originalPadding.x, originalPadding.y, endValue, originalPadding.w);

        while (Time.time - tiempoInicio < duration)
        {
            float progreso = (Time.time - tiempoInicio) / duration;
            float nuevoRight = Mathf.Lerp(startValue, endValue, progreso);

            if (mask != null)
            {
                mask.padding = new Vector4(originalPadding.x, originalPadding.y, nuevoRight, originalPadding.w);
            }

            yield return null;
        }

        if (mask != null)
        {
            mask.padding = paddingFinal;
        }
    }

    public void StartPressed()
    {
        StartCoroutine(StartStaggeredMaskAnimation(masksStartMenu, 0.5f, 0.5f, 1f, false));
    }

    public void StartTutorial()
    {
        StartCoroutine(StartStaggeredMaskAnimation(masksMainMenu,0.5f,0.5f,0.5f, true));
        StartCoroutine(StartStaggeredMaskAnimation(masksStartMenu, 0.5f, 0.5f, 0.5f, true));

        GameManager.Instance.isGameOver = false;
        GameManager.Instance.isInMenu = false;
        GameManager.Instance.life = 10;


        AnimatorController.Instance.SetAnimatorBool("pasar_varias", true);
    }
}

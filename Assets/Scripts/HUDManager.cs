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
 

    private Vector4 originalPadding = new Vector4(0, 0, 1.1f, 0);

    bool pasar_pagina;
    bool pasar_varias;
    //bool hasWrited = false;
    bool readyMenu = false;

    public GameObject Turorial;
    public GameObject MainMenu;

    private void Awake()
    {
        Instance = this;
        UpdateLifeDisplay();
    }

    void Update()
    {
        pasar_pagina = AnimatorController.Instance.GetAnimatorBool("pasar_pagina");
        pasar_varias = AnimatorController.Instance.GetAnimatorBool("pasar_varias");

        if (GameManager.Instance.menuIsReady)
        {
            GameManager.Instance.menuIsReady = false;
            StartCoroutine(StartStaggeredMaskAnimation(masksMainMenu, 2f, 0.5f, 1f));
        }

        if (LevelGenerator.canPaint)
        {
            LifeHUD.SetActive(true);
        }
        //else { LifeHUD.SetActive(false); }

        if (AnimatorController.isAnimationFinished) { MainMenu.SetActive(true); Turorial.SetActive(true); }
            
    }

    public void UpdateLifeDisplay()
    {
        if (lifeText != null)
        {
            lifeText.text = GameManager.Instance.life.ToString();
        }
    }

    IEnumerator StartStaggeredMaskAnimation(RectMask2D[] masks,float delayBeforeStart, float delayBetweenEach, float animationDuration)
    {
        yield return new WaitForSeconds(delayBeforeStart);

        for (int i = 0; i < masks.Length; i++)
        {
            float delay = i * delayBetweenEach;
            StartCoroutine(AnimateMaskPadding(masks[i], delay, animationDuration));
        }
    }

    IEnumerator AnimateMaskPadding(RectMask2D mask, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        float tiempoInicio = Time.time;
        Vector4 paddingFinal = new Vector4(originalPadding.x, originalPadding.y, 0f, originalPadding.w);

        while (Time.time - tiempoInicio < duration)
        {
            float progreso = (Time.time - tiempoInicio) / duration;
            float nuevoRight = Mathf.Lerp(originalPadding.z, 0f, progreso);

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
        StartCoroutine(StartStaggeredMaskAnimation(masksStartMenu, 0.5f, 0.5f, 1f));
    }

    public void StartTutorial()
    {
        GameManager.Instance.isGameOver = false;
        GameManager.Instance.life = 10;

        Turorial.SetActive(false);
        MainMenu.SetActive(false);
        AnimatorController.Instance.SetAnimatorBool("pasar_varias", true);
    }
}

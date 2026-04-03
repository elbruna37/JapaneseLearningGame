using System.Collections;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public static AnimatorController Instance { get; private set; }

    private Animator PageAnimator;
    private Animator BookAnimator;

    private string[] estados = {
        "Page_002|pasar_varias", "Page_003|pasar_varias", "Page_004|pasar_varias",
        "Page_005|pasar_varias", "Page_006|pasar_varias", "Page_007|pasar_varias",
        "Page_008|pasar_varias", "Page_009|pasar_varias", "Page_010|pasar_varias",
        "Page_011|pasar_varias", "Page_012|pasar_varias", "Page_013|pasar_varias",
        "Page_014|pasar_varias", "Page_015|pasar_varias", "Page_016|ultima_pagina"
    };


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        PageAnimator = GameObject.Find("Paginas").GetComponent<Animator>();
        BookAnimator = GameObject.Find("Cubierta").GetComponent<Animator>();
    }

    // =========================
    // MÉTODOS PÚBLICOS ANIMACIONES LIBRO (API)
    //  Llamadas:
    //      AnimatorController.Instance.AbrirLibro(true);
    //      AnimatorController.Instance.PasarVariasPaginas();
    //      AnimatorController.Instance.PasarPagina();
    //      AnimatorController.Instance.UltimaPagina();
    //      AnimatorController.Instance.NextRound();
    //      AnimatorController.Instance.VolverAlMenu();
    // =========================

    public void AbrirLibro(bool value)
    {
        BookAnimator.SetBool("abrir_libro", value);
    }

    public void PasarPagina()
    {
        PageAnimator.SetTrigger("pasar_pagina");
    }

    public void UltimaPagina()
    {
        PageAnimator.SetTrigger("ultima_pagina");
    }

    public void NextRound()
    {
        PageAnimator.SetTrigger("next_round");
    }

    public void PasarVariasPaginas(System.Action onComplete = null)
    {
        StartCoroutine(LunchAnimationsOnTime(onComplete));
    }

    public void VolverAlMenu(System.Action onComplete = null)
    {
        Debug.Log("Iniciando pasado de paginas invertido");
        StartCoroutine(ReverseAnimationsInTime(onComplete));
    }

    // =========================
    // COROUTINAS
    // =========================

    IEnumerator LunchAnimationsOnTime(System.Action onComplete)
    {
        float tiempoInicial = Time.time;

        for (int i = 0; i < estados.Length; i++)
        {
            int capa = i + 1;
            float tiempoObjetivo = tiempoInicial + (i * 0.1f);

            while (Time.time < tiempoObjetivo)
                yield return null;

            PageAnimator.Play(estados[i], capa);
        }

        onComplete?.Invoke();
    }

    public IEnumerator ReverseAnimationsInTime(System.Action onComplete)
    {
        float tiempoInicial = Time.time;

        for (int i = estados.Length - 1; i >= 0; i--)
        {
            int capa = i + 1;
            float tiempoObjetivo = tiempoInicial + ((estados.Length - 1 - i) * 0.1f);

            while (Time.time < tiempoObjetivo)
                yield return null;

            PageAnimator.Play(estados[i] + " reverse", capa);
        }

        GameManager.Instance.menuIsReady = true;
        onComplete?.Invoke();
    }
}
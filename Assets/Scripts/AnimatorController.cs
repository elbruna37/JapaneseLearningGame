using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public static AnimatorController Instance { get; private set; }

    Animator PageAnimator;
    Animator BookAnimator;


    private string[] estados = {
        "Page_002|pasar_varias", "Page_003|pasar_varias", "Page_004|pasar_varias",
        "Page_005|pasar_varias", "Page_006|pasar_varias", "Page_007|pasar_varias",
        "Page_008|pasar_varias", "Page_009|pasar_varias", "Page_010|pasar_varias",
        "Page_011|pasar_varias", "Page_012|pasar_varias", "Page_013|pasar_varias",
        "Page_014|pasar_varias", "Page_015|pasar_varias", "Page_016|ultima_pagina"
    };

    //BOOK ANIMATOR
    public bool abrir_libro = false;
    //PAGE ANIMATOR
    public bool pasar_varias = false;
    
    public static bool pasar_variasReverse = false;
    public static bool isAnimationFinished = false;

    public bool pasar_pagina = false;
    public bool ultima_pagina = false;
    public bool next_round = false;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        PageAnimator = GameObject.Find("Paginas").GetComponent<Animator>();
        BookAnimator = GameObject.Find("Cubierta").GetComponent<Animator>();
    }

    void Update()
    {
        BookAnimator.SetBool("abrir_libro", abrir_libro);
        PageAnimator.SetBool("pasar_varias", pasar_varias);
        PageAnimator.SetBool("pasar_pagina", pasar_pagina);
        PageAnimator.SetBool("ultima_pagina", ultima_pagina);
        PageAnimator.SetBool("next_round", next_round);

        TurnPagesActivator();
    }

    IEnumerator LanzarAnimacionesEnTiempo()
    {
        float tiempoInicial = Time.time;

        for (int i = 0; i < estados.Length; i++)
        {
            int capa = i + 1;
            float tiempoObjetivo = tiempoInicial + (i * 0.1f);

            // Espera hasta el tiempo exacto
            while (Time.time < tiempoObjetivo)
                yield return null;

            PageAnimator.Play(estados[i], capa);
        }
    }

    public IEnumerator RevertirAnimacionesEnTiempo()
    {
        isAnimationFinished = false;

        float tiempoInicial = Time.time;

        for (int i = estados.Length - 1; i >= 0; i--)
        {
            int capa = i + 1;
            float tiempoObjetivo = tiempoInicial + ((estados.Length - 1 - i) * 0.1f);

            while (Time.time < tiempoObjetivo)
                yield return null;

            PageAnimator.Play(estados[i] + " reverse", capa);
        }

        isAnimationFinished = true;
    }

    void TurnPagesActivator()
    {
        if (pasar_varias)
        {
            pasar_varias = false;
            StartCoroutine(LanzarAnimacionesEnTiempo());
        }

        /*if (GameManager.Instance.isGameOver && !pasar_variasReverse)
        {
            pasar_variasReverse = true;
            Debug.Log("Inciando pasado de paginas invertido");
            StartCoroutine(RevertirAnimacionesEnTiempo());
        }*/
    }
    public void SetAnimatorBool(string parameterName, bool value)
    {
        switch (parameterName)
        {
            case "abrir_libro": abrir_libro = value; break;
            case "pasar_varias": pasar_varias = value; break;
            case "pasar_pagina": pasar_pagina = value; break;
            case "ultima_pagina": ultima_pagina = value; break;
            case "next_round": next_round = value; break;
        }
    }

    public bool GetAnimatorBool(string parameterName)
    {
        switch (parameterName)
        {
            case "abrir_libro": return abrir_libro;
            case "pasar_varias": return pasar_varias;
            case "pasar_pagina": return pasar_pagina;
            case "ultima_pagina": return ultima_pagina;
            case "next_round": return next_round;
            default:
                Debug.LogWarning($"No se encontró el parámetro booleano: {parameterName}");
                return false;
        }
    }
}

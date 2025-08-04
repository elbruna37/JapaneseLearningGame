using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public GameObject LeftPageCanvas;
    public GameObject RightPageCanvas;

    public RectMask2D [] masksMainMenu;
    public RectMask2D [] masksStartMenu;
 

    private Vector4 originalPadding = new Vector4(0, 0, 1.1f, 0);

    bool pasar_pagina;
    bool hasWrited = false;
    bool readyMenu = false;

    public GameObject Turorial;
    public GameObject MainMenu;

    
    void Update()
    {
        pasar_pagina = AnimatorController.Instance.GetAnimatorBool("pasar_pagina");

        if (pasar_pagina && !hasWrited)
        {
            hasWrited = true;
            StartCoroutine(ReducePaddingAfterDelay(masksMainMenu , 2.5f, 1.2f));
        }
    }

    IEnumerator ReducePaddingAfterDelay(RectMask2D[] masks, float delayBeforeStart, float animationDuration)
    {
        yield return new WaitForSeconds(delayBeforeStart); // Espera 1 segundo


        float tiempoInicio = Time.time;
        Vector4 paddingFinal = new Vector4(originalPadding.x, originalPadding.y, 0f, originalPadding.w);

        while (Time.time -tiempoInicio < animationDuration)
        {
            float progreso = (Time.time - tiempoInicio) / animationDuration;
            float nuevoRight = Mathf.Lerp(originalPadding.z, 0f, progreso);  // Interpola el right

            // Aplica el cambio a TODOS los RectMask2D proporcionados
            foreach (RectMask2D mask in masks)
            {
                if (mask != null) // Verifica que la referencia no sea nula
                {
                    mask.padding = new Vector4(originalPadding.x, originalPadding.y, nuevoRight, originalPadding.w);
                }
            }
            yield return null;
        }

        // Asegura que todos lleguen al valor final
        foreach (RectMask2D mask in masks)
        {
            if (mask != null)
            {
                mask.padding = paddingFinal;
            }
        }

        readyMenu = true;
    }

    public void ResetAllMasks()
    {
        foreach (RectMask2D mask in masksMainMenu)
        {
            if (mask != null)
            {
                mask.padding = originalPadding;
            }
        }
    }

    public void StartPressed()
    {
        if (readyMenu)
        {
            StartCoroutine(ReducePaddingAfterDelay(masksStartMenu, 0.5f, 1.2f));
        }
    }

    public void StartTutorial()
    {
        Turorial.SetActive(false);
        MainMenu.SetActive(false);
        AnimatorController.Instance.SetAnimatorBool("pasar_varias", true);
    }
}

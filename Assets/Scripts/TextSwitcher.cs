using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private TextMeshProUGUI textA;
    private TextMeshProUGUI textB;

    private float originalASize;
    private float originalBSize;

    private bool isHovered = false;
    private bool isSelected = false;

    //evita doble clic sobre el mismo objeto
    private static TextSwitcher currentSelected = null;

    void Start()
    {
        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        if (texts.Length >= 2)
        {
            textA = texts[0];
            textB = texts[1];

            originalASize = textA.fontSize;
            originalBSize = textB.fontSize;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected) return;

        isHovered = true;
        ApplySwappedState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected) return;

        isHovered = false;
        RevertToOriginal();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Si ya está seleccionado, ignorar
        if (isSelected) return;

        // Deseleccionar el anterior
        if (currentSelected != null && currentSelected != this)
        {
            currentSelected.Deselect();
        }

        currentSelected = this;
        isSelected = true;

        if (!isHovered)
        {
            ApplySwappedState();
        }
    }

    private void ApplySwappedState()
    {
        // Intercambiar textos
        string temp = textA.text;
        textA.text = textB.text;
        textB.text = temp;

        // Cambiar tamaños
        textA.fontSize = originalASize * 1.2f;
        textB.fontSize = originalBSize * 0.8f;
    }

    private void RevertToOriginal()
    {
        // Intercambiar textos nuevamente
        string temp = textA.text;
        textA.text = textB.text;
        textB.text = temp;

        // Restaurar tamaños
        textA.fontSize = originalASize;
        textB.fontSize = originalBSize;
    }

    public void Deselect()
    {
        isSelected = false;
        isHovered = false;
        RevertToOriginal();
    }
}



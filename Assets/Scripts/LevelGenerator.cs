using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class LevelGenerator : MonoBehaviour
{

    XElement xmlDoc;

    public Image stroke0;
    public Image stroke1;
    public Image stroke2;
    public Image stroke3;
    public Image apostrophe;
    public Image circle;

    public float speedPaint;



    //HiraganaGenerator method variables
    int hiraganaRandom;
    string stringHiraganaRandom;
    int hiraganaRandomAux;
    string stringHiraganaRandomAux;
    int nStrokes;
    string nletter;

    bool needApostrophe = false;
    bool needCircle = false;
    //bool isPaintFinish = true;

    public static bool canPaint = false;
    bool hasStarted = false;

    int tutorialRound = 0;
    int repeatCount = 0;

    //Answer method variables
    public GameObject answerContainer;
    private List<string> currentOptions = new List<string>();

    private float originalSize;

    private bool hasAnswered = false;
    private string correctAnswer = "";


    void Start()
    {
        XMLloader();
    }

    
    void Update()
    {
        if (canPaint)
        {
            canPaint = false;
            TutorialHiraganaRandom();
            UpdateAnswersUI();
        }
        //else { answerContainer.SetActive(false); }

        if (GameManager.Instance.isGameOver && canPaint)
        {
            canPaint = false;
            StopCoroutine(ImageLoads(stringHiraganaRandom, nStrokes, nletter));
            stroke0.fillAmount = 0;
            stroke1.fillAmount = 0;
            stroke2.fillAmount = 0;
            stroke3.fillAmount = 0;
            apostrophe.fillAmount = 0;
            circle.fillAmount = 0;
        }
    }
    void XMLloader()
    {

        xmlDoc = XElement.Load($"Assets/Resources/XML Files/charList.xml");

    }

    //Generates a random number and checks if the corresponding hiragana needs an apostrophe or circle

    void TutorialHiraganaRandom()
    {

        switch (tutorialRound)
        {
            case 0 : 
                hiraganaRandom = UnityEngine.Random.Range(66, 71);    // a, e , i ,o , u 
               break;
            case 1: 
                hiraganaRandom = UnityEngine.Random.Range(0, 5);      //ka , ke , ki , ko , ku
                break;
            case 2:
                do
                {
                    hiraganaRandom = UnityEngine.Random.Range(0, 40);  //ka , ke , ki , ko , ku , ga , ge , gi, go , gu     
                } while (hiraganaRandom >= 5 && hiraganaRandom <= 34);
                break;
            case 3:
                hiraganaRandom = UnityEngine.Random.Range(5, 10);      //sa , se , si , so , su
                break;
            case 4:
                do
                {
                    hiraganaRandom = UnityEngine.Random.Range(5, 45);  //sa , se , si , so , su , za , ze , ji, zo , zu     
                } while (hiraganaRandom >= 10 && hiraganaRandom <= 39);
                break;
            case 5:
                hiraganaRandom = UnityEngine.Random.Range(10, 15);      //ta , te , chi , to , tsu
                break;
            case 6:
                do
                {
                    hiraganaRandom = UnityEngine.Random.Range(10, 50);  //ta , te , chi , to , tsu , da , de , ji, do , zu     
                } while (hiraganaRandom >= 15 && hiraganaRandom <= 44);
                break;
            case 7:
                hiraganaRandom = UnityEngine.Random.Range(15, 20);      //ha , he , hi , ho , hu
                break;
            case 8:
                do
                {
                    hiraganaRandom = UnityEngine.Random.Range(10, 60);  //ha , he , hi , ho , hu , ba , be , bi, bo , bu , pa , pe , pi , po , pu     
                } while (hiraganaRandom >= 20 && hiraganaRandom <= 49);
                break;
            case 9:
                hiraganaRandom = UnityEngine.Random.Range(20, 25);      //na , ne , ni , no , nu
                break;
            case 10:
                hiraganaRandom = UnityEngine.Random.Range(25, 30);      //ma , me , mi , mo , mu
                break;
            case 11:
                hiraganaRandom = UnityEngine.Random.Range(30, 35);      //ra , re , ri , ro , ru
                break;
            case 12:
                hiraganaRandom = UnityEngine.Random.Range(60, 66);      //wa, wo, n , ya , yo , yu
                break;
            case 13:
                hiraganaRandom = UnityEngine.Random.Range(0, 71);      //all
                break;
        }

        hiraganaRandomAux = hiraganaRandom;

        if (hiraganaRandom >= 35 && hiraganaRandom <= 54) //If the number is between 35 and 54 it corresponds to a hiragana with an apostrophe
        {
            needApostrophe = true;
            hiraganaRandom -= 35;
        }
        else if (hiraganaRandom >= 55 && hiraganaRandom <= 59) //If the number is between 55 and 59 it corresponds to a hiragana with an circle
        {
            needCircle = true;
            hiraganaRandom -= 40;
        }

        stringHiraganaRandom = String.Format("{0}", hiraganaRandom);

        nStrokes = StrokeFinder(stringHiraganaRandom);
        nletter = LetterFinder(stringHiraganaRandom);

        correctAnswer = nletter;

        StartCoroutine(ImageLoads(stringHiraganaRandom, nStrokes, nletter));
    }

    //Finds the number of strokes of a random hiragana within the XML saved in Resources/XML Files/charList.xml
    int StrokeFinder(string numRandom)
    {

        var strokes = from e in xmlDoc.Element("hiragana").Elements("id") where e.Attribute("cod").Value == numRandom select e.Element("strokes");

        foreach (var id in strokes)
        {
            nStrokes = Int32.Parse(id.Value);
        }

        return nStrokes;
    }

    //Finds the character of a random hiragana within the XML saved in Resources/XML Files/charList.xml
    string LetterFinder(string numRandom)
    {
    
        var letter = from e in xmlDoc.Element("hiragana").Elements("id") where e.Attribute("cod").Value == numRandom select e.Element("letter");

        foreach (var id in letter)
        {
            nletter = string.Format("{0}", id.Value);
        }

        return nletter;
    }

    //loads the hiragana sprite successively and in order
    IEnumerator ImageLoads(string numRandom, int nStrokes, string nLetter)
    {
        stroke0.fillAmount = 0;
        stroke1.fillAmount = 0;
        stroke2.fillAmount = 0;
        stroke3.fillAmount = 0;
        apostrophe.fillAmount = 0;
        circle.fillAmount = 0;


        for (int i = 0; i <= (nStrokes - 1); i++)
        {
            if(i == 0)
            {
                stroke0.sprite = Resources.Load<Sprite>("Sprites/Hiragana/" + nLetter + i);
                while (stroke0.fillAmount < 1)
                {
                    stroke0.fillAmount += 0.1f;
                    yield return new WaitForSeconds(speedPaint);
                }
                yield return new WaitForSeconds(0.3f);
                Debug.Log("primer trazo dibujado");
            }
            if (i == 1)
            {  
                stroke1.sprite = Resources.Load<Sprite>("Sprites/Hiragana/" + nLetter + i);
                while (stroke1.fillAmount < 1)
                {
                    stroke1.fillAmount += 0.1f;
                    yield return new WaitForSeconds(speedPaint);
                }
                yield return new WaitForSeconds(0.3f);
                Debug.Log("segundo trazo dibujado");
            }
            if (i == 2)
            {  
                stroke2.sprite = Resources.Load<Sprite>("Sprites/Hiragana/" + nLetter + i);
                while (stroke2.fillAmount < 1)
                {
                    stroke2.fillAmount += 0.1f;
                    yield return new WaitForSeconds(speedPaint);
                }
                yield return new WaitForSeconds(0.3f);
                Debug.Log("tercer trazo dibujado");
            }
            if (i == 3)
            {
                stroke3.sprite = Resources.Load<Sprite>("Sprites/Hiragana/" + nLetter + i);
                while (stroke3.fillAmount < 1)
                {
                    stroke3.fillAmount += 0.1f;
                    yield return new WaitForSeconds(speedPaint);
                }
                yield return new WaitForSeconds(0.3f);
                Debug.Log("cuarto trazo dibujado");
            }
        }

        if (needApostrophe)
        {
            apostrophe.sprite = Resources.Load<Sprite>("Sprites/apostrophe");
            while (apostrophe.fillAmount < 1)
            {
                apostrophe.fillAmount += 0.1f;
                yield return new WaitForSeconds(speedPaint);
            }
            needApostrophe = false;
            
        }
        if (needCircle)
        {
            circle.sprite = Resources.Load<Sprite>("Sprites/circle");
            while (circle.fillAmount < 1)
            {
                circle.fillAmount += 0.1f;
                yield return new WaitForSeconds(speedPaint);
            }
            needCircle = false;
            
        }
    }

    void UpdateAnswersUI()
    {
        currentOptions.Clear();
        hasAnswered = false;
        answerContainer.SetActive(true);

        // Obtener índices válidos para este tutorialRound
        List<int> validIndices = GetValidIndicesForRound(tutorialRound);


        if (validIndices.Count < 5)
        {
            Debug.LogError("Se necesitan exactamente 5 respuestas para este round.");
            return;
        }

        // Convertir a letras usando LetterFinder
        foreach (int index in validIndices)
        {
            string letter = LetterFinder(index.ToString());
            currentOptions.Add(letter);
        }

        

        // Asignar letras a los botones
        for (int i = 0; i < 5; i++)
        {
            Transform answerButton = answerContainer.transform.GetChild(i);

            // Asignar el texto
            TextMeshProUGUI tmp = answerButton.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = currentOptions[i];

            // Reset color
            Image img = answerButton.GetComponent<Image>();
            img.color = Color.white;

            // Asignar evento de click
            Button btn = answerButton.GetComponent<Button>();
            btn.onClick.RemoveAllListeners(); // limpiar listeners previos
            int index = i; // evitar error de closure
            btn.onClick.AddListener(() => CheckAnswer(index));

        }

        Debug.Log("Botones configurados. Correcta: " + correctAnswer);
    }

    List<int> GetValidIndicesForRound(int round)
    {
        switch (round)
        {
            case 0: return new List<int> { 66, 67, 68, 69, 70 }; // a, e ,i , o , u
            case 1: return new List<int> { 0, 1, 2, 3, 4 };       // か, き, く, け, こ
            case 2: return new List<int> { 0, 1, 2, 3, 4 };       // ka-group sin dakuten
            case 3: return new List<int> { 5, 6, 7, 8, 9 };       // さ-group
            case 4: return new List<int> { 5, 6, 7, 8, 9 };       // さ, し, す, せ, そ
            case 5: return new List<int> { 10, 11, 12, 13, 14 };  // た-group
            case 6: return new List<int> { 10, 11, 12, 13, 14 };  // た, ち, つ, て, と
            case 7: return new List<int> { 15, 16, 17, 18, 19 };  // は-group
            case 8: return new List<int> { 15, 16, 17, 18, 19 };  // は, ひ, ふ, へ, ほ
            case 9: return new List<int> { 20, 21, 22, 23, 24 };  // な-group
            case 10: return new List<int> { 25, 26, 27, 28, 29 }; // ま-group
            case 11: return new List<int> { 30, 31, 32, 33, 34 }; // ら-group
            case 12: return new List<int> { 60, 61, 62, 63, 65 }; // わ, を, ん, や, よ
            case 13: return new List<int> { 0, 1, 2, 3, 4 };       // Ejemplo para "todo" → los primeros 5 (puedes cambiarlo)
            default: return new List<int>();
        }
    }

    void CheckAnswer(int index)
    {
        if (hasAnswered)
            return;

        hasAnswered = true;
        Debug.Log("Clic en: " + currentOptions[index]);
        for (int i = 0; i < 5; i++)
        {
            Transform answerButton = answerContainer.transform.GetChild(i);
            TextMeshProUGUI tmp = answerButton.GetComponentInChildren<TextMeshProUGUI>();
            Image img = answerButton.GetComponent<Image>();

            if (i == index)
            {
                if (tmp.text == correctAnswer)
                {
                    originalSize = tmp.fontSize;
                    tmp.color = Color.green; // Correcto
                    tmp.fontStyle = FontStyles.Bold;
                    tmp.fontSize += 0.1f;
                }
                else
                {
                    tmp.color = Color.red; // Incorrecto

                    GameManager.Instance.life--;
                    HUDManager.Instance.UpdateLifeDisplay();
                    
                }
            }
            else if (tmp.text == correctAnswer)
            {
                originalSize = tmp.fontSize;
                tmp.color = Color.gray; // Mostrar cuál era la correcta
                tmp.fontStyle = FontStyles.Bold;
                tmp.fontSize += 0.1f; 
            }
        }
        repeatCount++;
        Debug.Log("Nivel Actual: " + tutorialRound + ". Ronda Actual: " + repeatCount);

        if (repeatCount > 5)
        {
            // Hemos llegado al límite, pasamos al siguiente round
            tutorialRound++;
            repeatCount = 0;

            // Opcional: límite máximo
            if (tutorialRound > 13)
            {
                tutorialRound = 13; // O lo que prefieras
            }
        }

        StartCoroutine(ProceedToNextQuestion());
    }

    void ResetAnswerColors()
    {
        for (int i = 0; i < answerContainer.transform.childCount; i++)
        {
            Transform answerButton = answerContainer.transform.GetChild(i);
            TextMeshProUGUI tmp = answerButton.GetComponentInChildren<TextMeshProUGUI>();
            tmp.color = Color.black; // o el color que uses normalmente
            tmp.fontStyle = FontStyles.Normal;
            tmp.fontSize = originalSize;
        }
    }

    IEnumerator ProceedToNextQuestion()
    {
        // Opcional: espera un segundo
        yield return new WaitForSeconds(0.5f);


        ResetAnswerColors();

        //Pone en 0 los fillAmount de los strokes
        float startTime = Time.time;

        float s0 = stroke0.fillAmount;
        float s1 = stroke1.fillAmount;
        float s2 = stroke2.fillAmount;
        float s3 = stroke3.fillAmount;
        float ap = apostrophe.fillAmount;
        float ci = circle.fillAmount;

        //HAY QUE AÑADIR QUE CAMBIE EL FILL METHOD DE IZQUIERDA A DERECHA  // stroke0.type = Image.Type.Filled;  stroke0.fillMethod = Image.FillMethod.Horizontal; stroke0.fillOrigin = 0;

        while (Time.time - startTime < 0.2f)
        {
            float t = (Time.time - startTime) / 0.2f;

            stroke0.fillAmount = Mathf.Lerp(s0, 0f, t);
            stroke1.fillAmount = Mathf.Lerp(s1, 0f, t);
            stroke2.fillAmount = Mathf.Lerp(s2, 0f, t);
            stroke3.fillAmount = Mathf.Lerp(s3, 0f, t);
            apostrophe.fillAmount = Mathf.Lerp(ap, 0f, t);
            circle.fillAmount = Mathf.Lerp(ci, 0f, t);

            yield return null;
        }

        stroke0.fillAmount = 0f;
        stroke1.fillAmount = 0f;
        stroke2.fillAmount = 0f;
        stroke3.fillAmount = 0f;
        apostrophe.fillAmount = 0f;
        circle.fillAmount = 0f;


        // Generar la siguiente pregunta
        canPaint = true;

    }

}



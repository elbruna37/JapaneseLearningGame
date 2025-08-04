using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System;


public class LevelGenerator : MonoBehaviour
{
    XElement xmlDoc;
    //bool finishedXMLLoading = false;

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

    void Start()
    {
        XMLloader();
    }

    
    void Update()
    {
        if (canPaint && !hasStarted)
        {
            hasStarted = true;
            TutorialHiraganaRandom();
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
}



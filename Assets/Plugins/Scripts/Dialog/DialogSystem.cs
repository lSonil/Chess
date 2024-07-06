using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogSystem : MonoBehaviour
{
    public GameObject textBox;
    private TMP_Text textDisplay;

    private static string[] sentences=null;
    private static DialogSetUp[] dialogs;
    private int indexLine;
    private int indexDialog;
    private static bool start;
    private GameState state;

    public GameObject baseBody;
    public GameObject scene;
    public Image sceneBg;
    // Start is called before the first frame update
    private void Awake()
    {
        textDisplay = textBox.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        textBox.SetActive(false);
    }
    public static void StartDialog(DialogSetUp[] act)
    {
        if(!GameSystem.load)
        start = true;
        dialogs = act;
    }
    void Update()
    {
        if (sentences != null)
            if (sentences.Length != 0)
                if (textDisplay.text == sentences[indexLine])
                    if (Input.GetMouseButtonDown(0) && GameSystem.state == GameState.DIALOG)
                    {
                        NextSentence();
                    }
        if (start)
        {
            GenerateCast();
            sentences = dialogs[0].sentences;
            state = GameSystem.state;
            GameSystem.state = GameState.DIALOG;
            textBox.SetActive(true);
            StartCoroutine(Type());
            start = false;
        }
    }
    private IEnumerator Type()
    {
        foreach (char letter in sentences[indexLine].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(GameSystem.typingSpeed);
        }
    }
    public void GenerateCast()
    {
        DeleteCast();
        foreach (Sprite character in dialogs[indexDialog].characters)
        {
            GameObject body = Instantiate(baseBody);
            Image img = body.GetComponent<Image>();
            sceneBg.sprite = dialogs[indexDialog].background;
            body.transform.SetParent(scene.transform);
            body.transform.position = new Vector2(Screen.width/(dialogs[indexDialog].characters.Length+1)*(body.transform.GetSiblingIndex()+1), Screen.height/2);
            img.sprite = character;
            img.color = (character == dialogs[indexDialog].focus)?Color.white:Color.gray;
        }

    }
    public void DeleteCast()
    {
        int nbChildren = scene.transform.childCount;

        for (int i = nbChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(scene.transform.GetChild(i).gameObject);
        }
    }
    public void NextSentence()
    {
        if (indexLine < sentences.Length - 1)
        {
            indexLine++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        if (indexDialog < dialogs.Length - 1)
        {
            indexDialog++;
            sentences = dialogs[indexDialog].sentences;
            indexLine = 0;
            textDisplay.text = "";
            GenerateCast();
            StartCoroutine(Type());

        }
        else
        {
            DeleteCast();
            GameSystem.state = state;
            textBox.SetActive(false);
            textDisplay.text = "";
            sentences = null;
            indexLine = 0;
            indexDialog = 0;
        }
    }
}
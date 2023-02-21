using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Runtime.Remoting.Channels;

public class DialogueLoader : MonoBehaviour
{
    #region Classes
    [Serializable]
    public class TextRow
    {
        public int indents;
        public string value;
        public bool isPlayerDialogue, waitWithPlayerInput, hasPlayerResponse;

        public List<Response> responses;


        public TextRow(int _indents, string _value, bool _isPlayerDialogue, bool _waitWithPlayerInput, bool _hasPlayerResponse)
        {
            indents = _indents;
            value = _value;
            isPlayerDialogue = _isPlayerDialogue;
            responses = new List<Response>();
            waitWithPlayerInput = _waitWithPlayerInput;
            hasPlayerResponse = _hasPlayerResponse;
        }
    }

    [Serializable]
    public class Response 
    {
        public string value;
        public List<TextRow> followUpDialogue;

        public Response(string _value)
        {
            value = _value;
        }
    }
    #endregion

    #region General Variables/Settings
    [Header("General Settings")]
    public Transform buttonParent;
    public GameObject responseButton;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    #endregion

    #region Dialogue Settings
    [Header("Dialogue Settings")]
    public TextAsset file;
    public List<TextRow> rows;

    public float waitTime = 0.5f, readSpeed = 0.05f;
    #endregion

    #region Hidden Variables
    public static DialogueLoader singleton;
    [HideInInspector] public TextMeshProUGUI speakerText;

    string receivedInput;
    string current;

    bool inputReceived;
    bool finishedLerpingText;
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Unity Runtime Functions
    void Awake()
    {
        singleton = this;

        current = "";
        inputReceived = false;
        finishedLerpingText = false;
    }
    #endregion

    #region Custom Functions
    public void SetFile(TextAsset dialogue)
    {
        file = dialogue;
        ReadFile();
        StartCoroutine(RunFile());
    }

    void ReadFile()
    {
        string[] rowValues = file.text.Split("\n");

        int responseDepth = 0;
        int rowDepth = 0;
        for (int i = 0; i < rowValues.Length; i++)
        {
            string value = rowValues[i].Replace("\\n","\n").Replace("\t", "");
            int indents = 0;
            int index = 0;

            while (true)
            {
                index = rowValues[i].IndexOf("\t", index);
                if (index == -1)
                    break;
                indents++;
                index++;
            }

            bool isPlayerDialogue = value.StartsWith('~');
            bool waitWithPlayerInput = (isPlayerDialogue ? value.Substring(1) : value).StartsWith('|');
            bool hasPlayerResponse = value.StartsWith("~?");
            bool isResponse = value.StartsWith('-');
            string finalValue = value.Substring((waitWithPlayerInput ? 1 : 0) + (isPlayerDialogue ? 1 : 0) + (hasPlayerResponse ? 1 : 0));

            if (isResponse)
            {
                responseDepth = indents;
                Response response = new Response(finalValue);
                rows[rowDepth].responses.Add(response);
            }
            else
            {
                rowDepth = i;
                TextRow textRow = new TextRow(indents, finalValue, isPlayerDialogue, waitWithPlayerInput, hasPlayerResponse);
                if (responseDepth > indents)
                    rows[indents].responses[responseDepth].followUpDialogue.Add(textRow);
                else
                    rows.Add(textRow);
            }
        }
    }

    bool finishedLoadingRow = false;
    bool finishedLoadingResponse = false;
    bool finishedLoadingFile = false;

    IEnumerator RunFile()
    {
        finishedLoadingFile = false;
        
        speakerText.gameObject.SetActive(true);
        dialoguePanel.SetActive(true);
        GameManager.ToggleCursor(true);

        StartCoroutine(LoadRows(rows));
        yield return new WaitWhile(() => !finishedLoadingFile);

        speakerText.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
        GameManager.ToggleCursor(false);
    }

    IEnumerator LoadRows(List<TextRow> rowsToLoad)
    {
        foreach (TextRow row in rowsToLoad)
        {
            bool hasResponses = row.responses.Count != 0;
            StartCoroutine(LerpText(current, row.value, readSpeed, row.isPlayerDialogue));

            if (row.waitWithPlayerInput && !row.hasPlayerResponse)
            {
                yield return new WaitWhile(() => {
                    bool waiting = (hasResponses ? (inputReceived && finishedLerpingText && Input.GetKeyDown
                        (KeyCode.Mouse0)) : finishedLerpingText && Input.GetKeyDown(KeyCode.Space));

                    return !waiting;
                });
            }
            else if (!row.waitWithPlayerInput && !row.hasPlayerResponse)
            {
                yield return new WaitWhile(() => !(hasResponses ? (inputReceived && finishedLerpingText) : finishedLerpingText));
                yield return new WaitForSeconds(waitTime);
            }
            if (row.hasPlayerResponse)
            {
                finishedLoadingRow = false;
                LoadResponses(row.responses);
                yield return new WaitWhile(() => !finishedLoadingResponse);
            }

            current = "";
            finishedLerpingText = false;
            inputReceived = false;
        }

        if (rowsToLoad == rows)
            finishedLoadingFile = true;

        finishedLoadingRow = true;
    }

    IEnumerator LoadResponses(List<Response> responses)
    {
        List<Button> instantiateButtons = new List<Button>();

        print("run load response");
        foreach (Response response in responses)
        {
            bool hasFollowupDialogue = response.followUpDialogue.Count != 0;

            Button button = Instantiate(responseButton.gameObject, buttonParent).GetComponent<Button>();
            print(button);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = response.value;
            button.onClick.AddListener(() =>
            {
                receivedInput = response.value;
                print(receivedInput);
                inputReceived = true;
            });

            if (hasFollowupDialogue) 
            {
                finishedLoadingResponse = false;
                StartCoroutine(LoadRows(response.followUpDialogue));
                yield return new WaitWhile(() => !finishedLoadingRow);
            }
        }

        finishedLoadingResponse = true;
    }

    public IEnumerator LerpText(string a, string b, float t, bool isPlayer)
    {
        while (a != b)
        {
            string calculatedString;

            int length = a.Length + (a.Length < b.Length ? 1 : 0);
            if (b[length - 1] == '<')
            {
                int index = b.IndexOf('>', length);
                length = index + ((index == b.Length - 1) ? 1 : 2);
            }

            calculatedString = b.Substring(0, length);

            a = calculatedString;

            current = calculatedString;
            if (isPlayer)
                dialogueText.text = calculatedString;
            else
                speakerText.text = calculatedString;
            yield return new WaitForSeconds(t);
        }
        finishedLerpingText = true;
        inputReceived = true;
    }
    #endregion
}

using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;

    private float typingTime = 0.05f;
    private bool isPlayerInrange;
    private bool didDialogueStart;
    private int LineIndex;

    void Update()
    {
        // CAMBIO AQUÍ: Usamos Input.GetKeyDown(KeyCode.F) sin comillas
        if (isPlayerInrange && Input.GetKeyDown(KeyCode.F))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == dialogueLines[LineIndex])
            {
                NextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[LineIndex];
            }
        }
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueMark.SetActive(false);
        LineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(showline());
    }

    private void NextDialogueLine()
    {
        LineIndex++;
        if (LineIndex < dialogueLines.Length)
        {
            StartCoroutine(showline());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            dialogueMark.SetActive(false); // Corregido el !true por false
            Time.timeScale = 1f;
        }
    }

    private IEnumerator showline()
    {
        dialogueText.text = string.Empty;
        foreach (char ch in dialogueLines[LineIndex])
        {
            dialogueText.text += ch;
            // Usamos WaitForSecondsRealtime porque pusiste Time.timeScale = 0f
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            isPlayerInrange = true;
            dialogueMark.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            isPlayerInrange = false;
            dialogueMark.SetActive(false);
        }
    }
}

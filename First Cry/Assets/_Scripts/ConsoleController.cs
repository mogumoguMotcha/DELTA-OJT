using UnityEngine;
using TMPro;

public class ConsoleController : MonoBehaviour
{
    public TMP_Text consoleText;

    public void PrintMessage()
    {
        consoleText.text = "Hello! This is your console message.";
    }

    public void AppendMessage()
    {
        consoleText.text += "\n> New line added.";
    }

    public void ClearConsole()
    {
        consoleText.text = "";
    }
}
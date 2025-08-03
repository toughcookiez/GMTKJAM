using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSystem : MonoBehaviour
{
    protected IEnumerator WriteText(string input, TextMeshProUGUI textHolder, Color textColor, TMP_FontAsset textFont)
    {
        textHolder.color = textColor;
        textHolder.font = textFont;

        for (int i = 0; i < input.Length; i++)
        {
            textHolder.text += input[i];
            yield return new WaitForSeconds(0.05f);
        }

    }

    protected void ClearText(TextMeshProUGUI textHolder)
    {
        textHolder.text = string.Empty;
    }
}


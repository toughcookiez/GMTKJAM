using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialougeLine : TextSystem
{

    private TextMeshProUGUI textHolder;

    [Header("Text Options")]
    [SerializeField] private string[] input;
    [SerializeField] private Color _textColor;
    [SerializeField] private TMP_FontAsset _textFont;



    private int i = 0;

    private void Awake()
    {
        textHolder = GetComponent<TextMeshProUGUI>();
        if (textHolder == null )
        {
            Debug.LogWarning("textHolder is null.");
            return;
        }
        //StartCoroutine(WriteText(input[0], textHolder, _textColor, _textFont));
    }


    public void ResetDialouge()
    {
        i = 0;
    }

    public void UpdateText()
    {
        ClearText(textHolder);
        StartCoroutine(WriteText(input[i], textHolder, _textColor, _textFont));
        i++;
        if (i > input.Length )
        {
            i = 0;
        }
        
    }
}

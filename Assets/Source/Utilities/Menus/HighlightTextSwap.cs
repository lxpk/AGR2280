using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighlightTextSwap : MonoBehaviour {

    public GameObject textObject;
    public string selectedText;
    public string unselectedText;

    void Start()
    {
        if (GetComponent<TextSwapManager>())
        {
            GetComponent<TextSwapManager>().functionClass = this;
        } else
        {
            Debug.LogError(gameObject.name + ": No Text Swap Manager found! Please add one or remove this component.");
            Destroy(this);
        }
        textObject.GetComponent<Text>().text = unselectedText;
    }

    public void Select()
    {
        textObject.GetComponent<Text>().text = selectedText;
    }

    public void Unselect()
    {
        textObject.GetComponent<Text>().text = unselectedText;
    }
}

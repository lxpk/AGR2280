using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextSwapManager : Button {

    public HighlightTextSwap functionClass;

    void LateUpdate()
    {
        if (currentSelectionState == SelectionState.Highlighted || currentSelectionState == SelectionState.Pressed)
        {
            functionClass.Select();
        } else
        {
            functionClass.Unselect();
        }
    }

}

using UnityEngine;

public class MouseOverGuiElementHandler : MonoBehaviour
{
    private bool isMouseOverGuiElement;

    // Start is called before the first frame update
    void Start()
    {
        this.isMouseOverGuiElement = false;
    }

    public void SetMouseIsOverGuiElement()
    {
        this.isMouseOverGuiElement = true;
    }

    public void SetMouseExitGuiElement()
    {
        this.isMouseOverGuiElement = false;
    }

    public bool IsMouseOverGuiElement()
    {
        return this.isMouseOverGuiElement;
    }
}

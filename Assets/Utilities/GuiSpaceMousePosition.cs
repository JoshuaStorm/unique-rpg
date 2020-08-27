using UnityEngine;

namespace Assets.Utilities
{
    // Unity defines (X,Y) origin to be top-left in GUI space, but origin bottom-left for mouse position.
    // I find this cumbersome, so this is convenience function to transform mouse position into GUI space
    internal static class GuiSpaceMousePosition
    {
        public static double GetMouseX() => Input.mousePosition.x;
        public static double GetMouseY() => Screen.height - Input.mousePosition.y;

    }
}
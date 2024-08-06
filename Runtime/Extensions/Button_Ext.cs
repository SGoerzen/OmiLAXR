using TMPro;
using UnityEngine.UI;

namespace OmiLAXR.xAPI.Extensions
{
    public static class Selectable_Ext
    {
        public static void SetDisabled(this Selectable selectable, bool flag)
        {
            selectable.OnDeselect(null);
            selectable.interactable = !flag;
        }
    }
    public static class Button_Ext
    {
        public static void SetDisabled(this Button button, bool flag)
        {
            button.OnDeselect(null);
            button.interactable = !flag;
        }
        
        public static string GetTextOrDefault(this Button button, string defaultText = "")
        {
            var textMesh = button.GetComponentInChildren<TextMeshProUGUI>();
            return !textMesh ? defaultText : textMesh.text;
        }
    }
}
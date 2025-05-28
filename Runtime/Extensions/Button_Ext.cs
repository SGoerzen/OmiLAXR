using TMPro;
using UnityEngine.UI;

namespace OmiLAXR.Extensions
{
    public static class Button_Ext
    {
        public static void SetDisabled(this Button button, bool flag)
        {
            button.OnDeselect(null);
            button.interactable = !flag;
        }
        
        public static string GetTextOrDefault(this Button button, string defaultText = "")
        {
            var textMesh = button.GetComponentInChildren<TMP_Text>();
            return !textMesh ? defaultText : textMesh.text;
        }
    }
}
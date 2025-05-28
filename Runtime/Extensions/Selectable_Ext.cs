using System.Globalization;
using UnityEngine.UI;

namespace OmiLAXR.Extensions
{
    public static class Selectable_Ext
    {
        public static void SetDisabled(this Selectable selectable, bool flag)
        {
            selectable.OnDeselect(null);
            selectable.interactable = !flag;
        }

        public static string GetTrackingType(this Selectable selectable)
        {
            var typeName = selectable.GetType().Name.ToLower();
            return typeName.Replace("TMP_", "");
        }

        public static string GetValueOrDefault(this Selectable selectable, string defaultText = "")
        {
            var type = selectable.GetType();
            if (type == typeof(Button))
            {
                return ((Button)selectable).GetTextOrDefault();
            } else if (type == typeof(Toggle))
            {
                return ((Toggle)selectable).isOn.ToString();
            } else if (type == typeof(Slider))
            {
                return ((Slider)selectable).value.ToString(CultureInfo.InvariantCulture);
            }
            else if (type == typeof(InputField))
            {
                return ((InputField)selectable).text;
            }
            else if (type == typeof(Dropdown))
            {
                return ((Dropdown)selectable).value.ToString();
            }
            else if (type == typeof(Scrollbar))
            {
                return ((Scrollbar)selectable).value.ToString(CultureInfo.InvariantCulture);
            }
            else if (type == typeof(Scrollbar))
            {
                return ((Scrollbar)selectable).value.ToString(CultureInfo.InvariantCulture);
            }
            return defaultText;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.ComponentModel;

namespace ExtensionMethods
{
    public static class ClassExtensions
    {
        public static string toJSON<T>(this HashSet<T> hash) where T : Enum {
            if (hash != null)
            {
                if (hash.Count == 0)
                {
                    return "";
                }
                return "\"" + typeof(T).Name + "\"" + ":[{\"Type\":\"" + string.Join("\"},{\"Type\":\"", hash) + "\"}],";
            }
            else {
                Debug.Log(typeof(T).Name + "is Null.");
                return "";
            }
        }

        public static void ShowTextWith(this Text textComponent, string text)
        {
            Show(textComponent);
            textComponent.text = text;
        }

        public static void Hide(this Graphic component)
        {
            Color c = component.color;
            c.a = 0f;
            component.color = c;
        }

        public static void Show(this Graphic component)
        {
            Color c = component.color;
            c.a = 1f;
            component.color = c;
        }
        
        public static string getCurrentValue(this Dropdown dropdown)
        {
            return dropdown.captionText.text;
        }

        public static IEnumerator animateAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            return animateAlpha(canvasGroup, alpha, duration, null);
        }

        public static IEnumerator animateAlpha(this CanvasGroup canvasGroup, float alpha, float duration, Action finised)
        {
            float initialAlpha = canvasGroup.alpha;
            bool isValueGreaterThanCurrentAlpha = alpha > canvasGroup.alpha;
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                if (isValueGreaterThanCurrentAlpha)
                {
                    if (initialAlpha == 0)
                    {
                        canvasGroup.alpha = initialAlpha + (t / duration);
                    }
                    else
                    {
                        canvasGroup.alpha = initialAlpha + (t / duration * initialAlpha);
                    }
                }
                else
                {
                    canvasGroup.alpha = initialAlpha - (t / duration * initialAlpha);
                }
                yield return null;
            }
            canvasGroup.alpha = alpha;
            if (finised != null)
            {
                finised();
            }
        }
        public static void setStreched(this RectTransform rectTransform)
        {
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }

    public static class EnumExtensions
    {
        public static string getString(this WeaponModes weaponModes) =>
            weaponModes switch
            {
                WeaponModes.auto => "Auto",
                WeaponModes.burst => "Burst",
                WeaponModes.single => "Single",
                _ => throw new NotImplementedException("getString not implemented for WeaponMode")
            };
        

        public static string GetDescription<T>(this T enumerationValue) where T : Enum
        {
            var type = enumerationValue.GetType();
           
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }

        public static float minimumValue(this WeaponStatType weaponStatType) =>
            weaponStatType switch
            {
                WeaponStatType.FirePower => 1,
                WeaponStatType.FireRate => 0.1f,
                WeaponStatType.ReloadSpeed => 0.8f,
                WeaponStatType.Recoil => 3,
                WeaponStatType.Damage => 1,
                _ => throw new NotImplementedException("minimumValue not implemented for WeaponStatType")
            };
        

        public static float maximumValue(this WeaponStatType weaponStatType) =>
            weaponStatType switch
            {
                WeaponStatType.FirePower => 1000,
                WeaponStatType.FireRate => 4,
                WeaponStatType.ReloadSpeed => 4,
                WeaponStatType.Recoil => 50,
                WeaponStatType.Damage => 100,
                _ => throw new NotImplementedException("maximumValue not implemented for WeaponStatType")
            };

        public static bool moreIsPositive(this WeaponStatType weaponStatType) =>
            weaponStatType switch
            {
                WeaponStatType.FirePower => true,
                WeaponStatType.FireRate => true,
                WeaponStatType.ReloadSpeed => false,
                WeaponStatType.Recoil => true,
                WeaponStatType.Damage => true,
                _ => throw new NotImplementedException("moreIsPositive not implemented for WeaponStatType")
            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace CustomExtension
{
    public static class CustomExtensions
    {
        #region Linq

        public static T WhereMax<T, U>(this IEnumerable<T> items, Func<T, U> selector)
        {
            return Where(items, selector, (comparer, item1, item2) => comparer.Compare(item1, item2) > 0);
        }

        public static T WhereMin<T, U>(this IEnumerable<T> items, Func<T, U> selector)
        {
            return Where(items, selector, (comparer, item1, item2) => comparer.Compare(item1, item2) < 0);
        }

        private static T Where<T, U>(this IEnumerable<T> items, Func<T, U> selector,
            Func<Comparer<U>, U, U, bool> comparerValue)
        {
            if (!items.Any())
            {
                throw new InvalidOperationException("Empty input sequence");
            }

            var comparer = Comparer<U>.Default;
            T lookingItem = items.First();
            U lookingValue = selector(lookingItem);

            foreach (T item in items.Skip(1))
            {
                // Get the value of the item and compare it to the current max.
                U value = selector(item);
                if (comparerValue(comparer, value, lookingValue))
                {
                    lookingValue = value;
                    lookingItem = item;
                }
            }

            return lookingItem;
        }

        #endregion

        #region GameObject

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null) component = gameObject.AddComponent<T>();

            return component;
        }

        #endregion

        #region Array

        public static T[] Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }

            return array;
        }

        public static bool TryGetElement<T>(this T[] array, int index, out T element)
        {
            if (index < array.Length)
            {
                element = array[index];
                return true;
            }

            element = default(T);
            return false;
        }

        #endregion

        #region string

        public static string ClearNewLineSpaces(this string s)
        {
            string replacement = Regex.Replace(s, @"\t|\n|\r", "");
            replacement = replacement.Replace(Environment.NewLine, String.Empty);
            return replacement;
        }

        /// <summary>
        /// Format strings: regex char: "_" to space.
        /// </summary>
        /// <param name="title">string_string</param>
        /// <returns>formated: string string</returns>
        public static string FormatEnumTitle(this string title)
        {
            string pattern = "_";
            string replacement = " ";

            Regex regEx = new Regex(pattern);
            return regEx.Replace(title, replacement);
        }
        
        public static string Replace(this string s, char[] separators, string newVal)
        {
            string[] temp;

            temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return String.Join( newVal, temp );
        }

        #endregion

        #region Dictionary

        public static KeyValuePair<TKey, TValue> GetEntry<TKey, TValue>
        (this IDictionary<TKey, TValue> dictionary,
            TKey key)
        {
            return new KeyValuePair<TKey, TValue>(key, dictionary[key]);
        }

        #endregion

        #region Transform

        public static void Clear(this Transform transform)
        {
            for (var i = 0; i < transform.childCount; i++) Object.Destroy(transform.GetChild(i).gameObject);
        }

        #endregion

        #region Color

        /// <summary>
        /// Change Color brightness ONLY. 
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="V">Brightness value [0..1].</param>
        public static void Brightness(this Graphic graphic, float v)
        {
            float H, S, V;
            Color.RGBToHSV(graphic.color, out H, out S, out V);
            graphic.color = Color.HSVToRGB(H, S, v);
        }

        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            var color = graphic.color;
            graphic.color = new Color(color.r, color.g, color.b, alpha);
        }

        public static string ToRGBHex(Color c)
        {
            return $"#{ToByte(c.r):X2}{ToByte(c.g):X2}{ToByte(c.b):X2}";

            byte ToByte(float f)
            {
                f = Mathf.Clamp01(f);
                return (byte) (f * 255);
            }
        }

        #endregion



        #region mesh

        public static Vector3[] CalculateNormals(this Mesh mesh)
        {
            return mesh.normals.Distinct().ToArray();
        }

        #endregion

        #region Date

        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        #endregion
    }
}
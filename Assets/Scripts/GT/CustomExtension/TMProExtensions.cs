using System;
using System.Collections;

namespace CustomExtension
{
    public static class TMProExtensions
    {
        /// <summary>
        /// Score animation effect: Increasing / Decreasing number.
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="prefix"></param>
        /// <param name="postfix"></param>
        /// <param name="current">current score value</param>
        /// <param name="value"> new score value</param>
        /// <param name="isInt">round to integer</param>
        /// <param name="complete">on complete action</param>
        /// <returns></returns>
        public static IEnumerator ScoreAnimationEffect(this TMPro.TMP_Text tmp, string prefix, string postfix,
            double current, double value, bool isInt, Action complete = null)
        {
            bool increase = value > current;

            double frameStep = 0F;
            if (increase)
            {
                value = value - current;
                frameStep = value / 240F;

                while (value > 0)
                {
                    value -= frameStep;
                    if (value < 0) frameStep += value;

                    current += frameStep;
                    tmp.text = isInt ? $"{prefix}{current:0}{postfix}" : $"{prefix}{current:0.0}{postfix}";

                    yield return null;
                }
            }
            else
            {
                value = current - value;
                frameStep = value / 240F;

                while (value > 0)
                {
                    value -= frameStep;
                    if (value < 0) frameStep += value;

                    current -= frameStep;
                    tmp.text = isInt ? $"{prefix}{current:0}{postfix}" : $"{prefix}{current:0.0}{postfix}";

                    yield return null;
                }
            }

            complete?.Invoke();
        }
        
        public static bool IsEmpty(this TMPro.TMP_InputField inputField)
        {
            return string.IsNullOrEmpty(inputField.text);
        }
    }
}
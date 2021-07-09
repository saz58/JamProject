using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CustomExtension
{
    public static partial class CustomAnimations
    {
        /// <summary>
        /// Rotate UI container by axis.
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="id"></param>
        /// <param name="quaternion"></param>
        /// <param name="duration"></param>
        /// <param name="anglePointOnProcess">angle between start and finish for action.</param> // used for swap sprites inside container.
        /// <param name="duringProcessAction"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public static IEnumerator UIOpenCard(this Transform tr, byte id, Quaternion quaternion, float duration,
            float anglePointOnProcess, Action<byte> duringProcessAction, Action completed)
        {
            float currentTime = 0f;
            Quaternion origin = tr.localRotation;

            yield return new WaitForSeconds(UnityEngine.Random.value * .4f);

            while (currentTime <= duration)
            {
                currentTime += Time.deltaTime;
                tr.localRotation = Quaternion.Lerp(origin, quaternion, currentTime / duration);

                if (duringProcessAction != null && Quaternion.Angle(origin, tr.localRotation) >= anglePointOnProcess)
                {
                    duringProcessAction.Invoke(id);
                    duringProcessAction = null;
                }

                yield return null;
            }

            tr.localRotation = quaternion;
            completed?.Invoke();
        }


        public static IEnumerator Scale(this Transform tr, Vector3 targetScale, float duration,
            float preStartWaitDuration = 0, Action completed = null,
            EasingFunction.Ease ease = EasingFunction.Ease.Linear)
        {
            Vector3 origin = tr.localScale;
            yield return new WaitForSeconds(preStartWaitDuration);
            float currentTime = 0f;

            var easeFn = EasingFunction.GetEasingFunction(ease);

            while (currentTime <= duration)
            {
                currentTime += Time.deltaTime;
                tr.localScale = Vector3.Lerp(origin, targetScale, easeFn.Invoke(0, 1, currentTime / duration));

                yield return null;
            }

            tr.localScale = targetScale;
            completed?.Invoke();
        }

        public static void CycleScale(this Transform tr, MonoBehaviour context, byte iterations, Vector3 scale,
            float cycleDuration, Action completed = null)
        {
            context.StartCoroutine(tr.Scale(scale, cycleDuration, completed: () =>
                context.StartCoroutine(tr.Scale(Vector3.one, cycleDuration, completed: () =>
                {
                    iterations--;
                    if (iterations > 0)
                        CycleScale(tr, context, iterations, scale, cycleDuration, completed);
                    else
                        completed?.Invoke();
                }))));
        }

        public static IEnumerator StepMove(this Transform tr, Vector3 moveTo, float speed, Action complete = null)
        {
            while (Mathf.Abs(Vector3.Distance(tr.position, moveTo)) >= 0.001f)
            {
                float step = speed * Time.deltaTime;
                tr.position = Vector3.MoveTowards(tr.position, moveTo, step);

                yield return null;
            }

            complete?.Invoke();
        }

        public static IEnumerator LocalStepMove(this Transform tr, Vector3 moveTo, float speed, Action complete = null)
        {
            while (Mathf.Abs(Vector3.Distance(tr.localPosition, moveTo)) >= 0.001f)
            {
                float step = speed * Time.deltaTime;
                tr.localPosition = Vector3.MoveTowards(tr.localPosition, moveTo, step);

                yield return null;
            }

            complete?.Invoke();
        }

        /// <summary>
        /// Move gameobject from a - to - b. With ease.
        /// </summary>
        /// <param name="tr">transform of gameobject</param>
        /// <param name="moveTo">target position</param>
        /// <param name="duration">time</param>
        /// <param name="completed">callback</param>
        /// <param name="ease">type of easing</param>
        public static IEnumerator EaseMove(this Transform tr, Vector3 moveTo, float duration,
            Action completed = null, EasingFunction.Ease ease = EasingFunction.Ease.Linear)
        {
            var easeFn = EasingFunction.GetEasingFunction(ease);
            Vector3 originPos = tr.position;
            float t = 0f;
            while (t <= duration)
            {
                t += Time.deltaTime;

                var normalizedValue = easeFn.Invoke(0F, 1F, t / duration);
                tr.position = Vector3.Lerp(originPos, moveTo, normalizedValue);

                yield return null;
            }

            tr.position = moveTo;
            completed?.Invoke();
        }


        public static IEnumerator LocalEaseMove(this Transform tr, Vector3 moveTo, float duration,
            Action completed = null,
            EasingFunction.Ease ease = EasingFunction.Ease.Linear)
        {
            var easeFn = EasingFunction.GetEasingFunction(ease);
            Vector3 originPos = tr.localPosition;
            float t = 0f;
            while (t <= duration)
            {
                t += Time.deltaTime;

                var normalizedValue = easeFn.Invoke(0F, 1F, t / duration);
                tr.localPosition = Vector3.Lerp(originPos, moveTo, normalizedValue);

                yield return null;
            }

            tr.localPosition = moveTo;
            completed?.Invoke();
        }

        /// <summary>
        /// Method move and rotate Transform.
        /// </summary>
        /// <param name="tr">Transform</param>
        /// <param name="moveTo">target position</param>
        /// <param name="rotateTo">target rotation</param>
        /// <param name="duration">time</param>
        /// <param name="completed">complete Action</param>
        /// <param name="ease">easing function</param>
        public static IEnumerator MoveAndRotateTo(this Transform tr, Vector3 moveTo, Quaternion rotateTo,
            float duration,
            Action completed = null, EasingFunction.Ease ease = EasingFunction.Ease.Linear)
        {
            Vector3 originPos = tr.position;
            Quaternion originRotation = tr.rotation;
            var easeFn = EasingFunction.GetEasingFunction(ease);
            float t = 0F;
            while (t <= duration)
            {
                t += Time.deltaTime;

                var normalizedValue = easeFn.Invoke(0F, 1F, t / duration);
                tr.position = Vector3.Lerp(originPos, moveTo, normalizedValue);
                tr.rotation = Quaternion.Lerp(originRotation, rotateTo, normalizedValue);

                yield return null;
            }

            completed?.Invoke();
        }

        public static void MoveLookAt(this Transform tr, Vector3 target, Transform lookAt, float duration,
            Action completed = null)
        {
            var context = tr.GetComponent<MonoBehaviour>();
            context.StartCoroutine(LoopLook());

            IEnumerator LoopLook()
            {
                float currentTime = 0f;
                Vector3 pos;
                while (currentTime <= duration)
                {
                    currentTime += Time.deltaTime;
                    var normalizedValue = currentTime / duration;

                    pos = tr.position = Vector3.Lerp(tr.position, target, normalizedValue);

                    tr.LookAt(lookAt);
                    if (Vector3.Distance(pos, target) < 0.05f)
                    {
                        tr.position = target;
                        completed?.Invoke();
                        break;
                    }

                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public static IEnumerator RotateDuringTime(this Transform tr, Vector3 axis, float duration, float angleStep,
            Action completed = null)
        {
            float currentTime = 0f;

            while (currentTime <= duration)
            {
                currentTime += Time.deltaTime;
                tr.Rotate(axis, angleStep);
                yield return null;
            }

            completed?.Invoke();
        }

        public static IEnumerator RotateByAngle(this Transform tr, Quaternion rotateTo, float duration,
            float preStartWaitDuration = 0, Action complete = null, bool local = false, bool shortWay = true)
        {
            yield return new WaitForSeconds(preStartWaitDuration);

            float t = 0f;
            Quaternion origin = tr.rotation;
            if (local) // todo: local move to own method.
                origin = tr.localRotation;

            while (t <= duration)
            {
                t += Time.deltaTime;
                var normalizedValue = t / duration;

                if (local)
                    tr.localRotation = Quaternion.Lerp(origin, rotateTo, normalizedValue);
                else // todo: Explore Extension.Lerp: short / long way rotations.
                    tr.rotation = QuaternionExtension.Lerp(origin, rotateTo, normalizedValue, shortWay);

                yield return null;
            }

            if (local)
                tr.localRotation = rotateTo;
            else
                tr.rotation = rotateTo;

            complete?.Invoke();
        }

        /// <summary>
        ///  Rotate transform around point. 
        /// </summary>
        /// <param name="tr">transform</param>
        /// <param name="point">point around rotation</param>
        /// <param name="axis">axis rotation</param>
        /// <param name="angle">angle distance</param>
        /// <param name="duration">time</param>
        /// <param name="lookAt">Transform for look at during rotation</param>
        /// <param name="completed">On complete callback</param>
        public static IEnumerator RotateAroundPoint(this Transform tr, Vector3 point, Vector3 axis, float angle,
            float duration, Transform lookAt = null, Action completed = null)
        {
            bool lookExist = lookAt != null;
            float step = 0.0f;
            float rate = 1.0f / duration;
            float smoothStep;
            float lastStep = 0.0f;
            while (step < 1.0)
            {
                if (lookExist) tr.LookAt(lookAt);

                step += Time.deltaTime * rate;
                smoothStep = Mathf.SmoothStep(0.0f, 1.0f, step);
                tr.RotateAround(point, axis, angle * (smoothStep - lastStep));
                lastStep = smoothStep;

                yield return null;
            }

            if (step > 1.0) tr.RotateAround(point, axis, angle * (1.0f - lastStep));
            completed?.Invoke();
        }

        public static IEnumerator RotateAroundTransform(this Transform tr, Transform targetTr, Vector3 axis,  float duration, float angle,Action completed = null)
        {
            float step = 0.0f;
            float rate = 1.0f / duration;
            float smoothStep;
            float lastStep = 0.0f;
            while (step < 1.0)
            {
                step += Time.deltaTime * rate;
                smoothStep = Mathf.Lerp(0.0f, 1.0f, step);
                tr.RotateAround(targetTr.position, axis, angle * (smoothStep - lastStep));
                lastStep = smoothStep;

                yield return null;
            }

            completed?.Invoke();
        }


        public static IEnumerator RotateAroundByDirection(this Transform tr, float duration, float cycles,
            Vector3 direction, EasingFunction.Ease ease = EasingFunction.Ease.Linear, Action completed = null)
        {
            var easeFn = EasingFunction.GetEasingFunction(ease);
            Quaternion origin = tr.rotation;
            float t = 0.0f;
            float a360 = 360.0f;
            a360 *= cycles;
            while (t < duration)
            {
                t += Time.deltaTime;

                tr.rotation = origin * Quaternion.AngleAxis(easeFn.Invoke(0, 1, t / duration) * a360, direction);
                yield return null;
            }
            // tr.rotation = origin;
            completed?.Invoke();
        }


        public static IEnumerator RotateTowards(this Transform tr, Quaternion to, float t, Action completed = null)
        {
            float currentTime = 0f;
            while (currentTime <= t)
            {
                currentTime += Time.deltaTime;
                tr.rotation = Quaternion.Slerp(tr.rotation, to, currentTime / t);

                yield return null;
            }

            completed?.Invoke();
            tr.rotation = to;
        }

        /// <summary>
        /// Executes during time.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fn">invokes with normalized time value</param>
        public static IEnumerator ExecuteDuringTime(float t, System.Action<float> fn)
        {
            float currentTime = 0f;

            while (currentTime <= t)
            {
                currentTime += Time.deltaTime;
                fn.Invoke(currentTime / t);
                yield return null;
            }
        }

        public static IEnumerator Fade(this Graphic gr, float targetAlpha, float duration, Action completed = null)
        {
            float t = 0;
            float startAlpha = gr.color.a;
            while (t < duration)
            {
                t += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
                gr.color = new Color(gr.color.r, gr.color.g, gr.color.b, newAlpha);

                yield return null;
            }

            completed?.Invoke();
        }


        public static void DelayCoroutine(this MonoBehaviour mono, float delay, Action complete)
        {
            mono.StartCoroutine(Delay());

            IEnumerator Delay()
            {
                yield return new WaitForSeconds(delay);
                complete?.Invoke();
            }
        }
    }
}
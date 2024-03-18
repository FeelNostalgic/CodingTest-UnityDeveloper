using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodingTest.Utility
{
    public static class ExtendedMethods
    {
        #region Transform

        public static void AddMoveToX(this Transform t, float moveX)
        {
            t.position = new Vector3(t.position.x + moveX, t.position.y, t.position.z);
        }
        
        public static void AddMoveToY(this Transform t, float moveY)
        {
            t.position = new Vector3(t.position.x, t.position.y + moveY, t.position.z);
        }

        public static void SetZ(this Transform t, float z)
        {
            t.position = new Vector3(t.position.x, t.position.y, z);
        }
        
        public static void SetY(this Transform t, float y)
        {
            t.position = new Vector3(t.position.x, y, t.position.z);
        }

        public static void SetZLocalRotation(this Transform t, float zRotation)
        {
            t.localRotation = Quaternion.Euler(new Vector3(t.transform.localRotation.eulerAngles.x, t.transform.localRotation.eulerAngles.y, zRotation));
        }

        public static void AddZLocalRotation(this Transform t, float zToAdd)
        {
            t.localRotation = Quaternion.Euler(new Vector3(t.transform.localRotation.eulerAngles.x, t.transform.localRotation.eulerAngles.y, t.transform.localRotation.eulerAngles.z + zToAdd));
        }
        
        public static void SetXLocalRotation(this Transform t, float xRotation)
        {
            t.localRotation = Quaternion.Euler(new Vector3(xRotation, t.transform.localRotation.eulerAngles.y, t.transform.localRotation.eulerAngles.z));
        }

        public static float AnglePointingTowards(this Transform t, Vector3 towards)
        {
            return Mathf.Atan2(towards.y - t.position.y, towards.x - t.position.x) * Mathf.Rad2Deg;
        }

        public static float InverseAnglePointingTowards(this Transform t, Vector3 towards)
        {
            return Mathf.Atan2(t.position.y - towards.y , t.position.x - towards.x) * Mathf.Rad2Deg;
        }
        
        #endregion

        #region Color
        
        public static Color SetAlpha(this Color c, float alpha)
        {
            var newColor = c;
            newColor.a = alpha;
            return newColor;
        }

        #endregion
        
        #region Image
        
        public static void SetAlpha(this Image image, float alpha)
        {
            var newColor = image.color.SetAlpha(alpha);
            image.color = newColor;
        }
        
        #endregion

        #region int

        public static string KiloFormat(this int num)
        {
            if (num >= 100000000)
                return (num / 1000000).ToString("#,0M");

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0K");

            if (num >= 10000)
                return (num / 1000).ToString("0.#") + "K";

            return num.ToString("#,0");
        }

        #endregion

        #region List

        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count == 0;
        }

        #endregion

        #region Vector2

        public static Vector2 RandomNormalizedDirection(this Vector2 v2)
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
        
        public static Vector2 NormalClockwise(this Vector2 v2)
        {
            return new Vector2(v2.y, -v2.x);
        } 
        
        public static Vector2 NormalCounterClockwise(this Vector2 v2)
        {
            return new Vector2(-v2.y, v2.x);
        } 

        #endregion

        #region bool

        public static void Not(this bool b)
        {
            b = !b;
        }

        #endregion
        
        #region RectTransform
        
        public static Vector3 GetVector3X(this RectTransform rt, float newX)
        {
            return new Vector3(newX, rt.anchoredPosition3D.y, rt.anchoredPosition3D.z);
        }
        
        public static Vector3 GetVector3Y(this RectTransform rt, float newY)
        {
            return new Vector3(rt.anchoredPosition3D.x, newY, rt.anchoredPosition3D.z);
        }
        
        /// <summary>
        /// Animate a rectTransform scale
        /// </summary>
        public static void AnimateRectTransformScale(this RectTransform rectTransform, Vector3 startingScale, Vector3 finalScale, float duration, Ease ease, TweenCallback onPlayAction = null, TweenCallback onCompleteAction = null)
        {
            rectTransform.localScale = startingScale;
            rectTransform.DOScale(finalScale, duration).SetEase(ease).SetUpdate(true).OnPlay(onPlayAction).OnComplete(onCompleteAction).Play();
        }
        
        /// <summary>
        /// Animate a rectTransform position
        /// </summary>
        public static void AnimateRectTransformPosition(this RectTransform rectTransform, Vector3 startingPosition, Vector3 finalPosition, float duration, Ease ease, TweenCallback onPlayAction = null, TweenCallback onCompleteAction = null)
        {
            rectTransform.anchoredPosition3D = startingPosition;
            rectTransform.DOAnchorPos3D(finalPosition, duration).SetEase(ease).SetUpdate(true).OnPlay(onPlayAction).OnComplete(onCompleteAction).Play();
        }
        
        #endregion
    }
}
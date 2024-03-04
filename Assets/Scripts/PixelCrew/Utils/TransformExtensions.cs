using UnityEngine;

namespace PixelCrew.Utils
{
    public static class TransformExtensions
    {
        public static Vector3 GetScaleSign(this Transform transform)
        {
            var lossyScale = transform.lossyScale;

            return new Vector3(Mathf.Sign(lossyScale.x), Mathf.Sign(lossyScale.y), Mathf.Sign(lossyScale.z));
        }

        public static void AdjustScale(this Transform transform, Transform origin)
        {
            var signs = origin.GetScaleSign();
            var localScale = transform.localScale;

            localScale.x *= signs.x;
            localScale.y *= signs.y;
            localScale.z *= signs.z;

            transform.localScale = localScale;
        }
    }
}
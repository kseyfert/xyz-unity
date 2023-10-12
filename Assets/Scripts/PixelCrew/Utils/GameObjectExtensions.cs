using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GameObjectExtensions
    {
        public static bool IsInLayer(this GameObject gameObject, LayerMask layerMask)
        {
            return layerMask == (layerMask | (1 << gameObject.layer));
        }

        public static bool CompareTags(this GameObject gameObject, IEnumerable<string> tags)
        {
            return tags.Any(gameObject.CompareTag);
        }

        public static bool CompareTags(this GameObject gameObject, string tags, char separator = ',')
        {
            var split = tags.Split(separator);
            for (var i = 0; i < split.Length; i++)
            {
                split[i] = Regex.Replace(split[i], @"^(\s*)", "");
                split[i] = Regex.Replace(split[i], @"(\s*)$", "");
            }

            return gameObject.CompareTags(split);
        }
    }
}
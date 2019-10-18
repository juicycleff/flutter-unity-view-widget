using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
    interface IPropertyKeyDataSource
    {
        float[] GetKeys(); // Get the keys
        Dictionary<float, string> GetDescriptions(); // Caches for descriptions
    }

    abstract class BasePropertyKeyDataSource : IPropertyKeyDataSource
    {
        static readonly StringBuilder k_StringBuilder = new StringBuilder();

        protected abstract AnimationClip animationClip { get; }

        public virtual float[] GetKeys()
        {
            if (animationClip == null)
                return null;

            var info =  AnimationClipCurveCache.Instance.GetCurveInfo(animationClip);
            return info.keyTimes.Select(TransformKeyTime).ToArray();
        }

        public virtual Dictionary<float, string> GetDescriptions()
        {
            var map = new Dictionary<float, string>();
            var info = AnimationClipCurveCache.Instance.GetCurveInfo(animationClip);
            var processed = new HashSet<string>();

            foreach (var b in info.bindings)
            {
                var groupID = b.GetGroupID();
                if (processed.Contains(groupID))
                    continue;

                var group = info.GetGroupBinding(groupID);
                var prefix = AnimationWindowUtility.GetNicePropertyGroupDisplayName(b.type, b.propertyName);

                foreach (var t in info.keyTimes)
                {
                    k_StringBuilder.Length = 0;

                    var key = TransformKeyTime(t);
                    if (map.ContainsKey(key))
                        k_StringBuilder.Append(map[key])
                            .Append('\n');

                    k_StringBuilder.Append(prefix)
                        .Append(" : ")
                        .Append(group.GetDescription(key));

                    map[key] = k_StringBuilder.ToString();
                }
                processed.Add(groupID);
            }

            return map;
        }

        protected virtual float TransformKeyTime(float keyTime)
        {
            return keyTime;
        }
    }
}

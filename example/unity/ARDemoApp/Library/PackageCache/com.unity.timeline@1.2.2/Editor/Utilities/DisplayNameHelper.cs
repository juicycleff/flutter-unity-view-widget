using System.Text;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    static class DisplayNameHelper
    {
        static readonly string k_NoAssetDisplayName = L10n.Tr("<No Asset>");
        static readonly string k_ReadOnlyDisplayName = L10n.Tr("[Read Only]");
        static readonly StringBuilder k_StringBuilder = new StringBuilder();

        public static string GetDisplayName(ISequenceState sequence)
        {
            string displayName = sequence.director != null ? GetDisplayName(sequence.director) : GetDisplayName(sequence.asset);
            if (sequence.asset != null && sequence.isReadOnly)
                displayName += " " + k_ReadOnlyDisplayName;
            return displayName;
        }

        public static string GetDisplayName(PlayableAsset asset)
        {
            return asset != null ? asset.name : k_NoAssetDisplayName;
        }

        public static string GetDisplayName(PlayableDirector director)
        {
            k_StringBuilder.Length = 0;
            k_StringBuilder.Append(GetDisplayName(director.playableAsset));
            k_StringBuilder.Append(" (").Append(director.name).Append(')');
            return k_StringBuilder.ToString();
        }
    }
}

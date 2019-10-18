using UnityEngine;

namespace UnityEditor.Timeline.Signals
{
    static class Styles
    {
        public static readonly GUIContent RetroactiveLabel = EditorGUIUtility.TrTextContent("Retroactive", "Use retroactive to emit this signal even if playback starts afterwards.");
        public static readonly GUIContent EmitOnceLabel = EditorGUIUtility.TrTextContent("Emit Once", "Emit the signal once during loops.");
        public static readonly GUIContent EmitSignalLabel = EditorGUIUtility.TrTextContent("Emit Signal", "Select which Signal Asset to emit.");
        public static readonly GUIContent ObjectLabel = EditorGUIUtility.TrTextContent("Receiver Component on", "The Signal Receiver Component on the bound GameObject.");

        public static readonly GUIContent CreateNewSignal = EditorGUIUtility.TrTextContent("Create Signal…");
        public static readonly GUIContent AddSignalReceiverComponent = EditorGUIUtility.TrTextContent("Add Signal Receiver", "Creates a Signal Receiver component on the track binding and the reaction for the current signal.");
        public static readonly GUIContent EmptySignalList = EditorGUIUtility.TrTextContent("None");
        public static readonly GUIContent AddReactionButton = EditorGUIUtility.TrTextContent("Add Reaction");

        public static readonly GUIContent NewSignalWindowTitle = EditorGUIUtility.TrTextContent("Create Signal Key");
        public static readonly GUIContent NewSignalDefaultName = EditorGUIUtility.TrTextContent("New Signal");
        public static readonly GUIContent NewSignalWindowMessage = EditorGUIUtility.TrTextContent("Create Signal Key");

        public static readonly string SignalListDuplicateOption = L10n.Tr("Duplicate");
        public static readonly string SignalListDeleteOption = L10n.Tr("Delete");
        public static readonly string NoBoundGO = L10n.Tr("Track has no bound GameObject.");
        public static readonly string MultiEditNotSupportedOnDifferentBindings = L10n.Tr("Multi-edit not supported for SignalReceivers on tracks bound to different GameObjects.");
        public static readonly string MultiEditNotSupportedOnDifferentSignals = L10n.Tr("Multi-edit not supported for SignalReceivers when SignalEmitters use different Signals.");

        public static readonly string UndoCreateSignalAsset = L10n.Tr("Create New Signal Asset");
        public static readonly string UndoAddReaction = L10n.Tr("Add Signal Receiver Reaction");
        public static readonly string NoReaction = L10n.Tr("No reaction for {0} has been defined in this receiver");
        public static readonly string NoSignalReceiverComponent = L10n.Tr("There is no Signal Receiver component on {0}");
        public static readonly string ProjectHasNoSignalAsset = L10n.Tr("Your project contains no Signal assets");

        //Icons
        public static readonly GUIStyle OptionsStyle = DirectorStyles.GetGUIStyle("Icon.Options");
        public static readonly GUIContent SignalEmitterIcon = EditorGUIUtility.IconContent("SignalEmitter Icon");
    }
}

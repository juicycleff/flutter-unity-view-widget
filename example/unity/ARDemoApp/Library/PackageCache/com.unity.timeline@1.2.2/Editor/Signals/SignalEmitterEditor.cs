using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline.Signals
{
    [CustomTimelineEditor(typeof(SignalEmitter))]
    class SignalEmitterEditor : MarkerEditor
    {
        static readonly string MissingAssetError = LocalizationDatabase.GetLocalizedString("No signal assigned");

        public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
        {
            var options = base.GetMarkerOptions(marker);
            SignalEmitter emitter = (SignalEmitter)marker;
            if (emitter.asset != null)
                options.tooltip = emitter.asset.name;
            else
                options.errorText = MissingAssetError;

            return options;
        }
    }
}

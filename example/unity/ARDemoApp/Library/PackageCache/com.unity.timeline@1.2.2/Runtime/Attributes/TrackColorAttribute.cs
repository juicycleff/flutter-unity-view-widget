using System;
using UnityEngine;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Attribute used to specify the color of the track and its clips inside the Timeline Editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TrackColorAttribute : Attribute
    {
        Color m_Color;

        /// <summary>
        ///
        /// </summary>
        public Color color
        {
            get { return m_Color; }
        }

        /// <summary>
        /// Specify the track color using [0-1] R,G,B values.
        /// </summary>
        /// <param name="r">Red value [0-1].</param>
        /// <param name="g">Green value [0-1].</param>
        /// <param name="b">Blue value [0-1].</param>
        public TrackColorAttribute(float r, float g, float b)
        {
            m_Color = new Color(r, g, b);
        }
    }
}

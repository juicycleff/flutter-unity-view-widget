// source https://gist.github.com/sebschaef/b803da53217c88e8c691aeed08602193

package com.xraph.plugin.flutter_unity_widget

import android.view.MotionEvent

/*
  Copies a MotionEvent. Use the named parameters to modify immutable properties.
  Don't forget to recycle the original event if it is not used anymore.
*/
fun MotionEvent.copy(
    downTime: Long = getDownTime(),
    eventTime: Long = getEventTime(),
    action: Int = getAction(),
    pointerCount: Int = getPointerCount(),
    pointerProperties: Array<MotionEvent.PointerProperties>? =
        (0 until getPointerCount())
            .map { index ->
                MotionEvent.PointerProperties().also { pointerProperties ->
                    getPointerProperties(index, pointerProperties)
                }
            }
            .toTypedArray(),
    pointerCoords: Array<MotionEvent.PointerCoords>? =
        (0 until getPointerCount())
            .map { index ->
                MotionEvent.PointerCoords().also { pointerCoords ->
                    getPointerCoords(index, pointerCoords)
                }
            }
            .toTypedArray(),
    metaState: Int = getMetaState(),
    buttonState: Int = getButtonState(),
    xPrecision: Float = getXPrecision(),
    yPrecision: Float = getYPrecision(),
    deviceId: Int = getDeviceId(),
    edgeFlags: Int = getEdgeFlags(),
    source: Int = getSource(),
    flags: Int = getFlags()
): MotionEvent =
    MotionEvent.obtain(
        downTime,
        eventTime,
        action,
        pointerCount,
        pointerProperties,
        pointerCoords,
        metaState,
        buttonState,
        xPrecision,
        yPrecision,
        deviceId,
        edgeFlags,
        source,
        flags
    )
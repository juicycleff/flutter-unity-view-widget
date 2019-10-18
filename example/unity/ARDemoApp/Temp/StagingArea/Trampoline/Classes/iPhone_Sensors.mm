#define SIMULATE_ATTITUDE_FROM_GRAVITY 1

#import "iPhone_Sensors.h"

#if UNITY_USES_LOCATION
#import <CoreLocation/CoreLocation.h>
#endif

#if !PLATFORM_TVOS
#import <CoreMotion/CoreMotion.h>
#endif
#import <GameController/GameController.h>

#include "OrientationSupport.h"
#include "Unity/UnityInterface.h"

#include "Vector3.h"
#include "Quaternion4.h"


typedef void (^ControllerPausedHandler)(GCController *controller);
static NSArray* QueryControllerCollection();

#if PLATFORM_TVOS
static bool gTVRemoteTouchesEnabled = true;
static bool gTVRemoteAllowRotationInitialValue = false;
static bool gTVRemoteReportsAbsoluteDpadValuesInitialValue = false;
#endif

static bool gCompensateSensors = true;
bool gEnableGyroscope = false;
extern "C" void UnityEnableGyroscope(bool value) { gEnableGyroscope = value; }

static bool gJoysticksInited = false;
#define MAX_JOYSTICKS 4
static bool gPausedJoysticks[MAX_JOYSTICKS] = {false, false, false, false};
static id gGameControllerClass = nil;
// This defines the number of maximum acceleration events Unity will queue internally for scripts to access.
extern "C" int UnityMaxQueuedAccelerationEvents() { return 2 * 60; } // 120 events or 2 seconds at 60Hz reporting.

static ControllerPausedHandler gControllerHandler = ^(GCController *controller)
{
    NSArray* list = QueryControllerCollection();
    if (list != nil)
    {
        NSUInteger idx = [list indexOfObject: controller];
        if (idx < MAX_JOYSTICKS)
        {
            gPausedJoysticks[idx] = !gPausedJoysticks[idx];
        }
    }
};

extern "C" bool IsCompensatingSensors() { return gCompensateSensors; }
extern "C" void SetCompensatingSensors(bool val) { gCompensateSensors = val; }

inline float UnityReorientHeading(float heading)
{
    if (IsCompensatingSensors())
    {
        float rotateBy = 0.f;
        switch (UnityCurrentOrientation())
        {
            case portraitUpsideDown:
                rotateBy = -180.f;
                break;
            case landscapeLeft:
                rotateBy = -270.f;
                break;
            case landscapeRight:
                rotateBy = -90.f;
                break;
            default:
                break;
        }

        return fmodf((360.f + heading + rotateBy), 360.f);
    }
    else
    {
        return heading;
    }
}

inline Vector3f UnityReorientVector3(float x, float y, float z)
{
    if (IsCompensatingSensors())
    {
        Vector3f res;
        switch (UnityCurrentOrientation())
        {
            case portraitUpsideDown:
            { res = (Vector3f) {-x, -y, z}; }
            break;
            case landscapeLeft:
            { res = (Vector3f) {-y, x, z}; }
            break;
            case landscapeRight:
            { res = (Vector3f) {y, -x, z}; }
            break;
            default:
            { res = (Vector3f) {x, y, z}; }
        }
        return res;
    }
    else
    {
        return (Vector3f) {x, y, z};
    }
}

inline Quaternion4f UnityReorientQuaternion(float x, float y, float z, float w)
{
    if (IsCompensatingSensors())
    {
        Quaternion4f res, inp = {x, y, z, w};
        switch (UnityCurrentOrientation())
        {
            case landscapeLeft:
                QuatMultiply(res, inp, gQuatRot[1]);
                break;
            case portraitUpsideDown:
                QuatMultiply(res, inp, gQuatRot[2]);
                break;
            case landscapeRight:
                QuatMultiply(res, inp, gQuatRot[3]);
                break;
            default:
                res = inp;
        }
        return res;
    }
    else
    {
        return (Quaternion4f) {x, y, z, w};
    }
}

#if PLATFORM_TVOS
static bool sGCMotionForwardingEnabled = false;
static bool sGCMotionForwardedForCurrentFrame = false;
#else
static CMMotionManager*     sMotionManager  = nil;
static NSOperationQueue*    sMotionQueue    = nil;
#endif

// Current update interval or 0.0f if not initialized. This is returned
// to the user as current update interval and this value is set to 0.0f when
// gyroscope is disabled.
static float sUpdateInterval = 0.0f;

// Update interval set by the user. Core motion will be set-up to use
// this update interval after disabling and re-enabling gyroscope
// so users can set update interval, disable gyroscope, enable gyroscope and
// after that gyroscope will be updated at this previously set interval.
#if !PLATFORM_TVOS
static float sUserUpdateInterval = 1.0f / 30.0f;
#endif

void SensorsCleanup()
{
#if !PLATFORM_TVOS
    if (sMotionManager != nil)
    {
        [sMotionManager stopGyroUpdates];
        [sMotionManager stopDeviceMotionUpdates];
        [sMotionManager stopAccelerometerUpdates];
        sMotionManager = nil;
    }

    sMotionQueue = nil;
#endif
}

extern "C" void UnityCoreMotionStart()
{
#if PLATFORM_TVOS
    sGCMotionForwardingEnabled = true;
#else
    if (sMotionQueue == nil)
        sMotionQueue = [[NSOperationQueue alloc] init];

    bool initMotionManager = (sMotionManager == nil);
    if (initMotionManager)
        sMotionManager = [[CMMotionManager alloc] init];

    if (gEnableGyroscope && sMotionManager.gyroAvailable)
    {
        [sMotionManager startGyroUpdates];
        [sMotionManager setGyroUpdateInterval: sUpdateInterval];
    }

    if (gEnableGyroscope && sMotionManager.deviceMotionAvailable)
    {
        [sMotionManager startDeviceMotionUpdates];
        [sMotionManager setDeviceMotionUpdateInterval: sUpdateInterval];
    }

    // we (ab)use UnityCoreMotionStart to both init sensors and restart gyro
    // make sure we touch accelerometer only on init
    if (initMotionManager && sMotionManager.accelerometerAvailable)
    {
        const int frequency = UnityGetAccelerometerFrequency();
        if (frequency > 0)
        {
            sMotionManager.accelerometerUpdateInterval = 1.0f / frequency;
            [sMotionManager startAccelerometerUpdates];
        }
    }
#endif
}

extern "C" void UnityCoreMotionStop()
{
#if PLATFORM_TVOS
    sGCMotionForwardingEnabled = false;
#else
    if (sMotionManager != nil)
    {
        [sMotionManager stopGyroUpdates];
        [sMotionManager stopDeviceMotionUpdates];
    }
#endif
}

extern "C" void UnityUpdateAccelerometerData()
{
#if !PLATFORM_TVOS
    if (sMotionManager)
    {
        CMAccelerometerData* data = sMotionManager.accelerometerData;
        if (data != nil)
        {
            Vector3f res = UnityReorientVector3(data.acceleration.x, data.acceleration.y, data.acceleration.z);
            UnityDidAccelerate(res.x, res.y, res.z, data.timestamp);
        }
    }
#endif
}

extern "C" void UnitySetGyroUpdateInterval(int idx, float interval)
{
#if !PLATFORM_TVOS
    static const float _MinUpdateInterval = 1.0f / 60.0f;
    static const float _MaxUpdateInterval = 1.0f;

    if (interval < _MinUpdateInterval)
        interval = _MinUpdateInterval;
    else if (interval > _MaxUpdateInterval)
        interval = _MaxUpdateInterval;

    sUserUpdateInterval = interval;

    if (sMotionManager)
    {
        sUpdateInterval = interval;

        [sMotionManager setGyroUpdateInterval: interval];
        [sMotionManager setDeviceMotionUpdateInterval: interval];
    }
#endif
}

extern "C" float UnityGetGyroUpdateInterval(int idx)
{
    return sUpdateInterval;
}

extern "C" void UnityUpdateGyroData()
{
#if !PLATFORM_TVOS
    CMRotationRate rotationRate = { 0.0, 0.0, 0.0 };
    CMRotationRate rotationRateUnbiased = { 0.0, 0.0, 0.0 };
    CMAcceleration userAcceleration = { 0.0, 0.0, 0.0 };
    CMAcceleration gravity = { 0.0, 0.0, 0.0 };
    CMQuaternion attitude = { 0.0, 0.0, 0.0, 1.0 };

    if (sMotionManager != nil)
    {
        CMGyroData *gyroData = sMotionManager.gyroData;
        CMDeviceMotion *motionData = sMotionManager.deviceMotion;

        if (gyroData != nil)
        {
            rotationRate = gyroData.rotationRate;
        }

        if (motionData != nil)
        {
            CMAttitude *att = motionData.attitude;

            attitude = att.quaternion;
            rotationRateUnbiased = motionData.rotationRate;
            userAcceleration = motionData.userAcceleration;
            gravity = motionData.gravity;
        }
    }

    Vector3f reorientedRotRate = UnityReorientVector3(rotationRate.x, rotationRate.y, rotationRate.z);
    UnitySensorsSetGyroRotationRate(0, reorientedRotRate.x, reorientedRotRate.y, reorientedRotRate.z);

    Vector3f reorientedRotRateUnbiased = UnityReorientVector3(rotationRateUnbiased.x, rotationRateUnbiased.y, rotationRateUnbiased.z);
    UnitySensorsSetGyroRotationRateUnbiased(0, reorientedRotRateUnbiased.x, reorientedRotRateUnbiased.y, reorientedRotRateUnbiased.z);

    Vector3f reorientedUserAcc = UnityReorientVector3(userAcceleration.x, userAcceleration.y, userAcceleration.z);
    UnitySensorsSetUserAcceleration(0, reorientedUserAcc.x, reorientedUserAcc.y, reorientedUserAcc.z);

    Vector3f reorientedG = UnityReorientVector3(gravity.x, gravity.y, gravity.z);
    UnitySensorsSetGravity(0, reorientedG.x, reorientedG.y, reorientedG.z);

    Quaternion4f reorientedAtt = UnityReorientQuaternion(attitude.x, attitude.y, attitude.z, attitude.w);
    UnitySensorsSetAttitude(0, reorientedAtt.x, reorientedAtt.y, reorientedAtt.z, reorientedAtt.w);
#endif
}

extern "C" int UnityIsGyroEnabled(int idx)
{
#if PLATFORM_TVOS
    return sGCMotionForwardingEnabled;
#else
    if (sMotionManager == nil)
        return 0;

    return sMotionManager.gyroAvailable && sMotionManager.gyroActive;
#endif
}

extern "C" int UnityIsGyroAvailable()
{
#if PLATFORM_TVOS
    return true;
#else
    if (sMotionManager != nil)
        return sMotionManager.gyroAvailable;
#endif

    return 0;
}

// -- Joystick stuff --
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wobjc-method-access"
enum JoystickButtonNumbers
{
    BTN_PAUSE = 0,
    BTN_DPAD_UP = 4,
    BTN_DPAD_RIGHT = 5,
    BTN_DPAD_DOWN = 6,
    BTN_DPAD_LEFT = 7,
    BTN_Y = 12,
    BTN_B = 13,
    BTN_A = 14,
    BTN_X = 15,
    BTN_L1 = 8,
    BTN_L2 = 10,
    BTN_R1 = 9,
    BTN_R2 = 11,
    BTN_MENU = 16,
    BTN_COUNT
};

typedef struct
{
    int buttonCode;
    bool state;
} JoystickButtonState;

JoystickButtonState gAggregatedJoystickState[BTN_COUNT];


static float GetAxisValue(GCControllerAxisInput* axis)
{
    return axis.value;
}

static float GetButtonValue(GCControllerButtonInput* button)
{
    return button.value;
}

static BOOL GetButtonPressed(GCControllerButtonInput* button)
{
    return button.pressed;
}

extern "C" void UnityInitJoysticks()
{
    if (!gJoysticksInited)
    {
        NSBundle* bundle = [NSBundle bundleWithPath: @"/System/Library/Frameworks/GameController.framework"];
        if (bundle)
        {
            [bundle load];
            gGameControllerClass = [bundle classNamed: @"GCController"];

            //Apply settings that could have been set by user scripts before controller initialization
        #if PLATFORM_TVOS
            UnitySetAppleTVRemoteAllowRotation(gTVRemoteAllowRotationInitialValue);
            UnitySetAppleTVRemoteReportAbsoluteDpadValues(gTVRemoteReportsAbsoluteDpadValuesInitialValue);
        #endif
        }

        for (int i = 0; i < BTN_COUNT; i++)
        {
            char buf[128];
            sprintf(buf, "joystick button %d", i);

            gAggregatedJoystickState[i].buttonCode = UnityStringToKey(buf);
            gAggregatedJoystickState[i].state = false;
        }

        gJoysticksInited = true;
    }
}

static NSArray* QueryControllerCollection()
{
    return gGameControllerClass != nil ? (NSArray*)[gGameControllerClass performSelector: @selector(controllers)] : nil;
}

static void ResetAggregatedJoystickState()
{
    for (int i = 0; i < BTN_COUNT; i++)
    {
        gAggregatedJoystickState[i].state = false;
    }
}

static void SetAggregatedJoystickState()
{
    for (int i = 0; i < BTN_COUNT; i++)
    {
        UnitySetKeyState(gAggregatedJoystickState[i].buttonCode, gAggregatedJoystickState[i].state);
    }
}

// Mirror button input into virtual joystick 0
static void ReportAggregatedJoystickButton(int buttonNum, int state)
{
    assert(buttonNum < BTN_COUNT);
    gAggregatedJoystickState[buttonNum].state |= (bool)state;
}

static void SetJoystickButtonState(int joyNum, int buttonNum, int state)
{
    char buf[128];
    sprintf(buf, "joystick %d button %d", joyNum, buttonNum);
    UnitySetKeyState(UnityStringToKey(buf), state);
    ReportAggregatedJoystickButton(buttonNum, state);
}

static void ReportJoystickButton(int idx, JoystickButtonNumbers num, GCControllerButtonInput* button)
{
    SetJoystickButtonState(idx + 1, num, GetButtonPressed(button));
    UnitySetJoystickPosition(idx + 1, num, GetButtonValue(button));
}

template<class ClassXYZ>
static void ReportJoystickXYZAxes(int idx, int xaxis, int yaxis, int zaxis, const ClassXYZ& xyz)
{
    UnitySetJoystickPosition(idx + 1, xaxis, xyz.x);
    UnitySetJoystickPosition(idx + 1, yaxis, xyz.y);
    UnitySetJoystickPosition(idx + 1, zaxis, xyz.z);
}

template<class ClassXYZW>
static void ReportJoystickXYZWAxes(int idx, int xaxis, int yaxis, int zaxis, int waxis,
    const ClassXYZW& xyzw)
{
    UnitySetJoystickPosition(idx + 1, xaxis, xyzw.x);
    UnitySetJoystickPosition(idx + 1, yaxis, xyzw.y);
    UnitySetJoystickPosition(idx + 1, zaxis, xyzw.z);
    UnitySetJoystickPosition(idx + 1, waxis, xyzw.w);
}

#if PLATFORM_TVOS
static void ReportJoystickMicro(int idx, GCMicroGamepad* gamepad)
{
    GCControllerDirectionPad* dpad = [gamepad dpad];

    UnitySetJoystickPosition(idx + 1, 0, GetAxisValue([dpad xAxis]));
    UnitySetJoystickPosition(idx + 1, 1, -GetAxisValue([dpad yAxis]));

    ReportJoystickButton(idx, BTN_DPAD_UP, [dpad up]);
    ReportJoystickButton(idx, BTN_DPAD_RIGHT, [dpad right]);
    ReportJoystickButton(idx, BTN_DPAD_DOWN, [dpad down]);
    ReportJoystickButton(idx, BTN_DPAD_LEFT, [dpad left]);

    ReportJoystickButton(idx, BTN_A, [gamepad buttonA]);
    ReportJoystickButton(idx, BTN_X, [gamepad buttonX]);
}

#endif

static void ReportJoystickBasic(int idx, GCGamepad* gamepad)
{
    GCControllerDirectionPad* dpad = [gamepad dpad];

    UnitySetJoystickPosition(idx + 1, 0, GetAxisValue([dpad xAxis]));
    UnitySetJoystickPosition(idx + 1, 1, -GetAxisValue([dpad yAxis]));
    ReportJoystickButton(idx, BTN_DPAD_UP, [dpad up]);
    ReportJoystickButton(idx, BTN_DPAD_RIGHT, [dpad right]);
    ReportJoystickButton(idx, BTN_DPAD_DOWN, [dpad down]);
    ReportJoystickButton(idx, BTN_DPAD_LEFT, [dpad left]);

    ReportJoystickButton(idx, BTN_A, [gamepad buttonA]);
    ReportJoystickButton(idx, BTN_B, [gamepad buttonB]);
    ReportJoystickButton(idx, BTN_Y, [gamepad buttonY]);
    ReportJoystickButton(idx, BTN_X, [gamepad buttonX]);

    ReportJoystickButton(idx, BTN_L1, [gamepad leftShoulder]);
    ReportJoystickButton(idx, BTN_R1, [gamepad rightShoulder]);
}

static void ReportJoystickExtended(int idx, GCExtendedGamepad* gamepad)
{
    GCControllerDirectionPad* dpad = [gamepad dpad];
    GCControllerDirectionPad* leftStick = [gamepad leftThumbstick];
    GCControllerDirectionPad* rightStick = [gamepad rightThumbstick];

    UnitySetJoystickPosition(idx + 1, 0, GetAxisValue([leftStick xAxis]));
    UnitySetJoystickPosition(idx + 1, 1, -GetAxisValue([leftStick yAxis]));

    UnitySetJoystickPosition(idx + 1, 2, GetAxisValue([rightStick xAxis]));
    UnitySetJoystickPosition(idx + 1, 3, -GetAxisValue([rightStick yAxis]));
    ReportJoystickButton(idx, BTN_DPAD_UP, [dpad up]);
    ReportJoystickButton(idx, BTN_DPAD_RIGHT, [dpad right]);
    ReportJoystickButton(idx, BTN_DPAD_DOWN, [dpad down]);
    ReportJoystickButton(idx, BTN_DPAD_LEFT, [dpad left]);

    ReportJoystickButton(idx, BTN_A, [gamepad buttonA]);
    ReportJoystickButton(idx, BTN_B, [gamepad buttonB]);
    ReportJoystickButton(idx, BTN_Y, [gamepad buttonY]);
    ReportJoystickButton(idx, BTN_X, [gamepad buttonX]);

    ReportJoystickButton(idx, BTN_L1, [gamepad leftShoulder]);
    ReportJoystickButton(idx, BTN_R1, [gamepad rightShoulder]);
    ReportJoystickButton(idx, BTN_L2, [gamepad leftTrigger]);
    ReportJoystickButton(idx, BTN_R2, [gamepad rightTrigger]);
}

static void SimulateAttitudeViaGravityVector(const Vector3f& gravity, Quaternion4f& currentAttitude, Vector3f& rotationRate)
{
    static Quaternion4f lastAttitude = QuatIdentity();
    static double lastTime = 0.0;
    double currentTime = [NSDate timeIntervalSinceReferenceDate];
    double deltaTime = lastTime - currentTime;
    currentAttitude = QuatRotationFromTo(gravity, VecMake(0.0f, 0.0f, -1.0f));
    rotationRate = VecScale(1.0f / deltaTime, QuatToEuler(QuatDifference(currentAttitude, lastAttitude)));
    lastAttitude = currentAttitude;
    lastTime = currentTime;
}

// Note that joystick axis numbers in documentation are shifted
// by one. 1st axis is referred to by index 0, 16th by 15, etc.
static void ReportJoystickMotion(int idx, GCMotion* motion)
{
    Vector3f rotationRate = VecMake(0.0f, 0.0f, 0.0f);
    Quaternion4f attitude = QuatMake(0.0f, 0.0f, 0.0f, 1.0f);

    bool gotRotationData = false;
    if (@available(iOS 11.0, tvOS 11.0, *))
    {
        if (motion.hasAttitudeAndRotationRate)
        {
            rotationRate = {(float)motion.rotationRate.x, (float)motion.rotationRate.y, (float)motion.rotationRate.z};
            attitude = {(float)motion.attitude.x, (float)motion.attitude.y, (float)motion.attitude.z, (float)motion.attitude.w};
            gotRotationData = true;
        }
    }
    else
    {
#if PLATFORM_IOS
        // on iOS we assume that rotationRate and attitude is correct, unless
        // hasAttitudeAndRotationRate tells us otherwise.
        // on tvOS, rotationRate and attitude are unavailable if hasAttitudeAndRotationRate is unavailable.
        rotationRate = {(float)motion.rotationRate.x, (float)motion.rotationRate.y, (float)motion.rotationRate.z};
        attitude = {(float)motion.attitude.x, (float)motion.attitude.y, (float)motion.attitude.z, (float)motion.attitude.w};
        gotRotationData = true;
#endif
    }

#if SIMULATE_ATTITUDE_FROM_GRAVITY
    if (!gotRotationData)
        SimulateAttitudeViaGravityVector(VecMake((float)motion.gravity.x, (float)motion.gravity.y, (float)motion.gravity.z), attitude, rotationRate);
#endif

    // From docs:
    // gravity (x,y,z) : 16, 17, 18
    // user acceleration: 19, 20, 21
    // rotation rate: 22, 23, 24
    // attitude quaternion (x,y,z,w): 25, 26, 27, 28
    ReportJoystickXYZAxes(idx, 15, 16, 17, motion.gravity);
    ReportJoystickXYZAxes(idx, 18, 19, 20, motion.userAcceleration);
    ReportJoystickXYZAxes(idx, 21, 22, 23, rotationRate);
    ReportJoystickXYZWAxes(idx, 24, 25, 26, 27, attitude);

#if PLATFORM_TVOS
    if (sGCMotionForwardingEnabled && !sGCMotionForwardedForCurrentFrame)
    {
        UnitySensorsSetGravity(0, motion.gravity.x, motion.gravity.y, motion.gravity.z);
        UnitySensorsSetUserAcceleration(0, motion.userAcceleration.x, motion.userAcceleration.y, motion.userAcceleration.z);
        UnitySensorsSetGyroRotationRate(0, rotationRate.y, rotationRate.x, rotationRate.z);
        UnitySensorsSetAttitude(0, attitude.x, attitude.y, attitude.z, attitude.w);
        UnityDidAccelerate(motion.userAcceleration.x + motion.gravity.x, motion.userAcceleration.y + motion.gravity.y, motion.userAcceleration.z + motion.gravity.z, [[NSDate date] timeIntervalSince1970]);
        sGCMotionForwardedForCurrentFrame = true;
    }
#endif
}

static void ReportJoystick(GCController* controller, int idx)
{
    if (controller.controllerPausedHandler == nil)
        controller.controllerPausedHandler = gControllerHandler;

    if ([controller extendedGamepad] != nil)
        ReportJoystickExtended(idx, [controller extendedGamepad]);
    else if ([controller gamepad] != nil)
        ReportJoystickBasic(idx, [controller gamepad]);
#if PLATFORM_TVOS
    else if ([controller microGamepad] != nil)
        ReportJoystickMicro(idx, [controller microGamepad]);
#endif
    else
    {
        // TODO: do something with not supported gamepad profiles
    }

    if (controller.motion != nil)
        ReportJoystickMotion(idx, controller.motion);

    // Map pause button
    SetJoystickButtonState(idx + 1, BTN_PAUSE, gPausedJoysticks[idx]);

    // Reset pause button
    gPausedJoysticks[idx] = false;
}

// On tvOS simulator we implement a fake remote as tvOS simulator
// does not support controllers (yet)
#if UNITY_TVOS_SIMULATOR_FAKE_REMOTE
struct FakeRemoteState
{
    bool pressedX, pressedA;
    bool pressedUp, pressedDown, pressedLeft, pressedRight;
    float xAxis, yAxis;

    FakeRemoteState() :
        pressedX(false),
        pressedA(false),
        pressedUp(false),
        pressedDown(false),
        pressedLeft(false),
        pressedRight(false),
        xAxis(0),
        yAxis(0)
    {}
};

static FakeRemoteState gFakeRemoteState;

static void ReportFakeRemoteButton(int idx, JoystickButtonNumbers num, bool pressed)
{
    SetJoystickButtonState(idx + 1, num, pressed);
    UnitySetJoystickPosition(idx + 1, num, pressed);
}

void ReportFakeRemote(int idx)
{
    UnitySetJoystickPosition(idx + 1, 0, gFakeRemoteState.xAxis);
    UnitySetJoystickPosition(idx + 1, 1, -gFakeRemoteState.yAxis);

    ReportFakeRemoteButton(idx, BTN_DPAD_UP, gFakeRemoteState.pressedUp);
    ReportFakeRemoteButton(idx, BTN_DPAD_RIGHT, gFakeRemoteState.pressedRight);
    ReportFakeRemoteButton(idx, BTN_DPAD_DOWN, gFakeRemoteState.pressedDown);
    ReportFakeRemoteButton(idx, BTN_DPAD_LEFT, gFakeRemoteState.pressedLeft);

    ReportFakeRemoteButton(idx, BTN_A, gFakeRemoteState.pressedA);
    ReportFakeRemoteButton(idx, BTN_X, gFakeRemoteState.pressedX);
}

#endif

extern "C" void UnityUpdateJoystickData()
{
    UnityInitJoysticks();

    NSArray* list = QueryControllerCollection();
#if PLATFORM_TVOS
    sGCMotionForwardedForCurrentFrame = false;
#endif

    // Clear aggregated joystick state
    ResetAggregatedJoystickState();

    if (list != nil)
    {
        for (int i = 0; i < [list count]; i++)
        {
            id controller = [list objectAtIndex: i];
            ReportJoystick(controller, i);
        }
    }

#if UNITY_TVOS_SIMULATOR_FAKE_REMOTE
    int remoteIndex = list != nil ? (int)[list count] : 0;
    ReportFakeRemote(remoteIndex);
#endif

    // Report all aggregated joystick button in bulk
    SetAggregatedJoystickState();
}

extern "C" int  UnityGetJoystickCount()
{
    NSArray* list = QueryControllerCollection();
    int count = list != nil ? (int)[list count] : 0;
#if UNITY_TVOS_SIMULATOR_FAKE_REMOTE
    count++;
#endif
    return count;
}

static void FormatJoystickIdentifier(int idx, char* buffer, int maxLen, const char* typeString,
    const char* attachment, const char* vendorName)
{
    snprintf(buffer, maxLen, "[%s,%s] joystick %d by %s",
        typeString, attachment, idx + 1, vendorName);
}

extern "C" void UnityGetJoystickName(int idx, char* buffer, int maxLen)
{
    GCController* controller = [QueryControllerCollection() objectAtIndex: idx];

    if (controller != nil)
    {
        // iOS 8 has bug, which is encountered when controller is being attached
        // while app is still running. It creates two instances of controller object:
        // one original and one "Forwarded", accesing later properties are causing crashes
        const char* attached = "unknown";

        // Controller is good one
        if ([[controller vendorName] rangeOfString: @"Forwarded"].location == NSNotFound)
            attached = (controller.attachedToDevice ? "wired" : "wireless");

        const char* typeString = [controller extendedGamepad] != nil ? "extended" : "basic";

        FormatJoystickIdentifier(idx, buffer, maxLen, typeString, attached,
            [[controller vendorName] UTF8String]);
    }
    else
    {
#if UNITY_TVOS_SIMULATOR_FAKE_REMOTE
        if (idx == [QueryControllerCollection() count])
        {
            FormatJoystickIdentifier(idx, buffer, maxLen, "basic", "wireless", "Unity");
            return;
        }
#endif
        strncpy(buffer, "unknown", maxLen);
    }
}

extern "C" void UnityGetJoystickAxisName(int idx, int axis, char* buffer, int maxLen)
{
}

extern "C" void UnityGetNiceKeyname(int key, char* buffer, int maxLen)
{
}

#pragma clang diagnostic pop

#if UNITY_USES_LOCATION
@interface LocationServiceDelegate : NSObject<CLLocationManagerDelegate>
@end
#endif

extern "C" void
UnitySetLastLocation(double timestamp,
    float latitude,
    float longitude,
    float altitude,
    float horizontalAccuracy,
    float verticalAccuracy);

extern "C" void
UnitySetLastHeading(float magneticHeading,
    float trueHeading,
    float rawX, float rawY, float rawZ,
    double timestamp);

#if UNITY_USES_LOCATION
struct LocationServiceInfo
{
private:
    LocationServiceDelegate* delegate;
    CLLocationManager* locationManager;
public:
    LocationServiceStatus locationStatus;
    LocationServiceStatus headingStatus;

    float desiredAccuracy;
    float distanceFilter;

    LocationServiceInfo();
    CLLocationManager* GetLocationManager();
};

LocationServiceInfo::LocationServiceInfo()
{
    locationStatus = kLocationServiceStopped;
    desiredAccuracy = kCLLocationAccuracyKilometer;
    distanceFilter = 500;

    headingStatus = kLocationServiceStopped;
}

static LocationServiceInfo gLocationServiceStatus;

CLLocationManager* LocationServiceInfo::GetLocationManager()
{
    if (locationManager == nil)
    {
        locationManager = [[CLLocationManager alloc] init];
        delegate = [LocationServiceDelegate alloc];

        locationManager.delegate = delegate;
    }

    return locationManager;
}

#endif

bool LocationService::IsServiceEnabledByUser()
{
#if UNITY_USES_LOCATION
    return [CLLocationManager locationServicesEnabled];
#else
    return false;
#endif
}

void LocationService::SetDesiredAccuracy(float val)
{
#if UNITY_USES_LOCATION
    gLocationServiceStatus.desiredAccuracy = val;
#endif
}

float LocationService::GetDesiredAccuracy()
{
#if UNITY_USES_LOCATION
    return gLocationServiceStatus.desiredAccuracy;
#else
    return NAN;
#endif
}

void LocationService::SetDistanceFilter(float val)
{
#if UNITY_USES_LOCATION
    gLocationServiceStatus.distanceFilter = val;
#endif
}

float LocationService::GetDistanceFilter()
{
#if UNITY_USES_LOCATION
    return gLocationServiceStatus.distanceFilter;
#else
    return NAN;
#endif
}

void LocationService::StartUpdatingLocation()
{
#if UNITY_USES_LOCATION
    if (gLocationServiceStatus.locationStatus != kLocationServiceRunning)
    {
        CLLocationManager* locationManager = gLocationServiceStatus.GetLocationManager();
        [locationManager requestWhenInUseAuthorization];

        locationManager.desiredAccuracy = gLocationServiceStatus.desiredAccuracy;
        // Set a movement threshold for new events
        locationManager.distanceFilter = gLocationServiceStatus.distanceFilter;

#if PLATFORM_IOS
        [locationManager startUpdatingLocation];
#else
        [locationManager requestLocation];
#endif

        gLocationServiceStatus.locationStatus = kLocationServiceInitializing;
    }
#endif
}

void LocationService::StopUpdatingLocation()
{
#if UNITY_USES_LOCATION
    if (gLocationServiceStatus.locationStatus != kLocationServiceStopped)
    {
        [gLocationServiceStatus.GetLocationManager() stopUpdatingLocation];
        gLocationServiceStatus.locationStatus = kLocationServiceStopped;
    }
#endif
}

void LocationService::SetHeadingUpdatesEnabled(bool enabled)
{
#if PLATFORM_IOS && UNITY_USES_LOCATION
    if (enabled)
    {
        if (gLocationServiceStatus.headingStatus != kLocationServiceRunning &&
            IsHeadingAvailable())
        {
            CLLocationManager* locationManager = gLocationServiceStatus.GetLocationManager();

            [locationManager startUpdatingHeading];
            gLocationServiceStatus.headingStatus = kLocationServiceInitializing;
        }
    }
    else
    {
        if (gLocationServiceStatus.headingStatus != kLocationServiceStopped)
        {
            [gLocationServiceStatus.GetLocationManager() stopUpdatingHeading];
            gLocationServiceStatus.headingStatus = kLocationServiceStopped;
        }
    }
#endif
}

bool LocationService::IsHeadingUpdatesEnabled()
{
#if UNITY_USES_LOCATION
    return (gLocationServiceStatus.headingStatus == kLocationServiceRunning);
#else
    return false;
#endif
}

LocationServiceStatus LocationService::GetLocationStatus()
{
#if UNITY_USES_LOCATION
    return (LocationServiceStatus)gLocationServiceStatus.locationStatus;
#else
    return kLocationServiceFailed;
#endif
}

LocationServiceStatus LocationService::GetHeadingStatus()
{
#if UNITY_USES_LOCATION
    return (LocationServiceStatus)gLocationServiceStatus.headingStatus;
#else
    return kLocationServiceFailed;
#endif
}

bool LocationService::IsHeadingAvailable()
{
#if PLATFORM_IOS && UNITY_USES_LOCATION
    return [CLLocationManager headingAvailable];
#else
    return false;
#endif
}

#if UNITY_USES_LOCATION
@implementation LocationServiceDelegate

- (void)locationManager:(CLLocationManager*)manager didUpdateLocations:(NSArray*)locations
{
    CLLocation* lastLocation = locations.lastObject;

    gLocationServiceStatus.locationStatus = kLocationServiceRunning;

    UnitySetLastLocation([lastLocation.timestamp timeIntervalSince1970],
        lastLocation.coordinate.latitude, lastLocation.coordinate.longitude, lastLocation.altitude,
        lastLocation.horizontalAccuracy, lastLocation.verticalAccuracy
    );
}

#if PLATFORM_IOS
- (void)locationManager:(CLLocationManager*)manager didUpdateHeading:(CLHeading*)newHeading
{
    gLocationServiceStatus.headingStatus = kLocationServiceRunning;

    Vector3f reorientedRawHeading = UnityReorientVector3(newHeading.x, newHeading.y, newHeading.z);

    UnitySetLastHeading(UnityReorientHeading(newHeading.magneticHeading),
        UnityReorientHeading(newHeading.trueHeading),
        reorientedRawHeading.x, reorientedRawHeading.y, reorientedRawHeading.z,
        [newHeading.timestamp timeIntervalSince1970]);
}

#endif

- (BOOL)locationManagerShouldDisplayHeadingCalibration:(CLLocationManager*)manager
{
    return NO;
}

- (void)locationManager:(CLLocationManager*)manager didFailWithError:(NSError*)error;
{
    gLocationServiceStatus.locationStatus = kLocationServiceFailed;
    gLocationServiceStatus.headingStatus = kLocationServiceFailed;
}

@end
#endif

#if PLATFORM_TVOS

GCMicroGamepad* QueryMicroController()
{
    NSArray* list = QueryControllerCollection();
    for (GCController* controller in list)
    {
        if (controller.microGamepad != nil)
            return controller.microGamepad;
    }

    return nil;
}

extern "C" int UnityGetAppleTVRemoteTouchesEnabled()
{
    return gTVRemoteTouchesEnabled;
}

extern "C" void UnitySetAppleTVRemoteTouchesEnabled(int val)
{
    gTVRemoteTouchesEnabled = val;
}

extern "C" int UnityGetAppleTVRemoteAllowExitToMenu()
{
    return ((GCEventViewController*)UnityGetGLViewController()).controllerUserInteractionEnabled;
}

extern "C" void UnitySetAppleTVRemoteAllowExitToMenu(int val)
{
    ((GCEventViewController*)UnityGetGLViewController()).controllerUserInteractionEnabled = val;
}

extern "C" int UnityGetAppleTVRemoteAllowRotation()
{
    GCMicroGamepad* controller = QueryMicroController();
    if (controller != nil)
        return controller.allowsRotation;
    else
        return false;
}

extern "C" void UnitySetAppleTVRemoteAllowRotation(int val)
{
    GCMicroGamepad* controller = QueryMicroController();
    if (controller != nil)
        controller.allowsRotation = val;
    else
        gTVRemoteAllowRotationInitialValue = val;
}

extern "C" int UnityGetAppleTVRemoteReportAbsoluteDpadValues()
{
    GCMicroGamepad* controller = QueryMicroController();
    if (controller != nil)
        return controller.reportsAbsoluteDpadValues;
    else
        return false;
}

extern "C" void UnitySetAppleTVRemoteReportAbsoluteDpadValues(int val)
{
    NSArray* list = QueryControllerCollection();
    for (GCController* controller in list)
    {
        if (controller.microGamepad != nil)
            controller.microGamepad.reportsAbsoluteDpadValues = val;
        else
            gTVRemoteReportsAbsoluteDpadValuesInitialValue = val;
    }
}

#endif

#if UNITY_TVOS_SIMULATOR_FAKE_REMOTE
static void FakeRemoteStateSetButton(UIPressType type, bool state)
{
    switch (type)
    {
        case UIPressTypeUpArrow: gFakeRemoteState.pressedUp = state; return;
        case UIPressTypeDownArrow: gFakeRemoteState.pressedDown = state; return;
        case UIPressTypeLeftArrow: gFakeRemoteState.pressedLeft = state; return;
        case UIPressTypeRightArrow: gFakeRemoteState.pressedRight = state; return;
        case UIPressTypeSelect: gFakeRemoteState.pressedA = state; return;
        case UIPressTypePlayPause: gFakeRemoteState.pressedX = state; return;
    }
}

void ReportSimulatedRemoteButtonPress(UIPressType type)
{
    FakeRemoteStateSetButton(type, true);
}

void ReportSimulatedRemoteButtonRelease(UIPressType type)
{
    FakeRemoteStateSetButton(type, false);
}

static float FakeRemoteMapTouchToAxis(float pos, float bounds)
{
    float halfRange = bounds / 2;
    return (pos - halfRange) / halfRange;
}

void ReportSimulatedRemoteTouchesBegan(UIView* view, NSSet* touches)
{
    ReportSimulatedRemoteTouchesMoved(view, touches);
}

void ReportSimulatedRemoteTouchesMoved(UIView* view, NSSet* touches)
{
    for (UITouch* touch in touches)
    {
        gFakeRemoteState.xAxis = FakeRemoteMapTouchToAxis([touch locationInView: view].x, view.bounds.size.width);
        gFakeRemoteState.yAxis = FakeRemoteMapTouchToAxis([touch locationInView: view].y, view.bounds.size.height);
        // We assume that at most single touch is received.
        break;
    }
}

void ReportSimulatedRemoteTouchesEnded(UIView* view, NSSet* touches)
{
    gFakeRemoteState.xAxis = 0;
    gFakeRemoteState.yAxis = 0;
}

#endif

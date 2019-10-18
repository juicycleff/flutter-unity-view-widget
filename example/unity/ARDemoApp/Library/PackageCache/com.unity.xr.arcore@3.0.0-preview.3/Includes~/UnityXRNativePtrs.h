#pragma once

// Various AR Subsystems have GetNativePtr methods on them, which return
// pointers to the following structs. The first field will always
// be a version number, so code which tries to interpret the native
// pointers can safely check the version prior to casting to the
// appropriate struct.

typedef struct UnityXRNativeSession_1
{
    int version;
    void* sessionPtr;
} UnityXRNativeSession_1;

typedef struct UnityXRNativeFrame_1
{
    int version;
    void* framePtr;
} UnityXRNativeFrame_1;

// XRPlaneExtensions.GetNativePtr
typedef struct UnityXRNativePlane_1
{
    int version;
    void* planePtr;
} UnityXRNativePlane_1;

// XRReferencePointExtensions.GetNativePtr
typedef struct UnityXRNativeReferencePoint_1
{
    int version;
    void* referencePointPtr;
} UnityXRNativeReferencePoint_1;

typedef struct UnityXRNativePointCloud_1
{
    int version;
    void* pointCloud;
} UnityXRNativePointCloud_1;

typedef struct UnityXRNativeImage_1
{
    int version;
    void* imageTrackable;
} UnityXRNativeImage_1;

static const int kUnityXRNativeSessionVersion = 1;
static const int kUnityXRNativeFrameVersion = 1;
static const int kUnityXRNativePlaneVersion = 1;
static const int kUnityXRNativeReferencePointVersion = 1;
static const int kUnityXRNativePointCloudVersion = 1;
static const int kUnityXRNativeImageVersion = 1;

typedef UnityXRNativeSession_1 UnityXRNativeSession;
typedef UnityXRNativeFrame_1 UnityXRNativeFrame;
typedef UnityXRNativePlane_1 UnityXRNativePlane;
typedef UnityXRNativeReferencePoint_1 UnityXRNativeReferencePoint;
typedef UnityXRNativePointCloud_1 UnityXRNativePointCloud;
typedef UnityXRNativeImage_1 UnityXRNativeImage;

// ? TODO: Pavell merge with TrampolineInterface.h to have singe source of truth for a function definitions

UnityExternCall(bool, UnityiOS81orNewer);
UnityExternCall(bool, UnityiOS82orNewer);
UnityExternCall(bool, UnityiOS90orNewer);
UnityExternCall(bool, UnityiOS91orNewer);
UnityExternCall(bool, UnityiOS100orNewer);
UnityExternCall(bool, UnityiOS101orNewer);
UnityExternCall(bool, UnityiOS102orNewer);
UnityExternCall(bool, UnityiOS103orNewer);
UnityExternCall(bool, UnityiOS110orNewer);
UnityExternCall(bool, UnityiOS111orNewer);
UnityExternCall(bool, UnityiOS112orNewer);

// CrashReporter.mm
UnityExternCall(void,     CrashedCheckBelowForHintsWhy);
UnityExternCall(const decltype(_mh_execute_header)*, UnityGetExecuteMachHeader);

// iPhone_Sensors.mm
UnityExternCall(void,  UnityInitJoysticks);
UnityExternCall(void,  UnityCoreMotionStart);
UnityExternCall(void,  UnityCoreMotionStop);
UnityExternCall(void,  UnityUpdateAccelerometerData);
UnityExternCall(int,   UnityIsGyroEnabled, int);
UnityExternCall(int,   UnityIsGyroAvailable);
UnityExternCall(void,  UnityUpdateGyroData);
UnityExternCall(void,  UnitySetGyroUpdateInterval, int, float);
UnityExternCall(float, UnityGetGyroUpdateInterval, int);
UnityExternCall(void,  UnityUpdateJoystickData);
UnityExternCall(int,   UnityGetJoystickCount);
UnityExternCall(void,  UnityGetJoystickName, int, char*, int);
UnityExternCall(void,  UnityGetJoystickAxisName, int, int, char*, int);
UnityExternCall(void,  UnityGetNiceKeyname, int, char*, int);
UnityExternCall(bool,  IsCompensatingSensors);
UnityExternCall(void,  SetCompensatingSensors, bool);
UnityExternCall(int,   UnityMaxQueuedAccelerationEvents);

// UnityAppController.mm
UnityExternCall(UIViewController*,  UnityGetGLViewController);
UnityExternCall(UIView*,            UnityGetGLView);
UnityExternCall(UIWindow*,          UnityGetMainWindow);
UnityExternCall(void,               UnityRequestQuit);
UnityExternCall(void,               UnityDestroyDisplayLink);

// UnityAppController+Rendering.mm
UnityExternCall(void,             UnityInitMainScreenRenderingCallback);
UnityExternCall(void,             UnityGfxInitedCallback);
UnityExternCall(void,             UnityPresentContextCallback, UnityFrameStats const*);
UnityExternCall(void,             UnityFramerateChangeCallback, int);
UnityExternCall(int,              UnitySelectedRenderingAPI);

UnityExternCall(NSBundle*,            UnityGetMetalBundle);
UnityExternCall(MTLDeviceRef,         UnityGetMetalDevice);
UnityExternCall(MTLCommandQueueRef,   UnityGetMetalCommandQueue);
UnityExternCall(MTLCommandQueueRef,   UnityGetMetalDrawableCommandQueue);
UnityExternCall(EAGLContext*,         UnityGetDataContextEAGL);

UnityExternCall(RenderSurfaceBase*,         UnityBackbufferColor);
UnityExternCall(RenderSurfaceBase*,         UnityBackbufferDepth);
UnityExternCall(void,                       DisplayManagerEndFrameRendering);
UnityExternCall(void,                       UnityPrepareScreenshot);

// Unity/MetalHelper.mm
UnityExternCall(MTLTextureRef,    AcquireDrawableMTL, UnityDisplaySurfaceMTL*);
UnityExternCall(int,              UnityCommandQueueMaxCommandBufferCountMTL);

// EAGLContextHelper.mm
UnityExternCall(void,             UnityMakeCurrentContextEAGL, EAGLContext*);
UnityExternCall(EAGLContext*,     UnityGetCurrentContextEAGL);

// UI/ActivityIndicator.mm
UnityExternCall(void,             UnityStartActivityIndicator);
UnityExternCall(void,             UnityStopActivityIndicator);

// UI/Keyboard.mm
UnityExternCall(void,             UnityKeyboard_Create, unsigned, int, int , int , int , const char*, const char*, int);
UnityExternCall(void,             UnityKeyboard_Show);
UnityExternCall(void,             UnityKeyboard_Hide);
UnityExternCall(void,             UnityKeyboard_GetRect, float*, float*, float*, float*);
UnityExternCall(void,             UnityKeyboard_SetText, const char*);
UnityExternCall(NSString*,        UnityKeyboard_GetText);
UnityExternCall(int,              UnityKeyboard_IsActive);
UnityExternCall(int,              UnityKeyboard_Status);
UnityExternCall(void,             UnityKeyboard_SetInputHidden, int);
UnityExternCall(int,              UnityKeyboard_IsInputHidden);
UnityExternCall(void,             UnityKeyboard_SetCharacterLimit, unsigned);

UnityExternCall(int,              UnityKeyboard_CanGetSelection);
UnityExternCall(void,             UnityKeyboard_GetSelection, int*, int*);
UnityExternCall(int,              UnityKeyboard_CanSetSelection);
UnityExternCall(void,             UnityKeyboard_SetSelection, int, int);


// UI/UnityViewControllerBase.mm
UnityExternCall(void,             UnityNotifyHideHomeButtonChange);
UnityExternCall(void,             UnityNotifyDeferSystemGesturesChange);

// UI/StoreReview.m
#if PLATFORM_IOS
UnityExternCall(bool,             UnityRequestStoreReview);
#endif

// Unity/AVCapture.mm
UnityExternCall(int,              UnityGetAVCapturePermission, int);
UnityExternCall(void,             UnityRequestAVCapturePermission, int);

// Unity/CameraCapture.mm
typedef void(*UnityEnumVideoCaptureDevicesCallback)(void* udata, const char* name, int frontFacing, int autoFocusPointSupported, int kind, const int* resolutions, int resCount);
UnityExternCall(void,             UnityEnumVideoCaptureDevices, void*, UnityEnumVideoCaptureDevicesCallback);
UnityExternCall(void*,            UnityInitCameraCapture, int, int, int, int, int, void*);
UnityExternCall(void,             UnityStartCameraCapture, void*);
UnityExternCall(void,             UnityPauseCameraCapture, void*);
UnityExternCall(void,             UnityStopCameraCapture, void*);
UnityExternCall(void,             UnityCameraCaptureExtents, void*, int*, int*);
UnityExternCall(void,             UnityCameraCaptureReadToMemory, void*, void*, int, int);
UnityExternCall(int,              UnityCameraCaptureVideoRotationDeg, void*);
UnityExternCall(int,              UnityCameraCaptureVerticallyMirrored, void*);
UnityExternCall(int,              UnityCameraCaptureSetAutoFocusPoint, void*, float, float);


// Unity/DeviceSettings.mm
UnityExternCall(const char*,      UnityDeviceUniqueIdentifier);
UnityExternCall(const char*,      UnityVendorIdentifier);
UnityExternCall(const char*,      UnityAdvertisingIdentifier);
UnityExternCall(int,              UnityAdvertisingTrackingEnabled);
UnityExternCall(const char*,      UnityDeviceName);
UnityExternCall(const char*,      UnitySystemName);
UnityExternCall(const char*,      UnitySystemVersion);
UnityExternCall(const char*,      UnityDeviceModel);
UnityExternCall(int,              UnityDeviceCPUCount);
UnityExternCall(int,              UnityGetPhysicalMemory);
UnityExternCall(int,              UnityDeviceGeneration);
UnityExternCall(int,              ParseDeviceGeneration);
UnityExternCall(int,              UnityDeviceSupportedOrientations);
UnityExternCall(int,              UnityDeviceIsStylusTouchSupported);
UnityExternCall(int,              UnityDeviceCanShowWideColor);
UnityExternCall(float,            UnityDeviceDPI);
UnityExternCall(const char*,      UnitySystemLanguage);
UnityExternCall(int,             UnityGetLowPowerModeEnabled);
UnityExternCall(int,             UnityGetWantsSoftwareDimming);
UnityExternCall(void,             UnitySetWantsSoftwareDimming, int);

// Unity/DisplayManager.mm
UnityExternCall(EAGLContext*,     UnityGetMainScreenContextGLES);
UnityExternCall(EAGLContext*,     UnityGetContextEAGL);
UnityExternCall(void,             UnityStartFrameRendering);
UnityExternCall(void,             UnityDestroyUnityRenderSurfaces);
UnityExternCall(int,              UnityMainScreenRefreshRate);
UnityExternCall(void,             UnitySetBrightness, float);
UnityExternCall(float,            UnityGetBrightness);


#if SUPPORT_MULTIPLE_DISPLAYS || PLATFORM_IOS
UnityExternCall(int,              UnityDisplayManager_DisplayCount);
UnityExternCall(void,             UnityDisplayManager_DisplayRenderingResolution, void*, int*, int*);
UnityExternCall(int,              UnityDisplayManager_PrimaryDisplayIndex);
UnityExternCall(bool,             UnityDisplayManager_DisplayActive, void*);
UnityExternCall(void,             UnityDisplayManager_DisplayRenderingBuffers, void*, RenderSurfaceBase**, RenderSurfaceBase**);
UnityExternCall(void,             UnityDisplayManager_SetRenderingResolution, void*, int, int);
UnityExternCall(void,             UnityDisplayManager_DisplaySystemResolution, void*, int*, int*);
#endif

// Unity/Filesystem.mm
UnityExternCall(const char*,      UnityDataBundleDir);
UnityExternCall(void,             UnitySetDataBundleDirWithBundleId, const char*);
UnityExternCall(const char*,      UnityDocumentsDir);
UnityExternCall(const char*,      UnityLibraryDir);
UnityExternCall(const char*,      UnityCachesDir);
UnityExternCall(int,              UnityUpdateNoBackupFlag, const char*, int);

// iPhoneMisc.mm
UnityExternCall(const char* const*, UnityFontFallbacks);

// Unity/WWWConnection.mm
UnityExternCall(void*,            UnityCreateWebRequestBackend, void*, const char*, const void*, const char*);
UnityExternCall(void,             UnitySendWebRequest, void*, unsigned, unsigned long, bool);
UnityExternCall(void,             UnityDestroyWebRequestBackend, void*);
UnityExternCall(void,             UnityCancelWebRequest, const void*);
UnityExternCall(bool,             UnityWebRequestIsDone, void*);
UnityExternCall(void,             UnityWebRequestClearCookieCache, const char*);

// Unity/FullScreenVideoPlayer.mm
UnityExternCall(void,             UnityPlayFullScreenVideo, const char*, const float*, unsigned, unsigned);
UnityExternCall(int,              UnityIsFullScreenPlaying);

// Unity/OnDemandResources.mm
struct OnDemandResourcesRequestData;
typedef void (*OnDemandResourcesRequestCompleteHandler)(void* handlerData, const char* error);
UnityExternCall(OnDemandResourcesRequestData*, UnityOnDemandResourcesCreateRequest, NSSet*, OnDemandResourcesRequestCompleteHandler, void*);
UnityExternCall(void,                          UnityOnDemandResourcesRelease, OnDemandResourcesRequestData*);
UnityExternCall(float,                         UnityOnDemandResourcesGetProgress, OnDemandResourcesRequestData*);
UnityExternCall(float,                         UnityOnDemandResourcesGetLoadingPriority, OnDemandResourcesRequestData*);
UnityExternCall(void,                          UnityOnDemandResourcesSetLoadingPriority, OnDemandResourcesRequestData*, float);
UnityExternCall(NSString*,                     UnityOnDemandResourcesGetResourcePath, OnDemandResourcesRequestData*, const char*);

// Unity/UnityReplayKit.mm
UnityExternCall(int,          UnityReplayKitAPIAvailable);
UnityExternCall(int,          UnityReplayKitRecordingAvailable);
UnityExternCall(const char*,  UnityReplayKitLastError);
UnityExternCall(int,          UnityReplayKitStartRecording);
UnityExternCall(int,          UnityReplayKitIsRecording);
UnityExternCall(int,          UnityReplayKitStopRecording);
UnityExternCall(int,          UnityReplayKitDiscard);
UnityExternCall(int,          UnityReplayKitPreview);
UnityExternCall(int,          UnityReplayKitIsPreviewControllerActive);

UnityExternCall(int,          UnityReplayKitBroadcastingAPIAvailable);
UnityExternCall(void,         UnityReplayKitStartBroadcasting, void*);
UnityExternCall(void,         UnityReplayKitStopBroadcasting);
UnityExternCall(void,         UnityReplayKitPauseBroadcasting);
UnityExternCall(void,         UnityReplayKitResumeBroadcasting);
UnityExternCall(int,          UnityReplayKitIsBroadcasting);
UnityExternCall(int,          UnityReplayKitIsBroadcastingPaused);
UnityExternCall(const char*,  UnityReplayKitGetBroadcastURL);

UnityExternCall(int,          UnityReplayKitIsCameraEnabled);
UnityExternCall(int,          UnityReplayKitSetCameraEnabled, bool);
UnityExternCall(int,          UnityReplayKitIsMicrophoneEnabled);
UnityExternCall(int,          UnityReplayKitSetMicrophoneEnabled, bool);
UnityExternCall(int,          UnityReplayKitShowCameraPreviewAt, float, float, float, float);
UnityExternCall(void,         UnityReplayKitHideCameraPreview);
UnityExternCall(void,         UnityReplayKitCreateOverlayWindow);

// LocationService static members to extern c
//UnityExternCall4StaticMember(void,  LocationService, SetDistanceFilter,float);
UnityExternCall4StaticMember(void,  LocationService, SetDesiredAccuracy, float);
UnityExternCall4StaticMember(float, LocationService, GetDesiredAccuracy);
UnityExternCall4StaticMember(void,  LocationService, SetDistanceFilter, float);
UnityExternCall4StaticMember(float, LocationService, GetDistanceFilter);
UnityExternCall4StaticMember(bool,  LocationService, IsServiceEnabledByUser);
UnityExternCall4StaticMember(void,  LocationService, StartUpdatingLocation);
UnityExternCall4StaticMember(void,  LocationService, StopUpdatingLocation);
UnityExternCall4StaticMember(void,  LocationService, SetHeadingUpdatesEnabled, bool);
UnityExternCall4StaticMember(bool,  LocationService, IsHeadingUpdatesEnabled);
UnityExternCall4StaticMember(LocationServiceStatus, LocationService, GetLocationStatus);
UnityExternCall4StaticMember(LocationServiceStatus, LocationService, GetHeadingStatus);
UnityExternCall4StaticMember(bool,  LocationService, IsHeadingAvailable);

//Apple TV Remote
#if PLATFORM_TVOS
UnityExternCall(int,      UnityGetAppleTVRemoteAllowExitToMenu);
UnityExternCall(void,     UnitySetAppleTVRemoteAllowExitToMenu, int);
UnityExternCall(int,      UnityGetAppleTVRemoteAllowRotation);
UnityExternCall(void,     UnitySetAppleTVRemoteAllowRotation, int);
UnityExternCall(int,      UnityGetAppleTVRemoteReportAbsoluteDpadValues);
UnityExternCall(void,     UnitySetAppleTVRemoteReportAbsoluteDpadValues, int);
UnityExternCall(int,      UnityGetAppleTVRemoteTouchesEnabled);
UnityExternCall(void,     UnitySetAppleTVRemoteTouchesEnabled, int);
#endif

// misc not in trampoline
UnityExternCall(bool,     Unity_il2cppNoExceptions);
UnityExternCall(void,     RegisterStaticallyLinkedModulesGranular);

UnityExternCall(NSArray<NSString*>*, GetLaunchImageNames, UIUserInterfaceIdiom, const OrientationMask&, const CGSize&, ScreenOrientation, float);

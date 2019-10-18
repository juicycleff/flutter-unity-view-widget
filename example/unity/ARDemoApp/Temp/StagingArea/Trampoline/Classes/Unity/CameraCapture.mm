#if !PLATFORM_TVOS && UNITY_USES_WEBCAM

#include "CameraCapture.h"
#include "AVCapture.h"
#include "CMVideoSampling.h"
#include "CVTextureCache.h"

#import <CoreVideo/CoreVideo.h>

#include <cmath>

#if UNITY_HAS_COLORANDDEPTH_CAMERA
static NSMutableArray<CameraCaptureController*> *activeColorAndDepthCameraControllers = nil;
#endif

@implementation CameraCaptureController
{
    AVCaptureDevice*            _captureDevice;
    AVCaptureSession*           _captureSession;
    AVCaptureDeviceInput*       _captureInput;
    AVCaptureVideoDataOutput*   _captureOutput;
#if UNITY_HAS_COLORANDDEPTH_CAMERA
    AVCaptureDepthDataOutput*   _captureDepthOutput;
    AVCaptureDataOutputSynchronizer*    _captureSynchronizer;

    @public bool                _isDepth;
#endif

    uint8_t*                    _pixelBufferCopy;
    CMVideoSampling             _cmVideoSampling;
    NSString*                   _preset;
    CGPoint                     _focusPoint;
    AVCaptureFocusMode          _focusMode;
    @public void*               _userData;
    @public size_t              _width, _height;
}

- (bool)initCapture:(AVCaptureDevice*)device
{
    if (UnityGetAVCapturePermission(avVideoCapture) == avCapturePermissionDenied)
        return false;

    self.captureDevice = device;

    self.captureInput   = [AVCaptureDeviceInput deviceInputWithDevice: device error: nil];
    self.captureOutput  = [[AVCaptureVideoDataOutput alloc] init];

    if (self.captureOutput == nil || self.captureInput == nil)
        return false;

    self.captureOutput.alwaysDiscardsLateVideoFrames = YES;
    NSDictionary* options = @{ (NSString*)kCVPixelBufferPixelFormatTypeKey: @(kCVPixelFormatType_32BGRA) };
    [self.captureOutput setVideoSettings: options];

    CMVideoSampling_Initialize(&self->_cmVideoSampling);

    _width = _height = 0;
    _focusPoint = CGPointMake(0.5, 0.5); // default focus point is center
    _focusMode = AVCaptureFocusModeContinuousAutoFocus;
    _pixelBufferCopy = nullptr;

    return true;
}

- (void)setCaptureFPS:(float)fps
{
    if ([self.captureDevice lockForConfiguration: nil])
    {
        if (self.captureDevice.activeFormat)
        {
            fps = [self pickAvailableFrameRate: fps];
            self.captureDevice.activeVideoMinFrameDuration = CMTimeMake(1, fps);
            self.captureDevice.activeVideoMaxFrameDuration = CMTimeMake(1, fps);
        }
        else
        {
            // In some corner cases (seeing this on iPod iOS 6.1.5) activeFormat is null.
        #pragma clang diagnostic push
        #pragma clang diagnostic ignored "-Wdeprecated-declarations"
            self.captureOutput.minFrameDuration = CMTimeMake(1, fps);
        #pragma clang diagnostic pop
        }
        [self.captureDevice unlockForConfiguration];
    }
}

- (bool)initCapture:(AVCaptureDevice*)device preset:(NSString*)preset fps:(float)fps
{
    if (![self initCapture: device])
        return false;

    self.captureSession = [[AVCaptureSession alloc] init];
    [self.captureSession addInput: self.captureInput];
    [self.captureSession addOutput: self.captureOutput];

    // queue on main thread to simplify gles life
    [self.captureOutput setSampleBufferDelegate: self queue: dispatch_get_main_queue()];

    self->_preset = preset;
    [self.captureSession setSessionPreset: preset];
    [self setCaptureFPS: fps];

    return true;
}

- (void)captureOutput:(AVCaptureOutput*)captureOutput didOutputSampleBuffer:(CMSampleBufferRef)sampleBuffer fromConnection:(AVCaptureConnection*)connection
{
    intptr_t tex = (intptr_t)CMVideoSampling_SampleBuffer(&self->_cmVideoSampling, sampleBuffer, &_width, &_height);
    UnityDidCaptureVideoFrame(tex, self->_userData);
}

- (void)capturePixelBufferToMemBuffer:(uint8_t*)dst
{
    CVPixelBufferRef pbuf = (CVPixelBufferRef)self->_cmVideoSampling.cvImageBuffer;
    const size_t srcRowSize = CVPixelBufferGetBytesPerRow(pbuf);
    const size_t bufSize    = srcRowSize * self->_height;
    if (self->_pixelBufferCopy == nullptr)
    {
        self->_pixelBufferCopy = (uint8_t*)::malloc(bufSize);
    }

    // while not the best way memory-wise, we want to minimize stalling
    CVPixelBufferLockBaseAddress(pbuf, kCVPixelBufferLock_ReadOnly);
    {
        ::memcpy(self->_pixelBufferCopy, CVPixelBufferGetBaseAddress(pbuf), bufSize);
    }
    CVPixelBufferUnlockBaseAddress(pbuf, kCVPixelBufferLock_ReadOnly);

    OSType pixelFormat = CVPixelBufferGetPixelFormatType(pbuf);
    size_t bpp = 0;
    switch (pixelFormat)
    {
        case kCVPixelFormatType_32BGRA:
            bpp = 4;
            break;
#if UNITY_HAS_IOSSDK_11_0
        case kCVPixelFormatType_DepthFloat16:
            bpp = 2;
            break;
#endif
        default:
            assert(false);
            break;
    }
    const size_t dstRowSize = self->_width * bpp;

    uint8_t* src = self->_pixelBufferCopy + (self->_height - 1) * srcRowSize;
    for (size_t i = 0; i < self->_height; ++i)
    {
        ::memcpy(dst, src, dstRowSize);
        dst += dstRowSize;
        src -= srcRowSize;
    }
}

- (int)isCVTextureFlipped
{
    return IsCVTextureFlipped(self->_cmVideoSampling.cvTextureCacheTexture);
}

+ (BOOL)focusPointSupported:(AVCaptureDevice*)captureDevice withFocusMode:(AVCaptureFocusMode)focusMode
{
    return captureDevice.focusPointOfInterestSupported && [captureDevice isFocusModeSupported: focusMode];
}

- (int)setFocusPointWithX:(float)x Y:(float)y
{
    if (x < 0 || x > 1 || y < 0 || y > 1)
    {
        _focusPoint = CGPointMake(0.5, 0.5); // default value for iOS
        _focusMode = AVCaptureFocusModeContinuousAutoFocus;
    }
    else
    {
        _focusPoint = CGPointMake(x, 1.0 - y);
        _focusMode = AVCaptureFocusModeAutoFocus;
    }
    return [self setFocusPoint];
}

- (int)setFocusPoint
{
    if (self.captureDevice != nil && [CameraCaptureController focusPointSupported: self.captureDevice withFocusMode: _focusMode])
    {
        if ([self.captureDevice lockForConfiguration: nil])
        {
            self.captureDevice.focusPointOfInterest = _focusPoint;
            self.captureDevice.focusMode = _focusMode;
            [self.captureDevice unlockForConfiguration];
            return 1;
        }
    }
    return 0;
}

#if UNITY_HAS_COLORANDDEPTH_CAMERA
+ (NSMutableArray<CameraCaptureController*>*)getActiveColorAndDepthCameraControllers
{
    if (activeColorAndDepthCameraControllers == nil)
    {
        activeColorAndDepthCameraControllers = [[NSMutableArray alloc] init];
    }
    return activeColorAndDepthCameraControllers;
}

+ (void)addColorAndDepthCameraController:(CameraCaptureController*)controller
{
    CameraCaptureController* prevController = [self findColorAndDepthCameraController: controller.captureDevice isDepth: controller->_isDepth];
    if (prevController != nil)
        [prevController pause];
    CameraCaptureController* otherController = [self findColorAndDepthCameraController: controller.captureDevice isDepth: !controller->_isDepth];
    if (otherController != nil)
    {
        [otherController.captureSession stopRunning];
        [otherController clearColorAndDepthCameraCaptureSession];
    }
    [[self getActiveColorAndDepthCameraControllers] addObject: controller];
}

+ (void)removeColorAndDepthCameraController:(CameraCaptureController*)controller
{
    [[self getActiveColorAndDepthCameraControllers] removeObject: controller];
    CameraCaptureController* otherController = [self findColorAndDepthCameraController: controller.captureDevice isDepth: !controller->_isDepth];
    if (otherController != nil)
    {
        [otherController initColorAndDepthCameraCaptureSession];
        [otherController.captureSession startRunning];
    }
}

+ (void)clearColorAndDepthCameraControllers
{
    NSMutableArray<CameraCaptureController*>* activeColorAndDepthCameraControllers = [self getActiveColorAndDepthCameraControllers];
    for (CameraCaptureController *controller in activeColorAndDepthCameraControllers)
    {
        if (controller.captureSession != nil)
        {
            [controller.captureSession stopRunning];
            [controller clearColorAndDepthCameraCaptureSession];
        }
    }
    [activeColorAndDepthCameraControllers removeAllObjects];
}

+ (CameraCaptureController*)findColorAndDepthCameraController:(AVCaptureDevice*)device isDepth:(bool)isDepth
{
    for (CameraCaptureController *controller in [self getActiveColorAndDepthCameraControllers])
    {
        if (controller.captureDevice == device && controller->_isDepth == isDepth)
            return controller;
    }
    return nil;
}

- (bool)initColorAndDepthCameraCapture:(AVCaptureDevice*)device preset:(NSString*)preset fps:(float)fps isDepth:(bool)isDepth
{
    if (!UnityiOS110orNewer())
        return false;
    if (![self initCapture: device])
        return false;

    self.captureDepthOutput  = [[AVCaptureDepthDataOutput alloc] init];
    if (self.captureDepthOutput == nil)
        return false;
    self.captureDepthOutput.filteringEnabled = YES; // getting filtered depth data to avoid invalid values
    self.captureDepthOutput.alwaysDiscardsLateDepthData = YES;
    self->_preset = preset;
    [self initColorAndDepthCameraCaptureSession];
    [self setCaptureFPS: fps];
    NSArray<AVCaptureOutput*> *outputs = [NSArray arrayWithObjects: self.captureOutput, self.captureDepthOutput, nil];
    self.captureSynchronizer = [[AVCaptureDataOutputSynchronizer alloc] initWithDataOutputs: outputs];

    // queue on main thread to simplify gles life
    [self.captureSynchronizer setDelegate: self queue: dispatch_get_main_queue()];

    _isDepth = isDepth;

    return true;
}

- (void)initColorAndDepthCameraCaptureSession
{
    if (!UnityiOS110orNewer())
        return;
    self.captureSession = [[AVCaptureSession alloc] init];
    [self.captureSession setSessionPreset: self->_preset];
    [self.captureSession addInput: self.captureInput];
    [self.captureSession addOutput: self.captureOutput];
    [self.captureSession addOutput: self.captureDepthOutput];
}

- (void)clearColorAndDepthCameraCaptureSession
{
    if (!UnityiOS110orNewer())
        return;
    [self.captureSession removeInput: self.captureInput];
    [self.captureSession removeOutput: self.captureOutput];
    [self.captureSession removeOutput: self.captureDepthOutput];
    self.captureSession = nil;
}

- (void)dataOutputSynchronizer:(AVCaptureDataOutputSynchronizer *)synchronizer didOutputSynchronizedDataCollection:(AVCaptureSynchronizedDataCollection *)synchronizedDataCollection
{
    AVCaptureSynchronizedSampleBufferData *sampleData = (AVCaptureSynchronizedSampleBufferData*)[synchronizedDataCollection synchronizedDataForCaptureOutput: self.captureOutput];
    if (CMSampleBufferGetImageBuffer(sampleData.sampleBuffer) != nil)
    {
        CameraCaptureController* colorController = !self->_isDepth ? self : [CameraCaptureController findColorAndDepthCameraController: self.captureDevice isDepth: false];
        if (colorController != nil)
        {
            intptr_t tex = (intptr_t)CMVideoSampling_SampleBuffer(&colorController->_cmVideoSampling, sampleData.sampleBuffer, &(colorController->_width), &(colorController->_height));
            UnityDidCaptureVideoFrame(tex, colorController->_userData);
        }
    }

    AVCaptureSynchronizedDepthData *depthData = (AVCaptureSynchronizedDepthData*)[synchronizedDataCollection synchronizedDataForCaptureOutput: self.captureDepthOutput];
    if (depthData.depthData.depthDataMap != nil)
    {
        CameraCaptureController* depthController = self->_isDepth ? self : [CameraCaptureController findColorAndDepthCameraController: self.captureDevice isDepth: true];
        if (depthController != nil)
        {
            intptr_t tex = (intptr_t)CMVideoSampling_ImageBuffer(&depthController->_cmVideoSampling, [depthData.depthData depthDataByConvertingToDepthDataType: kCVPixelFormatType_DepthFloat16].depthDataMap, &(depthController->_width), &(depthController->_height));
            UnityDidCaptureVideoFrame(tex, depthController->_userData);
        }
    }
}

#endif

- (void)start
{
#if UNITY_HAS_COLORANDDEPTH_CAMERA
    if (self.captureDepthOutput != nil)
    {
        [CameraCaptureController addColorAndDepthCameraController: self];
    }
    else
    {
        [CameraCaptureController clearColorAndDepthCameraControllers];
    }
#endif
    [self.captureSession startRunning];
}

- (void)pause
{
    [self.captureSession stopRunning];
#if UNITY_HAS_COLORANDDEPTH_CAMERA
    if (self.captureDepthOutput != nil)
    {
        [CameraCaptureController removeColorAndDepthCameraController: self];
    }
#endif
}

- (void)stop
{
    [self.captureSession stopRunning];
    [self.captureSession removeInput: self.captureInput];
    [self.captureSession removeOutput: self.captureOutput];

    self.captureInput = nil;
    self.captureOutput = nil;
#if UNITY_HAS_COLORANDDEPTH_CAMERA
    if (self.captureDepthOutput != nil)
    {
        self.captureSynchronizer = nil;
        [self.captureSession removeOutput: self.captureDepthOutput];
        self.captureDepthOutput = nil;
        [CameraCaptureController removeColorAndDepthCameraController: self];
    }
#endif
    self.captureDevice = nil;
    self.captureSession = nil;

    if (self->_pixelBufferCopy != nullptr)
    {
        ::free(self->_pixelBufferCopy);
        self->_pixelBufferCopy = nullptr;
    }

    CMVideoSampling_Uninitialize(&self->_cmVideoSampling);
}

- (float)pickAvailableFrameRate:(float)fps
{
    AVFrameRateRange* bestRange = nil;
    float minDiff = INFINITY;
    float epsilon = 0.1;

    fps = fps > epsilon ? fps : 24;
    for (AVFrameRateRange* rate in self.captureDevice.activeFormat.videoSupportedFrameRateRanges)
    {
        if (fps + epsilon > rate.minFrameRate && fps - epsilon < rate.maxFrameRate)
            return fps;
        else
        {
            float diff = ::fmin(::fabs(fps - rate.minFrameRate), ::fabs(fps - rate.maxFrameRate));
            if (diff < minDiff)
            {
                minDiff = diff;
                bestRange = rate;
            }
        }
    }
    return fps > bestRange.maxFrameRate ? bestRange.maxFrameRate : bestRange.minFrameRate;
}

@synthesize captureDevice   = _captureDevice;
@synthesize captureSession  = _captureSession;
@synthesize captureOutput   = _captureOutput;
@synthesize captureInput    = _captureInput;
#if UNITY_HAS_COLORANDDEPTH_CAMERA
@synthesize captureDepthOutput = _captureDepthOutput;
@synthesize captureSynchronizer = _captureSynchronizer;
#endif

@end

#if UNITY_HAS_COLORANDDEPTH_CAMERA
// Preset for getting depth data with max resolution available
static NSString* const depthCaptureSessionPreset = AVCaptureSessionPresetPhoto;
#endif
static NSMutableArray<CameraCaptureDevice*> *videoCaptureDevices = nil;

@implementation CameraCaptureDevice
{
    @public AVCaptureDevice* _device;
    @public int _frontFacing;
    @public int _autoFocusPointSupported;
    @public WebCamKind _kind;
    @public NSMutableArray<NSValue*>* _resolutions;
    NSMutableArray<NSString*>* _resPresets;
}

- (bool)isColorAndDepthCaptureDevice
{
#if UNITY_HAS_COLORANDDEPTH_CAMERA
    if (UnityiOS110orNewer())
    {
        for (AVCaptureDeviceFormat *format in [self->_device formats])
        {
            if ([format supportedDepthDataFormats].count > 0)
                return true;
        }
    }
#endif
    return false;
}

- (WebCamKind)getKind
{
    if ([self->_device.localizedName containsString: @"Telephoto"])
        return kWebCamTelephoto;
    if ([self->_device.localizedName containsString: @"Dual"] && [self isColorAndDepthCaptureDevice])
        return kWebCamColorAndDepth;
    if ([self->_device.localizedName containsString: @"TrueDepth"] && [self isColorAndDepthCaptureDevice])
        return kWebCamColorAndDepth;
    return kWebCamWideAngle;
}

- (void)fillCaptureDeviceResolutions
{
    static NSString* preset[] =
    {
        AVCaptureSessionPresetLow, // usually 192x144
        AVCaptureSessionPreset352x288,
        AVCaptureSessionPresetMedium, // usually 480x320
        AVCaptureSessionPreset640x480,
        AVCaptureSessionPreset1280x720,
        AVCaptureSessionPreset1920x1080, // usually the same as AVCaptureSessionPresetHigh
        AVCaptureSessionPreset3840x2160,
    };
    const int count = sizeof(preset) / sizeof(preset[0]);

    self->_resolutions = [NSMutableArray arrayWithCapacity: count];
    self->_resPresets = [NSMutableArray arrayWithCapacity: count];
    AVCaptureInput* captureInput = [AVCaptureDeviceInput deviceInputWithDevice: self->_device error: nil];

    //Don't attempt to setup an AVCaptureSession if the user has explicitly denied permission to use the camera.
    if (captureInput != nil)
    {
        AVCaptureSession* captureSession = [[AVCaptureSession alloc] init];

        [captureSession addInput: captureInput];

#if UNITY_HAS_COLORANDDEPTH_CAMERA
        if (self->_kind == kWebCamColorAndDepth)
        {
            AVCaptureDepthDataOutput* captureDepthOutput = [[AVCaptureDepthDataOutput alloc] init];
            if ([captureSession canSetSessionPreset: depthCaptureSessionPreset])
            {
                [captureSession setSessionPreset: AVCaptureSessionPresetPhoto];
                [captureSession addOutput: captureDepthOutput];
                CMVideoDimensions dim = CMVideoFormatDescriptionGetDimensions(self->_device.activeDepthDataFormat.formatDescription); // for ColorAndDepth camera return depth buffer resolution
                [self->_resolutions addObject: [NSValue valueWithCGSize: CGSizeMake(dim.width, dim.height)]];
                [self->_resPresets addObject: AVCaptureSessionPresetPhoto];
            }
        }
        else
#endif
        {
            for (int i = 0; i < count; ++i)
            {
                if ([captureSession canSetSessionPreset: preset[i]])
                {
                    [captureSession setSessionPreset: preset[i]];
                    CMVideoDimensions dim = CMVideoFormatDescriptionGetDimensions(self->_device.activeFormat.formatDescription);
                    [self->_resolutions addObject: [NSValue valueWithCGSize: CGSizeMake(dim.width, dim.height)]];
                    [self->_resPresets addObject: preset[i]];
                }
            }
        }
    }
}

- (NSString*)pickPresetFromWidth:(int)w height:(int)h
{
#if UNITY_HAS_COLORANDDEPTH_CAMERA
    if (self->_kind == kWebCamColorAndDepth)
    {
        return depthCaptureSessionPreset;
    }
#endif
    int requestedWidth = w > 0 ? w : 640;
    int requestedHeight = h > 0 ? h : 480;
    if (requestedHeight > requestedWidth) // hardware camera frame is landscape oriented
        std::swap(requestedWidth, requestedHeight);
    NSInteger ret = -1;
    double bestMatch = std::numeric_limits<double>::max();
    for (NSInteger i = 0, n = [_resolutions count]; i < n; ++i)
    {
        double width = [self->_resolutions[i] CGSizeValue].width;
        double height = [self->_resolutions[i] CGSizeValue].height;
        double match = std::abs(std::log(requestedWidth / width)) + std::abs(std::log(requestedHeight / height));
        if (match < bestMatch)
        {
            ret = i;
            bestMatch = match;
        }
    }

    NSAssert(ret != -1, @"Cannot pick capture preset");
    return ret != -1 ? self->_resPresets[ret] : AVCaptureSessionPresetHigh;
}

- (CameraCaptureDevice*)initWithDevice:(AVCaptureDevice*)device
{
    self->_device = device;
    self->_frontFacing = device.position == AVCaptureDevicePositionFront ? 1 : 0;
    self->_autoFocusPointSupported = [CameraCaptureController focusPointSupported: device withFocusMode: AVCaptureFocusModeAutoFocus] ? 1 : 0;
    self->_kind = [self getKind];
    [self fillCaptureDeviceResolutions];
    return self;
}

- (bool)initCaptureForController:(CameraCaptureController*)controller width:(int)w height:(int)h fps:(float)fps isDepth:(bool)isDepth
{
    bool initResult = false;
    NSString *preset = [self pickPresetFromWidth: w height: h];
#if UNITY_HAS_COLORANDDEPTH_CAMERA
    if (UnityiOS110orNewer() && [self isColorAndDepthCaptureDevice])
    {
        initResult = [controller initColorAndDepthCameraCapture: self->_device preset: preset fps: fps isDepth: isDepth];
    }
    else
#endif
    {
        assert(!isDepth);
        initResult = [controller initCapture: self->_device preset: preset fps: fps];
    }
    return initResult;
}

+ (bool)initialized
{
    return videoCaptureDevices != nil;
}

+ (void)createCameraCaptureDevicesArray
{
    videoCaptureDevices = [NSMutableArray arrayWithCapacity: 2];
}

+ (void)addCameraCaptureDevice:(AVCaptureDevice*)device
{
    [videoCaptureDevices addObject: [[CameraCaptureDevice alloc] initWithDevice: device]];
}

@end


extern "C" void UnityEnumVideoCaptureDevices(void* udata, void(*callback)(void* udata, const char* name, int frontFacing, int autoFocusPointSupported, int kind, const int* resolutions, int resCount))
{
    AVCaptureDevice* device;
    if (![CameraCaptureDevice initialized])
    {
        [CameraCaptureDevice createCameraCaptureDevicesArray];

        for (device in [AVCaptureDevice devicesWithMediaType:AVMediaTypeVideo])
        {
            [CameraCaptureDevice addCameraCaptureDevice: device];
        }

        device = [AVCaptureDevice defaultDeviceWithDeviceType: AVCaptureDeviceTypeBuiltInTelephotoCamera mediaType: AVMediaTypeVideo position: AVCaptureDevicePositionBack];
        if (device != nil)
            [CameraCaptureDevice addCameraCaptureDevice: device];

        if (UnityiOS102orNewer())
        {
            device = [AVCaptureDevice defaultDeviceWithDeviceType: AVCaptureDeviceTypeBuiltInDualCamera mediaType: AVMediaTypeVideo position: AVCaptureDevicePositionBack];
            if (device != nil)
                [CameraCaptureDevice addCameraCaptureDevice: device];
        }

        if (UnityiOS111orNewer())
        {
            device = [AVCaptureDevice defaultDeviceWithDeviceType: AVCaptureDeviceTypeBuiltInTrueDepthCamera mediaType: AVMediaTypeVideo position: AVCaptureDevicePositionFront];
            if (device != nil)
                [CameraCaptureDevice addCameraCaptureDevice: device];
        }
    }
    for (CameraCaptureDevice *cameraCaptureDevice in videoCaptureDevices)
    {
        int resCount = [cameraCaptureDevice->_resolutions count];
        int *resolutions = new int[resCount * 2];
        for (int i = 0; i < resCount; ++i)
        {
            resolutions[i * 2] = [cameraCaptureDevice->_resolutions[i] CGSizeValue].width;
            resolutions[i * 2 + 1] = [cameraCaptureDevice->_resolutions[i] CGSizeValue].height;
        }
        callback(udata, [cameraCaptureDevice->_device.localizedName UTF8String], cameraCaptureDevice->_frontFacing, cameraCaptureDevice->_autoFocusPointSupported, cameraCaptureDevice->_kind, resolutions, resCount);
        delete[] resolutions;
    }
}

extern "C" void* UnityInitCameraCapture(int deviceIndex, int w, int h, int fps, int isDepth, void* udata)
{
    if (videoCaptureDevices != nil && deviceIndex < videoCaptureDevices.count)
    {
        CameraCaptureController* controller = [CameraCaptureController alloc];
        bool initResult = [videoCaptureDevices[deviceIndex] initCaptureForController: controller width: w height: h fps: (float)fps isDepth: (isDepth != 0)];
        if (initResult)
        {
            controller->_userData = udata;
            return (__bridge_retained void*)controller;
        }
        controller = nil;
    }
    return 0;
}

extern "C" void UnityStartCameraCapture(void* capture)
{
    [(__bridge CameraCaptureController*)capture start];
}

extern "C" void UnityPauseCameraCapture(void* capture)
{
    [(__bridge CameraCaptureController*)capture pause];
}

extern "C" void UnityStopCameraCapture(void* capture)
{
    CameraCaptureController* controller = (__bridge_transfer CameraCaptureController*)capture;
    [controller stop];
    controller = nil;
}

extern "C" void UnityCameraCaptureExtents(void* capture, int* w, int* h)
{
    CameraCaptureController* controller = (__bridge CameraCaptureController*)capture;
    if (controller == nil)
        return;
    *w = (int)controller->_width;
    *h = (int)controller->_height;
}

extern "C" void UnityCameraCaptureReadToMemory(void* capture, void* dst_, int w, int h)
{
    CameraCaptureController* controller = (__bridge CameraCaptureController*)capture;
    if (controller == nil)
        return;
    assert(w == controller->_width && h == controller->_height);
    [controller capturePixelBufferToMemBuffer: (uint8_t*)dst_];
}

extern "C" int UnityCameraCaptureVideoRotationDeg(void* capture)
{
    CameraCaptureController* controller = (__bridge CameraCaptureController*)capture;
    if (controller == nil)
        return 0;

    // all cams are landscape.
    switch (UnityCurrentOrientation())
    {
        case portrait:              return 90;
        case portraitUpsideDown:    return 270;
        case landscapeLeft:         return controller.captureDevice.position == AVCaptureDevicePositionFront ? 180 : 0;
        case landscapeRight:        return controller.captureDevice.position == AVCaptureDevicePositionFront ? 0 : 180;

        default:                    assert(false && "bad orientation returned from UnityCurrentOrientation()"); break;
    }
    return 0;
}

extern "C" int UnityCameraCaptureVerticallyMirrored(void* capture)
{
    CameraCaptureController* controller = (__bridge CameraCaptureController*)capture;
    if (controller == nil)
        return 0;
    return [controller isCVTextureFlipped];
}

extern "C" int UnityCameraCaptureSetAutoFocusPoint(void* capture, float x, float y)
{
    CameraCaptureController* controller = (__bridge CameraCaptureController*)capture;
    if (controller == nil)
        return 0;
    return [controller setFocusPointWithX: x Y: y];
}

#else

// STUBBED OUT UNTIL DEVELOPER FINDs AN AWESOME CAMERA SOLUTION FOR APPLE TV //

extern "C" void UnityEnumVideoCaptureDevices(void* udata, void(*callback)(void* udata, const char* name, int frontFacing, int autoFocusPointSupported, int kind, const int* resolutions, int resCount))
{
}

extern "C" void* UnityInitCameraCapture(int deviceIndex, int w, int h, int fps, int isDepth, void* udata)
{
    return 0;
}

extern "C" void UnityStartCameraCapture(void* capture)
{
}

extern "C" void UnityPauseCameraCapture(void* capture)
{
}

extern "C" void UnityStopCameraCapture(void* capture)
{
}

extern "C" void UnityCameraCaptureExtents(void* capture, int* w, int* h)
{
}

extern "C" void UnityCameraCaptureReadToMemory(void* capture, void* dst_, int w, int h)
{
}

extern "C" int UnityCameraCaptureVideoRotationDeg(void* capture)
{
    return 0;
}

extern "C" int UnityCameraCaptureVerticallyMirrored(void* capture)
{
    return 0;
}

extern "C" int UnityCameraCaptureSetAutoFocusPoint(void* capture, float x, float y)
{
    return 0;
}

#endif

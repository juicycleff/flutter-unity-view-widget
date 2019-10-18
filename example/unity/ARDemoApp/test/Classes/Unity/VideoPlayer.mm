#include "VideoPlayer.h"

#include "CVTextureCache.h"
#include "CMVideoSampling.h"
#include "GlesHelper.h"

#import <AVFoundation/AVFoundation.h>


static void* _ObserveItemStatusContext = (void*)0x1;
static void* _ObservePlayerItemContext = (void*)0x2;


@implementation VideoPlayerView
+ (Class)layerClass
{
    return [AVPlayerLayer class];
}

- (AVPlayer*)player
{
    return [(AVPlayerLayer*)[self layer] player];
}

- (void)setPlayer:(AVPlayer*)player
{
    [(AVPlayerLayer*)[self layer] setPlayer: player];
}

- (void)dealloc
{
    self.player = nil;
}

@end


@implementation VideoPlayer
{
    AVPlayerItem*   _playerItem;
    AVPlayer*       _player;

    AVAssetReader*              _reader;
    AVAssetReaderTrackOutput*   _videoOut;

    CMSampleBufferRef           _cmSampleBuffer;
    CMVideoSampling             _videoSampling;

    CMTime          _duration;
    CMTime          _curTime;
    CMTime          _curFrameTimestamp;
    CMTime          _lastFrameTimestamp;
    CGSize          _videoSize;

    BOOL            _playerReady;

    // we need to have both because the order of asset/item getting ready is not strict
    BOOL            _assetReady;
    BOOL            _itemReady;
}

@synthesize delegate;
@synthesize player = _player;

- (BOOL)readyToPlay         { return _playerReady; }
- (CGSize)videoSize         { return _videoSize; }
- (CMTime)duration          { return _duration; }
- (float)durationSeconds    { return CMTIME_IS_VALID(_duration) ? (float)CMTimeGetSeconds(_duration) : 0.0f; }


+ (BOOL)CanPlayToTexture:(NSURL*)url    { return [url isFileURL]; }
+ (BOOL)CheckScalingModeAspectFill:(CGSize)videoSize screenSize:(CGSize)screenSize
{
    BOOL ret = NO;

    CGFloat screenAspect = (screenSize.width / screenSize.height);
    CGFloat videoAspect = (videoSize.width / videoSize.height);

    CGFloat width = ceilf(videoSize.width * videoAspect / screenAspect);
    CGFloat height = ceilf(videoSize.height * videoAspect / screenAspect);

    // Do additional input video and device resolution aspect ratio
    // rounding check to see if the width and height values are still
    // the ~same.
    //
    // If they still match, we can change the video scaling mode from
    // aspectFit to aspectFill, this works around some off-by-one scaling
    // errors with certain screen size and video resolution combos
    //
    // TODO: Shouldn't harm to extend width/height check to
    // match values within -1..+1 range from the original

    if (videoSize.width == width && videoSize.height == height)
    {
        ret = YES;
    }

    return ret;
}

- (void)reportError:(NSError*)error category:(const char*)category
{
    ::printf("[%s]Error: %s\n", category, [[error localizedDescription] UTF8String]);
    ::printf("%s\n", [[error localizedFailureReason] UTF8String]);
    [delegate onPlayerError: error];
}

- (void)reportErrorWithString:(const char*)error category:(const char*)category
{
    ::printf("[%s]Error: %s\n", category, error);
    [delegate onPlayerError: nil];
}

- (id)init
{
    if ((self = [super init]))
    {
        _duration = _curTime = kCMTimeZero;
        _curFrameTimestamp = _lastFrameTimestamp = kCMTimeZero;
    }
    return self;
}

- (void)cleanupCVTextureCache
{
    if (_cmSampleBuffer)
    {
        CFRelease(_cmSampleBuffer);
        _cmSampleBuffer = 0;
    }
    CMVideoSampling_Uninitialize(&_videoSampling);
}

- (void)cleanupAssetReader
{
    if (_reader)
        [_reader cancelReading];

    _reader = nil;
    _videoOut = nil;
}

- (void)cleanupPlayer
{
    if (_player)
    {
        [[NSNotificationCenter defaultCenter] removeObserver: self name: AVAudioSessionRouteChangeNotification object: nil];
        [_player.currentItem removeObserver: self forKeyPath: @"status"];
        [_player removeObserver: self forKeyPath: @"currentItem"];
        [_player pause];
        _player = nil;
    }

    if (_playerItem)
    {
        [[NSNotificationCenter defaultCenter] removeObserver: self name: AVPlayerItemDidPlayToEndTimeNotification object: _playerItem];
        _playerItem = nil;
    }
}

- (void)unloadPlayer
{
    [self cleanupCVTextureCache];
    [self cleanupAssetReader];
    [self cleanupPlayer];

    _videoSize = CGSizeMake(0, 0);
    _duration = _curTime = kCMTimeZero;
    _curFrameTimestamp = _lastFrameTimestamp = kCMTimeZero;

    self->_playerReady = self->_assetReady = self->_itemReady = NO;
}

- (BOOL)loadVideo:(NSURL*)url
{
    AVURLAsset* asset = [AVURLAsset URLAssetWithURL: url options: nil];
    if (!asset)
        return NO;

    NSArray* requestedKeys = @[@"tracks", @"playable"];
    [asset loadValuesAsynchronouslyForKeys: requestedKeys completionHandler:^{
        dispatch_async(dispatch_get_main_queue(), ^{
            [self prepareAsset: asset withKeys: requestedKeys];
        });
    }];
    return YES;
}

- (BOOL)_playWithPrepareBlock:(BOOL (^)())preparePlaybackBlock
{
    if (!_playerReady)
        return NO;
    if (preparePlaybackBlock && preparePlaybackBlock() == NO)
        return NO;

    // do not do seekTo and setRate here, it seems that http streaming may hang sometimes if you do so. go figure
    _curFrameTimestamp = _lastFrameTimestamp = kCMTimeZero;
    [_player play];
    return YES;
}

- (BOOL)playToView:(VideoPlayerView*)view
{
    return [self _playWithPrepareBlock:^() {
        view.player = _player;
        return YES;
    }];
}

- (BOOL)playToTexture
{
    return [self _playWithPrepareBlock:^() {
        return [self prepareReader];
    }];
}

- (BOOL)playVideoPlayer
{
    return [self _playWithPrepareBlock: nil];
}

- (BOOL)isPlaying   { return _playerReady && _player.rate != 0.0f; }
- (void)pause
{
    if (_playerReady && _player.rate != 0.0f)
        [_player pause];
}

- (void)resume
{
    if (_playerReady && _player.rate == 0.0f)
    {
        [self seekToTimestamp: _player.currentTime];
        [_player play];
    }
}

- (void)rewind                      { [self seekToTimestamp: kCMTimeZero]; }
- (void)seekTo:(float)timeSeconds   { [self seekToTimestamp: CMTimeMakeWithSeconds(timeSeconds, 1)]; }
- (void)seekToTimestamp:(CMTime)time
{
    [_player seekToTime: time toleranceBefore: kCMTimeZero toleranceAfter: kCMTimeZero];
    _curFrameTimestamp = _lastFrameTimestamp = time;
}

- (intptr_t)curFrameTexture
{
    if (!_reader)
        return 0;

    intptr_t curTex = CMVideoSampling_LastSampledTexture(&_videoSampling);

    CMTime time = [_player currentTime];

    // if we have changed audio route and due to current category apple decided to pause playback - resume automatically
    if (_AudioRouteWasChanged && _player.rate == 0.0f)
        _player.rate = 1.0f;

    if (CMTimeCompare(time, _curTime) == 0 || _reader.status != AVAssetReaderStatusReading)
        return curTex;

    _curTime = time;
    while (_reader.status == AVAssetReaderStatusReading && CMTimeCompare(_curFrameTimestamp, _curTime) <= 0)
    {
        if (_cmSampleBuffer)
            CFRelease(_cmSampleBuffer);

        // TODO: properly handle ending
        _cmSampleBuffer = [_videoOut copyNextSampleBuffer];
        if (_cmSampleBuffer == 0)
        {
            [self cleanupCVTextureCache];
            return 0;
        }

        _curFrameTimestamp = CMSampleBufferGetPresentationTimeStamp(_cmSampleBuffer);
    }

    if (CMTimeCompare(_lastFrameTimestamp, _curFrameTimestamp) < 0)
    {
        _lastFrameTimestamp = _curFrameTimestamp;

        size_t w, h;
        curTex = CMVideoSampling_SampleBuffer(&_videoSampling, _cmSampleBuffer, &w, &h);
        _videoSize = CGSizeMake(w, h);
    }

    return curTex;
}

- (BOOL)setAudioVolume:(float)volume
{
    if (!_playerReady)
        return NO;

    NSArray* audio = [_playerItem.asset tracksWithMediaType: AVMediaTypeAudio];
    NSMutableArray* params = [NSMutableArray array];
    for (AVAssetTrack* track in audio)
    {
        AVMutableAudioMixInputParameters* inputParams = [AVMutableAudioMixInputParameters audioMixInputParameters];
        [inputParams setVolume: volume atTime: kCMTimeZero];
        [inputParams setTrackID: [track trackID]];
        [params addObject: inputParams];
    }

    AVMutableAudioMix* audioMix = [AVMutableAudioMix audioMix];
    [audioMix setInputParameters: params];

    [_playerItem setAudioMix: audioMix];

    return YES;
}

- (void)playerItemDidReachEnd:(NSNotification*)notification
{
    [delegate onPlayerDidFinishPlayingVideo];
}

static bool _AudioRouteWasChanged = false;
- (void)audioRouteChanged:(NSNotification*)notification
{
    _AudioRouteWasChanged = true;
}

- (void)observeValueForKeyPath:(NSString*)path ofObject:(id)object change:(NSDictionary*)change context:(void*)context
{
    BOOL reportPlayerReady = NO;

    if (context == _ObserveItemStatusContext)
    {
        AVPlayerStatus status = (AVPlayerStatus)[[change objectForKey: NSKeyValueChangeNewKey] integerValue];
        switch (status)
        {
            case AVPlayerStatusUnknown:
                break;

            case AVPlayerStatusReadyToPlay:
            {
                NSArray* video = [_playerItem.asset tracksWithMediaType: AVMediaTypeVideo];
                if ([video count])
                    _videoSize = [(AVAssetTrack*)[video objectAtIndex: 0] naturalSize];

                _duration           = [_playerItem duration];
                _assetReady         = YES;
                reportPlayerReady   = _itemReady;
            }
            break;

            case AVPlayerStatusFailed:
            {
                AVPlayerItem *playerItem = (AVPlayerItem*)object;
                [self reportError: playerItem.error category: "prepareAsset"];
            }
            break;
        }
    }
    else if (context == _ObservePlayerItemContext)
    {
        if ([change objectForKey: NSKeyValueChangeNewKey] != (id)[NSNull null])
        {
            _itemReady = YES;
            reportPlayerReady = _assetReady;
        }
    }
    else
    {
        [super observeValueForKeyPath: path ofObject: object change: change context: context];
    }

    if (reportPlayerReady)
    {
        _playerReady = YES;
        [delegate onPlayerReady];
    }
}

- (void)prepareAsset:(AVAsset*)asset withKeys:(NSArray*)requestedKeys
{
    // check succesful loading
    for (NSString* key in requestedKeys)
    {
        NSError* error = nil;
        AVKeyValueStatus keyStatus = [asset statusOfValueForKey: key error: &error];
        if (keyStatus == AVKeyValueStatusFailed)
        {
            [self reportError: error category: "prepareAsset"];
            return;
        }
    }

    if (!asset.playable)
    {
        [self reportErrorWithString: "Item cannot be played" category: "prepareAsset"];
        return;
    }

    if (_playerItem)
    {
        [_playerItem removeObserver: self forKeyPath: @"status"];
        [[NSNotificationCenter defaultCenter] removeObserver: self name: AVPlayerItemDidPlayToEndTimeNotification object: _playerItem];

        _playerItem = nil;
    }

    _playerItem = [AVPlayerItem playerItemWithAsset: asset];
    [_playerItem    addObserver: self forKeyPath: @"status"
     options: NSKeyValueObservingOptionInitial | NSKeyValueObservingOptionNew
     context: _ObserveItemStatusContext
    ];
    [[NSNotificationCenter defaultCenter] addObserver: self selector: @selector(playerItemDidReachEnd:)
     name: AVPlayerItemDidPlayToEndTimeNotification object: _playerItem
    ];

    if (!_player)
    {
        _player = [AVPlayer playerWithPlayerItem: _playerItem];
        [_player    addObserver: self forKeyPath: @"currentItem"
         options: NSKeyValueObservingOptionInitial | NSKeyValueObservingOptionNew
         context: _ObservePlayerItemContext
        ];

        [_player setAllowsExternalPlayback: NO];

        // we want to subscribe to route change notifications, for that we need audio session active
        // and in case FMOD wasnt used up to this point it is still not active
        [[AVAudioSession sharedInstance] setActive: YES error: nil];
        [[NSNotificationCenter defaultCenter] addObserver: self selector: @selector(audioRouteChanged:)
         name: AVAudioSessionRouteChangeNotification object: nil
        ];
    }

    if (_player.currentItem != _playerItem)
        [_player replaceCurrentItemWithPlayerItem: _playerItem];
    else
        [_player seekToTime: kCMTimeZero];
}

- (BOOL)prepareReader
{
    if (!_playerReady)
        return NO;

    [self cleanupAssetReader];

    AVURLAsset* asset = (AVURLAsset*)_playerItem.asset;
    if (![asset.URL isFileURL])
    {
        [self reportErrorWithString: "non-file url. no video to texture." category: "prepareReader"];
        return NO;
    }

    NSError* error = nil;
    _reader = [AVAssetReader assetReaderWithAsset: _playerItem.asset error: &error];
    if (error)
        [self reportError: error category: "prepareReader"];

    _reader.timeRange = CMTimeRangeMake(kCMTimeZero, _duration);


    AVAssetTrack* videoTrack = [[_playerItem.asset tracksWithMediaType: AVMediaTypeVideo] objectAtIndex: 0];

    NSDictionary* options = @{ (NSString*)kCVPixelBufferPixelFormatTypeKey: @(kCVPixelFormatType_32BGRA) };
    _videoOut = [[AVAssetReaderTrackOutput alloc] initWithTrack: videoTrack outputSettings: options];
    _videoOut.alwaysCopiesSampleData = NO;

    if (![_reader canAddOutput: _videoOut])
    {
        [self reportErrorWithString: "canAddOutput returned false" category: "prepareReader"];
        return NO;
    }
    [_reader addOutput: _videoOut];

    if (![_reader startReading])
    {
        [self reportError: [_reader error] category: "prepareReader"];
        return NO;
    }

    [self cleanupCVTextureCache];
    CMVideoSampling_Initialize(&_videoSampling);

    return YES;
}

@end

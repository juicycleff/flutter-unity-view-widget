//
//  FlutterUnityViewController.m
//  FlutterUnityViewController
//
//  Updated by Rex Raphael on 8/27/2020.
//

#import "FlutterUnityViewController.h"
#import "UnityUtils.h"
#import "FlutterUnityView.h"
#include <UnityFramework/UnityFramework.h>

// Global view array
UnityUtils* unityUtils;
NSMutableArray * gFuwViews;

@implementation FLTUnityViewFactory {
    NSObject<FlutterPluginRegistrar>* _registrar;
}

- (instancetype)initWithRegistrar:(NSObject<FlutterPluginRegistrar>*)registrar {
    self = [super init];
    gFuwViews = [[NSMutableArray alloc] init];
    if (self) {
        _registrar = registrar;
    }
    return self;
}

- (NSObject<FlutterMessageCodec>*)createArgsCodec {
    return [FlutterStandardMessageCodec sharedInstance];
}

- (NSObject<FlutterPlatformView>*)createWithFrame:(CGRect)frame
                                   viewIdentifier:(int64_t)viewId
                                        arguments:(id _Nullable)args {
    FLTUnityViewController* controller = [[FLTUnityViewController alloc] initWithFrame:frame
                                                    viewIdentifier:viewId
                                                         arguments:args
                                                         registrar:_registrar];
    return controller;
}
@end


@implementation FLTUnityViewController {
    FLTUnityView* _uView;
    int64_t _viewId;
    FlutterMethodChannel* _channel;
    NSObject<FlutterPluginRegistrar>* _registrar;
    BOOL _disableUnload;
}

- (instancetype)initWithFrame:(CGRect)frame
               viewIdentifier:(int64_t)viewId
                    arguments:(id _Nullable)args
                    registrar:(NSObject<FlutterPluginRegistrar>*)registrar {
    if ([super init]) {
        [gFuwViews addObject: self];
        _viewId = viewId;
        
        // Initialize unity
        if (!unityUtils) {
            unityUtils = [[UnityUtils alloc] init];
        }
        
        if (!_uView) {
            _uView = [[FLTUnityView alloc] initWithFrame: frame];
        }
        
        NSString* channelName = [NSString stringWithFormat:@"plugins.xraph.com/unity_view_%lld", viewId];
        _channel = [FlutterMethodChannel methodChannelWithName:channelName binaryMessenger:registrar.messenger];
        __weak __typeof__(self) weakSelf = self;
        [_channel setMethodCallHandler:^(FlutterMethodCall* call, FlutterResult result) {
            [weakSelf onMethodCall:call result:result];
        }];
        
        [self initView];
        
        id safeMode = args[@"safeMode"];
        if ([safeMode isKindOfClass:[NSArray class]]) {
            // TODO: manage safeMode
        }
        
        id disableUnload = args[@"disableUnload"];
        if ([disableUnload isKindOfClass:[NSNumber class]]) {
            _disableUnload = disableUnload;
        }
        [self attachView];
    }
    return self;
}

- (void)initView {
    _uView = [[FLTUnityView alloc] init];
    if ([unityUtils unityIsInitialized]) {
        [_uView setUnityView: (UIView*)[GetAppController() unityView]];
    } else {
        [unityUtils createPlayer:^{
            [_uView setUnityView: (UIView*)[GetAppController() unityView]];
            [_uView setUfw:unityUtils.ufw];
            [_channel invokeMethod:@"events#onUnityCreated" arguments:nil];
        }];
        [GetAppController() setUnityMessageHandler: ^(const char* message)
        {
            [_channel invokeMethod:@"events#onUnityMessage" arguments:[NSString stringWithUTF8String:message]];
        }];
        [GetAppController() setUnitySceneLoadedHandler:^(const char *name, const int *buildIndex, const bool *isLoaded, const bool *isValid)
        {
            NSDictionary *addObject = @{
                @"name" : [NSString stringWithUTF8String:name],
                @"buildIndex" : [NSNumber numberWithInt:buildIndex],
                @"isLoaded" : [NSNumber numberWithBool:isLoaded],
                @"isValid" : [NSNumber numberWithBool:isValid]
            };
            
            [_channel invokeMethod:@"events#onUnitySceneLoaded" arguments:addObject];
        }];
    }
}

- (void)onMethodCall:(FlutterMethodCall*)call result:(FlutterResult)result {
    if ([call.method isEqualToString:@"unity#dispose"]) {
        [self dispose];
        result(nil);
    } else {
        [self reattachView];
        if ([call.method isEqualToString:@"unity#isReady"]) {
            NSNumber* res = @([unityUtils unityIsInitialized]);
            result(res);
        } else if ([call.method isEqualToString:@"unity#isLoaded"]) {
            NSNumber* res = @([unityUtils isUnityLoaded]);
            result(res);
        } else if ([call.method isEqualToString:@"unity#createUnityPlayer"]) {
            [self initView];
            result(nil);
        } else if ([call.method isEqualToString:@"unity#isPaused"]) {
            NSNumber* res = @([unityUtils isUnityPaused]);
            result(res);
        } else if ([call.method isEqualToString:@"unity#pausePlayer"]) {
            [self pausePlayer:call result:result];
        } else if ([call.method isEqualToString:@"unity#resumePlayer"]) {
            [self resumePlayer:call result:result];
        } else if ([call.method isEqualToString:@"unity#unloadPlayer"]) {
            [self unloadPlayer:call result:result];
        } else if ([call.method isEqualToString:@"unity#quitPlayer"]) {
            [self quitPlayer:call result:result];
        } else if ([[call method] isEqualToString:@"unity#postMessage"]) {
            [self postMessage:call result:result];
        } else if ([call.method isEqualToString:@"unity#waitForUnity"]) {
            result(nil);
        } else {
            result(FlutterMethodNotImplemented);
        }
    }
}

- (void)attachView {
    if (unityUtils) {
        [unityUtils initUnity];
        UIView * unityView = (UIView*)[[[unityUtils ufw] appController] unityView];
        UIView * superview = [unityView superview];
        if (superview) {
            [unityView removeFromSuperview];
            [superview layoutIfNeeded];
        }
        [_uView addSubview: unityView];
        [unityUtils resumeUnity];
    }
}

- (void)reattachView {
    UIView * unityView = (UIView*)[GetAppController() unityView];
    UIView * superview = [unityView superview];
    if (superview != _uView) {
        [self attachView];
    }
}


- (void)dispose {
    [gFuwViews removeObject: self];
    [_channel setMethodCallHandler: nil];
    if (unityUtils) {
        UIView * unityView = (UIView*)[[[unityUtils ufw] appController] unityView];
        UIView * superview = [unityView superview];
        if (superview == _uView) {
            if (![gFuwViews count]) {
                [unityView removeFromSuperview];
                [superview layoutIfNeeded];
                [unityUtils pauseUnity];
            } else {
                [[gFuwViews lastObject] reattachView];
            }
        }
    }
}


- (void)postMessage:(FlutterMethodCall*)call result:(FlutterResult)result {
    NSString* object = [call arguments][@"gameObject"];
    NSString* method = [call arguments][@"methodName"];
    NSString* message = [call arguments][@"message"];

    [unityUtils unityPostMessage:object unityMethodName:method unityMessage:message];
    result(nil);
}

- (void)pausePlayer:(FlutterMethodCall*)call result:(FlutterResult)result {
    [unityUtils pauseUnity];
    result(nil);
}

- (void)resumePlayer:(FlutterMethodCall*)call result:(FlutterResult)result {
    [unityUtils resumeUnity];
    result(nil);
}

- (void)unloadPlayer:(FlutterMethodCall*)call result:(FlutterResult)result {
    [unityUtils unloadUnity];
    result(nil);
}

- (void)quitPlayer:(FlutterMethodCall*)call result:(FlutterResult)result {
    [unityUtils quitUnity];
    result(nil);
}

- (void)setSafeModeEnabled:(BOOL)enabled {
    // _uView = enabled;
}

- (void)setDisabledUnload:(BOOL)enabled {
    _disableUnload = enabled;
}

- (void)setAREnabled:(BOOL)enabled {
    // _uView = enabled;
}


- (void)createPlayer:()call {
    [self initView];
}

- (UIView*)view {
    return _uView;
}

- (void)dealloc {
    NSLog(@"dealloc");
}

@end






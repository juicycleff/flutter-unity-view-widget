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

@implementation FLTUnityViewFactory {
    NSObject<FlutterPluginRegistrar>* _registrar;
}

- (instancetype)initWithRegistrar:(NSObject<FlutterPluginRegistrar>*)registrar {
    self = [super init];
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
        _viewId = viewId;

        NSString* channelName = [NSString stringWithFormat:@"plugins.xraph.com/unity_view_%lld", viewId];
        _channel = [FlutterMethodChannel methodChannelWithName:channelName binaryMessenger:registrar.messenger];
        __weak __typeof__(self) weakSelf = self;
        [_channel setMethodCallHandler:^(FlutterMethodCall* call, FlutterResult result) {
            [weakSelf onMethodCall:call result:result];
        }];
        
        [self initView];
        
        id arMode = args[@"ar"];
        if ([arMode isKindOfClass:[NSArray class]]) {
            // TODO: manage AR
        }
        
        id safeMode = args[@"safeMode"];
        if ([safeMode isKindOfClass:[NSArray class]]) {
            // TODO: manage safeMode
        }
        
        id fullscreenMode = args[@"fullscreen"];
        if ([fullscreenMode isKindOfClass:[NSArray class]]) {
            // TODO: manage fullscreen Mode
        }
        
        id disableUnload = args[@"disableUnload"];
        if ([disableUnload isKindOfClass:[NSNumber class]]) {
            _disableUnload = disableUnload;
        }
    }
    return self;
}

- (void)initView {
    _uView = [[FLTUnityView alloc] init];
    if ([UnityUtils isUnityReady]) {
        [_uView setUnityView: (UIView*)[GetAppController() unityView]];
    } else {
        [UnityUtils createPlayer:^{
            [_uView setUnityView: (UIView*)[GetAppController() unityView]];
        }];
        [GetAppController() setUnityMessageHandler: ^(const char* message)
        {
            [_channel invokeMethod:@"onUnityMessage" arguments:[NSString stringWithUTF8String:message]];
        }];
        [GetAppController() setUnitySceneLoadedHandler:^(const char *name, const int *buildIndex, const bool *isLoaded, const bool *IsValid)
        {
            NSDictionary *addObject = @{
                @"name" : [NSString stringWithUTF8String:name],
                @"buildIndex" : [NSNumber numberWithInt:buildIndex],
                @"isLoaded" : [NSNumber numberWithBool:isLoaded],
                @"FourthKey" : [NSNumber numberWithBool:IsValid]
            };
            
            [_channel invokeMethod:@"onUnitySceneLoaded" arguments:addObject];
        }];
    }
}

- (void)onMethodCall:(FlutterMethodCall*)call result:(FlutterResult)result {
    if ([call.method isEqualToString:@"isReady"]) {
        NSNumber* res = @([UnityUtils isUnityReady]);
        result(res);
    } else if ([call.method isEqualToString:@"isLoaded"]) {
        NSNumber* res = @(IsUnityLoaded());
        result(res);
    } else if ([call.method isEqualToString:@"createUnity"]) {
        [self initView];
        result(nil);
    } else if ([call.method isEqualToString:@"isPaused"]) {
        NSNumber* res = @(IsUnityPaused());
        result(res);
    } else if ([call.method isEqualToString:@"isInBackground"]) {
        NSNumber* res = @(IsUnityInBackground());
        result(res);
    } else if ([call.method isEqualToString:@"pause"]) {
        [self pausePlayer:call result:result];
    } else if ([call.method isEqualToString:@"dispose"]) {
        // [self openNative)];
        result(nil);
    } else if ([call.method isEqualToString:@"resume"]) {
        [self resumePlayer:call result:result];
    } else if ([call.method isEqualToString:@"unload"]) {
        [self unloadPlayer:call result:result];
    } else if ([call.method isEqualToString:@"silentQuitPlayer"]) {
        UnityShowWindowCommand();
        result(nil);
    }  else if ([call.method isEqualToString:@"quitPlayer"]) {
        [self quitPlayer:call result:result];
    } else if ([[call method] isEqualToString:@"postMessage"]) {
        [self postMessage:call result:result];
    } else {
        result(FlutterMethodNotImplemented);
    }
}

- (void)postMessage:(FlutterMethodCall*)call result:(FlutterResult)result {
    NSString* object = [call arguments][@"gameObject"];
    NSString* method = [call arguments][@"methodName"];
    NSString* message = [call arguments][@"message"];

    UnityPostMessage(object, method, message);

    result(nil);
}

- (void)pausePlayer:(FlutterMethodCall*)call result:(FlutterResult)result {
    UnityPauseCommand();
    result(nil);
}

- (void)resumePlayer:(FlutterMethodCall*)call result:(FlutterResult)result {
    UnityResumeCommand();
    result(nil);
}

- (void)unloadPlayer:(FlutterMethodCall*)call result:(FlutterResult)result {
    [UnityUtils unloadUnity];
    [_uView setUnityView: nil];
    result(nil);
}

- (void)quitPlayer:(FlutterMethodCall*)call result:(FlutterResult)result {
    UnityQuitCommand();
    result(nil);
}

- (void)setArEnabled:(BOOL)enabled {
    // _uView = enabled;
}

- (void)setFullscreenEnabled:(BOOL)enabled {
    // _uView = enabled;
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

@end






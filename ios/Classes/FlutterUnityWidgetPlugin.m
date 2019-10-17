//
//  FlutterUnityWidgetPlugin.m
//  FlutterUnityWidgetPlugin
//
//  Created by Kris Pypen on 8/1/19.
//

#import "FlutterUnityWidgetPlugin.h"
#import "UnityUtils.h"
#import "FlutterUnityView.h"

#include <UnityFramework/UnityFramework.h>

@implementation FlutterUnityWidgetPlugin
+ (void)registerWithRegistrar:(NSObject<FlutterPluginRegistrar>*)registrar {
    FUViewFactory* fuviewFactory = [[FUViewFactory alloc] initWithRegistrar:registrar];
    [registrar registerViewFactory:fuviewFactory withId:@"unity_view"];
}
@end

@implementation FUViewFactory {
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
    FUController* controller = [[FUController alloc] initWithFrame:frame
                                                                           viewIdentifier:viewId
                                                                                arguments:args
                                                                          registrar:_registrar];
    return controller;
}

@end

@implementation FUController {
    FlutterUnityView* _uView;
    int64_t _viewId;
    FlutterMethodChannel* _channel;
}

- (instancetype)initWithFrame:(CGRect)frame
               viewIdentifier:(int64_t)viewId
                arguments:(id _Nullable)args
                    registrar:(NSObject<FlutterPluginRegistrar>*)registrar {
    if ([super init]) {
        _viewId = viewId;
        
        NSString* channelName = [NSString stringWithFormat:@"unity_view_%lld", viewId];
        _channel = [FlutterMethodChannel methodChannelWithName:channelName binaryMessenger:registrar.messenger];
        __weak __typeof__(self) weakSelf = self;
        [_channel setMethodCallHandler:^(FlutterMethodCall* call, FlutterResult result) {
            [weakSelf onMethodCall:call result:result];
        }];
    }
    return self;
}

- (void)onMethodCall:(FlutterMethodCall*)call result:(FlutterResult)result {
    if ([[call method] isEqualToString:@"postMessage"]) {
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

- (UIView*)view {
    _uView = [[FlutterUnityView alloc] init];
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
    }
    return _uView;
}

@end






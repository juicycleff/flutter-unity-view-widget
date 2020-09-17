//
//  FlutterUnityWidgetPlugin.m
//  FlutterUnityWidgetPlugin
//
//  Created by Kris Pypen on 8/1/19.
//  Updated by Rex Raphael on 8/27/2020.
//

#import "FlutterUnityWidgetPlugin.h"
#import "FlutterUnityView.h"

@implementation FlutterUnityWidgetPlugin {
    NSObject<FlutterPluginRegistrar>* _registrar;
    FlutterMethodChannel* _channel;
    NSMutableDictionary* _mapControllers;
}

+ (void)registerWithRegistrar:(NSObject<FlutterPluginRegistrar>*)registrar {
    FLTUnityViewFactory* fuviewFactory = [[FLTUnityViewFactory alloc] initWithRegistrar:registrar];
    [registrar registerViewFactory:fuviewFactory withId:@"plugins.xraph.com/unity_view" gestureRecognizersBlockingPolicy:
     FlutterPlatformViewGestureRecognizersBlockingPolicyWaitUntilTouchesEnded];
}

- (FLTUnityViewController*)mapFromCall:(FlutterMethodCall*)call error:(FlutterError**)error {
    id mapId = call.arguments[@"map"];
    FLTUnityViewController* controller = _mapControllers[mapId];
    if (!controller && error) {
    *error = [FlutterError errorWithCode:@"unknown_map" message:nil details:mapId];
    }
    return controller;
}
@end

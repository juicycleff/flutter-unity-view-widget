//
//  FlutterUnityWidgetPlugin.h
//  FlutterUnityWidgetPlugin
//
//  Created by Kris Pypen on 8/1/19.
//

#import <Flutter/Flutter.h>
#import <UnityUtils.h>

@interface FlutterUnityWidgetPlugin : NSObject<FlutterPlugin>
@end

@interface FUController : NSObject <FlutterPlatformView, UnityEventListener>

- (instancetype)initWithFrame:(CGRect)frame
               viewIdentifier:(int64_t)viewId
                    arguments:(id _Nullable)args
              registrar:(NSObject<FlutterPluginRegistrar> *)registrar;

- (UIView*)view;
- (void)onMessage:(NSString *)message;
@end

@interface FUViewFactory : NSObject <FlutterPlatformViewFactory>
- (instancetype)initWithRegistrar:(NSObject<FlutterPluginRegistrar> *)registrar;
@end

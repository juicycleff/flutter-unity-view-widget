//
//  FlutterUnityWidgetPlugin.h
//  FlutterUnityWidgetPlugin
//
//  Created by Kris Pypen on 8/1/19.
//

#import <Flutter/Flutter.h>

@interface FlutterUnityWidgetPlugin : NSObject<FlutterPlugin>
@end

@interface FUController : NSObject <FlutterPlatformView>

- (instancetype)initWithFrame:(CGRect)frame
               viewIdentifier:(int64_t)viewId
                    arguments:(id _Nullable)args
              binaryMessenger:(NSObject<FlutterBinaryMessenger>*)messenger;

- (UIView*)view;
@end

@interface FUViewFactory : NSObject <FlutterPlatformViewFactory>
- (instancetype)initWithRegistrar:(NSObject<FlutterPluginRegistrar> *)registrar;
@end

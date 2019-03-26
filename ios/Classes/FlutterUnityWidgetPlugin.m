#import "FlutterUnityWidgetPlugin.h"
#import <flutter_unity_widget/flutter_unity_widget-Swift.h>

+ (void)registerWithRegistrar:(NSObject<FlutterPluginRegistrar>*)registrar {
  FlutterNativeWebFactory* webviewFactory =
      [[FlutterNativeWebFactory alloc] initWithMessenger:registrar.messenger];
  [registrar registerViewFactory:webviewFactory withId:@"unity_view"];
}


/*
#import "FlutterUnityWidgetPlugin.h"
#import <flutter_unity_widget/flutter_unity_widget-Swift.h>

@implementation FlutterUnityWidgetPlugin
+ (void)registerWithRegistrar:(NSObject<FlutterPluginRegistrar>*)registrar {
  [SwiftFlutterUnityWidgetPlugin registerWithRegistrar:registrar];
}
@end
*/
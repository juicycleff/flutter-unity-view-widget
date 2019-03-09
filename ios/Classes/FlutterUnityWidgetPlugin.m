#import "FlutterUnityWidgetPlugin.h"
#import <flutter_unity_widget/flutter_unity_widget-Swift.h>

@implementation FlutterUnityWidgetPlugin
+ (void)registerWithRegistrar:(NSObject<FlutterPluginRegistrar>*)registrar {
  [SwiftFlutterUnityWidgetPlugin registerWithRegistrar:registrar];
}
@end


#import <Flutter/Flutter.h>

NS_ASSUME_NONNULL_BEGIN

// Defines map UI options writable from Flutter.
@protocol FLTUnityOptionsSink
- (void)setAREnabled:(BOOL)enabled;
- (void)setFullscreenEnabled:(BOOL)enabled;
- (void)setSafeModeEnabled:(BOOL)enabled;
- (void)setDisabledUnload:(BOOL)enabled;
@end

// Defines unity controllable from Flutter.
@interface FLTUnityViewController
: NSObject <FLTUnityOptionsSink, FlutterPlatformView>
- (instancetype)initWithFrame:(CGRect)frame
        viewIdentifier:(int64_t)viewId
        arguments:(id _Nullable)args
        registrar:(NSObject<FlutterPluginRegistrar> *)registrar;
- (void)setAREnabled:(BOOL)enabled;
- (void)setFullscreenEnabled:(BOOL)enabled;
- (void)setSafeModeEnabled:(BOOL)enabled;
- (void)setDisabledUnload:(BOOL)enabled;
- (UIView*)view;
@end

@interface FLTUnityViewFactory : NSObject <FlutterPlatformViewFactory>
- (instancetype)initWithRegistrar:(NSObject<FlutterPluginRegistrar> *)registrar;
@end

NS_ASSUME_NONNULL_END

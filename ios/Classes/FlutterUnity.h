//
// Created by rex on 19/03/2019.
//

#ifndef FLUTTER_UNITY_WIDGET_FLUTTERUNITY_H
#define FLUTTER_UNITY_WIDGET_FLUTTERUNITY_H


#import <Flutter/Flutter.h>

@interface FlutterUnityController : NSObject <FlutterPlatformView>

- (instancetype)initWithWithFrame:(CGRect)frame
                   viewIdentifier:(int64_t)viewId
                        arguments:(id _Nullable)args
                  binaryMessenger:(NSObject<FlutterBinaryMessenger>*)messenger;

- (UIView*)view;
@end

@interface FlutterUnityFactory : NSObject <FlutterPlatformViewFactory>
- (instancetype)initWithMessenger:(NSObject<FlutterBinaryMessenger>*)messenger;
@end

#endif //FLUTTER_UNITY_WIDGET_FLUTTERUNITY_H

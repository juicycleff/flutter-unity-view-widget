//
//  FlutterUnityView.h
//  FlutterUnityView
//
//  Created by krispypen on 8/1/2019
//  Updated by Rex Raphael on 8/27/2020.
//

#import <UIKit/UIKit.h>

#import "UnityUtils.h"


@interface FLTUnityView : UIView

@property (nonatomic, strong) UIView* uView;

@property UnityFramework* ufw;

- (void)initUnity;
- (void)UnloadUnity;
- (void)setUnityView:(UIView *)view;

@end
/*
@protocol UnityEventListeners <NSObject>
    - (void)onMessage:(NSString *)message;
    - (void)onSceneLoaded:(NSString *)name buildIndex:(NSInteger *)bIndex loaded:(bool *)isLoaded valid:(bool *)IsValid;
@end
*/

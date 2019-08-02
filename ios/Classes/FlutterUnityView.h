//
//  FlutterUnityView.h
//  FlutterUnityView
//
//  Created by krispypen on 8/1/2019
//

#import <UIKit/UIKit.h>

#import "UnityUtils.h"

@interface FlutterUnityView : UIView

@property (nonatomic, strong) UIView* uView;

- (void)setUnityView:(UIView *)view;

@end

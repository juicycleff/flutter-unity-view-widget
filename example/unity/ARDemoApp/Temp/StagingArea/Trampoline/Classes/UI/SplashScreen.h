#pragma once

#include "UnityViewControllerBase.h"


@interface SplashScreen : UIImageView
{
}
+ (SplashScreen*)Instance;
@end

@interface SplashScreenController : UnityViewControllerBase
{
}
+ (SplashScreenController*)Instance;
- (void)viewWillTransitionToSize:(CGSize)size withTransitionCoordinator:(id<UIViewControllerTransitionCoordinator>)coordinator;
@end

void    ShowSplashScreen(UIWindow* window);
void    HideSplashScreen();

#pragma once

#import <UIKit/UIKit.h>

#if PLATFORM_IOS
    #define UNITY_VIEW_CONTROLLER_BASE_CLASS UIViewController
#elif PLATFORM_TVOS
    #import <GameController/GCController.h>
    #define UNITY_VIEW_CONTROLLER_BASE_CLASS GCEventViewController
#endif

@interface UnityViewControllerBase : UNITY_VIEW_CONTROLLER_BASE_CLASS
{
}
- (void)viewWillLayoutSubviews;
- (void)viewDidLayoutSubviews;
- (void)viewDidDisappear:(BOOL)animated;
- (void)viewWillDisappear:(BOOL)animated;
- (void)viewDidAppear:(BOOL)animated;
- (void)viewWillAppear:(BOOL)animated;
@end

#if PLATFORM_IOS
#include "UnityViewControllerBase+iOS.h"
#elif PLATFORM_TVOS
#include "UnityViewControllerBase+tvOS.h"
#endif

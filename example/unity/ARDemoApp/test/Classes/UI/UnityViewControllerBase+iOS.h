#pragma once

@interface UnityViewControllerBase (iOS)
- (BOOL)shouldAutorotate;

- (BOOL)prefersStatusBarHidden;
- (UIStatusBarStyle)preferredStatusBarStyle;

- (void)viewWillTransitionToSize:(CGSize)size withTransitionCoordinator:(id<UIViewControllerTransitionCoordinator>)coordinator;
@end

// for better handling of user-imposed screen orientation we will have specific ViewController implementations

// view controllers constrained to one orientation

@interface UnityPortraitOnlyViewController : UnityViewControllerBase
{
}
@end
@interface UnityPortraitUpsideDownOnlyViewController : UnityViewControllerBase
{
}
@end
@interface UnityLandscapeLeftOnlyViewController : UnityViewControllerBase
{
}
@end
@interface UnityLandscapeRightOnlyViewController : UnityViewControllerBase
{
}
@end

// this is default view controller implementation (autorotation enabled)

@interface UnityDefaultViewController : UnityViewControllerBase
{
}
@end

NSUInteger EnabledAutorotationInterfaceOrientations();

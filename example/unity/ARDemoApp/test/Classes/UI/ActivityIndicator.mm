#include "ActivityIndicator.h"
#include "OrientationSupport.h"

@interface ActivityIndicator : UIActivityIndicatorView
{
    UIView* _parent;
}
@end
static ActivityIndicator* _activityIndicator = nil;


@implementation ActivityIndicator
- (void)show:(UIView*)parent
{
    _parent = parent;
    [parent addSubview: self];
    [self startAnimating];
}

- (void)layoutSubviews
{
    self.center = CGPointMake([_parent bounds].size.width / 2, [_parent bounds].size.height / 2);
}

@end

void ShowActivityIndicator(UIView* parent, int style)
{
    if (_activityIndicator != nil)
        return;

    if (style >= 0)
    {
        _activityIndicator = [[ActivityIndicator alloc] initWithActivityIndicatorStyle: (UIActivityIndicatorViewStyle)style];
        _activityIndicator.contentScaleFactor = [UIScreen mainScreen].scale;
    }

    if (_activityIndicator != nil)
        [_activityIndicator show: parent];
}

void ShowActivityIndicator(UIView* parent)
{
    ShowActivityIndicator(parent, UnityGetShowActivityIndicatorOnLoading());
}

void HideActivityIndicator()
{
    if (_activityIndicator)
    {
        [_activityIndicator stopAnimating];
        [_activityIndicator removeFromSuperview];
        _activityIndicator = nil;
    }
}

extern "C" void UnityStartActivityIndicator()
{
    // AppleTV does not support activity indicators
    ShowActivityIndicator(UnityGetGLView());
}

extern "C" void UnityStopActivityIndicator()
{
    HideActivityIndicator();
}

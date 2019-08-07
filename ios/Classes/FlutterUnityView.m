//
//  FlutterUnityView.m
//  FlutterUnityView
//
//  Created by krispypen on 8/1/2019
//

#import "FlutterUnityView.h"

@implementation FlutterUnityView

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    return self;
}

- (void)dealloc
{
}

- (void)setUnityView:(UIView *)view
{
    self.uView = view;
    [self setNeedsLayout];
}

- (void)layoutSubviews
{
    [super layoutSubviews];
    [(UIView *)self.uView removeFromSuperview];
    [self insertSubview:(UIView *)self.uView atIndex:0];
    ((UIView *)self.uView).frame = self.bounds;
    [(UIView *)self.uView setNeedsLayout];
}

@end

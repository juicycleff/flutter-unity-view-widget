#pragma once

@interface UnityView (iOS)

// will simply update content orientation (it might be tweaked in layoutSubviews, due to disagreement between unity and view controller)
- (void)willRotateToOrientation:(UIInterfaceOrientation)toOrientation fromOrientation:(UIInterfaceOrientation)fromOrientation;
// will recreate gles backing if needed and repaint once to make sure we dont have black frame creeping in
- (void)didRotate;

- (void)touchesBegan:(NSSet*)touches withEvent:(UIEvent*)event;
- (void)touchesEnded:(NSSet*)touches withEvent:(UIEvent*)event;
- (void)touchesCancelled:(NSSet*)touches withEvent:(UIEvent*)event;
- (void)touchesMoved:(NSSet*)touches withEvent:(UIEvent*)event;

@end

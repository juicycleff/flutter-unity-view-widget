#pragma once

@interface UnityView (tvOS)

- (void)touchesBegan:(NSSet*)touches withEvent:(UIEvent*)event;
- (void)touchesEnded:(NSSet*)touches withEvent:(UIEvent*)event;
- (void)touchesCancelled:(NSSet*)touches withEvent:(UIEvent*)event;
- (void)touchesMoved:(NSSet*)touches withEvent:(UIEvent*)event;

#if UNITY_TVOS_SIMULATOR_FAKE_REMOTE
- (void)pressesBegan:(NSSet<UIPress*>*)presses withEvent:(UIEvent*)event;
- (void)pressesEnded:(NSSet<UIPress*>*)presses withEvent:(UIEvent*)event;
#endif

@end

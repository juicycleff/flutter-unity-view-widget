#pragma once

#include "UnityForwardDecls.h"
#include "UnityAppController.h"
#include "UnityRendering.h"

@interface UnityAppController (Rendering)

- (void)createDisplayLink;
- (void)repaintDisplayLink;
- (void)destroyDisplayLink;

- (void)repaint;

- (void)selectRenderingAPI;
@property (readonly, nonatomic) UnityRenderingAPI   renderingAPI;

@end

// helper to run unity loop along with proper handling of the rendering
#ifdef __cplusplus
extern "C" {
#endif

void UnityRepaint();

#ifdef __cplusplus
} // extern "C"
#endif

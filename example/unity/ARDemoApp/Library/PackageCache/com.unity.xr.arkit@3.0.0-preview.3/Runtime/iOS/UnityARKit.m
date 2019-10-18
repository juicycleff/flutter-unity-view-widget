#import <Foundation/Foundation.h>
#include "IUnityInterface.h"
#include "UnityAppController.h"

void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityARKitXRPlugin_PluginLoad(IUnityInterfaces* unityInterfaces);
void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityARKitXRPlugin_SetupiOS(UIView* appController);

CGSize UNITY_INTERFACE_API GetUnityRootViewSize()
{
    UnityAppController* appController = _UnityAppController;
    return appController.rootView.bounds.size;
}

void UnityARKit_ensureRootViewIsSetup()
{
    UnityARKitXRPlugin_SetupiOS(_UnityAppController.rootView);
}

@interface UnityARKit : NSObject

+ (void)loadPlugin;

@end

@implementation UnityARKit

+ (void)loadPlugin
{
    UnityRegisterRenderingPluginV5(UnityARKitXRPlugin_PluginLoad, NULL);
    UnityARKitXRPlugin_SetupiOS(_UnityAppController.rootView);
}

@end

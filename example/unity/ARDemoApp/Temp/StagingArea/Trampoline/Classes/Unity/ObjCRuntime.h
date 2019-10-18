#pragma once

#include <objc/objc.h>
#include <objc/runtime.h>
#include <objc/message.h>

// simulator and device differ in how they want objc_msgSendXXX to be called:
// device wants objc_msgSendXXX to be casted to proper type (same as selector we want to call)
// while simulator wants to call them directly
#if TARGET_IPHONE_SIMULATOR || TARGET_TVOS_SIMULATOR
    #define UNITY_OBJC_SEND_MSG(selectorType, msgSendFunc) msgSendFunc
#else
    #define UNITY_OBJC_SEND_MSG(selectorType, msgSendFunc) ((selectorType)msgSendFunc)
#endif


#define UNITY_OBJC_FORWARD_TO_SUPER(self_, super_, selector, selectorType, ...)                 \
    do {                                                                                        \
        struct objc_super super = { .receiver = self_, .super_class = super_ };                 \
        UNITY_OBJC_SEND_MSG(selectorType, objc_msgSendSuper)(&super, selector, __VA_ARGS__);    \
    } while(0)

#define UNITY_OBJC_CALL_ON_SELF(self_, selector, selectorType, ...)                     \
    do {                                                                                \
        UNITY_OBJC_SEND_MSG(selectorType, objc_msgSend)(self_, selector, __VA_ARGS__);  \
    } while(0)


// method type encoding for methods we override
// to get this you need to do: method_getTypeEncoding(class_getInstanceMethod(class, sel)) or method_getTypeEncoding(class_getClassMethod(class, sel))
#define UIView_LayerClass_Enc "#8@0:4"
#define UIViewController_supportedInterfaceOrientations_Enc "Q16@0:8"
#define UIViewController_prefersStatusBarHidden_Enc "B16@0:8"
#define UIScreen_maximumFramesPerSecond_Enc "q16@0:8"
#define UIView_safeAreaInsets_Enc "{UIEdgeInsets=dddd}16@0:8"

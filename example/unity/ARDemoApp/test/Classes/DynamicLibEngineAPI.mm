/* SINGLE CPP FILE TO GENERATE SEAMLESS BRIDGE BETWEEN BINARIES < SHARED ENGINE LIBRARY WITH ABSTRACT EXTERN FUNCTIONS> | < PLAYER EXECUTABLE WITH ABSTRACT FUNCTION IMPLEMENTATION >
1. if building shared engine library this file will:
    define body for Unity* methods that proxy call to actual method
    actual method will be set later from outside with respective call to SetUnity*Body
    defines SetUnity*Body method to set actual method for call, theese functions are exported from library

2. if building player against shared engine library this file will:
    calls SetUnity*Body providing actual method to be called by shared engine library later
    wraps all SetUnity*Body calls in one single method SetAllUnityFunctionsForDynamicPlayerLib

- notes:
  file will be included only if development / il2ccp and:
   - for xcode project if BuildSettings.UseDynamicPlayerLib is true
   - for player if (build.pl staticLib=1, jam BUILD_IOS_DYNAMIC_PLAYER=1)

  DynamicLibEngineAPI-functions.h include list of functions to proxy calls from player to trampoline
   - each function inlist is defined with UnityExternCall or UnityExternCall4StaticMember
*/

// deal with __VA_ARGS__ to convert them to formated lists with provided M macro
#define VA_ARGS_COUNT(...) INTERNAL_GET_ARG_COUNT_PRIVATE(0, ## __VA_ARGS__, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)
#define INTERNAL_GET_ARG_COUNT_PRIVATE(_0, _1_, _2_, _3_, _4_, _5_, _6_, _7_, _8_, _9_, _10_, _11_, _12_, _13_, _14_, _15_, _16_, _17_, _18_, _19_, _20_, count, ...) count

#define JOIN_VA_ARGS_0(M, ...)
#define JOIN_VA_ARGS_1(M, T1) M(T1,1)
#define JOIN_VA_ARGS_2(M, T1, T2) M(T1,1), M(T2,2)
#define JOIN_VA_ARGS_3(M, T1, T2, T3) M(T1,1), M(T2,2), M(T3,3)
#define JOIN_VA_ARGS_4(M, T1, T2, T3, T4) M(T1,1), M(T2,2), M(T3,3), M(T4,4)
#define JOIN_VA_ARGS_5(M, T1, T2, T3, T4, T5) M(T1,1), M(T2,2), M(T3,3), M(T4,4), M(T5,5)
#define JOIN_VA_ARGS_6(M, T1, T2, T3, T4, T5, T6) M(T1,1), M(T2,2), M(T3,3), M(T4,4), M(T5,5), M(T6,6)
#define JOIN_VA_ARGS_7(M, T1, T2, T3, T4, T5, T6, T7) M(T1,1), M(T2,2), M(T3,3), M(T4,4), M(T5,5), M(T6,6), M(T7,7)
#define JOIN_VA_ARGS_8(M, T1, T2, T3, T4, T5, T6, T7, T8) M(T1,1), M(T2,2), M(T3,3), M(T4,4), M(T5,5), M(T6,6), M(T7,7), M(T8,8)
#define JOIN_VA_ARGS_9(M, T1, T2, T3, T4, T5, T6, T7, T8, T9) M(T1,1), M(T2,2), M(T3,3), M(T4,4), M(T5,5), M(T6,6), M(T7,7), M(T8,8), M(T9,9)

#define JOIN_VA_ARGS___(M, N, ...) JOIN_VA_ARGS_##N(M, __VA_ARGS__ )
#define JOIN_VA_ARGS__(M, N, ...) JOIN_VA_ARGS___(M,N,__VA_ARGS__)
#define JOIN_VA_ARGS_(M, ...) JOIN_VA_ARGS__(M,VA_ARGS_COUNT(__VA_ARGS__), __VA_ARGS__)
#define JOIN_VA_ARGS(M, ...) JOIN_VA_ARGS_(M,__VA_ARGS__)

// convert to function definition params:
// egz: VA_ARGS_TO_PARAMS(int, char, bool) expands to: int p3, char p2, bool p1
#define VA_JOIN_AS_PARAMS(type, index) type p##index
#define VA_ARGS_TO_PARAMS(...) JOIN_VA_ARGS(VA_JOIN_AS_PARAMS,__VA_ARGS__)

// convert to function call params
// egz: VA_ARGS_TO_CALL(int,char,bool) exapnds to: p3, p2, p1
#define VA_JOIN_AS_CALL(type, index) p##index
#define VA_ARGS_TO_CALL(...) JOIN_VA_ARGS(VA_JOIN_AS_CALL,__VA_ARGS__)

#ifndef UNITY_ENGINE_DYNAMICLIB_MODE
#define UNITY_ENGINE_DYNAMICLIB_MODE 0
#endif

#if UNITY_ENGINE_DYNAMICLIB_MODE
// [ part of Unity Player ]
// this part generates Unity* functions that act as proxy to call actual function from trampoline
// for each function in DynamicLibEngineAPI-functions.h will be generated proxy function

// proxy for extern "C" function
// egz: UnityExternCall(int,   UnityTestFunctionName, int);
// will expand to:
//  static int(*gPtrUnityTestFunctionName)(int) = nullptr;
//  extern "C" int UnityTestFunctionName(int p1) {
//      assert(gPtrUnityTestFunctionName) != nullptr);
//      return gPtrUnityTestFunctionName(p1);
//  }
//  __attribute__((visibility("default")))
//  extern "C" void SetUnityTestFunctionNameBody(decltype(&UnityTestFunctionName) fPtr) {
//      gPtrUnityTestFunctionName = fPtr;
//  }
    #define UnityExternCall(returnType, funcName, ...)              \
    static returnType(*gPtr##funcName)(__VA_ARGS__) = nullptr;      \
    extern "C" returnType funcName(VA_ARGS_TO_PARAMS(__VA_ARGS__)) {\
        assert(gPtr##funcName != nullptr);                          \
        return gPtr##funcName(VA_ARGS_TO_CALL(__VA_ARGS__));        \
    }                                                               \
    __attribute__((visibility("default")))                          \
    extern "C" void Set##funcName##Body(decltype(&funcName) fPtr) { \
        gPtr##funcName = fPtr;                                      \
    }

// proxy for class static methods
// egz: UnityExternCall4StaticMember(int,  MyClass MyMethod, int);
// will expand to:
//  static int(*gPtrMyClassMyMethod)(int) = nullptr;
//  int MyClass::MyMethod(int p1) {
//      assert(gPtrMyClassMyMethod) != nullptr);
//      return gPtrMyClassMyMethod(p1);
//  }
//  __attribute__((visibility("default")))
//  extern "C" void SetMyClassMyMethodBody(decltype(gPtrMyClassMyMethod) fPtr) {
//      gPtrMyClassMyMethod = fPtr;
//  }
    #define UnityExternCall4StaticMember(returnType, className, funcName, ...)                    \
    static returnType(*gPtr##className##funcName)(__VA_ARGS__) = nullptr;                         \
    returnType className::funcName(VA_ARGS_TO_PARAMS(__VA_ARGS__)) {                              \
        assert(gPtr##className##funcName != nullptr);                                             \
        return gPtr##className##funcName(VA_ARGS_TO_CALL(__VA_ARGS__));                           \
    }                                                                                             \
    __attribute__((visibility("default")))                                                        \
    extern "C" void Set##className##funcName##Body(decltype(gPtr##className##funcName) fPtr) {    \
        gPtr##className##funcName = fPtr;                                                         \
    }

    #include "PlatformDependent/iPhonePlayer/Trampoline/Classes/Unity/UnitySharedDecls.h"
    #include "PlatformDependent/iPhonePlayer/Trampoline/Classes/Unity/UnityRendering.h"
    #include "PlatformDependent/iPhonePlayer/TrampolineInterface.h"
    #include "Runtime/Graphics/DisplayManager.h"
    #include "Runtime/Input/LocationService.h"

    #import <UIKit/UIKit.h>

    #include "External/baselib/builds/Include/PreExternalInclude.h"
    #include <mach-o/ldsyms.h>
    #include "External/baselib/builds/Include/PostExternalInclude.h"

    #include "DynamicLibEngineAPI-functions.h"

    #undef UnityExternCall
    #undef UnityExternCall4StaticMember
#else
// [ part of Xcode project ]
//  for each function defined in DynamicLibEngineAPI-functions.h will be generated SetUnity*Body function

// for extern "C" functions
// egz: UnityExternCall(int,   UnityTestFunctionName, int);
// will expand to:
//  extern "C" UnityTestFunctionName(int);
//  extern "C" SetUnityTestFunctionName(decltype(&UnityTestFunctionName));
    #define UnityExternCall(returnType, funcName, ...)        \
    extern "C" returnType funcName(__VA_ARGS__);              \
    extern "C" void Set##funcName##Body(decltype(&funcName));

// for class static method
// egz: UnityExternCall4StaticMember(int,  MyClass MyMethod, int);
// will expand to:
//  extern "C" void SetMyClassMyMethodBody(decltype(&MyClass::MyMethod));
    #define UnityExternCall4StaticMember(returnType, className, funcName, ...)     \
    extern "C" void Set##className##funcName##Body(decltype(&className::funcName));

    #include "UnityRendering.h"
    #include "Classes/iPhone_Sensors.h"

    #include "UndefinePlatforms.h"
    #include <mach-o/ldsyms.h>
    #include "RedefinePlatforms.h"

    #include "DynamicLibEngineAPI-functions.h"

    #undef UnityExternCall
    #undef UnityExternCall4StaticMember

// single function to call every Set*Body function from DynamicLibEngineAPI-functions.h
    #define UnityExternCall(returnType, funcName, ...)   Set##funcName##Body(funcName);
    #define UnityExternCall4StaticMember(returnType, className, funcName, ...) Set##className##funcName##Body(className::funcName)

extern "C" void SetAllUnityFunctionsForDynamicPlayerLib()
{
        #include "DynamicLibEngineAPI-functions.h"
}

    #undef UnityExternCall
    #undef UnityExternCall4StaticMember
#endif

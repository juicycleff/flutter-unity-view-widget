
#include "RegisterFeatures.h"

extern "C" void UnityEnableGyroscope(bool value);
extern "C" void UnityEnableStylusTouch(bool value);

void RegisterFeatures()
{
    UnityEnableGyroscope(false);
    UnityEnableStylusTouch(true);
}


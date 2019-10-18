#if !INIT_SCRIPTING_BACKEND

extern void RegisterAllClassesGranular();
void RegisterAllClasses()
{
    // Register classes for unit tests
    RegisterAllClassesGranular();
}

void RegisterAllStrippedInternalCalls() {}

void InvokeRegisterStaticallyLinkedModuleClasses() {}
extern "C" void RegisterStaticallyLinkedModulesGranular() {}

#endif // INIT_SCRIPTING_BACKEND

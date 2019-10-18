#pragma once

// Enabling this will force app to do a hard crash instead of a nice exit when UnhandledException
// is thrown. This will force iOS to generate a standard crash report, that can be submitted to
// iTunes by app users and inspected by developers.
#define ENABLE_IOS_CRASH_REPORTING 1

// Enabling this will add a custom Objective-C Uncaught Exception handler, which will print out
// exception information to console.
#define ENABLE_OBJC_UNCAUGHT_EXCEPTION_HANDLER 1

// Enable custom crash reporter to capture crashes. Crash logs will be available to scripts via
// CrashReport API.
#define ENABLE_CUSTOM_CRASH_REPORTER 0

// Enable submission of custom crash reports to Unity servers. This will enable custom crash
// reporter.
#define ENABLE_CRASH_REPORT_SUBMISSION 0


#if ENABLE_CRASH_REPORT_SUBMISSION && !ENABLE_CUSTOM_CRASH_REPORTER
    #undef ENABLE_CUSTOM_CRASH_REPORTER
    #define ENABLE_CUSTOM_CRASH_REPORTER 1
#endif

#if PLATFORM_TVOS
    #undef ENABLE_CUSTOM_CRASH_REPORTER
    #define ENABLE_CUSTOM_CRASH_REPORTER 0
#endif

extern "C" void UnityInstallPostCrashCallback();
void InitCrashHandling();

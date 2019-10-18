#import "PLCrashReporter.h"
#import "CrashReporter.h"

#include "UndefinePlatforms.h"
#include <mach-o/ldsyms.h>
#include "RedefinePlatforms.h"

extern "C" NSString* UnityGetCrashReportsPath();

static NSUncaughtExceptionHandler* gsCrashReporterUEHandler = NULL;

static decltype(_mh_execute_header) * sExecuteHeader = NULL;
extern "C" void UnitySetExecuteMachHeader(const decltype(_mh_execute_header)* header)
{
    sExecuteHeader = header;
}

extern "C" const decltype(_mh_execute_header) * UnityGetExecuteMachHeader() {
    return sExecuteHeader;
}

static void SavePendingCrashReport()
{
    if (![[UnityPLCrashReporter sharedReporter] hasPendingCrashReport])
        return;

    NSFileManager *fm = [NSFileManager defaultManager];
    NSError *error;

    if (![fm createDirectoryAtPath: UnityGetCrashReportsPath() withIntermediateDirectories: YES attributes: nil error: &error])
    {
        ::printf("CrashReporter: could not create crash report directory: %s\n", [[error localizedDescription] UTF8String]);
        return;
    }

    NSData *data = [[UnityPLCrashReporter sharedReporter] loadPendingCrashReportDataAndReturnError: &error];
    if (data == nil)
    {
        ::printf("CrashReporter: failed to load crash report data: %s\n", [[error localizedDescription] UTF8String]);
        return;
    }

    NSString* file = [UnityGetCrashReportsPath() stringByAppendingPathComponent: @"crash-"];
    unsigned long long seconds = (unsigned long long)[[NSDate date] timeIntervalSince1970];
    file = [file stringByAppendingString: [NSString stringWithFormat: @"%llu", seconds]];
    file = [file stringByAppendingString: @".plcrash"];
    if ([data writeToFile: file atomically: YES])
    {
        ::printf("CrashReporter: saved pending crash report.\n");
        if (![[UnityPLCrashReporter sharedReporter] purgePendingCrashReportAndReturnError: &error])
        {
            ::printf("CrashReporter: couldn't remove pending report: %s\n", [[error localizedDescription] UTF8String]);
        }
    }
    else
    {
        ::printf("CrashReporter: couldn't save crash report.\n");
    }

    // Now copy out a pending version that we can delete if/when we send it
    file = [UnityGetCrashReportsPath() stringByAppendingPathComponent: @"crash-pending.plcrash"];
    if ([data writeToFile: file atomically: YES])
    {
        ::printf("CrashReporter: saved copy of pending crash report.\n");
    }
    else
    {
        ::printf("CrashReporter: couldn't save copy of pending crash report.\n");
    }
}

static void InitCrashReporter()
{
    NSError *error;

    UnityInstallPostCrashCallback();
    if ([[UnityPLCrashReporter sharedReporter] enableCrashReporterAndReturnError: &error])
        ::printf("CrashReporter: initialized\n");
    else
        NSLog(@"CrashReporter: could not enable crash reporter: %@", error);

    SavePendingCrashReport();
}

static void UncaughtExceptionHandler(NSException *exception)
{
    NSLog(@"Uncaught exception: %@: %@\n%@", [exception name], [exception reason], [exception callStackSymbols]);
    if (gsCrashReporterUEHandler)
        gsCrashReporterUEHandler(exception);
}

static void InitObjCUEHandler()
{
    // Crash reporter sets its own handler, so we have to save it and call it manually
    gsCrashReporterUEHandler = NSGetUncaughtExceptionHandler();
    NSSetUncaughtExceptionHandler(&UncaughtExceptionHandler);
}

void InitCrashHandling()
{
#if ENABLE_CUSTOM_CRASH_REPORTER
    InitCrashReporter();
#endif

#if ENABLE_OBJC_UNCAUGHT_EXCEPTION_HANDLER
    InitObjCUEHandler();
#endif
}

// This function will be called when AppDomain.CurrentDomain.UnhandledException event is triggered.
// When running on device the app will do a hard crash and it will generate a crash log.
extern "C" void CrashedCheckBelowForHintsWhy()
{
#if ENABLE_IOS_CRASH_REPORTING || ENABLE_CUSTOM_CRASH_REPORTER
    // Make app crash hard here
    __builtin_trap();

    // Just in case above doesn't work
    abort();
#endif
}

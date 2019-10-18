const CFIndex streamSize = 1024;
static NSOperationQueue* webOperationQueue;
static NSURLSession* unityWebRequestSession;


@interface UnityURLRequest : NSMutableURLRequest

@property (readonly, nonatomic) void* udata;
@property (readwrite, nonatomic) NSUInteger taskIdentifier;
@property (readonly, nonatomic) long long estimatedContentLength;
@property (readonly, nonatomic) long long receivedBytes;
@property (readwrite, nonatomic) bool wantCertificateCallback;
@property (readwrite, nonatomic) bool redirecting;
@property (readwrite) bool isDone;

- (id)init:(void*)udata;

@end

static NSMutableArray<UnityURLRequest*>* currentRequests;
static NSLock* currentRequestsLock;

@implementation UnityURLRequest
{
    void* _udata;
    NSUInteger _taskIdentifier;
    long long _estimatedContentLength;
    long long _receivedBytes;
    bool _wantCertificateCallback;
    bool _redirecting;
    bool _isDone;
}

@synthesize udata = _udata;
@synthesize taskIdentifier = _taskIdentifier;
@synthesize estimatedContentLength = _estimatedContentLength;
@synthesize receivedBytes = _receivedBytes;
@synthesize wantCertificateCallback = _wantCertificateCallback;
@synthesize redirecting = _redirecting;
@synthesize isDone = _isDone;

+ (void)storeRequest:(UnityURLRequest *)request taskID:(NSUInteger)taskId
{
    request.taskIdentifier = taskId;
    [currentRequestsLock lock];
    [currentRequests addObject: request];
    [currentRequestsLock unlock];
}

+ (UnityURLRequest*)requestForTask:(NSURLSessionTask*)task
{
    UnityURLRequest* request = nil;
    [currentRequestsLock lock];
    for (unsigned i = 0; i < currentRequests.count; ++i)
        if (currentRequests[i].taskIdentifier == task.taskIdentifier)
        {
            request = currentRequests[i];
            break;
        }
    [currentRequestsLock unlock];
    return request;
}

+ (void)removeRequest:(UnityURLRequest*)request
{
    [currentRequestsLock lock];
    [currentRequests removeObject: request];
    [currentRequestsLock unlock];
}

+ (void)writeBody:(NSOutputStream*)outputStream task:(NSURLSessionTask*)task udata:(void*)udata
{
    unsigned dataSize = streamSize;
    BOOL uploadComplete = FALSE;
    while (!uploadComplete)
    {
        dataSize = streamSize;
        const UInt8* data = (const UInt8*)UnityWebRequestGetUploadData(udata, &dataSize);
        if (dataSize == 0)
            break;
        NSInteger transmitted = [outputStream write: data maxLength: dataSize];
        if (transmitted > 0)
            UnityWebRequestConsumeUploadData(udata, (unsigned)transmitted);
        switch (task.state)
        {
            case NSURLSessionTaskStateCanceling:
            case NSURLSessionTaskStateCompleted:
                uploadComplete = TRUE;
                break;
            default:
                uploadComplete = FALSE;
        }
    }
    [outputStream close];
}

- (id)init:(void*)udata
{
    self = [super init];
    _udata = udata;
    _taskIdentifier = 0;
    _estimatedContentLength = 0;
    _receivedBytes = 0;
    _wantCertificateCallback = false;
    _redirecting = false;
    _isDone = false;
    return self;
}

- (void)updateEstimatedContentLength:(long long)contentLength
{
    if (contentLength > _estimatedContentLength)
        _estimatedContentLength = contentLength;
}

- (void)updateReceivedBytes:(long long)receivedBytes
{
    _receivedBytes += receivedBytes;
    if (_receivedBytes > _estimatedContentLength)
        _estimatedContentLength = _receivedBytes;
}

@end


@interface UnityWebRequestDelegate : NSObject<NSURLSessionDataDelegate, NSURLSessionTaskDelegate>
@end


@implementation UnityWebRequestDelegate

- (void)URLSession:(NSURLSession *)session dataTask:(NSURLSessionDataTask *)dataTask didReceiveResponse:(nonnull NSURLResponse *)response completionHandler:(nonnull void (^)(NSURLSessionResponseDisposition))completionHandler
{
    [self handleResponse: (NSHTTPURLResponse*)response task: dataTask];
    completionHandler(NSURLSessionResponseAllow);
}

- (void)handleResponse:(NSHTTPURLResponse *)response task:(NSURLSessionTask*)task
{
    UnityURLRequest* urequest = [UnityURLRequest requestForTask: task];
    if (urequest == nil)
        return;
    [urequest updateEstimatedContentLength: [response expectedContentLength]];
    UnityReportWebRequestStatus(urequest.udata, (int)[response statusCode]);
    NSDictionary* respHeader = [response allHeaderFields];
    NSEnumerator* headerEnum = [respHeader keyEnumerator];
    for (id headerKey = [headerEnum nextObject]; headerKey; headerKey = [headerEnum nextObject])
        UnityReportWebRequestResponseHeader(urequest.udata, [headerKey UTF8String], [[respHeader objectForKey: headerKey] UTF8String]);
    UnityReportWebRequestReceivedResponse(urequest.udata, (unsigned int)urequest.estimatedContentLength);
}

- (void)URLSession:(NSURLSession *)session dataTask:(NSURLSessionDataTask *)dataTask didReceiveData:(NSData *)data
{
    UnityURLRequest* urequest = [UnityURLRequest requestForTask: dataTask];
    if (urequest == nil)
        return;
    [urequest updateEstimatedContentLength: [dataTask countOfBytesExpectedToReceive]];
    [data enumerateByteRangesUsingBlock:^(const void* bytes, NSRange range, BOOL* stop) {
        UnityReportWebRequestReceivedData(urequest.udata, bytes, (unsigned int)range.length, (unsigned int)urequest.estimatedContentLength);
    }];
}

- (void)URLSession:(NSURLSession *)session task:(NSURLSessionTask *)task willPerformHTTPRedirection:(NSHTTPURLResponse *)response newRequest:(NSURLRequest *)request completionHandler:(void (^)(NSURLRequest * _Nullable))completionHandler
{
    UnityURLRequest* urequest = [UnityURLRequest requestForTask: task];
    if (urequest == nil)
        return;
    urequest.redirecting = true;
    [self handleResponse: response task: task];
    completionHandler(nil);
    [task cancel];
}

- (void)URLSession:(NSURLSession *)session task:(NSURLSessionTask *)task didReceiveChallenge:(NSURLAuthenticationChallenge *)challenge completionHandler:(void (^)(NSURLSessionAuthChallengeDisposition disposition, NSURLCredential* credential))completionHandler
{
    if ([[challenge protectionSpace] authenticationMethod] == NSURLAuthenticationMethodServerTrust)
    {
        UnityURLRequest* urequest = [UnityURLRequest requestForTask: task];
        if (urequest == nil || !urequest.wantCertificateCallback)
        {
            completionHandler(NSURLSessionAuthChallengePerformDefaultHandling, nil);
            return;
        }

#if !defined(DISABLE_WEBREQUEST_CERTIFICATE_CALLBACK)
        SecTrustResultType systemResult;
        SecTrustRef serverTrust = [[challenge protectionSpace] serverTrust];
        if (serverTrust == nil || errSecSuccess != SecTrustEvaluate(serverTrust, &systemResult))
        {
            systemResult = kSecTrustResultOtherError;
        }

        switch (systemResult)
        {
            case kSecTrustResultUnspecified:
            case kSecTrustResultProceed:
            case kSecTrustResultRecoverableTrustFailure:
                break;
            default:
                completionHandler(NSURLSessionAuthChallengePerformDefaultHandling, nil);
                return;
        }

        SecCertificateRef serverCertificate = SecTrustGetCertificateAtIndex(serverTrust, 0);
        if (serverCertificate != nil)
        {
            CFDataRef serverCertificateData = SecCertificateCopyData(serverCertificate);
            const UInt8* const data = CFDataGetBytePtr(serverCertificateData);
            const CFIndex size = CFDataGetLength(serverCertificateData);
            bool trust = UnityReportWebRequestValidateCertificate(urequest.udata, (const char*)data, (unsigned)size);
            CFRelease(serverCertificateData);
            if (trust)
            {
                NSURLCredential *credential = [NSURLCredential credentialForTrust: challenge.protectionSpace.serverTrust];
                completionHandler(NSURLSessionAuthChallengeUseCredential, credential);
                return;
            }
        }
#endif
        completionHandler(NSURLSessionAuthChallengeCancelAuthenticationChallenge, nil);
        return;
    }
    else
        completionHandler(NSURLSessionAuthChallengePerformDefaultHandling, nil);
}

- (void)URLSession:(NSURLSession *)session task:(NSURLSessionTask *)task didCompleteWithError:(NSError *)error
{
    UnityURLRequest* urequest = [UnityURLRequest requestForTask: task];
    if (urequest == nil)
        return;
    urequest.isDone = true;
    if (urequest.redirecting)
        return;
    if (error != nil)
        UnityReportWebRequestNetworkError(urequest.udata, (int)[error code]);
    UnityReportWebRequestFinishedLoadingData(urequest.udata);
}

@end


extern "C" void* UnityCreateWebRequestBackend(void* udata, const char* methodString, const void* headerDict, const char* url)
{
    @autoreleasepool
    {
        static dispatch_once_t onceToken;
        dispatch_once(&onceToken, ^{
            @autoreleasepool
            {
                webOperationQueue = [[NSOperationQueue alloc] init];
                webOperationQueue.name = @"com.unity3d.WebOperationQueue";

                currentRequests = [[NSMutableArray<UnityURLRequest*> alloc] init];
                currentRequestsLock = [[NSLock alloc] init];

                NSURLSessionConfiguration* config = [NSURLSessionConfiguration defaultSessionConfiguration];
                UnityWebRequestDelegate* delegate = [[UnityWebRequestDelegate alloc] init];
                unityWebRequestSession = [NSURLSession sessionWithConfiguration: config delegate: delegate delegateQueue: webOperationQueue];
            }
        });

        UnityURLRequest* request = [[UnityURLRequest alloc] init: udata];
        request.URL = [NSURL URLWithString: [NSString stringWithUTF8String: url]];
        request.HTTPMethod = [NSString stringWithUTF8String: methodString];
        request.allHTTPHeaderFields = (__bridge NSMutableDictionary*)headerDict;
        [request setCachePolicy: NSURLRequestReloadIgnoringLocalCacheData];
        return (__bridge_retained void*)request;
    }
}

extern "C" void UnitySendWebRequest(void* connection, unsigned length, unsigned long timeoutSec, bool wantCertificateCallback)
{
    @autoreleasepool
    {
        UnityURLRequest* request = (__bridge UnityURLRequest*)connection;
        request.wantCertificateCallback = wantCertificateCallback;
        request.timeoutInterval = timeoutSec;

        NSOutputStream* outputStream = nil;
        if (length > 0)
        {
            CFReadStreamRef readStream;
            CFWriteStreamRef writeStream;
            CFStreamCreateBoundPair(kCFAllocatorDefault, &readStream, &writeStream, streamSize);
            CFWriteStreamOpen(writeStream);
            outputStream = (__bridge_transfer NSOutputStream*)writeStream;
            request.HTTPBodyStream = (__bridge_transfer NSInputStream*)readStream;
        }
        NSURLSessionTask* task = [unityWebRequestSession dataTaskWithRequest: request];
        [UnityURLRequest storeRequest: request taskID: task.taskIdentifier];
        [task resume];
        if (length > 0)
            [webOperationQueue addOperationWithBlock:^{
                [UnityURLRequest writeBody: outputStream task: task udata: request.udata];
            }];
    }
}

extern "C" bool UnityWebRequestIsDone(void* connection)
{
    @autoreleasepool
    {
        UnityURLRequest* request = (__bridge UnityURLRequest*)connection;
        return request.isDone;
    }
}

extern "C" void UnityDestroyWebRequestBackend(void* connection)
{
    @autoreleasepool
    {
        UnityURLRequest* request = (__bridge_transfer UnityURLRequest*)connection;
        [UnityURLRequest removeRequest: request];
    }
}

extern "C" void UnityCancelWebRequest(void* connection)
{
    @autoreleasepool
    {
        UnityURLRequest* request = (__bridge UnityURLRequest*)connection;
        [unityWebRequestSession getAllTasksWithCompletionHandler:^(NSArray<NSURLSessionTask*>* _Nonnull tasks) {
            for (unsigned i = 0; i < tasks.count; ++i)
                if (tasks[i].taskIdentifier == request.taskIdentifier)
                {
                    [tasks[i] cancel];
                    break;
                }
        }];
    }
}

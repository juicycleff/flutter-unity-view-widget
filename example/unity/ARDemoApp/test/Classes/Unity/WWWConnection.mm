#include "WWWConnection.h"

#if 0 // old UnityWebRequest backend

// WARNING: this MUST be c decl (NSString ctor will be called after +load, so we cant really change its value)

// If you need to communicate with HTTPS server with self signed certificate you might consider UnityWWWConnectionSelfSignedCertDelegate
// Though use it on your own risk. Blindly accepting self signed certificate is prone to MITM attack

//const char* WWWDelegateClassName      = "UnityWWWConnectionSelfSignedCertDelegate";
const char* WWWDelegateClassName        = "UnityWWWConnectionDelegate";
const char* WWWRequestProviderClassName = "UnityWWWRequestDefaultProvider";
const CFIndex streamSize = 1024;
static NSOperationQueue *webOperationQueue;

@interface UnityWWWConnectionDelegate ()
@property (readwrite, nonatomic) void*                         udata;
@property (readwrite, retain, nonatomic) NSURL*                url;
@property (readwrite, retain, nonatomic) NSString*             user;
@property (readwrite, retain, nonatomic) NSString*             password;
@property (readwrite, retain, atomic)    NSMutableURLRequest*  request;
@property (readwrite, retain, atomic)    NSURLConnection*      connection;
@property (nonatomic)                    BOOL                  manuallyHandleRedirect;
@property (nonatomic)                    BOOL                  wantCertificateCallback;
@property (readwrite, retain, nonatomic) NSOutputStream*       outputStream;
@end


@implementation UnityWWWConnectionDelegate
{
    // link to unity WWW implementation
    void*               _udata;

    // connection parameters
    NSMutableURLRequest* _request;
    // connection that we manage
    NSURLConnection*    _connection;

    // NSURLConnection do not quite handle user:pass@host urls
    // so we need to extract user/pass ourselves
    NSURL*              _url;
    NSString*           _user;
    NSString*           _password;

    // response
    NSInteger           _status;
    size_t              _estimatedLength;
    size_t              _dataRecievd;
    int                 _retryCount;
    NSOutputStream*     _outputStream;

    BOOL                _connectionStarted;
    BOOL                _connectionCancelled;
}

@synthesize url         = _url;
@synthesize user        = _user;
@synthesize password    = _password;
@synthesize request     = _request;
@synthesize connection  = _connection;

@synthesize udata       = _udata;
@synthesize outputStream = _outputStream;

- (NSURL*)extractUserPassFromUrl:(NSURL*)url
{
    self.user       = url.user;
    self.password   = url.password;

    // strip user/pass from url
    NSString* newUrl = [NSString stringWithFormat: @"%@://%@%s%s%@%s%s",
                        url.scheme, url.host,
                        url.port ? ":" : "", url.port ? [[url.port stringValue] UTF8String] : "",
                        url.path,
                        url.fragment ? "#" : "", url.fragment ? [url.fragment UTF8String] : ""
        ];
    return [NSURL URLWithString: newUrl];
}

- (id)initWithURL:(NSURL*)url udata:(void*)udata;
{
    self->_retryCount = 0;
    if ((self = [super init]))
    {
        self.url    = url.user != nil ? [self extractUserPassFromUrl: url] : url;
        self.udata  = udata;

        if ([url.scheme caseInsensitiveCompare: @"http"] == NSOrderedSame)
            NSLog(@"You are using download over http. Currently Unity adds NSAllowsArbitraryLoads to Info.plist to simplify transition, but it will be removed soon. Please consider updating to https.");
    }

    return self;
}

+ (id)newDelegateWithURL:(NSURL*)url udata:(void*)udata
{
    Class target = NSClassFromString([NSString stringWithUTF8String: WWWDelegateClassName]);
    NSAssert([target isSubclassOfClass: [UnityWWWConnectionDelegate class]], @"You MUST subclass UnityWWWConnectionDelegate");

    return [[target alloc] initWithURL: url udata: udata];
}

+ (id)newDelegateWithCStringURL:(const char*)url udata:(void*)udata
{
    return [UnityWWWConnectionDelegate newDelegateWithURL: [NSURL URLWithString: [NSString stringWithUTF8String: url]] udata: udata];
}

+ (NSMutableURLRequest*)newRequestForHTTPMethod:(NSString*)method url:(NSURL*)url headers:(NSDictionary*)headers
{
    Class target = NSClassFromString([NSString stringWithUTF8String: WWWRequestProviderClassName]);
    NSAssert([target conformsToProtocol: @protocol(UnityWWWRequestProvider)], @"You MUST implement UnityWWWRequestProvider protocol");

    return [target allocRequestForHTTPMethod: method url: url headers: headers];
}

- (void)startConnection
{
    if (!_connectionCancelled)
        [self.connection start];
    _connectionStarted = YES;
}

- (void)cancelConnection
{
    if (_connectionStarted)
        [self.connection cancel];
    _connectionCancelled = YES;
}

- (void)abort
{
    [self cancelConnection];
}

- (void)cleanup
{
    [self cancelConnection];
    self.connection = nil;
    self.request = nil;
}

// NSURLConnection Delegate Methods
- (NSURLRequest *)connection:(NSURLConnection *)connection
    willSendRequest:(NSURLRequest *)request
    redirectResponse:(NSURLResponse *)response;
{
    if (response && self.manuallyHandleRedirect)
    {
        // notify TransportiPhone of the redirect and signal to process the next response.
        if ([response isKindOfClass: [NSHTTPURLResponse class]])
        {
            NSHTTPURLResponse *httpresponse = (NSHTTPURLResponse*)response;
            NSMutableDictionary *headers = [httpresponse.allHeaderFields mutableCopy];
            // grab the correct URL from the request that would have
            // automatically been called through NSURLConnection.
            // The reason we do this is that WebRequestProto's state needs to
            // get updated internally, so we intercept redirects, cancel the current
            // NSURLConnection, notify WebRequestProto and let it construct a new
            // request from the updated URL
            [headers setObject: [request.URL absoluteString] forKey: @"Location"];
            httpresponse = [[NSHTTPURLResponse alloc] initWithURL: response.URL statusCode: httpresponse.statusCode HTTPVersion: nil headerFields: headers];
            [self handleResponse: httpresponse];
        }
        else
        {
            [self handleResponse: response];
        }
        [self cancelConnection];
        return nil;
    }
    return request;
}

- (void)connection:(NSURLConnection*)connection didReceiveResponse:(NSURLResponse*)response
{
    [self handleResponse: response];
}

- (void)handleResponse:(NSURLResponse*)response
{
    NSHTTPURLResponse* httpResponse = (NSHTTPURLResponse*)response;
    NSDictionary* respHeader = [httpResponse allHeaderFields];
    NSEnumerator* headerEnum = [respHeader keyEnumerator];

    self->_status = [httpResponse statusCode];
    UnityReportWebRequestStatus(self.udata, (int)self->_status);

    for (id headerKey = [headerEnum nextObject]; headerKey; headerKey = [headerEnum nextObject])
        UnityReportWebRequestResponseHeader(self.udata, [headerKey UTF8String], [[respHeader objectForKey: headerKey] UTF8String]);

    long long contentLength = [response expectedContentLength];

    // ignore any data that we might have recieved during a redirect
    self->_estimatedLength  =  contentLength > 0 && (self->_status / 100 != 3) ? contentLength : 0;
    self->_dataRecievd = 0;
    UnityReportWebRequestReceivedResponse(self.udata, (unsigned int)self->_estimatedLength);
}

- (void)connection:(NSURLConnection*)connection didReceiveData:(NSData*)data
{
    UnityReportWebRequestReceivedData(self.udata, data.bytes, (unsigned int)[data length], (unsigned int)self->_estimatedLength);
}

- (void)connection:(NSURLConnection*)connection didFailWithError:(NSError*)error
{
    UnityReportWebRequestNetworkError(self.udata, (int)[error code]);
    UnityReportWebRequestFinishedLoadingData(self.udata);
}

- (void)connectionDidFinishLoading:(NSURLConnection*)connection
{
    UnityReportWebRequestFinishedLoadingData(self.udata);
}

- (void)connection:(NSURLConnection*)connection didSendBodyData:(NSInteger)bytesWritten totalBytesWritten:(NSInteger)totalBytesWritten totalBytesExpectedToWrite:(NSInteger)totalBytesExpectedToWrite
{
    UnityReportWebRequestSentData(self.udata, (unsigned int)totalBytesWritten, (unsigned int)totalBytesExpectedToWrite);
    if (_outputStream != nil)
    {
        unsigned dataSize = streamSize;
        unsigned transmitted = 0;
        const UInt8* bytes = (const UInt8*)UnityWebRequestGetUploadData(_udata, &dataSize);
        if (dataSize > 0)
        {
            transmitted = [_outputStream write: bytes maxLength: dataSize];
            UnityWebRequestConsumeUploadData(_udata, transmitted);
        }
        if (dataSize < streamSize && transmitted >= dataSize)
        {
            [_outputStream close];
            _outputStream = nil;
        }
    }
}

- (BOOL)connection:(NSURLConnection*)connection handleAuthenticationChallenge:(NSURLAuthenticationChallenge*)challenge
{
    return NO;
}

- (void)connection:(NSURLConnection*)connection willSendRequestForAuthenticationChallenge:(NSURLAuthenticationChallenge*)challenge
{
    if ([[challenge protectionSpace] authenticationMethod] == NSURLAuthenticationMethodServerTrust)
    {
        if (!self.wantCertificateCallback)
        {
            [challenge.sender performDefaultHandlingForAuthenticationChallenge: challenge];
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
                [challenge.sender performDefaultHandlingForAuthenticationChallenge: challenge];
                return;
        }

        SecCertificateRef serverCertificate = SecTrustGetCertificateAtIndex(serverTrust, 0);
        if (serverCertificate != nil)
        {
            CFDataRef serverCertificateData = SecCertificateCopyData(serverCertificate);
            const UInt8* const data = CFDataGetBytePtr(serverCertificateData);
            const CFIndex size = CFDataGetLength(serverCertificateData);
            bool trust = UnityReportWebRequestValidateCertificate(self.udata, (const char*)data, (unsigned)size);
            CFRelease(serverCertificateData);
            if (trust)
            {
                NSURLCredential *credential = [NSURLCredential credentialForTrust: challenge.protectionSpace.serverTrust];
                [challenge.sender useCredential: credential forAuthenticationChallenge: challenge];
                return;
            }
        }
#endif
        [challenge.sender cancelAuthenticationChallenge: challenge];
        return;
    }
    else
    {
        BOOL authHandled = [self connection: connection handleAuthenticationChallenge: challenge];

        if (authHandled == NO)
        {
            self->_retryCount++;

            // Empty user or password
            if (self->_retryCount > 1 || self.user == nil || [self.user length] == 0 || self.password == nil || [self.password length]  == 0)
            {
                [[challenge sender] cancelAuthenticationChallenge: challenge];
                return;
            }

            NSURLCredential* newCredential =
                [NSURLCredential credentialWithUser: self.user password: self.password persistence: NSURLCredentialPersistenceNone];

            [challenge.sender useCredential: newCredential forAuthenticationChallenge: challenge];
        }
    }
}

@end


@implementation UnityWWWConnectionSelfSignedCertDelegate

- (BOOL)connection:(NSURLConnection*)connection handleAuthenticationChallenge:(NSURLAuthenticationChallenge*)challenge
{
    if ([[challenge.protectionSpace authenticationMethod] isEqualToString: @"NSURLAuthenticationMethodServerTrust"])
    {
        [challenge.sender useCredential: [NSURLCredential credentialForTrust: challenge.protectionSpace.serverTrust]
         forAuthenticationChallenge: challenge];

        return YES;
    }

    return [super connection: connection handleAuthenticationChallenge: challenge];
}

@end


@implementation UnityWWWRequestDefaultProvider
+ (NSMutableURLRequest*)allocRequestForHTTPMethod:(NSString*)method url:(NSURL*)url headers:(NSDictionary*)headers
{
    NSMutableURLRequest* request = [[NSMutableURLRequest alloc] init];
    [request setURL: url];
    [request setHTTPMethod: method];
    [request setAllHTTPHeaderFields: headers];
    [request setCachePolicy: NSURLRequestReloadIgnoringLocalCacheData];

    return request;
}

@end

//
// unity interface
//

extern "C" void UnitySendWebRequest(void* connection, unsigned length, unsigned long timeoutSec, bool wantCertificateCallback)
{
    UnityWWWConnectionDelegate* delegate = (__bridge UnityWWWConnectionDelegate*)connection;

    NSMutableURLRequest* request = delegate.request;

    if (length > 0)
    {
        unsigned dataSize = streamSize;
        const void* bytes = UnityWebRequestGetUploadData(delegate.udata, &dataSize);
        if (dataSize > 0)
        {
            CFReadStreamRef readStream;
            CFWriteStreamRef writeStream;
            CFStreamCreateBoundPair(kCFAllocatorDefault, &readStream, &writeStream, streamSize);
            [request setHTTPBodyStream: (__bridge NSInputStream*)readStream];
            CFWriteStreamOpen(writeStream);
            unsigned transmitted = CFWriteStreamWrite(writeStream, (UInt8*)bytes, dataSize);
            UnityWebRequestConsumeUploadData(delegate.udata, transmitted);
            if (dataSize < streamSize && transmitted >= dataSize)
                CFWriteStreamClose(writeStream);
            else
                delegate.outputStream = (__bridge NSOutputStream*)writeStream;
        }
    }

    [request setTimeoutInterval: timeoutSec];

    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        webOperationQueue = [[NSOperationQueue alloc] init];
        webOperationQueue.maxConcurrentOperationCount = [NSProcessInfo processInfo].activeProcessorCount * 5;
        webOperationQueue.name = @"com.unity3d.WebOperationQueue";
    });

    if (wantCertificateCallback)
    {
        delegate.wantCertificateCallback = YES;
    }

    delegate.connection = [[NSURLConnection alloc] initWithRequest: request delegate: delegate startImmediately: NO];
    delegate.manuallyHandleRedirect = YES;
    [delegate.connection setDelegateQueue: webOperationQueue];
    [delegate startConnection];
}

extern "C" void* UnityCreateWebRequestBackend(void* udata, const char* methodString, const void* headerDict, const char* url)
{
    UnityWWWConnectionDelegate* delegate = [UnityWWWConnectionDelegate newDelegateWithCStringURL: url udata: udata];

    delegate.request = [UnityWWWConnectionDelegate newRequestForHTTPMethod: [NSString stringWithUTF8String: methodString] url: delegate.url headers: (__bridge NSDictionary*)headerDict];

    return (__bridge_retained void*)delegate;
}

extern "C" bool UnityWebRequestIsDone(void* connection)
{
    UnityWWWConnectionDelegate* delegate = (__bridge UnityWWWConnectionDelegate*)connection;
    return (delegate.request == nil);
}

extern "C" void UnityDestroyWebRequestBackend(void* connection)
{
    UnityWWWConnectionDelegate* delegate = (__bridge_transfer UnityWWWConnectionDelegate*)connection;

    [delegate cleanup];
    delegate = nil;
}

extern "C" void UnityCancelWebRequest(const void* connection)
{
    UnityWWWConnectionDelegate* delegate = (__bridge UnityWWWConnectionDelegate*)connection;
    [delegate cancelConnection];
}

#endif


extern "C" void UnityWebRequestClearCookieCache(const char* domain)
{
    NSArray<NSHTTPCookie*>* cookies;
    NSHTTPCookieStorage* cookieStorage = [NSHTTPCookieStorage sharedHTTPCookieStorage];
    if (domain == NULL)
        cookies = [cookieStorage cookies];
    else
    {
        NSURL* url = [NSURL URLWithString: [NSString stringWithUTF8String: domain]];
        if (url.path == nil || [url.path isEqualToString: [NSString string]])
        {
            NSMutableArray<NSHTTPCookie*>* hostCookies = [[NSMutableArray<NSHTTPCookie *> alloc] init];
            cookies = [cookieStorage cookies];
            NSUInteger cookieCount = [cookies count];
            for (unsigned i = 0; i < cookieCount; ++i)
                if ([cookies[i].domain isEqualToString: url.host])
                    [hostCookies addObject: cookies[i]];
            cookies = hostCookies;
        }
        else
            cookies = [cookieStorage cookiesForURL: url];
    }
    NSUInteger cookieCount = [cookies count];
    for (int i = 0; i < cookieCount; ++i)
        [cookieStorage deleteCookie: cookies[i]];
}

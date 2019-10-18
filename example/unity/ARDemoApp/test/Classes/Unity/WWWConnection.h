#pragma once

#if 0 // old UnityWebRequest backend

@interface UnityWWWConnectionDelegate : NSObject<NSURLConnectionDataDelegate>
{
}

+ (id)newDelegateWithURL:(NSURL*)url udata:(void*)udata;
+ (id)newDelegateWithCStringURL:(const char*)url udata:(void*)udata;

+ (NSMutableURLRequest*)newRequestForHTTPMethod:(NSString*)method url:(NSURL*)url headers:(NSDictionary*)headers;

@property (readonly, retain, nonatomic) NSURL*              url;
@property (readonly, retain, nonatomic) NSString*           user;
@property (readonly, retain, nonatomic) NSString*           password;
@property (readonly, retain, nonatomic) NSData*             data;
@property (readonly, retain, atomic)    NSURLConnection*    connection;

@property (readonly, nonatomic)         void*               udata;
@property (nonatomic)                   BOOL                shouldAbort;

@end

@interface UnityWWWConnectionSelfSignedCertDelegate : UnityWWWConnectionDelegate
{
}
@end

@protocol UnityWWWRequestProvider<NSObject>
+ (NSMutableURLRequest*)allocRequestForHTTPMethod:(NSString*)method url:(NSURL*)url headers:(NSDictionary*)headers;
@end

@interface UnityWWWRequestDefaultProvider : NSObject<UnityWWWRequestProvider>
{
}
+ (NSMutableURLRequest*)allocRequestForHTTPMethod:(NSString*)method url:(NSURL*)url headers:(NSDictionary*)headers;
@end


// Put this into mm file with your subclass implementation
// pass subclass name to define

// in case you want custom authentication mecanism, you need to subclass unity WWW delegate
#define IMPL_WWW_DELEGATE_SUBCLASS(ClassName)       \
@interface ClassName(OverrideWWWDelegate)           \
{                                                   \
}                                                   \
+(void)load;                                        \
@end                                                \
@implementation ClassName(OverrideWWWDelegate)      \
+(void)load                                         \
{                                                   \
    extern const char* WWWDelegateClassName;        \
    WWWDelegateClassName = #ClassName;              \
}                                                   \
@end                                                \

// in case you want to tweak URL request settings you need to implement UnityWWWRequestProvider protocol
#define IMPL_WWW_REQUEST_PROVIDER(ClassName)        \
@interface ClassName(OverrideRequestProvider)       \
{                                                   \
}                                                   \
+(void)load;                                        \
@end                                                \
@implementation ClassName(OverrideRequestProvider)  \
+(void)load                                         \
{                                                   \
    extern const char* WWWRequestProviderClassName; \
    WWWRequestProviderClassName = #ClassName;       \
}                                                   \
@end                                                \

#endif

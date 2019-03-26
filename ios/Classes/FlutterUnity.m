//
// Created by rex on 19/03/2019.
//

#include "FlutterUnity.h"

@implementation FlutterUnityFactory {
  NSObject<FlutterBinaryMessenger>* _messenger;
}

- (instancetype)initWithMessenger:(NSObject<FlutterBinaryMessenger>*)messenger {
  self = [super init];
  if (self) {
    _messenger = messenger;
  }
  return self;
}


@implementation FlutterUnityController {
  WKWebView* _webView;
  int64_t _viewId;
  FlutterMethodChannel* _channel;
}

- (instancetype)initWithWithFrame:(CGRect)frame
                   viewIdentifier:(int64_t)viewId
                        arguments:(id _Nullable)args
                  binaryMessenger:(NSObject<FlutterBinaryMessenger>*)messenger {
  if ([super init]) {
    _viewId = viewId;
    _webView = [[WKWebView alloc] initWithFrame:frame];
    NSString* channelName = [NSString stringWithFormat:@"nativeweb_%lld", viewId];
    _channel = [FlutterMethodChannel methodChannelWithName:channelName binaryMessenger:messenger];
    __weak __typeof__(self) weakSelf = self;
    [_channel setMethodCallHandler:^(FlutterMethodCall* call, FlutterResult result) {
      [weakSelf onMethodCall:call result:result];
    }];

  }
  return self;
}

- (UIView*)view {
  return _webView;
}

- (void)onMethodCall:(FlutterMethodCall*)call result:(FlutterResult)result {
  if ([[call method] isEqualToString:@"postMessage"]) {
    [self postMessage:call result:result];
  } else if ([[call method] isEqualToString:@"isReady"]) {
    [self postMessage:call result:result];
  } else if ([[call method] isEqualToString:@"createUnity"]) {
    [self postMessage:call result:result];
  } else if ([[call method] isEqualToString:@"pause"]) {
    [self postMessage:call result:result];
  } else if ([[call method] isEqualToString:@"resume"]) {
    [self postMessage:call result:result];
  } else {
    result(FlutterMethodNotImplemented);
  }
}

- (void)postMessage:(FlutterMethodCall*)call result:(FlutterResult)result {
  NSString* url = [call arguments];
  if (![self postMessage:url]) {
    result([FlutterError errorWithCode:@"loadUrl_failed"
                               message:@"Failed parsing the URL"
                               details:[NSString stringWithFormat:@"URL was: '%@'", url]]);
  } else {
    result(nil);
  }
}

- (bool)onPostMessage:(NSString*)url {
  NSURL* nsUrl = [NSURL URLWithString:url];
  if (!nsUrl) {
    return false;
  }
  NSURLRequest* req = [NSURLRequest requestWithURL:nsUrl];
  [_webView loadRequest:req];
  return true;
}

@end



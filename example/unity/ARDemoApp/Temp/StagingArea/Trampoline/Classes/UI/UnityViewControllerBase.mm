#import "UnityViewControllerBase.h"
#import "UnityAppController.h"
#import "UnityAppController+ViewHandling.h"
#import "PluginBase/UnityViewControllerListener.h"

@implementation UnityViewControllerBase

- (void)viewWillLayoutSubviews
{
    [super viewWillLayoutSubviews];
    AppController_SendUnityViewControllerNotification(kUnityViewWillLayoutSubviews);
}

- (void)viewDidLayoutSubviews
{
    [super viewDidLayoutSubviews];
    AppController_SendUnityViewControllerNotification(kUnityViewDidLayoutSubviews);
}

- (void)viewDidDisappear:(BOOL)animated
{
    [super viewDidDisappear: animated];
    AppController_SendUnityViewControllerNotification(kUnityViewDidDisappear);
}

- (void)viewWillDisappear:(BOOL)animated
{
    [super viewWillDisappear: animated];
    AppController_SendUnityViewControllerNotification(kUnityViewWillDisappear);
}

- (void)viewDidAppear:(BOOL)animated
{
    [super viewDidAppear: animated];
    AppController_SendUnityViewControllerNotification(kUnityViewDidAppear);
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear: animated];
    AppController_SendUnityViewControllerNotification(kUnityViewWillAppear);
}

@end

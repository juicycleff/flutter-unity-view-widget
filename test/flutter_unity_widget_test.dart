import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';

import 'fake_unity_widget_controllers.dart';

Future<void> main() async {
  const MethodChannel channel = MethodChannel('plugin.xraph.com/unity_view');
  final FakePlatformViewsController fakePlatformViewsController =
      FakePlatformViewsController();

  TestWidgetsFlutterBinding.ensureInitialized();

  setUpAll(() {
    SystemChannels.platform_views.setMockMethodCallHandler(
        fakePlatformViewsController.fakePlatformViewsMethodHandler);
  });

  setUp(() {
    fakePlatformViewsController.reset();
  });

  tearDown(() {
    channel.setMockMethodCallHandler(null);
  });

  testWidgets('Unity widget ready', (WidgetTester tester) async {
    await tester.pumpWidget(
      UnityWidget(
        onUnityCreated: (UnityWidgetController controller) {},
        printSeupLog: false,
      ),
    );

    final FakePlatformUnityWidget platformUnityWidget =
        fakePlatformViewsController.lastCreatedView!;

    expect(platformUnityWidget.unityReady, true);
  });

  testWidgets('Unity widget pause called successfully',
      (WidgetTester tester) async {
    await tester.pumpWidget(
      UnityWidget(
        onUnityCreated: (UnityWidgetController controller) {},
        printSeupLog: false,
      ),
    );

    final FakePlatformUnityWidget platformUnityWidget =
        fakePlatformViewsController.lastCreatedView!;

    platformUnityWidget.pause();
    expect(platformUnityWidget.unityPaused, true);
  });

  testWidgets(
    'Default Android widget is PlatformViewLink',
    (WidgetTester tester) async {
      await tester.pumpWidget(
        Directionality(
          textDirection: TextDirection.ltr,
          child: UnityWidget(
            onUnityCreated: (UnityWidgetController controller) {},
            printSeupLog: false,
          ),
        ),
      );

      expect(find.byType(PlatformViewLink), findsOneWidget);
    },
  );

  testWidgets('Use AndroidView on Android', (WidgetTester tester) async {
    final MethodChannelUnityWidgetFlutter platform =
        UnityWidgetFlutterPlatform.instance as MethodChannelUnityWidgetFlutter;
    platform.useAndroidViewSurface = false;

    await tester.pumpWidget(
      Directionality(
        textDirection: TextDirection.ltr,
        child: UnityWidget(
          onUnityCreated: (UnityWidgetController controller) {},
          printSeupLog: false,
        ),
      ),
    );

    expect(find.byType(AndroidView), findsOneWidget);
    platform.useAndroidViewSurface = true;
  });
}

import 'package:flutter/material.dart';
import 'package:flutterunitydemo/menu_screen.dart';
import 'package:flutterunitydemo/screens/api_screen.dart';
import 'package:flutterunitydemo/screens/loader_screen.dart';
import 'package:flutterunitydemo/screens/simple_screen.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Unity Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      initialRoute: '/',
      routes: {
        '/': (context) => MenuScreen(),
        '/simple': (context) => SimpleScreen(),
        '/loader': (context) => LoaderScreen(),
        '/api': (context) => ApiScreen(),
      },
    );
  }
}

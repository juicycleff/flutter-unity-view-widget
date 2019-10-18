import 'package:flutter/material.dart';

import 'screens/menu_screen.dart';
import 'screens/with_arkit_screen.dart';
import 'screens/without_arkit_screen.dart';

void main() => runApp(MaterialApp(
  title: 'Named Routes Demo',
  // Start the app with the "/" named route. In this case, the app starts
  // on the FirstScreen widget.
  initialRoute: '/',
  routes: {
    '/': (context) => MenuScreen(),
    '/ar': (context) => WithARkitScreen(),
    '/standard': (context) => WithoutARkitScreen(),
  },
));
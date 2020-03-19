import 'package:flutter/material.dart';

import 'screens/menu_screen.dart';
import 'screens/with_ark_screen.dart';

var MyApp = MaterialApp(
  title: 'Named Routes Demo',
  // Start the app with the "/" named route. In this case, the app starts
  // on the FirstScreen widget.
  initialRoute: '/',
  routes: {
  '/': (context) => MenuScreen(),
  '/ar': (context) => WithARkitScreen(),
  },
);

void main() => runApp(MyApp);
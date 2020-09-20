import 'package:flutter/material.dart';
import 'package:flutter_unity_widget_example/utils/screen_utils.dart';

class MenuScreen extends StatefulWidget {
  MenuScreen({Key key}) : super(key: key);

  @override
  _MenuScreenState createState() => _MenuScreenState();
}

class _MenuScreenState extends State<MenuScreen> {
  List<_MenuListItem> menus = [
    new _MenuListItem(
      description: 'Simple demonstration of unity flutter library',
      route: '/simple',
      title: 'Simple Unity Demo',
      enableAR: false,
    ),
    new _MenuListItem(
      description:
          'Simple demonstration of unity flutter library with AR enabled',
      route: '/simple',
      title: 'Simple Unity Demo (AR)',
      enableAR: true,
    ),
    new _MenuListItem(
      description: 'Unity load and unload unity demo',
      route: '/loader',
      title: 'Safe mode Demo',
      enableAR: false,
    ),
    new _MenuListItem(
      description: 'Unity load and unload unity demo with AR enabled',
      route: '/loader',
      title: 'Safe mode Demo (AR)',
      enableAR: true,
    ),
    new _MenuListItem(
      description:
          'This example shows various native API exposed by the library',
      route: '/api',
      title: 'Native exposed API demo',
      enableAR: false,
    ),
    new _MenuListItem(
      description:
          'This example shows various native API exposed by the library with AR enabled',
      route: '/api',
      title: 'Native exposed API demo (AR)',
      enableAR: true,
    ),
    new _MenuListItem(
      description: 'Unity native activity demo',
      route: '/activity',
      title: 'Native Activity Demo ',
      enableAR: true,
    ),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Menu List'),
      ),
      body: Center(
        child: ListView.builder(
          itemCount: menus.length,
          itemBuilder: (BuildContext context, int i) {
            return ListTile(
              title: Text(menus[i].title),
              subtitle: Text(menus[i].description),
              onTap: () {
                Navigator.of(context).pushNamed(
                  menus[i].route,
                  arguments: ScreenArguments(enableAR: menus[i].enableAR),
                );
              },
            );
          },
        ),
      ),
    );
  }
}

class _MenuListItem {
  final String title;
  final String description;
  final String route;
  final bool enableAR;

  _MenuListItem({this.title, this.description, this.route, this.enableAR});
}

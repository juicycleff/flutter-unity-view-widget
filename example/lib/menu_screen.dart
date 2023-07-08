import 'package:flutter/material.dart';

class MenuScreen extends StatefulWidget {
  const MenuScreen({Key? key}) : super(key: key);

  @override
  State<MenuScreen> createState() => _MenuScreenState();
}

class _MenuScreenState extends State<MenuScreen> {
  List<_MenuListItem> menus = [
    _MenuListItem(
      description: 'Simple demonstration of unity flutter library',
      route: '/simple',
      title: 'Simple Unity Demo',
    ),
    _MenuListItem(
      description: 'No interaction of unity flutter library',
      route: '/none',
      title: 'No Interaction Unity Demo',
    ),
    _MenuListItem(
      description: 'Unity load and unload unity demo',
      route: '/loader',
      title: 'Safe mode Demo',
    ),
    _MenuListItem(
      description:
          'This example shows various native API exposed by the library',
      route: '/api',
      title: 'Native exposed API demo',
    ),
    _MenuListItem(
      title: 'Test Orientation',
      route: '/orientation',
      description: 'test orientation change',
    ),
    _MenuListItem(
      description: 'Unity native activity demo',
      route: '/activity',
      title: 'Native Activity Demo ',
    ),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Menu List'),
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

  _MenuListItem({
    required this.title,
    required this.description,
    required this.route,
  });
}

import 'package:flutter/material.dart';

class MenuScreen extends StatefulWidget {
  const MenuScreen({Key? key}) : super(key: key);

  @override
  State<MenuScreen> createState() => _MenuScreenState();
}

class _MenuScreenState extends State<MenuScreen> {
  bool enableAR = true;

  List<_MenuListItem> menus = [
    _MenuListItem(
      description: 'Simple demonstration of unity flutter library',
      route: '/simple',
      title: 'Simple Unity Demo',
      enableAR: false,
    ),
    _MenuListItem(
      description: 'No interaction of unity flutter library',
      route: '/none',
      title: 'No Interaction Unity Demo',
      enableAR: false,
    ),
    _MenuListItem(
      description: 'Unity load and unload unity demo',
      route: '/loader',
      title: 'Safe mode Demo',
      enableAR: false,
    ),
    _MenuListItem(
      description:
          'This example shows various native API exposed by the library',
      route: '/api',
      title: 'Native exposed API demo',
      enableAR: false,
    ),
    _MenuListItem(
      title: 'Test Orientation',
      route: '/orientation',
      description: 'test orientation change',
      enableAR: false,
    ),
    _MenuListItem(
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
        title: const Text('Menu List'),
        actions: [
          Row(
            children: [
              const Text("Enable AR"),
              Checkbox(
                value: enableAR,
                onChanged: (changed) {
                  if (changed != null) {
                    setState(() {
                      enableAR = changed;
                    });
                  }
                },
              ),
            ],
          ),
        ],
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
  final bool enableAR;

  _MenuListItem({
    required this.title,
    required this.description,
    required this.route,
    required this.enableAR,
  });
}

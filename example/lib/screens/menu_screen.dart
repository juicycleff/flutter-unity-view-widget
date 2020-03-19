import 'package:flutter/material.dart';

class MenuScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Menu'),
      ),
      body: Container(
        child: Column(
          children: <Widget>[
            Expanded(
              flex: 1,
              child: Card(
                  color: Colors.grey,
                  child: Container(
                    width: double.infinity,
                    child: RaisedButton(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.center,
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: <Widget>[
                          Text('AR Demo', style: TextStyle(
                              fontWeight: FontWeight.bold,
                              fontSize: 18
                          ),),
                          Text('Please make sure you exported the ARKIT unity demo example', textAlign: TextAlign.center,),
                        ],
                      ),
                      onPressed: () {
                        // Navigate to the second screen using a named route.
                        Navigator.pushNamed(context, '/ar');
                      },
                    ),
                  )
              ),
            ),
          ],
        ),
      ),
    );
  }
}
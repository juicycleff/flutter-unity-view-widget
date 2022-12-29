class SceneLoaded {
  final String? name;
  final int? buildIndex;
  final bool? isLoaded;
  final bool? isValid;

  SceneLoaded({this.name, this.buildIndex, this.isLoaded, this.isValid});

  /// Mainly for internal use when calling [CameraUpdate.newCameraPosition].
  dynamic toMap() => <String, dynamic>{
        'name': name,
        'buildIndex': buildIndex,
        'isLoaded': isLoaded,
        'isValid': isValid,
      };

  /// Deserializes [SceneLoaded] from a map.
  ///
  /// Mainly for internal use.
  static SceneLoaded fromMap(dynamic json) {
    return SceneLoaded(
      name: json['name'],
      buildIndex: json['buildIndex'],
      isLoaded: json['isLoaded'],
      isValid: json['isValid'],
    );
  }
}

enum UnityEventTypes {
  OnUnityViewCreated,
  OnUnityPlayerReInitialize,
  OnViewReattached,
  OnUnityPlayerCreated,
  OnUnityPlayerUnloaded,
  OnUnityPlayerQuited,
  OnUnitySceneLoaded,
  OnUnityMessage,
}

class EventDataPayload {
  final UnityEventTypes eventType;
  final dynamic data;

  EventDataPayload({required this.eventType, this.data});

  /// Mainly for internal use when calling [CameraUpdate.newCameraPosition].
  dynamic toMap() => <String, dynamic>{
        'eventType': eventType.name,
        'data': data,
      };

  /// Deserializes [SceneLoaded] from a map.
  ///
  /// Mainly for internal use.
  static EventDataPayload? fromMap(dynamic json) {
    if (json == null) {
      return null;
    }

    final eventSourceType = json['eventType'];
    var eventType = UnityEventTypes.OnUnityMessage;

    switch (eventSourceType) {
      case 'OnUnityMessage':
        eventType = UnityEventTypes.OnUnityMessage;
        break;
      case 'OnUnityPlayerCreated':
        eventType = UnityEventTypes.OnUnityPlayerCreated;
        break;
      case 'OnUnityPlayerQuited':
        eventType = UnityEventTypes.OnUnityPlayerQuited;
        break;
      case 'OnUnityPlayerReInitialize':
        eventType = UnityEventTypes.OnUnityPlayerReInitialize;
        break;
      case 'OnUnityPlayerUnloaded':
        eventType = UnityEventTypes.OnUnityPlayerUnloaded;
        break;
      case 'OnUnitySceneLoaded':
        eventType = UnityEventTypes.OnUnitySceneLoaded;
        break;
      case 'OnUnityViewCreated':
        eventType = UnityEventTypes.OnUnityViewCreated;
        break;
      case 'OnViewReattached':
        eventType = UnityEventTypes.OnViewReattached;
        break;
    }

    return EventDataPayload(
      eventType: eventType,
      data: json['data'],
    );
  }
}

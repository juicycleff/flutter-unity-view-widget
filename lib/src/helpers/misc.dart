import 'package:flutter_unity_widget/src/helpers/types.dart';

/// Error thrown when an unknown unity ID is provided to a method channel API.
class UnknownUnityIDError extends Error {
  /// Creates an assertion error with the provided [unityId] and optional
  /// [message].
  UnknownUnityIDError(this.unityId, [this.message]);

  /// The unknown ID.
  final int unityId;

  /// Message describing the assertion error.
  final Object? message;

  @override
  String toString() {
    if (message != null) {
      return "Unknown unity ID $unityId: ${Error.safeToString(message)}";
    }
    return "Unknown unity ID $unityId";
  }
}

typedef UnityMessageCallback = void Function(dynamic handler);

typedef UnitySceneChangeCallback = void Function(SceneLoaded? message);

typedef UnityUnloadCallback = void Function();

part of flutter_unity_widget;

/// Error thrown when an unknown unity ID is provided to a method channel API.
class UnknownUnityIDError extends Error {
  /// Creates an assertion error with the provided [unityId] and optional
  /// [message].
  UnknownUnityIDError(this.unityId, [this.message]);

  /// The unknown ID.
  final int unityId;

  /// Message describing the assertion error.
  final Object? message;

  String toString() {
    if (message != null) {
      return "Unknown unity ID $unityId: ${Error.safeToString(message)}";
    }
    return "Unknown unity ID $unityId";
  }
}

typedef void UnityMessageCallback(dynamic handler);

typedef void UnitySceneChangeCallback(SceneLoaded? message);

typedef void UnityUnloadCallback();

mergeInto(LibraryManager.library, {
  OnUnityMessageWeb: function (message) {
     window.handleUnityMessage(UTF8ToString(message));
  },
    
  OnUnitySceneLoadedWeb: function (name, buildIndex, isLoaded, IsValid) {
     window.handleUnitySceneLoaded(UTF8ToString(name), buildIndex, isLoaded, IsValid);
  },
});
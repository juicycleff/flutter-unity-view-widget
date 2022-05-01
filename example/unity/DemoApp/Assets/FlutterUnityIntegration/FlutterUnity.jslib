mergeInto(LibraryManager.library, {
  OnUnityMessageWeb: function (str) {
     window.handleUnityMessage(str);
  },
    
  OnUnitySceneLoadedWeb: function (name, buildIndex, isLoaded, IsValid) {
     window.handleUnitySceneLoaded(name, buildIndex, isLoaded, IsValid);
  },
});
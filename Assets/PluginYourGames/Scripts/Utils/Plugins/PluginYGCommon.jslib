var FileIO = {

  FreeBuffer_js: function (ptr) {
    _free(ptr);
  }
};

mergeInto(LibraryManager.library, FileIO);

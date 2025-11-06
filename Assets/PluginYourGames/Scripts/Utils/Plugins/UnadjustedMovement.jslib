var FileIO = {

  UnadjustedMovement_js: function () {
    try {
      var canvas = Module['canvas'];
      if (!canvas || typeof canvas.requestPointerLock !== 'function') {
        console.warn('PointerLock not available on canvas.');
        return;
      }

      var original = canvas.requestPointerLock.bind(canvas);
      canvas.requestPointerLock = function (options) {
        options = options || {};
        // Включаем raw/unadjusted movement, если браузер поддерживает
        options.unadjustedMovement = true;
        try {
          return original(options);
        } catch (err) {
          console.log(err);
        }
      };
      console.log('MouseFix_js: canvas.requestPointerLock patched with unadjustedMovement.');
    } catch (e) {
      console.warn('MouseFix_js failed:', e);
    }
  }
};

mergeInto(LibraryManager.library, FileIO);

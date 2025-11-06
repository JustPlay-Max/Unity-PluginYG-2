var FileIO = {
  // нужен базовый элемент для постсета
  $MouseFix_Auto: {},

  // код, который выполнится автоматически при подключении библиотеки
  $MouseFix_Auto__postset:
    "(function(){\
      function patch(){\
        try {\
          var canvas = (typeof Module !== 'undefined') ? Module['canvas'] : null;\
          if (!canvas || typeof canvas.requestPointerLock !== 'function') return false;\
          if (canvas.__mousefixPatched) return true; /* уже пропатчено */\
          var original = canvas.requestPointerLock.bind(canvas);\
          canvas.requestPointerLock = function(options){\
            options = options || {};\
            if (options.unadjustedMovement == null) options.unadjustedMovement = true;\
            try { return original(options); } catch (err) { console.log(err); }\
          };\
          canvas.__mousefixPatched = true;\
          console.log('MouseFix: canvas.requestPointerLock patched (auto).');\
          return true;\
        } catch(e){\
          console.warn('MouseFix auto patch failed:', e);\
          return true; /* чтобы не зациклиться */\
        }\
      }\
      if (!patch()){\
        var id = setInterval(function(){ if (patch()) clearInterval(id); }, 50);\
      }\
    })();",

  FreeBuffer_js: function (ptr) {
    _free(ptr);
  }
};

mergeInto(LibraryManager.library, FileIO);

mergeInto(LibraryManager.library, {
    GetClipboardTextAsync_js: function(callbackPointer) {
        var invokeCallback = function(textPointer) {
            if (typeof Runtime !== 'undefined' &&
                typeof Runtime.dynCall === 'function') {
                Runtime.dynCall('vi', callbackPointer, [textPointer]);
                return;
            }

            if (typeof dynCall === 'function') {
                dynCall('vi', callbackPointer, [textPointer]);
                return;
            }

            if (typeof getWasmTableEntry === 'function') {
                getWasmTableEntry(callbackPointer)(textPointer);
                return;
            }

            throw new Error('No supported WebAssembly callback mechanism found');
        };

        navigator.clipboard.readText().then(
            text => {
                var buffer = _malloc((text.length + 1) * 2); // резервируем память
                stringToUTF16(text, buffer, (text.length + 1) * 2); // конвертируем строку в UTF-16
                invokeCallback(buffer);
            },
            error => {
                console.error("Failed to read text from the clipboard: ", error);
                invokeCallback(0); // возвращаем null при ошибке
            }
        );
    },

    FreeClipboardText_js: function(textPointer) {
        _free(textPointer); // освобождаем память
    },

    SetClipboardText_js: function(text) {
        if (ysdk !== null) {
            ysdk.clipboard.writeText(UTF8ToString(text));
        }
        else {
            navigator.clipboard.writeText(UTF8ToString(text))
                .then(() => console.log("Text copied to the clipboard"))
                .catch(error => console.error("Failed to copy text to the clipboard: ", error));
        }
    }
});

mergeInto(LibraryManager.library, {
    GetClipboardTextAsync_js: function(callbackPointer) {
        navigator.clipboard.readText()
            .then(text => {
                var buffer = _malloc((text.length + 1) * 2); // резервируем память
                stringToUTF16(text, buffer, (text.length + 1) * 2); // конвертируем строку в UTF-16
                Runtime.dynCall('vi', callbackPointer, [buffer]); // вызываем callback с буфером
            })
            .catch(error => {
                console.error("Ошибка получения текста из буфера обмена: ", error);
                Runtime.dynCall('vi', callbackPointer, [0]); // возвращаем null при ошибке
            });
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
				.then(() => console.log("Текст скопирован в буфер обмена"))
				.catch(error => console.error("Ошибка при копировании текста в буфер обмена: ", error));
		}
    }
});

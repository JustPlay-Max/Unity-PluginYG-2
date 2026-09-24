mergeInto(LibraryManager.library,
{
	$PaymentsYG: {
		focusGame: function () {
			setTimeout(function () {
				if (typeof FocusGame === 'function') {
					FocusGame(false);
					return;
				}

				var canvas = document.getElementById('unity-canvas');
				if (canvas && typeof canvas.focus === 'function') {
					canvas.focus({ preventScroll: true });
				}
			}, 50);
		},

		restoreFocusGame: function () {
			PaymentsYG.focusGame();
			setTimeout(PaymentsYG.focusGame, 300);
			setTimeout(PaymentsYG.focusGame, 1000);
		},

		callbackString: function (callback, value) {
			var bufferSize = lengthBytesUTF8(value || '') + 1;
			var buffer = _malloc(bufferSize);
			stringToUTF8(value || '', buffer, bufferSize);
			wasmTable.get(callback)(buffer);
		},

		error: function (error) {
			if (!error) return 'Unknown error';
			if (error.message) return error.message;
			if (error.error_type) return error.error_type;
			return JSON.stringify(error);
		},

		setPauseGame: function (pause) {
			if (typeof YG2Instance === 'function') {
				YG2Instance('SetPauseGame', pause ? 'true' : 'false');
			}
		},

		ensureYandexPaymentsSigned: function () {
			if (PaymentsYG.yandexPaymentsSigned) {
				return Promise.resolve(PaymentsYG.yandexPaymentsSigned);
			}

			if (typeof ysdk === 'undefined' || ysdk === null || typeof ysdk.getPayments !== 'function') {
				return Promise.reject(new Error('Yandex SDK payments are not initialized'));
			}

			return ysdk.getPayments({ signed: true }).then(function (payments) {
				PaymentsYG.yandexPaymentsSigned = payments;
				return payments;
			});
		},

		yandexPaymentsSigned: null
	},

	InitPayments_js: function()
	{
		var returnStr = paymentsData;
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},
	
	ConsumePurchase_js: function(id, onPurchaseSuccess)
	{
		ConsumePurchase(UTF8ToString(id), onPurchaseSuccess);
	},
	
	ConsumePurchases_js: function(onPurchaseSuccess)
	{
		ConsumePurchases(onPurchaseSuccess);
	},
	
	BuyPayments_js: function(id)
	{
		BuyPayments(UTF8ToString(id));
	},

	YandexPayments_InitSigned__deps: ['$PaymentsYG'],
	YandexPayments_InitSigned: function()
	{
		PaymentsYG.ensureYandexPaymentsSigned().catch(function (error) {
			console.warn('[PluginYG Payments] Yandex signed payments init failed:', PaymentsYG.error(error));
		});
	},

	YandexPayments_PurchaseSigned__deps: ['$PaymentsYG'],
	YandexPayments_PurchaseSigned: function(idPtr, successCallback, errorCallback)
	{
		var id = UTF8ToString(idPtr);

		PaymentsYG.ensureYandexPaymentsSigned()
			.then(function (payments) {
				PaymentsYG.setPauseGame(true);
				return payments.purchase(id);
			})
			.then(function (purchase) {
				PaymentsYG.setPauseGame(false);
				PaymentsYG.restoreFocusGame();
				var signature = purchase && (purchase.signature || purchase._signature)
					? (purchase.signature || purchase._signature)
					: '';
				if (!signature) {
					PaymentsYG.callbackString(errorCallback, 'Yandex purchase signature is empty');
					return;
				}

				PaymentsYG.callbackString(successCallback, signature);
			})
			.catch(function (error) {
				PaymentsYG.setPauseGame(false);
				PaymentsYG.restoreFocusGame();
				PaymentsYG.callbackString(errorCallback, PaymentsYG.error(error));
			});
	},

	YandexPayments_ConsumeToken__deps: ['$PaymentsYG'],
	YandexPayments_ConsumeToken: function(tokenPtr)
	{
		var token = UTF8ToString(tokenPtr);
		if (!token) return;

		PaymentsYG.ensureYandexPaymentsSigned()
			.then(function (payments) {
				return payments.consumePurchase(token);
			})
			.catch(function (error) {
				console.warn('[PluginYG Payments] Yandex consume token failed:', PaymentsYG.error(error));
			});
	},

	YandexPayments_GetPurchasesSigned__deps: ['$PaymentsYG'],
	YandexPayments_GetPurchasesSigned: function(successCallback, errorCallback)
	{
		PaymentsYG.ensureYandexPaymentsSigned()
			.then(function (payments) {
				return payments.getPurchases();
			})
			.then(function (result) {
				var signature = result && (result.signature || result._signature)
					? (result.signature || result._signature)
					: '';
				if (!signature) {
					PaymentsYG.callbackString(errorCallback, 'Yandex purchases signature is empty');
					return;
				}

				PaymentsYG.callbackString(successCallback, signature);
			})
			.catch(function (error) {
				PaymentsYG.callbackString(errorCallback, PaymentsYG.error(error));
			});
	}
});

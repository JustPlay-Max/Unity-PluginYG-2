mergeInto(LibraryManager.library, {
    InitYandexMetrica: function(counterId) {
        const id = UTF8ToString(counterId);

        const script1 = document.createElement("script");
        script1.type = "text/javascript";
        script1.textContent = `
            window.yandexMetricaCounterId = parseInt(${id});
        `;
        document.head.appendChild(script1);

        const script2 = document.createElement("script");
        script2.type = "text/javascript";
        script2.textContent = `
            (function (m, e, t, r, i, k, a) {
                m[i] = m[i] || function () { (m[i].a = m[i].a || []).push(arguments) };
                m[i].l = 1 * new Date();
                for (var j = 0; j < document.scripts.length; j++) { if (document.scripts[j].src === r) { return; } }
                k = e.createElement(t), a = e.getElementsByTagName(t)[0], k.async = 1, k.src = r, a.parentNode.insertBefore(k, a)
            })(window, document, "script", "https://mc.yandex.ru/metrika/tag.js", "ym");

            ym(window.yandexMetricaCounterId, "init", {
                clickmap: false,
                trackLinks: true,
                accurateTrackBounce: true
            });
        `;
        document.head.appendChild(script2);

        const noscript = document.createElement("noscript");
        noscript.innerHTML = `
            <div>
                <img src="https://mc.yandex.ru/watch/${id}" style="position:absolute; left:-9999px;" alt="" />
            </div>
        `;
        document.body.appendChild(noscript);
    },

    YandexMetricaSend_js: function(eventName) {
        try {
            if (typeof ym === "function") {
                ym(window.yandexMetricaCounterId, 'reachGoal', UTF8ToString(eventName));
            } else {
                console.warn("Yandex Metrica is not ready yet.");
            }
        } catch (e) {
            console.error("Error sending data to Yandex Metrica:", e);
        }
    },

    YandexMetricaSend2_js: function(eventName, eventDataJson) {
        try {
            var jsonString = UTF8ToString(eventDataJson);
            if (!jsonString) {
                console.warn("Event data is empty or invalid");
                return;
            }
            var data = JSON.parse(jsonString);
            if (typeof ym === "function") {
                ym(window.yandexMetricaCounterId, 'reachGoal', UTF8ToString(eventName), data);
            } else {
                console.warn("Yandex Metrica is not ready yet.");
            }
        } catch (e) {
            console.error("Error sending data to Yandex Metrica:", e);
        }
    }
});

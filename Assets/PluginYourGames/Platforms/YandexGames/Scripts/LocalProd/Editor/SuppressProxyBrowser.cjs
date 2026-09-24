const childProcess = require('child_process');
const https = require('https');
const spawn = childProcess.spawn;
const createServer = https.createServer;

// sdk-dev-proxy serves Unity's precompressed files without Content-Encoding.
// Add the headers at the HTTPS response boundary, leaving its SDK and CSP routes intact.
https.createServer = function (options, listener) {
    if (typeof listener !== 'function' || !options || !options.cert)
        return createServer.apply(this, arguments);

    return createServer.call(this, options, function (request, response) {
        const pathname = request.url.split('?')[0];
        const match = /\.(br|gz)$/i.exec(pathname);

        if (match) {
            const encoding = match[1].toLowerCase() === 'br' ? 'br' : 'gzip';
            const type = /\.wasm\.(br|gz)$/i.test(pathname) ? 'application/wasm'
                : /\.js\.(br|gz)$/i.test(pathname) ? 'application/javascript'
                : 'application/octet-stream';
            const writeHead = response.writeHead;

            response.writeHead = function (statusCode) {
                if (statusCode >= 200 && statusCode < 300) {
                    response.setHeader('Content-Encoding', encoding);
                    response.setHeader('Content-Type', type);
                }

                return writeHead.apply(this, arguments);
            };
        }

        return listener(request, response);
    });
};

childProcess.spawn = function (command, args, options) {
    const isBrowser = command === 'explorer.exe' || command === 'open' || command === 'xdg-open';
    const isDraft = Array.isArray(args) && args.length === 1 &&
        /^https:\/\/yandex\.[^/]+\/games\/app\//.test(args[0]);

    if (isBrowser && isDraft)
        return spawn(process.execPath, ['-e', ''], { stdio: ['ignore', 'ignore', 'pipe'] });

    return spawn.apply(this, arguments);
};

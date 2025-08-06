let flasgsData = NO_DATA;

function GetFlags() {
    return new Promise((resolve) => {
        if (ysdk == null) {
            Final(NO_DATA);
            return;
        }
        try {
            ysdk.getFlags().then(flags => {
                let names = [];
                let values = [];

                for (let key in flags) {
                    if (flags.hasOwnProperty(key)) {
                        names.push(key);
                        values.push(flags[key]);
                    }
                }

                let jsonFlags = {
                    "names": names,
                    "values": values
                };

                Final(JSON.stringify(jsonFlags));
            });
        } catch (e) {
            console.error('CRASH Get Flags: ', e.message);
            Final(NO_DATA);
        }

        function Final(res) {
            flasgsData = res
            resolve(res);
        }
    });
}
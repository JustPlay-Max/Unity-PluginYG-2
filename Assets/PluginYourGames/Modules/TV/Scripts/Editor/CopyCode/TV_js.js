
let lastBackPressTime = 0;
let previousButtonState = {};

function TVInit() {
    LogStyledMessage('Init TV ysdk');

    if (ysdk.deviceInfo.isTV()) {
        ysdk.onEvent(ysdk.EVENTS.HISTORY_BACK, () => {
            const currentTime = Date.now();

            if (!initGame || currentTime - lastBackPressTime < 300) {
                ysdk.dispatchEvent(ysdk.EVENTS.EXIT);
            } else {
                lastBackPressTime = currentTime;
                YG2Instance('TVKeyBack');
            }
        });

        document.addEventListener("keydown", (e) => {
            YG2Instance('TVKeyDown', e.key);
        });

        document.addEventListener("keyup", (e) => {
            YG2Instance('TVKeyUp', e.key);
        });

        function UpdateGamepadState() {
            var gamepads = navigator.getGamepads();
            for (var i = 0; i < gamepads.length; i++) {
                var gamepad = gamepads[i];
                if (gamepad) {
                    if (gamepad.buttons[12].pressed && !previousButtonState[12]) {
                        YG2Instance('TVKeyDown', 'Up');
                    } else if (!gamepad.buttons[12].pressed && previousButtonState[12]) {
                        YG2Instance('TVKeyUp', 'Up');
                    }
                    if (gamepad.buttons[13].pressed && !previousButtonState[13]) {
                        YG2Instance('TVKeyDown', 'Down');
                    } else if (!gamepad.buttons[13].pressed && previousButtonState[13]) {
                        YG2Instance('TVKeyUp', 'Down');
                    }
                    if (gamepad.buttons[14].pressed && !previousButtonState[14]) {
                        YG2Instance('TVKeyDown', 'Left');
                    } else if (!gamepad.buttons[14].pressed && previousButtonState[14]) {
                        YG2Instance('TVKeyUp', 'Left');
                    }
                    if (gamepad.buttons[15].pressed && !previousButtonState[15]) {
                        YG2Instance('TVKeyDown', 'Right');
                    } else if (!gamepad.buttons[15].pressed && previousButtonState[15]) {
                        YG2Instance('TVKeyUp', 'Right');
                    }

                    previousButtonState[12] = gamepad.buttons[12].pressed;
                    previousButtonState[13] = gamepad.buttons[13].pressed;
                    previousButtonState[14] = gamepad.buttons[14].pressed;
                    previousButtonState[15] = gamepad.buttons[15].pressed;
                }
            }
        }

        function GameLoop() {
            UpdateGamepadState();
            requestAnimationFrame(GameLoop);
        }

        GameLoop();
    }
}
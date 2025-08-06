window.addEventListener('beforeunload', (event) => {
    if (initGame)
        ygGameInstance.SendMessage('{{{ObjectName}}}', '{{{MethodName}}}');
});
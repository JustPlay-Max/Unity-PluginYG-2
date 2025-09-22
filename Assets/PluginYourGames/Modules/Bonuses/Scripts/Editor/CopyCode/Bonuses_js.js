function ConsumeBonus(token) {
    if (ysdk == null) {
        Final(NO_DATA);
        return;
    }

    return ysdk.bonuses.consume(token).then((response) => {
        let result;

        if (response.signature) {
            result = JSON.stringify({ signature: response.signature });
        } else {
            result = JSON.stringify({ token: response.token });
        }

        LogStyledMessage('Bonus consumed successfully');
        YG2Instance('OnConsumeBonusSuccess', result);
    }).catch((error) => {
        LogStyledMessage('Error consuming bonus:', error);
        YG2Instance('OnBonusesError');
    });

}

function GetBonusCatalog() {
    if (ysdk == null) {
        Final(NO_DATA);
        return;
    }

    return ysdk.bonuses.getCatalog().then((catalog) => {
        const result = JSON.stringify(catalog);

        LogStyledMessage('Bonus catalog retrieved successfully');
        YG2Instance('OnGetBonusCatalogSuccess', result);
    }).catch((error) => {
        LogStyledMessage('Error getting bonus catalog:', error);
        YG2Instance('OnBonusesError');
    });
}

function GetPlayerBonuses() {
    if (ysdk == null) {
        Final(NO_DATA);
        return;
    }

    ysdk.bonuses.getPlayerBonuses().then((bonuses) => {
        let result;

        if (bonuses.signature) {
            result = JSON.stringify({ signature: bonuses.signature });
        } else {
            result = JSON.stringify(bonuses);
        }

        LogStyledMessage('Player bonuses retrieved successfully');
        YG2Instance('OnGetPlayerBonusesSuccess', result);
    }).catch((error) => {
        LogStyledMessage('Error getting player bonuses:', error);
        YG2Instance('OnBonusesError');
    });
}

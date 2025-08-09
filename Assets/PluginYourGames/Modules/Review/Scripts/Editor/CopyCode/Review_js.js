var reviewData = 'false';

function InitReview() {
    return new Promise((resolve) => {
        if (ysdk == null) {
            resolve('false');
            return;
        }
        try {
            ysdk.feedback.canReview().then(({ value }) => {
                if (value) {
                    reviewData = 'true';
                    resolve('true');
                }
                else {
                    resolve('false');
                }
            });
        }
        catch (e) {
            console.error('CRASH Init Review: ', e.message);
            resolve('false');
        }
    });
}

function Review() {
    try {
        ysdk.feedback.canReview()
            .then(({ value, reason }) => {
                if (value) {
                    ysdk.feedback.requestReview().then(({ feedbackSent }) => {
                        LogStyledMessage('feedbackSent ', feedbackSent);
                        if (feedbackSent) {
                            LogStyledMessage('Review Success')
                            YG2Instance('ReviewSent', 'true');
                        }
                        else {
                            YG2Instance('ReviewSent', 'false');
                            LogStyledMessage('Review Fail', reason)
                        }
                        FocusGame();
                    })
                }
                else {
                    LogStyledMessage('Review can show = false', reason);
                    FocusGame();
                }
            })
    } catch (e) {
        console.error('CRASH Review: ', e.message);
        FocusGame();
    }
}
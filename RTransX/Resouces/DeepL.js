(function () {
    //const translator = window._webTranslator_LMT;
    const features = window._webTranslator_LMT.features;

    // features
    const config = features.get("/translator/core/config")
    const alternativeTranslations = features.get("/translator/common/alternativeTranslations");
    //const triggerSourceTranslation = features.get("/translator/core/triggerSourceTranslation");
    const translateSourceSentences = features.get("/translator/core/translateSourceSentences");
    const retranslationServices = features.get("/translator/core/retranslationServices");
    const sourceLanguages = features.get("/translator/core/sourceLanguages");
    const targetLanguages = features.get("/translator/core/targetLanguages");
    const domElements = features.get("/translator/core/domElements");

    // 제한해제
    config.set("CONFIG__MAX_NUM_CHARACTERS", 100000);
    console.log("TEST");

    // 배너 제거
    const banner = document.querySelector("[data-testid=dl-cookieBanner]");
    if (banner != null) banner.remove();

    let onChanged = null;
    translateSourceSentences.onTargetSentencesHaveChanged.push(() => {
        if (onChanged != null) setTimeout(onChanged, 0);
    });

    window.setSourceLang = function (lang) {
        SetLanguage(sourceLanguages, lang);
    }
    window.setTargetLang = function (lang) {
        SetLanguage(targetLanguages, lang);
    }

    window.translate = function (text) {
        return new Promise((resolve, reject) => {
            onChanged = () => {
                const result = getResult();
                if (result.text != "")
                    resolve(result);
            };

            domElements.sourceEdit.value = text;
            retranslationServices.reset();

            setTimeout(reject, 60000);
        });
    }

    function getResult() {
        const text = alternativeTranslations.targetText();
        const alternatives = alternativeTranslations.activeAlternatives().map((item) => item.text);
        return { text: text, alternatives: alternatives };
    }



    function SetLanguage(languages, lang) {
        if (lang == null || lang.length == 0) return;
        lang = lang.toLowerCase();
        for (const item of languages.getLanguages()) {
            if (item.lang.toLowerCase() == lang) {
                languages.setCurrentLanguage(lang);
                return;
            }
        }
    }

})();


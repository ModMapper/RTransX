(function () {
    const features = _webTranslator_LMT.features;
    const fConfig       = features.get("/translator/core/config");
    const fAT   = features.get("/translator/common/alternativeTranslations");
    const fTSS  = features.get("/translator/core/translateSourceSentences");
    const fDE   = features.get("/translator/core/domElements");
    const fTST = features.get("/translator/core/triggerSourceTranslation");
    const fLM = features.get("/translator/core/languageManagement");

    // 배너 제거
    const banner = document.querySelector("[data-testid=dl-cookieBanner]");
    if (banner != null) banner.remove();

    // 길이 제한 해제
    fConfig.set("CONFIG__MAX_NUM_CHARACTERS", 100000);

    // 번역
    window.translateAsync = function(text) {
        return new Promise((resolve) => {
            // 이벤트 추가
            fTSS.onTranslationsHaveChanged.push(onTranslated);
            // 번역 시작
            fDE.sourceEdit.value = text;
            fTST.startSourceUpdate("");

            function onTranslated(e) {
                // 이벤트 반환 및 resolve
                fTSS.onTranslationsHaveChanged.remove(onTranslated);
                resolve({
                    text: fAT.targetText(),
                    alternatives: fAT.activeAlternatives().map((v) => v.text)
                });
            }
        });
    }

    // 언어 설정
    window.setLanguage = function (lang) {
        if (!CheckLanguage(lang.source) || !CheckLanguage(lang.target)) return;
        fLM.updateActiveLanguages(lang.source, { lang: lang.target });
    }

    function CheckLanguage(lang) {
        return _webTranslator_LMT._config.languageConfig[lang] != null;
    }
})();

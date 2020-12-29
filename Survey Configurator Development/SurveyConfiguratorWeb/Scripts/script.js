function ChangeLanguage(pSelectedLanguageOption) {
    if (pSelectedLanguageOption != null && pSelectedLanguageOption.value != null) {
        document.location.href = `/Survey/SetLanguage?Language=${pSelectedLanguageOption.value}`;
    }
}
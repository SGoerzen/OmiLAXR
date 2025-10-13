/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;

namespace OmiLAXR.Types
{
    /// <summary>
    /// Comprehensive enumeration of language codes following ISO 639-1 and locale standards.
    /// Includes both generic language codes and specific regional variants.
    /// Each enum value is annotated with a DescriptionAttribute containing the human-readable language name.
    /// Used for internationalization and localization of analytics statements and user interfaces.
    /// </summary>
    public enum Languages
    {
        // ReSharper disable InconsistentNaming
        
        // Afrikaans language variants
        [Description("Afrikaans")] af ,
        [Description("Afrikaans (South Africa)")] af_ZA,
        
        // Arabic language variants
        [Description("Arabic")] ar,
        [Description("Arabic (U.A.E.)")] ar_AE,
        [Description("Arabic (Bahrain)")] ar_BH,
        [Description("Arabic (Algeria)")] ar_DZ,
        [Description("Arabic (Egypt)")] ar_EG,
        [Description("Arabic (Iraq)")] ar_IQ,
        [Description("Arabic (Jordan)")] ar_JO,
        [Description("Arabic (Kuwait)")] ar_KW,
        [Description("Arabic (Lebanon)")] ar_LB,
        [Description("Arabic (Libya)")] ar_LY,
        [Description("Arabic (Morocco)")] ar_MA,
        [Description("Arabic (Oman)")] ar_OM,
        [Description("Arabic (Qatar)")] ar_QA,
        [Description("Arabic (Saudi Arabia)")] ar_SA,
        [Description("Arabic (Syria)")] ar_SY,
        [Description("Arabic (Tunisia)")] ar_TN,
        [Description("Arabic (Yemen)")] ar_YE,
        
        // Azeri language variants
        [Description("Azeri (Latin)")] az,
        [Description("Azeri (Latin) (Azerbaijan)")] az_AZ,
        [Description("Azeri (Cyrillic) (Azerbaijan)")] az_Cyrl_AZ,
        
        // Belarusian language variants
        [Description("Belarusian")] be,
        [Description("Belarusian (Belarus)")] be_BY,
        
        // Bulgarian language variants
        [Description("Bulgarian")] bg,
        [Description("Bulgarian (Bulgaria)")] bg_BG,
        
        // Bosnian language variants
        [Description("Bosnian (Bosnia and Herzegovina)")] bs_BA,
        
        // Catalan language variants
        [Description("Catalan")] ca,
        [Description("Catalan (Spain)")] ca_ES,
        
        // Czech language variants
        [Description("Czech")] cs,
        [Description("Czech (Czech Republic)")] cs_CZ,
        
        // Welsh language variants
        [Description("Welsh")] cy,
        [Description("Welsh (United Kingdom)")] cy_GB,
        
        // Danish language variants
        [Description("Danish")] da,
        [Description("Danish (Denmark)")] da_DK,
        
        // German language variants
        [Description("German")] de,
        [Description("German (Austria)")] de_AT,
        [Description("German (Switzerland)")] de_CH,
        [Description("German (Germany)")] de_DE,
        [Description("German (Liechtenstein)")] de_LI,
        [Description("German (Luxembourg)")] de_LU,
        
        // Divehi language variants
        [Description("Divehi")] dv,
        [Description("Divehi (Maldives)")] dv_MV,
        
        // Greek language variants
        [Description("Greek")] el,
        [Description("Greek (Greece)")] el_GR,
        
        // English language variants
        [Description("English")] en,
        [Description("English (Australia)")] en_AU,
        [Description("English (Belize)")] en_BZ,
        [Description("English (Canada)")] en_CA,
        [Description("English (Caribbean)")] en_CB,
        [Description("English (United Kingdom)")] en_GB,
        [Description("English (Ireland)")] en_IE,
        [Description("English (Jamaica)")] en_JM,
        [Description("English (New Zealand)")] en_NZ,
        [Description("English (Republic of the Philippines)")] en_PH,
        [Description("English (Trinidad and Tobago)")] en_TT,
        [Description("English (United States)")] en_US,
        [Description("English (South Africa)")] en_ZA,
        [Description("English (Zimbabwe)")] en_ZW,
        
        // Esperanto
        [Description("Esperanto")] eo,
        
        // Spanish language variants
        [Description("Spanish")] es,
        [Description("Spanish (Argentina)")] es_AR,
        [Description("Spanish (Bolivia)")] es_BO,
        [Description("Spanish (Chile)")] es_CL,
        [Description("Spanish (Colombia)")] es_CO,
        [Description("Spanish (Costa Rica)")] es_CR,
        [Description("Spanish (Dominican Republic)")] es_DO,
        [Description("Spanish (Ecuador)")] es_EC,
        [Description("Spanish (Spain)")] es_ES,
        [Description("Spanish (Guatemala)")] es_GT,
        [Description("Spanish (Honduras)")] es_HN,
        [Description("Spanish (Mexico)")] es_MX,
        [Description("Spanish (Nicaragua)")] es_NI,
        [Description("Spanish (Panama)")] es_PA,
        [Description("Spanish (Peru)")] es_PE,
        [Description("Spanish (Puerto Rico)")] es_PR,
        [Description("Spanish (Paraguay)")] es_PY,
        [Description("Spanish (El Salvador)")] es_SV,
        [Description("Spanish (Uruguay)")] es_UY,
        [Description("Spanish (Venezuela)")] es_VE,
        
        // Estonian language variants
        [Description("Estonian")] et,
        [Description("Estonian (Estonia)")] et_EE,
        
        // Basque language variants
        [Description("Basque")] eu,
        [Description("Basque (Spain)")] eu_ES,
        
        // Farsi language variants
        [Description("Farsi")] fa,
        [Description("Farsi (Iran)")] fa_IR,
        
        // Finnish language variants
        [Description("Finnish")] fi,
        [Description("Finnish (Finland)")] fi_FI,
        
        // Faroese language variants
        [Description("Faroese")] fo,
        [Description("Faroese (Faroe Islands)")] fo_FO,
        
        // French language variants
        [Description("French")] fr,
        [Description("French (Belgium)")] fr_BE,
        [Description("French (Canada)")] fr_CA,
        [Description("French (Switzerland)")] fr_CH,
        [Description("French (France)")] fr_FR,
        [Description("French (Luxembourg)")] fr_LU,
        [Description("French (Principality of Monaco)")] fr_MC,
        
        // Galician language variants
        [Description("Galician")] gl,
        [Description("Galician (Spain)")] gl_ES,
        
        // Gujarati language variants
        [Description("Gujarati")] gu,
        [Description("Gujarati (India)")] gu_IN,
        
        // Hebrew language variants
        [Description("Hebrew")] he,
        [Description("Hebrew (Israel)")] he_IL,
        
        // Hindi language variants
        [Description("Hindi")] hi,
        [Description("Hindi (India)")] hi_IN,
        
        // Croatian language variants
        [Description("Croatian")] hr,
        [Description("Croatian (Bosnia and Herzegovina)")] hr_BA,
        [Description("Croatian (Croatia)")] hr_HR,
        
        // Hungarian language variants
        [Description("Hungarian")] hu,
        [Description("Hungarian (Hungary)")] hu_HU,
        
        // Armenian language variants
        [Description("Armenian")] hy,
        [Description("Armenian (Armenia)")] hy_AM,
        
        // Indonesian language variants
        [Description("Indonesian")] id,
        [Description("Indonesian (Indonesia)")] id_ID,
        
        // Icelandic language variants (using _is due to 'is' being a C# keyword)
        [Description("Icelandic")] _is,
        [Description("Icelandic (Iceland)")] is_IS,
        
        // Italian language variants
        [Description("Italian")] it,
        [Description("Italian (Switzerland)")] it_CH,
        [Description("Italian (Italy)")] it_IT,
        
        // Japanese language variants
        [Description("Japanese")] ja,
        [Description("Japanese (Japan)")] ja_JP,
        
        // Georgian language variants
        [Description("Georgian")] ka,
        [Description("Georgian (Georgia)")] ka_GE,
        
        // Kazakh language variants
        [Description("Kazakh")] kk,
        [Description("Kazakh (Kazakhstan)")] kk_KZ,
        
        // Kannada language variants
        [Description("Kannada")] kn,
        [Description("Kannada (India)")] kn_IN,
        
        // Korean language variants
        [Description("Korean")] ko,
        [Description("Korean (Korea)")] ko_KR,
        
        // Konkani language variants
        [Description("Konkani")] kok,
        [Description("Konkani (India)")] kok_IN,
        
        // Kyrgyz language variants
        [Description("Kyrgyz")] ky,
        [Description("Kyrgyz (Kyrgyzstan)")] ky_KG,
        
        // Lithuanian language variants
        [Description("Lithuanian")] lt,
        [Description("Lithuanian (Lithuania)")] lt_LT,
        
        // Latvian language variants
        [Description("Latvian")] lv,
        [Description("Latvian (Latvia)")] lv_LV,
        
        // Maori language variants
        [Description("Maori")] mi,
        [Description("Maori (New Zealand)")] mi_NZ,
        
        // Macedonian language variants
        [Description("FYRO Macedonian")] mk,
        [Description("FYRO Macedonian (Former Yugoslav Republic of Macedonia)")] mk_MK,
        
        // Mongolian language variants
        [Description("Mongolian")] mn,
        [Description("Mongolian (Mongolia)")] mn_MN,
        
        // Marathi language variants
        [Description("Marathi")] mr,
        [Description("Marathi (India)")] mr_IN,
        
        // Malay language variants
        [Description("Malay")] ms,
        [Description("Malay (Brunei Darussalam)")] ms_BN,
        [Description("Malay (Malaysia)")] ms_MY,
        
        // Maltese language variants
        [Description("Maltese")] mt,
        [Description("Maltese (Malta)")] mt_MT,
        
        // Norwegian language variants
        [Description("Norwegian (Bokm?l)")] nb,
        [Description("Norwegian (Bokm?l) (Norway)")] nb_NO,
        
        // Dutch language variants
        [Description("Dutch")] nl,
        [Description("Dutch (Belgium)")] nl_BE,
        [Description("Dutch (Netherlands)")] nl_NL,
        
        // Norwegian Nynorsk
        [Description("Norwegian (Nynorsk) (Norway)")] nn_NO,
        
        // Northern Sotho language variants
        [Description("Northern Sotho")] ns,
        [Description("Northern Sotho (South Africa)")] ns_ZA,
        
        // Punjabi language variants
        [Description("Punjabi")] pa,
        [Description("Punjabi (India)")] pa_IN,
        
        // Polish language variants
        [Description("Polish")] pl,
        [Description("Polish (Poland)")] pl_PL,
        
        // Pashto language variants
        [Description("Pashto")] ps,
        [Description("Pashto (Afghanistan)")] ps_AR,
        
        // Portuguese language variants
        [Description("Portuguese")] pt,
        [Description("Portuguese (Brazil)")] pt_BR,
        [Description("Portuguese (Portugal)")] pt_PT,
        
        // Quechua language variants
        [Description("Quechua")] qu,
        [Description("Quechua (Bolivia)")] qu_BO,
        [Description("Quechua (Ecuador)")] qu_EC,
        [Description("Quechua (Peru)")] qu_PE,
        
        // Romanian language variants
        [Description("Romanian")] ro,
        [Description("Romanian (Romania)")] ro_RO,
        
        // Russian language variants
        [Description("Russian")] ru,
        [Description("Russian (Russia)")] ru_RU,
        
        // Sanskrit language variants
        [Description("Sanskrit")] sa,
        [Description("Sanskrit (India)")] sa_IN,
        
        // Sami language variants
        [Description("Sami")] se,
        [Description("Sami (Finland)")] se_FI,
        [Description("Sami (Norway)")] se_NO,
        [Description("Sami (Sweden)")] se_SE,
        
        // Slovak language variants
        [Description("Slovak")] sk,
        [Description("Slovak (Slovakia)")] sk_SK,
        
        // Slovenian language variants
        [Description("Slovenian")] sl,
        [Description("Slovenian (Slovenia)")] sl_SI,
        
        // Albanian language variants
        [Description("Albanian")] sq,
        [Description("Albanian (Albania)")] sq_AL,
        
        // Serbian language variants
        [Description("Serbian (Latin) (Bosnia and Herzegovina)")] sr_BA,
        [Description("Serbian (Cyrillic) (Bosnia and Herzegovina)")] sr_Cyrl_BA,
        [Description("Serbian (Latin) (Serbia and Montenegro)")] sr_SP,
        [Description("Serbian (Cyrillic) (Serbia and Montenegro)")] sr_Cyrl_SP,
        
        // Swedish language variants
        [Description("Swedish")] sv,
        [Description("Swedish (Finland)")] sv_FI,
        [Description("Swedish (Sweden)")] sv_SE,
        
        // Swahili language variants
        [Description("Swahili")] sw,
        [Description("Swahili (Kenya)")] sw_KE,
        
        // Syriac language variants
        [Description("Syriac")] syr,
        [Description("Syriac (Syria)")] syr_SY,
        
        // Tamil language variants
        [Description("Tamil")] ta,
        [Description("Tamil (India)")] ta_IN,
        
        // Telugu language variants
        [Description("Telugu")] te,
        [Description("Telugu (India)")] te_IN,
        
        // Thai language variants
        [Description("Thai")] th,
        [Description("Thai (Thailand)")] th_TH,
        
        // Tagalog language variants
        [Description("Tagalog")] tl,
        [Description("Tagalog (Philippines)")] tl_PH,
        
        // Tswana language variants
        [Description("Tswana")] tn,
        [Description("Tswana (South Africa)")] tn_ZA,
        
        // Turkish language variants
        [Description("Turkish")] tr,
        [Description("Turkish (Turkey)")] tr_TR,
        
        // Tatar language variants
        [Description("Tatar")] tt,
        [Description("Tatar (Russia)")] tt_RU,
        
        // Tsonga
        [Description("Tsonga")] ts,
        
        // Ukrainian language variants
        [Description("Ukrainian")] uk,
        [Description("Ukrainian (Ukraine)")] uk_UA,
        
        // Urdu language variants
        [Description("Urdu")] ur,
        [Description("Urdu (Islamic Republic of Pakistan)")] ur_PK,
        
        // Uzbek language variants
        [Description("Uzbek (Latin)")] uz,
        [Description("Uzbek (Latin) (Uzbekistan)")] uz_UZ,
        [Description("Uzbek (Cyrillic) (Uzbekistan)")] uz_Cyrl_UZ,
        
        // Vietnamese language variants
        [Description("Vietnamese")] vi,
        [Description("Vietnamese (Viet Nam)")] vi_VN,
        
        // Xhosa language variants
        [Description("Xhosa")] xh,
        [Description("Xhosa (South Africa)")] xh_ZA,
        
        // Chinese language variants
        [Description("Chinese")] zh,
        [Description("Chinese (Simplified)")] zh_CN,
        [Description("Chinese (Hong Kong)")] zh_HK,
        [Description("Chinese (Macau)")] zh_MO,
        [Description("Chinese (Singapore)")] zh_SG,
        [Description("Chinese (Traditional)")] zh_TW,
        
        // Zulu language variants
        [Description("Zulu")] zu,
        [Description("Zulu (South Africa)")] zu_ZA
        // ReSharper restore InconsistentNaming
    }
}
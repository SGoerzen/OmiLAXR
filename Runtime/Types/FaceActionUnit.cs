namespace OmiLAXR.Types
{
    public enum FaceActionUnit
    {
        // Upper Face
        AU1_InnerBrowRaiser,
        AU2_OuterBrowRaiser,
        AU4_BrowLowerer,
        AU5_UpperLidRaiser,
        AU6_CheekRaiser,
        AU7_LidTightener,

        // Nose / Midface
        AU8_LipsTowardsEachOther, 
        AU9_NoseWrinkler,
        AU10_UpperLipRaiser,
        AU11_NasolabialDeepener,

        // Mouth
        AU12_LipCornerPuller,
        AU13_CheekPuff,               // sometimes mapped to "CheekPuff"
        AU14_Dimpler,
        AU15_LipCornerDepressor,
        AU16_LowerLipDepressor,
        AU17_ChinRaiser,
        AU18_LipPucker,
        AU20_LipStretcher,
        AU22_LipFunneler,
        AU23_LipTightener,
        AU24_LipPressor,
        AU25_LipsPart,
        AU26_JawDrop,
        AU27_MouthStretch,

        // Other Facial Movements
        AU28_LipSuck,
        AU29_JawThrust,
        AU30_JawSideways,
        AU31_JawClench,
        AU32_Bite,
        AU33_CheekBlow,
        AU34_Puff,
        AU35_CheekSuck,
        AU36_TongueBulge,

        // Eye Movements
        AU41_LidDroop,
        AU42_SlackJaw,
        AU43_EyesClosed,
        AU44_Squint,
        AU45_Blink,
        AU46_Wink,

        // Head Movements (if tracked)
        AU51_HeadTurnLeft,
        AU52_HeadTurnRight,
        AU53_HeadUp,
        AU54_HeadDown,
        AU55_HeadTiltLeft,
        AU56_HeadTiltRight,
        AU57_HeadForward,
        AU58_HeadBack,

        // Misc
        AU61_EyesTurnLeft,
        AU62_EyesTurnRight,
        AU63_EyesUp,
        AU64_EyesDown
    }

}
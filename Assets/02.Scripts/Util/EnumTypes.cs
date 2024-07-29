using UnityEngine;

namespace EnumTypes
{
    public enum SceneChangeEvent
    {
        WorldMapOpen,
        IntroMapOpen,
        BattleRoomOpen,
        Stage1_4MapOpen,
        Stage2_5MapOpen,
        Stage3_3MapOpen,
        StageWizardMapOpen,
        StageMineMapOpen,
        TestStageMapOpen,
        ResultOpen
    }

    public enum SoundEvents
    {
        MineBgmPlay,
        StageBgmFadeOut,
    }

    public enum UIEvents
    {
        MobilePlayerController,
    }

    public enum GameEvent
    {
        StartGame,
        ResetGameSetting,
        MovingCart
    }

    public enum SceneType
    {
        Intro,
        BattleLobby,
        WorldMap,
        BattleMap,
        StageMap,
        NetworkBattleMap,
        Result
    }

    public enum MapType
    {
        None,
        Tuto,
        Stage1_4,
        Stage2_5,
        Stage3_3,
        StageWizard,
        StageMine,
    }

    public class EnumTypes : MonoBehaviour { }
}
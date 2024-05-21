using UnityEngine;

namespace EnumTypes
{
    public enum LookDirection { Up, Down, Left, Right }
    public enum PlayerState { Stand, Move, Run, Hold, Cook }
    public enum Layers
    {
        Default,
        TransparentFX,
        IgnoreRaycast,
        Reserved1,
        Water,
        UI,
        Reserved2,
        Reserved3,
        Player,
        World,
    }

    public enum ThrowableType
    {
        Ingredient,
    };

    public enum GlobalEvents
    {
        PlayerDead,
        PlayerSpawned,
        PlayerInactive,
        PlayerDamaged,
    }

    public enum UIEvents
    {
        WorldMapOpen,
        IntroMapOpen,
        TestStageMapOpen,
        BattleRoomOpen,
    }

    public enum SoundEvents
    {
        FadeIn,
        FadeOut
    }

    public enum UIType
    {
        OptionUI,
        StopUI,
        BattleUI,
        ExitLobbyUI,
        LoadingKeyUI,
        LoadingMapUI,
        LoadingFoodUI,
        BusMap,
        StageMapEscUI,
        RecipeUI,
    }


    public enum SceneType
    {
        Intro,
        BattleLobby,
        BusMap,
        BattleMap,
        StageMap,
        
    }

    public class EnumTypes : MonoBehaviour { }
}
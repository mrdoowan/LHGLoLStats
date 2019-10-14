
namespace LoLStatsAPIv4 {
    public enum Align {
        LEFT,
        MIDDLE,
        RIGHT
    }

    public enum APIParam
    {
        SUMMONER_NAME,
        LEAGUES,
        MATCH
    }

    public enum TeamSide {
        BLUE,
        RED
    }

    public enum Role {
        TOP,
        JUNGLE,
        MIDDLE,
        BOTTOM,
        SUPPORT
    }

    public enum ObjectiveEvent {
        TOWER_DESTROYED,
        DRAGON_KILL,
        BARON_KILL,
        RIFT_HERALD,
        INHIBITOR_DESTROYED
    }

    public enum ObjectiveType {
        OUTER_TURRET,
        INNER_TURRET,
        BASE_TURRET,
        NEXUS_TURRET,
        AIR_DRAGON,     // Cloud Drake
        FIRE_DRAGON,    // Infernal Drake
        EARTH_DRAGON,   // Mountain Drake
        WATER_DRAGON,   // Ocean Drake
        RIFT_HERALD,
        BARON_NASHOR,
        INHIBITOR
    }

    public enum LaneMap {
        TOP,
        MID,
        BOT
    }
}

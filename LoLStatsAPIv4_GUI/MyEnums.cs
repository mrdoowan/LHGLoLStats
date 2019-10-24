
namespace LoLStatsAPIv4_GUI {
    public enum QueryType {
        COUNT,
        SELECT,
        INSERT,
        UPDATE,
        DELETE
    }

    public enum Operator {
        AND,
        OR
    }

    public enum Align {
        LEFT,
        MIDDLE,
        RIGHT
    }

    public enum APIParam
    {
        SUMMONER_NAME,
        LEAGUES,
        MATCH,
        TIMELINE
    }

    public enum Role {
        TOP,
        JUNGLE,
        MIDDLE,
        BOTTOM,
        SUPPORT,
        NONE
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
        ELDER_DRAGON,
        RIFT_HERALD,
        BARON_NASHOR,
        INHIBITOR
    }

    public enum LaneMap {
        TOP,
        MID,
        BOT
    }

    public enum TeamStat {
        KILLS,
        DEATHS,
        ASSISTS,
        DAMAGE_CHAMPS,
        DAMAGE_OBJECTIVES,
        GOLD,
        CREEP_SCORE,
        VISION_SCORE,
        GOLD_AT_15,
        GOLD_DIFF_15,
        XP_AT_15,
        XP_DIFF_15,
        GOLD_AT_25,
        GOLD_DIFF_25,
        XP_AT_25,
        XP_DIFF_25
    }
}


namespace LoLStatsAPIv4_GUI {
    public enum QueryType {
        COUNT,
        SELECT,
        UNIQUE,
        INSERT,
        UPDATE,
        DELETE
    }

    public enum Operator {
        AND,
        OR
    }

    public enum DB {
        INSERT,
        WHERE,
        SET,
        COLUMN
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
        NONE,
        TOWER_DESTROYED,
        DRAGON_KILL,
        BARON_KILL,
        HERALD_KILL,
        INHIBITOR_DESTROYED
    }
    public static class ObjectiveEventExtensions {
        public static string GetString(this ObjectiveEvent val) {
            switch (val) {
                case ObjectiveEvent.TOWER_DESTROYED:
                    return "Tower";
                case ObjectiveEvent.DRAGON_KILL:
                    return "Dragon";
                case ObjectiveEvent.BARON_KILL:
                    return "Baron Nashor";
                case ObjectiveEvent.HERALD_KILL:
                    return "Rift Herald";
                case ObjectiveEvent.INHIBITOR_DESTROYED:
                    return "Inhibitor";
                default:
                    return "";
            }
        }
    }

    public enum ObjectiveType {
        NONE,
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
    public static class ObjectiveTypeExtensions {
        public static string GetString(this ObjectiveType val) {
            switch (val) {
                case ObjectiveType.OUTER_TURRET:
                    return "Outer Turret";
                case ObjectiveType.INNER_TURRET:
                    return "Inner Turret";
                case ObjectiveType.BASE_TURRET:
                    return "Base Turret";
                case ObjectiveType.NEXUS_TURRET:
                    return "Nexus Turret";
                case ObjectiveType.AIR_DRAGON:
                    return "Cloud Drake";
                case ObjectiveType.FIRE_DRAGON:
                    return "Infernal Drake";
                case ObjectiveType.EARTH_DRAGON:
                    return "Mountain Drake";
                case ObjectiveType.WATER_DRAGON:
                    return "Ocean Drake";
                case ObjectiveType.ELDER_DRAGON:
                    return "Elder Dragon";
                case ObjectiveType.RIFT_HERALD:
                    return "Rift Herald";
                case ObjectiveType.BARON_NASHOR:
                    return "Baron Nashor";
                case ObjectiveType.INHIBITOR:
                    return "Inhibitor";
                default:
                    return "";
            }
        }
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

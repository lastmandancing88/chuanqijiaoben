using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace chuanqijiaoben
{
    public enum Verb { 战士 = 1, 法师, 道士 }
    public enum Gender { 男 = 1, 女 }
    public enum ShortCutKey { F1, F2, F3, F4, F5, F6, F7, F8 }
    public enum SkillName { 火球术, 大火球, 地狱火, 火墙, 爆裂火焰 }
    public enum ScrollType { 随机传送卷 = 1, 回城卷 }
    public enum Buff { }
    public enum Resource { 元宝 = 1, 积分, 金币 }
    public enum MoveStrategy { 行走 = 1, 随机传送卷, 瞬息移动, 原地 }
    public struct Map
    {
        string mapName;
        bool transportable;
        List<Coordinate> battlePoint;
        public static bool operator ==(Map mapA, Map mapB)
        {
            if (mapA.mapName == mapB.mapName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(Map mapA, Map mapB)
        {
            if (mapA == mapB)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public struct Coordinate
    {
        public int x;
        public int y;
        public static bool operator ==(Coordinate coordinateA, Coordinate coordinateB)
        {
            if (coordinateA.x == coordinateB.x && coordinateA.y == coordinateB.y)
                return true;
            else
                return false;
        }
        public static bool operator !=(Coordinate coordinateA, Coordinate coordinateB)
        {
            if (coordinateA == coordinateB)
                return false;
            else
                return true;
        }
    }
    public struct Role
    {
        public Map map;
        public Coordinate coordinate;
        public Verb verb;
        public Gender gender;
        public Target target;
    }
    public struct Target
    {
        public string name;
        public Coordinate coorninate;
        public string direction;
        public Target(string targetInfo) : this()
        {
            string pattern = "(?<= [).*? (?= ])";
            string result = Regex.Match(targetInfo, pattern).Value;
        }
    }
    public class Game : Singleton<Game>
    {
        #region 游戏常量信息
        public static string kLifebarColor = "f81010-000000";
        public static string kMinimapMonsterColor = "ff0000-000000";
        public static string kCoordinateRGBColor = "ffffff-040404";
        public static string kDialogueColor = "ffff00-050504";
        public static string kMonsterNameColor = "008000-050505";
        public static string kStatusColor = "ffff00-040404";
        public static string kGoodLootColor = "ff009b-040404";
        public static string kTargetStatusColor = "00ff00-060505";
        public static string kCharMinimapColor = "00ff00-050505";
        public static string kCharPropertyColor = "c7e0f9-111111";
        public static int kCharActualHorizontalPosition = 400;
        public static int kCharActualVerticalPosition = 230;
        public static int kPositionHorizontalBaseLength = 48;
        public static int kPositionVerticalBaseLength = 32;
        public static int kMinimapSideLength = 128;	//小地图各边的距离
        public static int[] char_actual_position = { 400, 210 };
        public static int[] minimap_area = { 672, 0, 799, 127 };
        public static int[] minimap_center_position = { 737, 64 };
        public static string[] direction_list = { "←", "→", "↙", "↘", "↑", "↓", "↖", "↗" };
        public static int[] screen_area = { 0, 0, 800, 456 };
        public static int[] dialogue_area = { 25, 18, 362, 479 };
        public static int[] status_area = { 50, 0, 650, 15 };
        public static int[] target_status_area = { 50, 13, 350, 29 };
        public static int[] buff_status_area = { 571, 12, 672, 37 };
        public static int[] map_status_area = { 783, 0, 800, 49 };
        public static int[] map_name_area = { 696, 0, 782, 13 };
        public static int[] coordinate_area = { 697, 114, 774, 126 };
        public static int[] life_status_area = { 170, 565, 229, 577 };
        #endregion
        public Role role;
        public Game()
        {

        }
    }
}

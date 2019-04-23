using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chuanqijiaoben
{
    public enum Class { 战士 = 1, 法师, 道士 }
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
    public struct Skill { SkillName sillName; ShortCutKey shortCutKey; }
    public struct Scroll { ScrollType scroll; int count; }
}

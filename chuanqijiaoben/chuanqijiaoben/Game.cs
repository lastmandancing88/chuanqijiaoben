using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace chuanqijiaoben
{
    public enum Verb { 战士 = 1, 法师, 道士 }
    public enum Buff { }
    public enum Resource { 元宝 = 1, 积分, 金币 }
    public struct Map
    {
        string mapName;
        bool transportable;
        List<Coordinate> battlePoint;
        double horizontalFactor;
        int horizontalBase;
        int mapWidth;
        int mapHeight;
        string property;
        public Map(string mapName) : this()
        {
            this.mapName = mapName;
            var mapSetting = ConfigurationManager.GetSection("Maps/"+mapName) as NameValueCollection;
            if (mapSetting.Count == 0)
            {
                MessageBox.Show("无法读取Map配置");
            }
            else
            {
                transportable = mapSetting["进入方式"] == "传送" ? true : false;
                mapWidth = Convert.ToInt32(mapSetting["地图宽"]);
                mapHeight = Convert.ToInt32(mapSetting["地图高"]);
                horizontalBase = Convert.ToInt32(mapSetting["左边界值"]);
                horizontalFactor = (double)(800 - horizontalBase) / (mapWidth + 1);
                property = mapSetting["地图属性"];
            }
        }
        public double HorizontalFactor { get => horizontalFactor; }
        public int HorizontalBase { get => horizontalBase; }
        public int MapWidth { get => mapWidth; }
        public int MapHeight { get => mapHeight; }
        public string MapName { get => mapName; }
        public string Property { get => property; }

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
    public class Position
    {
        public int x;
        public int y;
        public Position()
        {
            x = 0;
            y = 0;
        }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
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
        public Coordinate(string x, string y) : this()
        {
            this.x = Convert.ToInt32(x);
            this.y = Convert.ToInt32(y);
        }
        public Coordinate(string[] temp) : this(temp[0], temp[1])
        {

        }
        public Coordinate(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
        }
    }
    public struct NPC
    {
        public string name;
        public Map inMap;
        public Coordinate coordinate;
        public string[] dialogues;
        public NPC(string name) : this()
        {
            this.name = name;
            var npcSetting = ConfigurationManager.GetSection("NPCs/" + name) as NameValueCollection;
            if (npcSetting.Count == 0)
            {
                MessageBox.Show("无法读取NPC配置");
            }
            else
            {
                inMap = new Map(npcSetting["所在地图"] );
                coordinate.x = Convert.ToInt32(npcSetting["x坐标"]);
                coordinate.y = Convert.ToInt32(npcSetting["y坐标"]);
                dialogues = npcSetting["对话历程"].Split(',');
            }
        }
    }
    public struct Misc
    {
        public string name;
        public int weight;
        public int batch;
        public NPC seller;
        public Misc(string name) : this()
        {
            this.name = name;
            var miscSetting = ConfigurationManager.GetSection("Miscs/" + name) as NameValueCollection;
            if (miscSetting.Count == 0)
            {
                MessageBox.Show("无法读取Misc配置");
            }
            else
            {
                weight = Convert.ToInt32(miscSetting["重量"]);
                batch = Convert.ToInt32(miscSetting["批量"]);
                seller = new NPC(miscSetting["小贩"]);
            }
        }
    }
    public class Game
    {
        #region 游戏常量信息
        public static string kLifebarColor = "f81010-000000";
        public static string kMinimapMonsterColor = "ff0000-000000";
        public static string kCoordinateRGBColor = "ffffc8-040404";
        public static string kDialogueColor = "ffff00-000000";
        public static string kMonsterNameColor = "008000-050505";
        public static string kStatusColor = "ffff01-040404";
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
        public static int[] dialogue_area = { 25, 18, 362, 400 };
        public static int[] status_area = { 50, 0, 650, 15 };
        public static int[] target_status_area = { 50, 13, 350, 29 };
        public static int[] buff_status_area = { 571, 12, 672, 37 };
        public static int[] map_status_area = { 783, 0, 800, 49 };
        public static int[] map_name_area = { 696, 0, 782, 13 };
        public static int[] coordinate_area = { 5, 574, 146, 591 };
        public static int[] life_status_area = { 170, 565, 229, 577 };
        #endregion
        private DM dm;
        private int hwnd;
        public Role role;
        public Game()
        {
            dm = DM.GetInstance();
            role = new Role();
            dm.EnableDisplayDebug(1);
        }
        public void Initial()
        {
            hwnd = dm.FindWindow("King GEngine", null);
            if (hwnd != 0)
            {
                if (dm.BindWindow(hwnd, "normal", "normal", "normal", 0) == 0)
                {
                    MessageBox.Show("绑定窗口错误, 错误代码为: "+dm.GetLastError());
                }
                else
                {
                    if (dm.SetWindowState(hwnd, 8) == 0)
                    {
                        MessageBox.Show("游戏置顶失败, 错误代码为: " + dm.GetLastError());
                    }
                    else
                    {
                        if (dm.MoveWindow(hwnd, -8, -31) == 0)
                        {
                            MessageBox.Show("游戏重置位置失败, 错误代码为: " + dm.GetLastError());
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("无法找到游戏窗口, 错误代码为: " + dm.GetLastError());
            }
        }
    }
}

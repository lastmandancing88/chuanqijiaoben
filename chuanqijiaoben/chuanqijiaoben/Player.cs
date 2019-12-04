using System;
using System.Text.RegularExpressions;

namespace chuanqijiaoben
{
    public struct Target
    {
        public string name;
        private Coordinate coorninate;
        public string direction;
        private DM dm;
        public Coordinate Coorninate { get => GetCoordinate(); }
        private Coordinate GetCoordinate()
        {
            string[] temp;
            string info = dm.Ocr(Game.coordinate_area[0], Game.coordinate_area[0], Game.coordinate_area[0], Game.coordinate_area[0], Game.kCoordinateRGBColor, 1.0);
            string result = Regex.Match(info, @"\[.*(\b\d{1,},\d{1,})\]").Value;
            temp = result[0].ToString().Split(',');
            return new Coordinate(temp[0], temp[1]);
        }
    }
    public struct Monster
    {
        private Coordinate position;
        public Coordinate Position { get => position; set => position = value; }
        public Monster(Coordinate position) : this()
        {
            this.position = position;
        }
    }
    public class Player
    {
        private DM dm;
        private Game game;
        private Target target;
        private Map inMap;
        public Coordinate Coordinate { get => GetCoordinate(); }
        public Coordinate MiniMapPosition { get => GetMiniMapPosition(); }
        public Player()
        {
            dm = DM.GetInstance();
        }
        private Coordinate GetMiniMapPosition()
        {
            Coordinate coordinate = new Coordinate();
            int[] temp = new int[] { Coordinate.x, Coordinate.y };
            if (temp[0] < 42)
            {
                coordinate.x = Convert.ToInt32(Math.Round((decimal)(Game.minimap_area[0] + temp[0] * inMap.HorizontalFactor)));
            }
            else if (inMap.MapWidth - temp[0] < 42)
            {
                coordinate.x = Convert.ToInt32(Math.Round((decimal)(Game.minimap_area[2] - (inMap.MapWidth - temp[0]) * inMap.HorizontalFactor))) - (temp[0] % 2 == 0?1:0);
            }
            else
            {
                coordinate.x = temp[0] % 2 == 0 ? Game.minimap_center_position[0] - 1 : Game.minimap_center_position[0];
            }
            coordinate.y = temp[1] < 64 ? Game.minimap_area[1] + temp[1] : (inMap.MapHeight - temp[1] < 64? Game.minimap_area[3] - (inMap.MapHeight - temp[1]): Game.minimap_center_position[1]);
            return coordinate;
        }
        private Coordinate GetCoordinate()
        {
            string[] temp;
            temp = dm.Ocr(Game.coordinate_area[0], Game.coordinate_area[0], Game.coordinate_area[0], Game.coordinate_area[0], Game.kCoordinateRGBColor, 1.0).Split(':');
            return new Coordinate(temp[0], temp[1]);
        }
        public bool Attack(Coordinate coordinate)
        {
            int[] temp = GetDistance(target.Coorninate);
            if (temp[0] >= 2 | temp[1] >= 2)
            {
                dm.MoveTo(coordinate.x, coordinate.y);
                dm.LeftClick(coordinate.x, coordinate.y);
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool MovetoCoordinate(Coordinate coordinate)
        {
            int[] distance = GetDistance(coordinate);
            if (Math.Abs(distance[0]) <= 2 & Math.Abs(distance[1]) <= 2)
            {
                return true;
            }
            else
            {
                if (Math.Abs(distance[0]) <= 10 & Math.Abs(distance[1]) <= 10)
                {
                    NearMove(distance);
                }
                else
                {
                    //FarMove(coordinate);
                }
                return false;
            }
        }
        private void NearMove(int[] distance)
        {
            int x, y;
            x = Game.kCharActualHorizontalPosition + Math.Sign(distance[0]) * Game.kPositionHorizontalBaseLength * 2;
            y = Game.kPositionVerticalBaseLength + Math.Sign(distance[1]) * Game.kPositionVerticalBaseLength * 2;
            dm.MoveTo(x, y);
            dm.RightClick();
        }
        private int[] GetDistance(Coordinate coordinate)
        {
            return new int[] { Coordinate.x - coordinate.x, Coordinate.y - coordinate.y };
        }
        public bool HasMonsterBesides()
        {
            Coordinate position = MiniMapPosition;
            string[] temp = dm.FindMultiColorE(position.x - 7 - 1, position.y - 7, position.x + 7, position.y + 7, "ff0000-000000", "-1|1|ff0000-000000,0|1|ff0000-000000", 1.0, 3).Split('|');
            if (Convert.ToInt32(temp[0]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Monster SearchMonster()
        {
            Monster monster=new Monster();
            int x, y;
            double factor;
            Coordinate tempPostion = new Coordinate();
            Coordinate position = MiniMapPosition;
            string[] temp;
            for (int i = 0; i<7; i++)
            {
                temp = dm.FindMultiColorE(position.x - i - 1, position.y - i, position.x + i, position.y + i, "ff0000-000000", "-1|1|ff0000-000000,0|1|ff0000-000000", 1.0, 3).Split('|');
                if (Convert.ToInt32(temp[0]) > 0)
                {
                    if (Convert.ToInt32(temp[0]) % 2 == 0)
                    {
                        x = Convert.ToInt32(temp[0]);
                        if (x > position.x)
                        {
                            factor = Math.Floor((x - position.x) / inMap.HorizontalFactor + 0.5);
                        }
                        else if (x < position.x)
                        {
                            factor = Math.Truncate((double)(x - position.x) / inMap.HorizontalFactor);
                        }
                        else
                        {
                            factor = 0;
                        }
                    }
                    else
                    {
                        x = Convert.ToInt32(temp[0]) + 1;
                        if (x > position.x)
                        {
                            factor = Math.Floor((x - position.x) / inMap.HorizontalFactor + 0.5);
                        }
                        else if (x < position.x)
                        {
                            factor = Math.Floor((double)(x - position.x) / inMap.HorizontalFactor);
                        }
                        else
                        {
                            factor = 0;
                        }
                    }
                    y = Convert.ToInt32(temp[1]) + 1;
                    tempPostion.x = Game.char_actual_position[0] + x * Game.kPositionHorizontalBaseLength;
                    tempPostion.y= Game.char_actual_position[1] + (y - MiniMapPosition.y) * Game.kPositionVerticalBaseLength;
                    monster.Position = new Coordinate(tempPostion.x, tempPostion.y);
                }
            }
            return monster;
        }
        private bool IsCharMoving()
        {
            string[] pos = dm.FindColorE(Game.minimap_area[0], Game.minimap_area[1], Game.minimap_area[2], Game.minimap_area[3], "00ffff-000000", 1.0, 4).Split('|');
            return Convert.ToInt32(pos[0]) > 0 ? true : false;
        }
        private bool DoesGoodLootExist(ref Coordinate goodLootRoughPosition)
        {
            string[] temp = dm.FindColorE(Game.screen_area[0], Game.screen_area[1], Game.screen_area[2], Game.screen_area[3], Game.kGoodLootColor, 1.0, 4).Split('|');
            if (Convert.ToInt32(temp[0]) > 0)
            {
                goodLootRoughPosition = new Coordinate(temp[1], temp[2]);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsCharDead()
        {
            
        }
    }
}

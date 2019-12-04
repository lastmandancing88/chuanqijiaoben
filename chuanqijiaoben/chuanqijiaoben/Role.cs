using System;
using System.IO;
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
            dm = DM.GetInstance();
            string[] temp;
            string info = dm.Ocr(Game.target_status_area[0], Game.target_status_area[1], Game.target_status_area[2], Game.target_status_area[3], Game.kTargetStatusColor, 1.0);
            MatchCollection mc = Regex.Matches(info, @"\d+");
            if(mc.Count==4)
            {
                int i = 0;
                temp = new string[4];
                foreach (Match m in mc)
                {
                    temp[i] = m.Groups[0].Value;
                    i += 1;
                }
                return new Coordinate(temp[0], temp[1]);
            }
            else
            {
                return new Coordinate(0, 0);
            }
        }
    }
    public struct Monster
    {
        private Position position;
        public Position Position { get => position; set => position = value; }
        public Monster(Position position) : this()
        {
            this.position = position;
        }
    }
    public class Role
    {
        private DM dm;
        private Target target;
        public Map InMap { get => GetInMap(); }
        public Coordinate Coordinate { get => GetCoordinate(); }
        public Position MiniMapPosition { get => GetMiniMapPosition(); }
        public bool IsMoving { get => CheckIsMoving(); }
        public int RemainWeight { get => GetRemainLoad(); }
        public Role()
        {
            dm = DM.GetInstance();
        }
        private Position GetMiniMapPosition()
        {
            Coordinate coordinate = this.Coordinate;
            Position position = new Position(0, 0);
            if (coordinate.x< 42)
            {
                position.x = Convert.ToInt32(Math.Round((decimal)(Game.minimap_area[0] + coordinate.x * InMap.HorizontalFactor)));
            }
            else if (InMap.MapWidth - coordinate.x < 42)
            {
                position.x = Convert.ToInt32(Math.Round((decimal)(Game.minimap_area[2] - (InMap.MapWidth - coordinate.x) * InMap.HorizontalFactor))) - (coordinate.x % 2 == 0?1:0);
            }
            else
            {
                position.x = coordinate.x % 2 == 0 ? Game.minimap_center_position[0] - 1 : Game.minimap_center_position[0];
            }
            position.y = coordinate.y< 64 ? Game.minimap_area[1] + coordinate.y : (InMap.MapHeight - coordinate.y < 64? Game.minimap_area[3] - (InMap.MapHeight - coordinate.y) : Game.minimap_center_position[1]);
            return position;
        }
        private Coordinate GetCoordinate()
        {
            string[] temp;
            temp = dm.Ocr(Game.coordinate_area[0], Game.coordinate_area[1], Game.coordinate_area[2], Game.coordinate_area[3], Game.kCoordinateRGBColor, 1.0).Split(':');
            return new Coordinate(Regex.Match(temp[1], @"(?<=\[)\S+(?=\])").Value.Split(','));
        }
        private int GetRemainLoad()
        {
            string temp;
            temp = dm.Ocr(Game.status_area[0], Game.status_area[1], Game.status_area[2], Game.status_area[3], Game.kStatusColor, 1);
           return Convert.ToInt32(Regex.Match(temp, @"(?<=\()\S+(?=\))").Value.Replace("剩", ""));
        }
        private bool CheckIsMoving()
        {
            string temp = dm.FindColorE(Game.minimap_area[0], Game.minimap_area[1], Game.minimap_area[2], Game.minimap_area[3], "00ffff-000000", 1.0, 4);
            return Convert.ToInt32(temp.Split('|')[0]) > 0 ? true : false;
        }
        private Map GetInMap()
        {
            string[] temp;
            temp = dm.Ocr(Game.coordinate_area[0], Game.coordinate_area[1], Game.coordinate_area[2], Game.coordinate_area[3], Game.kCoordinateRGBColor, 1.0).Split(':');
            return new Map(temp[0]);
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
                    FarMove(coordinate);
                }
                return false;
            }
        }
        public void StopMove()
        {
            while (IsMoving)
            {
                dm.MoveTo(400, 250);
                dm.Delay(200);
                dm.LeftClick();
            }
        }
        private void NearMove(int[] distance)
        {
            int x, y;
            x = Game.kCharActualHorizontalPosition - Math.Sign(distance[0]) * Game.kPositionHorizontalBaseLength * 2;
            y = Game.kCharActualVerticalPosition - Math.Sign(distance[1]) * Game.kPositionVerticalBaseLength * 2;
            dm.MoveTo(x, y);
            dm.Delay(500);
            if (distance[0] <= 2 & distance[1] <= 2)
            {
                dm.LeftClick();
            }
            else
            {
                dm.RightClick();
            }
            dm.Delay(500);
        }
        private int[] GetDistance(Coordinate coordinate)
        {
            return new int[] { Coordinate.x - coordinate.x, Coordinate.y - coordinate.y };
        }
        public bool HasMonsterBesides()
        {
            Position position = MiniMapPosition;
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
        public Monster SearchMonster(int range)
        {
            Monster monster=new Monster();
            int x, y;
            int factor;
            Position tempPostion = new Position();
            Position position = MiniMapPosition;
            string[] temp;
            for (int i = 1; i<= range; i++)
            {
                temp = dm.FindMultiColorE(position.x - i - 1, position.y - i, position.x + i, position.y + i, "ff0000-000000", "-1|1|ff0000-000000,0|1|ff0000-000000", 1.0, 3).Split('|');
                if (Convert.ToInt32(temp[0]) > 0)
                {
                    if (Convert.ToInt32(temp[0]) % 2 == 0)
                    {
                        x = Convert.ToInt32(temp[0]);
                        if (x > position.x)
                        {
                            factor = Convert.ToInt32(Math.Floor((x - position.x) / InMap.HorizontalFactor + 0.5));
                        }
                        else if (x < position.x)
                        {
                            factor = Convert.ToInt32(Math.Truncate((double)(x - position.x) / InMap.HorizontalFactor));
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
                            factor = Convert.ToInt32(Math.Floor((x - position.x) / InMap.HorizontalFactor + 0.5));
                        }
                        else if (x < position.x)
                        {
                            factor = Convert.ToInt32(Math.Floor((double)(x - position.x) / InMap.HorizontalFactor));
                        }
                        else
                        {
                            factor = 0;
                        }
                    }
                    y = Convert.ToInt32(temp[1]) + 1;
                    tempPostion.x = Game.char_actual_position[0] + factor * Game.kPositionHorizontalBaseLength;
                    tempPostion.y= Game.char_actual_position[1] + (y - MiniMapPosition.y) * Game.kPositionVerticalBaseLength;
                    monster.Position = new Position(tempPostion.x, tempPostion.y);
                }
            }
            return monster;
        }
        private Position SearchGoodLoots()
        {
            string[] temp = dm.FindColorE(Game.screen_area[0], Game.screen_area[1], Game.screen_area[2], Game.screen_area[3], Game.kGoodLootColor, 1.0, 4).Split('|');
            if (Convert.ToInt32(temp[0]) > 0)
            {
                return new Position(Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]));
            }
            else
            {
                return null;
            }
        }
        public bool IsCharDead()
        {
            return false;
        }
        private bool MinMapOpened()
        {
            object x = 0;
            object y = 0;
            return dm.FindColor(310, 91, 340, 121, "384130-030303", 1.0, 0, out x, out y) == 1 ? true : false;
        }
        private bool MaxMapOpened()
        {
            object x = 0;
            object y = 0;
            return dm.FindColor(767, 0, 769, 8, "202920-030303", 1.0, 0, out x, out y) == 1 ? true : false;
        }
        public void FarMove(Coordinate coordinate)
        {
            if (!IsMoving)
            {
                while (!MaxMapOpened())
                {
                    dm.KeyPressChar("U");
                    dm.Delay(200);
                }
                dm.Delay(200);
                dm.MoveTo(Convert.ToInt32(coordinate.x * InMap.HorizontalFactor) + InMap.HorizontalBase, coordinate.y);
                dm.Delay(200);
                dm.LeftDoubleClick();
                dm.Delay(200);
                while (MaxMapOpened())
                {
                    dm.KeyPressChar("U");
                    dm.Delay(200);
                }
            }
        }
        public bool InteractWithNPC(NPC npc)
        {
            Position postion = PositionofCoordinate(Coordinate, npc.coordinate);
            dm.MoveTo(postion.x, postion.y);
            dm.Delay(200);
            dm.LeftClick();
            dm.Delay(200);
            string s = dm.Ocr(Game.dialogue_area[0], Game.dialogue_area[1], Game.dialogue_area[2], Game.dialogue_area[3], Game.kDialogueColor, 1.0);
            if (s.Contains("对话") | s.Contains("关闭") | s.Contains("取消"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PressDialogue(string dialogue)
        {
            object x, y;
            string temp = dm.FindStrFastE(Game.dialogue_area[0], Game.dialogue_area[1], Game.dialogue_area[2], Game.dialogue_area[3], dialogue, Game.kDialogueColor, 1.0);
            string[] tempArray = temp.Split('|');
            if (tempArray[0] == "0")
            {
                dm.MoveTo(Convert.ToInt32(tempArray[1]) + 8, Convert.ToInt32(tempArray[2]) + 4);
                dm.Delay(500);
                dm.LeftClick();
                dm.Delay(500);
                return dm.FindStr(Game.dialogue_area[0], Game.dialogue_area[1], Game.dialogue_area[2], Game.dialogue_area[3], dialogue, Game.kDialogueColor, 1.0, out x, out y) == -1 ? true : false;
            }
            return false;
        }
        public bool BacktoTown()
        {
            //if (InMap.Property == "城镇")
            //{
            //    return true;
            //}
            //else
            //{
                return UseMisc("回城卷");
            //}
        }
        private bool UseMisc(string misc)
        {
            object x, y;
            //string path = new DirectoryInfo("../../").FullName;
            //string filePath = path +@"Resources\" + misc + ".bmp";
            string filePath = misc + ".bmp";
            dm.KeyPressChar("W");
            dm.Delay(500);
            dm.FindPic(0, 0, 800, 356, filePath, "000000", 0.4, 1, out x, out y);
            if ((int)x > 0 | (int)y > 0)
            {
                return true;
            }
            else
            {
                dm.CapturePre("screen.bmp");
                return false;
            }
        }
        public static Position PositionofCoordinate(Coordinate roleCoordinate, Coordinate aimCoordinate)
        {
            int x, y;
            x = (aimCoordinate.x - roleCoordinate.x) * Game.kPositionHorizontalBaseLength + Game.kCharActualHorizontalPosition;
            y = (aimCoordinate.y - roleCoordinate.y) * Game.kPositionVerticalBaseLength + Game.kCharActualVerticalPosition;
            return new Position(x, y);
        }
        public void Mend(string npcName, string[] mendList)
        {
            NPC npc = new NPC(npcName);
            while (!MovetoCoordinate(npc.coordinate))
            {
                dm.Delay(200);
            }
            StopMove();
            dm.Delay(200);
            foreach (var item in mendList)
            {
                InteractWithNPC(npc);
                dm.Delay(200);
                foreach (var dialogue in npc.dialogues)
                {
                    if (!PressDialogue(dialogue))
                    {
                        return;
                    }
                }
                dm.Delay(200);
                if (PressDialogue(item))
                {
                    dm.Delay(200);
                    if (PressDialogue("现在修理"))
                    {
                        dm.Delay(200);
                        dm.MoveTo(360, 175);
                        dm.Delay(200);
                        dm.LeftClick();
                        dm.Delay(200);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        public bool PhysicalAttack(Position position)
        {
            if (!TargetBeside())
            {
                dm.MoveTo(position.x, position.y);
                dm.Delay(200);
                dm.LeftClick();
                dm.Delay(200);
            }
            return true;
        }
        private bool TargetBeside()
        {
            int[] temp = GetDistance(target.Coorninate);
            return temp[0] <= 1 & temp[1] <= 1 ? true : false;
        }
    }
}

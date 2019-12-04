using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace chuanqijiaoben
{
    public class ScriptSettingElements : System.Configuration.ConfigurationElement
    {
        [System.Configuration.ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                return this["key"] as string;
            }
        }
        [System.Configuration.ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return this["value"] as string;
            }
            set
            {
                this["value"] = value;
            }
        }
    }
    public class ScriptSettingCollection : System.Configuration.ConfigurationElementCollection
    {
        public ScriptSettingElements this[string key]
        {
            get
            {
                return base.BaseGet(key) as ScriptSettingElements;
            }
            set
            {
                if (base.BaseGet(key) != null)
                {
                    base.BaseRemove(key);
                }
                this.BaseAdd(value);
            }
        }
        public ScriptSettingElements this[int index]
        {
            get
            {
                return base.BaseGet(index) as ScriptSettingElements;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(value);
            }
        }
        protected  override System.Configuration.ConfigurationElement CreateNewElement()
        {
            return new ScriptSettingElements();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ScriptSettingElements)element).Key;
        }
    }
    public class ScriptSettingSection : System.Configuration.ConfigurationSection
    {
        [ConfigurationProperty("defaultmethod", DefaultValue = "", IsRequired = false)]
        public string DefaultMethod
        {
            get
            {
                return (string)this["defaultmethod"];
            }
            set
            {
                this["defaultmethod"] = value;
            }
        }
        [System.Configuration.ConfigurationProperty("ScriptSettingElements")]
        public ScriptSettingCollection ScriptSettings
        {
            get
            {
                return (ScriptSettingCollection)this["ScriptSettingElements"];
            }
        }
    }
    class Script
    {
        private bool autoUseStatusPotion;
        private bool autoRevive;
        private bool autoSupply;
        private bool autoAdoreFlower;
        private bool autoPickUp;
        private bool autoUnblock;
        private bool autoMend;
        private int SearchMonsterRange;
        private int suplyInterval;
        private string scriptPath;
        private IDictionary<string, int> supplyList;
        private IDictionary<string, string> npcList;
        private IDictionary<string, string> shortKeyList;
        private IDictionary<string, bool> autoList;
        private IDictionary<string, bool> mendList;
        private string mode;
        private string moveMethod;
        private string supplyMethod;
        private string mendMethod;

        public bool AutoUseStatusPotion { get => autoUseStatusPotion; set => autoUseStatusPotion = value; }
        public bool AutoRevive { get => autoRevive; set => autoRevive = value; }
        public bool AutoSupply { get => autoSupply; set => autoSupply = value; }
        public bool AutoAdoreFlower { get => autoAdoreFlower; set => autoAdoreFlower = value; }
        public bool AutoPickUp { get => autoPickUp; set => autoPickUp = value; }
        public bool AutoUnblock { get => autoUnblock; set => autoUnblock = value; }
        public bool AutoMend { get => autoMend; set => autoMend = value; }
        public int SearchMonsterRange1 { get => SearchMonsterRange; set => SearchMonsterRange = value; }
        public int SuplyInterval { get => suplyInterval; set => suplyInterval = value; }
        public string ScriptPath { get => scriptPath; set => scriptPath = value; }
        public string Mode { get => mode; set => mode = value; }
        public string MoveMethod { get => moveMethod; set => moveMethod = value; }
        public string SupplyMethod { get => supplyMethod; set => supplyMethod = value; }
        public string MendMethod { get => mendMethod; set => mendMethod = value; }
        public IDictionary<string, int> SupplyList { get => supplyList; set => supplyList = value; }
        public IDictionary<string, string> NpcList { get => npcList; set => npcList = value; }
        public IDictionary<string, string> ShortkeyList { get => shortKeyList; set => shortKeyList = value; }
        public IDictionary<string, bool> AutoList { get => autoList; set => autoList = value; }
        public IDictionary<string, bool> MendList { get => mendList; set => mendList = value; }

        public Script()
        {
            LoadBasicSetting();
            LoadMendSetting();
            LoadSupplySetting();
            LoadNPCSetting();
            LoadShortKeySetting();
        }
        private void LoadBasicSetting()
        {
            ScriptSettingSection basicSetting = ConfigurationManager.GetSection("脚本设定/基本") as ScriptSettingSection;
            if (!(basicSetting.ScriptSettings.Count == 0))
            {
                autoList = new Dictionary<string, bool>();
                for (int i = 0; i < basicSetting.ScriptSettings.Count; i++)
                {
                    autoList.Add(basicSetting.ScriptSettings[i].Key, basicSetting.ScriptSettings[i].Value == "true" ? true : false);
                }
            }
        }
        private void LoadMendSetting()
        {
            ScriptSettingSection mendSetting = ConfigurationManager.GetSection("脚本设定/修理") as ScriptSettingSection;
            if (!(mendSetting.ScriptSettings.Count == 0))
            {
                mendList = new Dictionary<string, bool>();
                mendMethod = mendSetting.DefaultMethod;
                for (int i = 0; i < mendSetting.ScriptSettings.Count; i++)
                {
                    mendList.Add(mendSetting.ScriptSettings[i].Key, mendSetting.ScriptSettings[i].Value == "true" ? true : false);
                }
            }
        }
        private void LoadSupplySetting()
        {
            ScriptSettingSection supplySetting = ConfigurationManager.GetSection("脚本设定/补给") as ScriptSettingSection;
            if (!(supplySetting.ScriptSettings.Count == 0))
            {
                supplyList = new Dictionary<string, int>();
                supplyMethod = supplySetting.DefaultMethod;
                for(int i = 0; i < supplySetting.ScriptSettings.Count; i++)
                {
                    supplyList.Add(supplySetting.ScriptSettings[i].Key, Convert.ToInt32(supplySetting.ScriptSettings[i].Value));
                }
            }
        }
        private void LoadNPCSetting()
        {
            ScriptSettingSection npcSetting = ConfigurationManager.GetSection("脚本设定/NPC") as ScriptSettingSection;
            if (!(npcSetting.ScriptSettings.Count == 0))
            {
                npcList = new Dictionary<string, string>();
                for (int i = 0; i < npcSetting.ScriptSettings.Count; i++)
                {
                    npcList.Add(npcSetting.ScriptSettings[i].Key, npcSetting.ScriptSettings[i].Value);
                }
            }
        }
        private void LoadShortKeySetting()
        {
            ScriptSettingSection shortKeySetting = ConfigurationManager.GetSection("脚本设定/快捷键") as ScriptSettingSection;
            if (!(shortKeySetting.ScriptSettings.Count == 0))
            {
                shortKeyList = new Dictionary<string, string>();
                for (int i = 0; i < shortKeySetting.ScriptSettings.Count; i++)
                {
                    shortKeyList.Add(shortKeySetting.ScriptSettings[i].Key, shortKeySetting.ScriptSettings[i].Value);
                }
            }
        }
        public void SaveSetting(Script script)
        {
            SaveBasicSetting(script.AutoList);
            SaveMendSetting(script.MendMethod, script.MendList);
            SaveSupplySetting(script.SupplyList);
            SaveNPCSetting(script.NpcList);
            SaveShortKeySetting(script.ShortkeyList);
        }
        private void SaveBasicSetting(IDictionary<string, bool> autoList)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ScriptSettingSection section = (ScriptSettingSection)config.GetSection("脚本设定/基本");
            for (int i = 0; i < autoList.Count; i++)
            {
                section.ScriptSettings[i].Value = autoList[section.ScriptSettings[i].Key] == true ? "true" : "false";
            }
            config.Save();
        }
        private void SaveMendSetting(string mendMethod, IDictionary<string, bool> mendList)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ScriptSettingSection section = (ScriptSettingSection)config.GetSection("脚本设定/修理");
            section.DefaultMethod = mendMethod;
            for (int i = 0; i < mendList.Count; i++)
            {
                section.ScriptSettings[i].Value = mendList[section.ScriptSettings[i].Key] == true ? "true" : "false";
            }
            config.Save();
        }
        private void SaveSupplySetting(IDictionary<string, int> supplyList)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ScriptSettingSection section = (ScriptSettingSection)config.GetSection("脚本设定/补给");
            for (int i = 0; i < supplyList.Count; i++)
            {
                section.ScriptSettings[i].Value = supplyList[section.ScriptSettings[i].Key].ToString();
            }
            config.Save();
        }
        private void SaveNPCSetting(IDictionary<string, string> npcList)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ScriptSettingSection section = (ScriptSettingSection)config.GetSection("脚本设定/NPC");
            for (int i = 0; i < npcList.Count; i++)
            {
                section.ScriptSettings[i].Value = npcList[section.ScriptSettings[i].Key];
            }
            config.Save();
        }
        private void SaveShortKeySetting(IDictionary<string, string> shortKeyList)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ScriptSettingSection section = (ScriptSettingSection)config.GetSection("脚本设定/快捷键");
            for (int i = 0; i < shortKeyList.Count; i++)
            {
                section.ScriptSettings[i].Value = shortKeyList[section.ScriptSettings[i].Key];
            }
            config.Save();
        }
    }
}

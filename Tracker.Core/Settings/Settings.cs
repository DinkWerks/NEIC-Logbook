using Prism.Mvvm;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Tracker.Core.Settings
{
    public sealed class Settings
    {
        private static readonly Settings _instance = new Settings();

        public static Settings Instance
        {
            get { return _instance; }
        }

        public Setting DatabaseAddress { get; set; }
        public Setting DefaultFeeStructure { get; set; }

        //Constructors
        static Settings()
        {

        }

        private Settings()
        {
            LoadSettings();
        }

        //Methods
        private void LoadSettings()
        {
            XElement settingsFile = XElement.Load($"{@"Resources\Settings.xml"}");

            var SettingsCollection = from item in settingsFile.Descendants("Setting")
                                     select new Setting(
                                         (string)item.Attribute("setting_name"),
                                         (string)item.Attribute("group"),
                                         (string)item.Element("Value"),
                                         Type.GetType((string)item.Element("ValueType"))
                                         );

            DatabaseAddress = SettingsCollection.Where(s => s.SettingName == "Database Address").SingleOrDefault();
            DefaultFeeStructure = SettingsCollection.Where(s => s.SettingName == "Default Fee Structure").SingleOrDefault();
        }

        public void SaveSettings()
        {
            Setting[] settingsList = new Setting[] { DatabaseAddress, DefaultFeeStructure};
            foreach (Setting s in settingsList)
            {
                if (s.IsChanged == true)
                {
                    var settingsFile = XDocument.Load($"{@"Resources\Settings.xml"}");

                    var items = from item in settingsFile.Descendants("Setting")
                                where (string)item.Attribute("setting_name") == s.SettingName
                                select item;

                    foreach (XElement element in items)
                        element.SetElementValue("Value", s.Value);

                    settingsFile.Save($"{@"Resources\Settings.xml"}");
                }
            }
        }
    }

    public class Setting : BindableBase
    {
        private string _settingName;
        private string _group;
        private string _value;
        private Type _valueType;
        private bool _isChanged;

        public string SettingName
        {
            get { return _settingName; }
            set { SetProperty(ref _settingName, value); }
        }

        public string Group
        {
            get { return _group; }
            set { SetProperty(ref _group, value); }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                SetProperty(ref _value, value);
                IsChanged = true;
            }
        }

        public Type ValueType
        {
            get { return _valueType; }
            set { SetProperty(ref _valueType, value); }
        }

        public bool IsChanged
        {
            get { return _isChanged; }
            set { SetProperty(ref _isChanged, value); }
        }

        //Constructor
        public Setting(string name, string group, string value, Type valueType)
        {
            SettingName = name;
            Group = group;
            Value = value;
            ValueType = valueType;
            IsChanged = false;
        }

        //Methods
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

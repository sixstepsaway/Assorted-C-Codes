using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NameSpace.Settings
{
    /// <summary>
    /// A simple class for saving and retrieving settings.
    /// </summary>
    public class Setting {
        public string SettingName {get; set;}
        public string SettingValue {get; set;}
    }

    /// <summary>
    /// Creates and initializes a settings file into which I can save settings and information.
    /// Replace "NameSpace" with app, replace "App" in AppName with App's name.
    /// </summary>
    public class SettingsFile
    {
        public static string AppName = "App";
        public static string MyDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string AppFolder = Path.Combine(MyDocuments, AppName);
        public static string SettingsFile = Path.Combine(AppFolder, "Settings.ini");
        public static List<Setting> Settings = new();

        public static void SaveSetting(string name, string value){
            var setting = Settings.Where(x => x.SettingName == name).FirstOrDefault();
            if (setting.Any()){
                Settings.Remove(setting);
            }
            Settings.Add(new Setting(){SettingName = name, SettingValue = value});                    
            SaveSettingsFile();
        }

        public static string LoadSetting (string name){
            var setting = Settings.Where(x => x.SettingName == name).FirstOrDefault();
            if (setting.Any()){
                return setting.value;
            } else {
                return null;
            }
        }
        
        public static void SaveSettingsFile(){
            if (!Directory.Exists(AppFolder)){
                DirectoryInfo di = Directory.CreateDirectory(AppFolder);
            }
            using (StreamWriter streamWriter = new(SettingsFile)){
                foreach (Setting setting in Settings){
                    streamWriter.WriteLine(string.Format("{0} = {1}", setting.SettingName, setting.SettingValue));
                }
                streamWriter.Flush();
                streamWriter.Close();
            }  
            return;          
        }

        public static void LoadSettingsFile(){            
            if (!File.Exists(SettingsFile)){
                Settings.Clear();
                using (StreamReader streamReader = new StreamReader(SettingsFile)){
                    bool eos = false;                
                    while (eos == false){
                        if(!streamReader.EndOfStream){
                            string settings = streamReader.ReadLine();
                            var line = settings.Split(" = ");
                            Setting setting = new()
                            {
                                SettingName = line[0],
                                SettingValue = line[1]
                            };
                            Settings.Add(setting);
                        } else {
                            eos = true;
                        }
                    }
                    streamReader.Close();
                }
                return;
            } else {
                return;
            }            
        }
    }
}

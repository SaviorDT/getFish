using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using 撈金魚.ActionPerform;
using 撈金魚.ActionPerform.FatHouse;
using 撈金魚.Properties;

namespace 撈金魚.FileManager
{
    public class CountSettings
    {
        public int GetFish { get; set; }
        public int BuyFatNutrient { get; set; }
        public int ElementKnight { get; set; }
        public int Dragon { get; set; }
        public int MoMoTree { get; set; }
    }
    public class MoMoTreeSettings
    {
        public MoMoTreePara Para { get; set; }
        public bool ReadFile { get; set; }
        public MoMoTreeSettings()
        {
            Para = new MoMoTreePara(false, false, -1, -1, -1, -1);
        }
    }
    public class AllSettings
    {
        public CountSettings Counts { get; set; }
        public MoMoTreeSettings Momo { get; set; }
        public int Dragon { get; set; }
        public AllSettings()
        {
            Counts = new CountSettings();
            Momo = new MoMoTreeSettings();
        }
    }
    internal class UserSettings
    {
        public static void Save(AllSettings settings)
        {
            string jsonString = JsonSerializer.Serialize(settings);
            //UserInterface.Message.ShowMessageToUser(settings.counts.GetFish + "");
            FileActionPerformer.SaveTextFile("settings.json", jsonString);
        }
        public static AllSettings Load()
        {
            string jsonString = FileActionPerformer.LoadTextFile("settings.json");
            return JsonSerializer.Deserialize<AllSettings>(jsonString.Equals("") ? "{}" : jsonString);
        }
    }

    internal class AccountDatum
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public AccountDatum(string account, string password)
        {
            Account = account;
            Password = password;
        }
    }

    internal class AccountDataLoader
    {
        public static List<AccountDatum> Load()
        {
            string accountText = FileActionPerformer.LoadTextFile("帳號密碼.txt");
            List<AccountDatum> accountData = new();
            string[] lines = accountText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                string account = trimmedLine.Split(':')[0].Trim();
                string password = trimmedLine.Split(':')[1].Trim();
                accountData.Add(new AccountDatum(account, password));
            }

            return accountData;
        }
    }   
}

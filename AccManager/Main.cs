using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Net;

namespace AccManager
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
            GetAccsNames();
        }

        RegistryKey currUser = Registry.CurrentUser;
        List<account> allAccounts = new List<account>();
        WebClient wc = new WebClient();
        string steamPath = "";

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> accs64IDToDelete = new List<string>();
            foreach(var i in clbAccounts.CheckedItems) { accs64IDToDelete.Add(i.ToString().Split('/')[1].Trim(' ')); }
            List<account> accsToDelete = new List<account>();
            foreach(account i in allAccounts) { if(accs64IDToDelete.Contains(i.Steam64ID)) { accsToDelete.Add(i); } }
            foreach(account i in accsToDelete)
            {
                //Delete accs from userdata
                if (i.Places["Userdata"] == true) { Directory.Delete(steamPath + "userdata\\" + i.AccountID, true); }

                //Delete accs from loginusers.vdf
                if (i.Places["Loginusers"] == true) 
                {
                    string newLoginusersBad = File.ReadAllText(steamPath + "config\\loginusers.vdf").Split(new string[] { "	\"" + i.Steam64ID + "\"" }, StringSplitOptions.None)[0];
                    string newLoginusers = "";
                    foreach (string lu in File.ReadAllText(steamPath + "config\\loginusers.vdf").Split(new string[] { "	\"" + i.Steam64ID + "\"" }, StringSplitOptions.None)[1]
                        .Split('}').Skip(1))
                    { newLoginusersBad += lu + "}"; }
                    for (int q = 0; q < newLoginusersBad.Length - 1; q += 1) 
                    {
                        newLoginusers += newLoginusersBad[q];
                    }
                    File.WriteAllText(steamPath + "config\\loginusers.vdf", newLoginusers);
                }

                //Delete accs from registry
                if (i.Places["Registry"] == true) 
                {
                    var x = currUser.OpenSubKey("SOFTWARE", true);
                    if (x.GetSubKeyNames().Contains("Valve")) { x = x.OpenSubKey("Valve", true); }
                    if (x.GetSubKeyNames().Contains("Steam")) { x = x.OpenSubKey("Steam", true); }
                    if (x.GetSubKeyNames().Contains("Users")) { x = x.OpenSubKey("Users", true); try { x.DeleteSubKeyTree(i.AccountID); } catch (Exception) { } }
                    x.Close();
                }
                allAccounts.Clear();
                clbAccounts.Items.Clear();
            }
            GetAccsNames();
        }

        void GetAccsNames()
        {
            //Get steam path
            var x = currUser.OpenSubKey("SOFTWARE", true);
            if (x.GetSubKeyNames().Contains("Valve")) { x = x.OpenSubKey("Valve", true); }
            if (x.GetSubKeyNames().Contains("Steam")) { x = x.OpenSubKey("Steam", true); }
            if (x.GetSubKeyNames().Contains("ActiveProcess")) { x = x.OpenSubKey("ActiveProcess", true); }
            if (x.GetValueNames().Contains("SteamClientDll")) { steamPath = x.GetValue("SteamClientDll").ToString().Replace("steamclient.dll", ""); }
            x.Close();

            //Get accs from userdata
            foreach (string i in Directory.GetDirectories(steamPath + "userdata"))
            {
                if (!i.Contains("userdata\\ac"))
                {
                    string user = File.ReadAllText(i + "\\config\\localconfig.vdf").Split(new string[] { "\"PersonaName\"		\"" }, StringSplitOptions.None)[1]
                                .Split(new string[] { "\"\n		\"communitypreferences\"" }, StringSplitOptions.None)[0].Trim(' ');
                    allAccounts.Add(new account
                    {
                        Name = user,
                        AccountID = i.Split(new string[] { "userdata\\" }, StringSplitOptions.None)[1].Trim(' '),
                        Steam64ID = Convert.ToString(Convert.ToInt64(i.Split(new string[] { "userdata\\" }, StringSplitOptions.None)[1].Trim(' ')) + 76561197960265728)
                    }); 
                }
            }
            foreach (account i in allAccounts) { i.Places["Userdata"] = true; }

            //Get accs from loginusers.vdf 765
            string[] stringsNotNeed = { "AccountName", "RememberPassword", "WantsOfflineMode", "SkipOfflineModeWarning", "AllowAutoLogin", "MostRecent", "Timestamp" };
            List<string> loginusers = new List<string>();
            foreach (string i in File.ReadAllText(steamPath + "config\\loginusers.vdf")
                .Replace("\"users\"\n{", "").Replace("	", "").Replace("\"", "").Replace("{", "").Replace("}", "").Replace("\n\n", "\n").Split('\n'))
            {
                bool doINeedThisString = true;
                foreach (string snn in stringsNotNeed) { if (i.Contains(snn)) { doINeedThisString = false; } }
                if (doINeedThisString) { loginusers.Add(i.Replace("PersonaName", "")); }
            }
            while(loginusers.Contains("")) { loginusers.Remove(""); }
            for (int i = 0; i < loginusers.Count - 2; i += 2)
            {
                bool doINeedThisAcc = true;
                foreach (account name in allAccounts) { if (loginusers[i + 1] == name.Name) { doINeedThisAcc = false; name.Places["Loginusers"] = true; } }
                if (doINeedThisAcc)
                {
                    allAccounts.Add(new account
                    {
                        Name = loginusers[i + 1],
                        AccountID = Convert.ToString(Convert.ToInt64(loginusers[i]) - 76561197960265728),
                        Steam64ID = loginusers[i]
                    });
                    allAccounts[allAccounts.Count - 1].Places["Loginusers"] = true;
                }
            }

            //Get accs from registry
            string[] registryAccs = new string[] { };
            x = currUser.OpenSubKey("SOFTWARE", true);
            if (x.GetSubKeyNames().Contains("Valve")) { x = x.OpenSubKey("Valve", true); }
            if (x.GetSubKeyNames().Contains("Steam")) { x = x.OpenSubKey("Steam", true); }
            if (x.GetSubKeyNames().Contains("Users")) { x = x.OpenSubKey("Users", true); }
            if (x.Name.Contains("Users")) { registryAccs = x.GetSubKeyNames(); }
            x.Close();
            foreach (string i in registryAccs)
            {
                bool doINeedThisAcc = true;
                foreach (account name in allAccounts) { if (i == name.AccountID) { doINeedThisAcc = false; name.Places["Registry"] = true; } }
                if (doINeedThisAcc)
                {
                    string accName = "";
                    try{ accName = wc.DownloadString("https://steamcommunity.com/profiles/" + Convert.ToString(Convert.ToInt64(i) + 76561197960265728))
                            .Split(new string[] { "<title>" }, StringSplitOptions.None)[1]
                            .Split(new string[] { "</title>" }, StringSplitOptions.None)[0]
                            .Split(new string[] { " :: " }, StringSplitOptions.None)[1]; }
                    catch (Exception) { accName = "Registry Account Without Name"; }
                    allAccounts.Add(new account
                    {
                        Name = accName,
                        AccountID = i,
                        Steam64ID = Convert.ToString(Convert.ToInt64(i) + 76561197960265728)
                    });
                    allAccounts[allAccounts.Count - 1].Places["Registry"] = true;
                }
            }

            //Draw all accounts
            foreach (account acc in allAccounts)
            {
                clbAccounts.Items.Add(acc.Name + " / " + acc.Steam64ID + " / " + acc.AccountID);
            }
        }

    }
}

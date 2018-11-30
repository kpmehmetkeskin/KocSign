using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace KocSign
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var pc = new PrincipalContext(ContextType.Domain, "koctas.local"))
            {
                String path = ".\\";
                try
                {
                    string name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    var user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, name);
                    DirectoryEntry de = (user.GetUnderlyingObject() as DirectoryEntry);

                    String displayName = user.DisplayName;
                    String description = user.Description;
                    String samAccountName = user.SamAccountName;
                    String voiceTelephoneNumber = user.VoiceTelephoneNumber;
                    String streetAddress = de.Properties["streetAddress"].Value.ToString();
                    String mobile = de.Properties["extensionAttribute14"].Value.ToString();

                    path = "C:\\Users\\"+samAccountName+"\\AppData\\Roaming\\Microsoft\\Signatures\\";
                    
                    String filetext = File.ReadAllText("template.htm");
                    filetext = filetext.Replace("\n", "").Replace("**displayName**", displayName).Replace("**description**", description);
                    filetext = filetext.Replace("**voiceTelephoneNumber**", voiceTelephoneNumber).Replace("**streetAddress**", streetAddress).Replace("**mobile**", mobile);

                    File.WriteAllText(path + "me.htm", filetext, Encoding.UTF8);

                    String plainText = "\n\n\n" + user.DisplayName + "\n" + user.Description + "\n" + "KOÇTAŞ AŞ";
                    File.WriteAllText(path + "me.rtf", plainText, Encoding.Unicode);
                    File.WriteAllText(path + "me.txt", plainText, Encoding.Unicode);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("İmza Dosyaları oluşturulurken hata..\n" + ex.Message,"Hata!");
                    Environment.Exit(0);
                }

                MessageBox.Show("me.htm\nme.rtf\nme.txt\ndosyaları "+path+" dizinine başarıyla oluşturuldu.","Başarılı");
                Process.Start(path);
                
                Environment.Exit(0);
            }
        }
    }
}

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
                try
                {
                    string name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    var user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, name);

                    String filetext = File.ReadAllText("template.htm");
                    filetext = filetext.Replace("\n", "").Replace("**name**", user.DisplayName).Replace("**position**", user.Description);
                    File.WriteAllText("./me.htm", filetext, Encoding.UTF8);

                    String plainText = "\n\n\n" + user.DisplayName + "\n" + user.Description + "\n" + "KOÇTAŞ AŞ";
                    File.WriteAllText("./me.rtf", plainText, Encoding.Unicode);
                    File.WriteAllText("./me.txt", plainText, Encoding.Unicode);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("İmza Dosyaları oluşturulurken hata..\n" + ex.Message,"Hata!");
                    Environment.Exit(0);
                }

                MessageBox.Show("me.htm\nme.rtf\nme.txt\ndosyaları aynı dizine başarıyla oluşturuldu.","Başarılı");
                Process.Start(System.IO.Path.GetDirectoryName(Application.ExecutablePath));
                
                Environment.Exit(0);
            }
        }
    }
}

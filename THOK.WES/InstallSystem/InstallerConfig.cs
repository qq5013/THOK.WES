using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace InstallSystem
{
    [RunInstaller(true)]
    public partial class InstallerConfig : Installer
    {
        public InstallerConfig()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            string systemType = Context.Parameters["SYSTEMTYPE"];
            string path = Context.Parameters["targetdir"];
            string fileName = path + "ViewConfig.xml." + systemType;
            try
            {
                System.IO.File.Move(fileName, path + "ViewConfig.xml");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }
    }
}
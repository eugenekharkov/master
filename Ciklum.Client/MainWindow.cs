using Ciklum.Client.BusinessLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ciklum.Types;
namespace Ciklum.Client
{
    public partial class MainWindow : Form
    {
        private CommandManager taskManager;

        public MainWindow()
        {
            InitializeComponent();
            txtUrlAdress.Text = ConfigurationManager.GetAppSettings("serverUrl");
            taskManager = new CommandManager();
            lstEncryptedFiles.DragEnter += new DragEventHandler(lstEncryptedFiles_DragEnter);
            lstEncryptedFiles.DragDrop += new DragEventHandler(lstEncryptedFiles_DragDrop);
        }

        private void lstEncryptedFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            { 
                e.Effect = DragDropEffects.Copy; 
            }
        }

        private void lstEncryptedFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            Dictionary<string, object> parameters = new Dictionary<string, object> {

            { "urlAddress", txtUrlAdress.Text },
            { "UI", this }

            };

            taskManager.AddFilesToProcessQueue(files, "encrypt", parameters);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> {

            { "urlAddress", txtUrlAdress.Text },
            { "UI", this }

            };

            taskManager.AddFilesToProcessQueue(((IEnumerable)lstEncryptedFiles.SelectedItems).Cast<string>(), "decrypt", parameters);
        }

        public void RefreshList(IDictionary<string, string> parameters)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(()=> { RefreshList(parameters); }));
            }
            else
            {
                foreach (var keyValue in parameters)
                {
                    if (keyValue.Key.Equals("add", StringComparison.InvariantCultureIgnoreCase))
                    {
                        lstEncryptedFiles.Items.Add(keyValue.Value);
                    }
                    else
                        if (keyValue.Key.Equals("remove", StringComparison.InvariantCultureIgnoreCase))
                        {
                            lstEncryptedFiles.Items.Remove(keyValue.Value);
                        }
                        else
                            if (keyValue.Key.Equals("error", StringComparison.InvariantCultureIgnoreCase))
                            {
                                MessageBox.Show(keyValue.Value, "Error");
                            }
                }
            }
        }
    }
}

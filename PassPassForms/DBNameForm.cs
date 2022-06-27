using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PassPassForms
{
    public partial class DBNameForm : Form
    {
        public DBNameForm()
        {
            InitializeComponent();
        }

        public string DatabaseName;

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            DatabaseName = NameTextBox.Text;
            Hide();
        }
    }
}

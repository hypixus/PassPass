using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PassPassLib;

namespace PassPassForms
{
    public partial class EntryEditForm : Form
    {
        public EntryEditForm(DbEntry entry, bool isNew)
        {
            InitializeComponent();
            editedEntry = entry;
            _isNew = isNew;
        }

        public DbEntry editedEntry;
        private bool _isNew;

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            var passForm = new DBPasswordForm();
            passForm.ShowDialog();
            editedEntry.Name = NameTextBox.Text;
            editedEntry.SetLogin(LoginTextBox.Text, passForm.Password);
            editedEntry.SetPassword(PassTextBox.Text, passForm.Password);
            Hide();
        }

        private void EntryEditForm_Load(object sender, EventArgs e)
        {
            if (_isNew) return;
            var passForm = new DBPasswordForm();
            passForm.ShowDialog();
            try
            {
                NameTextBox.Text = editedEntry.Name;
                LoginTextBox.Text = editedEntry.DecryptLogin(passForm.Password);
                PassTextBox.Text = editedEntry.DecryptPassword(passForm.Password);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unknown error occurred.");
            }
        }
    }
}

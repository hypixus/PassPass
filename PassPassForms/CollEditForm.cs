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
    public partial class CollEditForm : Form
    {
        public CollEditForm(DbCollection collection)
        {
            editedCollection = collection;
            InitializeComponent();
        }

        public DbCollection editedCollection;

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            editedCollection.Name = NameTextBox.Text;
            editedCollection.Description = DescTextBox.Text;
            Hide();
        }

        private void CollEditForm_Load(object sender, EventArgs e)
        {

        }
    }
}

﻿using System;
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
    public partial class DBPasswordForm : Form
    {
        public DBPasswordForm()
        {
            InitializeComponent();
        }

        public string Password;

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Password = PwdTextBox.Text;
            Hide();
        }
    }
}

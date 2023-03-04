﻿using pwdvault.Modeles;
using pwdvault.Services;
using Serilog;

namespace pwdvault.Forms
{
    public partial class AddPassword : Form
    {
        public AddPassword()
        {
            InitializeComponent();
            comBoxCat.DataSource = Enum.GetValues(typeof(Categories));
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            txtBoxPwd.Text = PasswordService.GeneratePassword();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtBoxApp.Text) &&
                !String.IsNullOrWhiteSpace(txtBoxUser.Text) &&
                !String.IsNullOrWhiteSpace(txtBoxPwd.Text) &&
                !String.IsNullOrWhiteSpace(comBoxCat.Text) && 
                !errorProvider.HasErrors)
            {
                try
                {
                    // Encrypt password and store it, success message and hide the form
                    byte[] encryptedPassword = EncryptionService.EncryptPassword(txtBoxPwd.Text, EncryptionService.GetKeyFromFile());
                    UserPassword userPassword = new UserPassword(comBoxCat.Text, txtBoxApp.Text, txtBoxUser.Text, encryptedPassword) { CreationTime = DateTime.Now };

                } catch(Exception ex)
                {
                    Log.Logger.Error("\nSource : " + ex.Source + "\nMessage : " + ex.Message);
                }

                MessageBox.Show("ok");
                Close();
            }
            else
            {
                MessageBox.Show("Please complete all fields.", "Incomplete form", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void txtBoxPwd_TextChanged(object sender, EventArgs e)
        {
            if (!PasswordService.IsPasswordStrong(txtBoxPwd.Text))
            {
                errorProvider.SetError(txtBoxPwd, "Password must be atleast 16 characters long and contain the following : " + Environment.NewLine +
                        "- Uppercase" + Environment.NewLine + "- Lowercase" + Environment.NewLine + "- Numbers" + Environment.NewLine + "- Symbols");
            }
            else
            {
                errorProvider.SetError(txtBoxPwd, String.Empty);
                errorProvider.Clear();
            }
        }
    }
}

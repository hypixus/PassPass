namespace PassPassForms;

public partial class DBPasswordForm : Form
{
    public string Password;

    public DBPasswordForm()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object sender, EventArgs e)
    {
        Password = PwdTextBox.Text;
        Hide();
    }
}
namespace PassPassForms;

public partial class DBNameForm : Form
{
    public string DatabaseName;

    public DBNameForm()
    {
        InitializeComponent();
    }

    private void SubmitButton_Click(object sender, EventArgs e)
    {
        DatabaseName = NameTextBox.Text;
        Hide();
    }
}
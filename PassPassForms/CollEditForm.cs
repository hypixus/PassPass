using PassPassLib;

namespace PassPassForms;

public partial class CollEditForm : Form
{
    public DbCollection editedCollection;

    public CollEditForm(DbCollection collection)
    {
        editedCollection = collection;
        InitializeComponent();
    }

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
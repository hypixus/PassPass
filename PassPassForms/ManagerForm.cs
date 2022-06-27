using PassPassLib;
using Exception = System.Exception;

namespace PassPassForms
{
    public partial class ManagerForm : Form
    {
        private Database _currentDatabase = new();

        public ManagerForm()
        {
            InitializeComponent();
        }

        private void EntriesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ignore
        }

        private void ReloadListboxes()
        {
            collListBox.Items.Clear();
            entriesListBox.Items.Clear();
            for (int i = 0; i < _currentDatabase.Collections.Count; i++)
            {
                collListBox.Items.Add(_currentDatabase.Collections[i].Name);
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            collListBox.Items.Clear();
            entriesListBox.Items.Clear();
            var nameForm = new DBNameForm();
            nameForm.ShowDialog();
            _currentDatabase = new Database(nameForm.DatabaseName);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
 
            saveDialog.Filter = @"PassPass Database (*.ppdb)|*.ppdb|All files (*.*)|*.*";
            saveDialog.RestoreDirectory = true;

            if (saveDialog.ShowDialog() != DialogResult.OK) return;
            try
            {
                var passForm = new DBPasswordForm();
                passForm.ShowDialog();
                toolStripStatusLabel.Text = "Saving...";
                Refresh();
                _currentDatabase.ExportToFile(saveDialog.FileName, passForm.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown exception occurred.");
            }
            toolStripStatusLabel.Text = "Ready.";
            MessageBox.Show("Database successfully saved.","Info", MessageBoxButtons.OK,  MessageBoxIcon.Information);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = @"PassPass Database (*.ppdb)|*.ppdb|All files (*.*)|*.*";
            openDialog.RestoreDirectory = true;
            if (openDialog.ShowDialog() != DialogResult.OK) return;
            var passForm = new DBPasswordForm();
            passForm.ShowDialog();
            toolStripStatusLabel.Text = "Opening...";
            Refresh();
            _currentDatabase = new Database(openDialog.FileName, passForm.Password);
            toolStripStatusLabel.Text = "Ready.";
            Refresh();
            ReloadListboxes();
        }

        private void EntriesListBox_DoubleClick(object sender, EventArgs e)
        {
            DbCollection collection;
            DbEntry entry;
            try
            {
                collection = _currentDatabase.Collections[collListBox.SelectedIndex];
                entry = collection.Entries[entriesListBox.SelectedIndex];
                var editForm = new EntryEditForm(entry, false);
                editForm.ShowDialog();
                toolStripStatusLabel.Text = "Editing...";
                Refresh();
                _currentDatabase.Collections[collListBox.SelectedIndex].Entries[entriesListBox.SelectedIndex] =
                    editForm.editedEntry;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unknown error occurred.");
            }
            finally
            {
                toolStripStatusLabel.Text = "Ready.";
                Refresh();
            }
        }

        private void NewCollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newColl = new DbCollection();
            var collForm = new CollEditForm(newColl);
            toolStripStatusLabel.Text = "Editing...";
            Refresh();
            collForm.ShowDialog();
            try
            {
                _currentDatabase.Collections.Add(newColl);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unknown error occurred.");
            }
            finally
            {
                toolStripStatusLabel.Text = "Ready.";
                Refresh();
            }
            ReloadListboxes();
        }

        private void NewEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newEntry = new DbEntry();
            toolStripStatusLabel.Text = "Editing...";
            Refresh();
            var editForm = new EntryEditForm(newEntry, true);
            editForm.ShowDialog();
            try
            {
                _currentDatabase.Collections[collListBox.SelectedIndex].Entries.Add(newEntry);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unknown error occurred.");
            }
            finally
            {
                toolStripStatusLabel.Text = "Ready.";
                Refresh();
            }
            ReloadListboxes();
        }

        private void CollListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            entriesListBox.Items.Clear();
            foreach (var entry in _currentDatabase.Collections[collListBox.SelectedIndex].Entries)
            {
                entriesListBox.Items.Add(entry.Name);
            }
        }
    }
}
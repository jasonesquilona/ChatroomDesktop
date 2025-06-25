using System.ComponentModel;

namespace ChatroomDesktop.Views;

public partial class GroupCreationForm : Form
{
    public GroupCreationForm()
    {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string GroupNameEntered
    {
        get;
        private set;
    }

    private void groupNameTextbox_TextChanged(object sender, EventArgs e)
    {
    }

    private void Cancel_Click(object sender, EventArgs e)
    {
        DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.Close();
    }

    private void enterButton_Click(object sender, EventArgs e)
    {
        string name = groupNameTextbox.Text;
        if (!string.IsNullOrEmpty(name))
        {
            GroupNameEntered = name;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        else
        {
            MessageBox.Show("Please enter a name.");
        }
        this.Close();
    }

    public void ChangeLabelText(string labelText)
    {
        label1.Text = labelText;
    }

    public void ChangeTextLength(int length)
    {
        groupNameTextbox.MaxLength = length;
    }
}
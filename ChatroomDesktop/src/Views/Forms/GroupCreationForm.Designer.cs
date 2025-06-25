using System.ComponentModel;

namespace ChatroomDesktop.Views;

partial class GroupCreationForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        groupNameTextbox = new System.Windows.Forms.TextBox();
        label1 = new System.Windows.Forms.Label();
        enterButton = new System.Windows.Forms.Button();
        Cancel = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // groupNameTextbox
        // 
        groupNameTextbox.Location = new System.Drawing.Point(46, 127);
        groupNameTextbox.MaxLength = 30;
        groupNameTextbox.Name = "groupNameTextbox";
        groupNameTextbox.Size = new System.Drawing.Size(300, 23);
        groupNameTextbox.TabIndex = 0;
        groupNameTextbox.TextChanged += groupNameTextbox_TextChanged;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(46, 101);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(117, 23);
        label1.TabIndex = 1;
        label1.Text = "Enter Group Name";
        // 
        // enterButton
        // 
        enterButton.Location = new System.Drawing.Point(46, 156);
        enterButton.Name = "enterButton";
        enterButton.Size = new System.Drawing.Size(84, 24);
        enterButton.TabIndex = 2;
        enterButton.Text = "Enter";
        enterButton.UseVisualStyleBackColor = true;
        enterButton.Click += enterButton_Click;
        // 
        // Cancel
        // 
        Cancel.Location = new System.Drawing.Point(271, 156);
        Cancel.Name = "Cancel";
        Cancel.Size = new System.Drawing.Size(75, 23);
        Cancel.TabIndex = 3;
        Cancel.Text = "Cancel";
        Cancel.UseVisualStyleBackColor = true;
        Cancel.Click += Cancel_Click;
        // 
        // GroupCreationForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(403, 270);
        Controls.Add(Cancel);
        Controls.Add(enterButton);
        Controls.Add(label1);
        Controls.Add(groupNameTextbox);
        Text = "GroupCreationForm";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Button enterButton;
    private System.Windows.Forms.Button Cancel;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.TextBox groupNameTextbox;

    #endregion
}
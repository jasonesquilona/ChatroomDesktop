using System.ComponentModel;

namespace ChatroomDesktop.Views;

partial class GroupChatsForm
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
        createGroupButton = new System.Windows.Forms.Button();
        groupList = new System.Windows.Forms.Panel();
        button1 = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // createGroupButton
        // 
        createGroupButton.Font = new System.Drawing.Font("Segoe UI", 12F);
        createGroupButton.Location = new System.Drawing.Point(579, 77);
        createGroupButton.Name = "createGroupButton";
        createGroupButton.Size = new System.Drawing.Size(167, 51);
        createGroupButton.TabIndex = 0;
        createGroupButton.Text = "+ Create Group";
        createGroupButton.UseVisualStyleBackColor = true;
        createGroupButton.Click += button1_Click;
        // 
        // groupList
        // 
        groupList.AutoScroll = true;
        groupList.Location = new System.Drawing.Point(28, 23);
        groupList.Name = "groupList";
        groupList.Size = new System.Drawing.Size(517, 397);
        groupList.TabIndex = 1;
        // 
        // button1
        // 
        button1.Font = new System.Drawing.Font("Segoe UI", 12F);
        button1.Location = new System.Drawing.Point(579, 305);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(167, 51);
        button1.TabIndex = 2;
        button1.Text = "Join Group";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click_1;
        // 
        // GroupChatsForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(button1);
        Controls.Add(groupList);
        Controls.Add(createGroupButton);
        Text = "GroupChatForm";
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button button1;

    private System.Windows.Forms.Panel groupList;

    private System.Windows.Forms.Button createGroupButton;

    #endregion
}
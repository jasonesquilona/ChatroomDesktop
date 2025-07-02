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
        JoinGroupButton = new System.Windows.Forms.Button();
        groupChatList = new System.Windows.Forms.FlowLayoutPanel();
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
        // JoinGroupButton
        // 
        JoinGroupButton.Font = new System.Drawing.Font("Segoe UI", 12F);
        JoinGroupButton.Location = new System.Drawing.Point(579, 305);
        JoinGroupButton.Name = "JoinGroupButton";
        JoinGroupButton.Size = new System.Drawing.Size(167, 51);
        JoinGroupButton.TabIndex = 2;
        JoinGroupButton.Text = "Join Group";
        JoinGroupButton.UseVisualStyleBackColor = true;
        JoinGroupButton.Click += button1_Click_1;
        // 
        // groupChatList
        // 
        groupChatList.Location = new System.Drawing.Point(30, 46);
        groupChatList.Name = "groupChatList";
        groupChatList.Size = new System.Drawing.Size(543, 319);
        groupChatList.TabIndex = 3;
        // 
        // GroupChatsForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(groupChatList);
        Controls.Add(JoinGroupButton);
        Controls.Add(createGroupButton);
        Text = "GroupChatForm";
        ResumeLayout(false);
    }

    private System.Windows.Forms.FlowLayoutPanel groupChatList;

    private System.Windows.Forms.Button JoinGroupButton;

    private System.Windows.Forms.Button createGroupButton;

    #endregion
}
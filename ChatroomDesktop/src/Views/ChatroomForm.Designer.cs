using System.ComponentModel;

namespace ChatroomDesktop.Views;

partial class ChatroomForm
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
        sendMsgTextbox = new System.Windows.Forms.RichTextBox();
        sendButton = new System.Windows.Forms.Button();
        chatText = new System.Windows.Forms.RichTextBox();
        memberList = new System.Windows.Forms.RichTextBox();
        SuspendLayout();
        // 
        // sendMsgTextbox
        // 
        sendMsgTextbox.Location = new System.Drawing.Point(40, 376);
        sendMsgTextbox.Name = "sendMsgTextbox";
        sendMsgTextbox.Size = new System.Drawing.Size(489, 37);
        sendMsgTextbox.TabIndex = 1;
        sendMsgTextbox.Text = "";
        // 
        // sendButton
        // 
        sendButton.Font = new System.Drawing.Font("Segoe UI", 12F);
        sendButton.Location = new System.Drawing.Point(535, 376);
        sendButton.Name = "sendButton";
        sendButton.Size = new System.Drawing.Size(61, 37);
        sendButton.TabIndex = 2;
        sendButton.Text = "Send";
        sendButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        sendButton.UseVisualStyleBackColor = true;
        // 
        // chatText
        // 
        chatText.Location = new System.Drawing.Point(40, 34);
        chatText.Name = "chatText";
        chatText.ReadOnly = true;
        chatText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
        chatText.Size = new System.Drawing.Size(595, 320);
        chatText.TabIndex = 3;
        chatText.Text = "";
        chatText.WordWrap = false;
        // 
        // memberList
        // 
        memberList.Location = new System.Drawing.Point(656, 34);
        memberList.Name = "memberList";
        memberList.ReadOnly = true;
        memberList.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
        memberList.Size = new System.Drawing.Size(134, 320);
        memberList.TabIndex = 4;
        memberList.Text = "";
        memberList.WordWrap = false;
        // 
        // ChatroomForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(memberList);
        Controls.Add(chatText);
        Controls.Add(sendButton);
        Controls.Add(sendMsgTextbox);
        Text = "Chatroom";
        ResumeLayout(false);
    }

    private System.Windows.Forms.RichTextBox memberList;

    private System.Windows.Forms.RichTextBox chatText;

    private System.Windows.Forms.RichTextBox sendMsgTextbox;
    private System.Windows.Forms.Button sendButton;

    #endregion
}
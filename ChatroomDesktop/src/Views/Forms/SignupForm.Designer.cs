using System.ComponentModel;

namespace ChatroomDesktop.Views;

partial class SignupForm
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
        nameText = new System.Windows.Forms.TextBox();
        passwordText = new System.Windows.Forms.TextBox();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        cancelButton = new System.Windows.Forms.Button();
        confirmButton = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // nameText
        // 
        nameText.Location = new System.Drawing.Point(168, 117);
        nameText.Name = "nameText";
        nameText.Size = new System.Drawing.Size(345, 23);
        nameText.TabIndex = 0;
        nameText.TextChanged += textBox1_TextChanged;
        // 
        // passwordText
        // 
        passwordText.Location = new System.Drawing.Point(168, 190);
        passwordText.Name = "passwordText";
        passwordText.Size = new System.Drawing.Size(345, 23);
        passwordText.TabIndex = 1;
        passwordText.UseSystemPasswordChar = true;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(168, 81);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(100, 23);
        label1.TabIndex = 2;
        label1.Text = "Username";
        label1.Click += label1_Click;
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(168, 164);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(100, 23);
        label2.TabIndex = 3;
        label2.Text = "Password\r\n";
        label2.Click += label2_Click;
        // 
        // cancelButton
        // 
        cancelButton.Location = new System.Drawing.Point(168, 233);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new System.Drawing.Size(75, 23);
        cancelButton.TabIndex = 4;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        // 
        // confirmButton
        // 
        confirmButton.Location = new System.Drawing.Point(438, 233);
        confirmButton.Name = "confirmButton";
        confirmButton.Size = new System.Drawing.Size(75, 23);
        confirmButton.TabIndex = 5;
        confirmButton.Text = "Confirm";
        confirmButton.UseVisualStyleBackColor = true;
        // 
        // SignupForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(confirmButton);
        Controls.Add(cancelButton);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(passwordText);
        Controls.Add(nameText);
        Text = "SignupForm";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button confirmButton;

    private System.Windows.Forms.Label label2;

    private System.Windows.Forms.TextBox passwordText;
    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.TextBox nameText;

    #endregion
}
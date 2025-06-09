using System.ComponentModel;

namespace ChatroomDesktop.Views;

partial class LoginForm
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
        txtName = new System.Windows.Forms.TextBox();
        enterBtn = new System.Windows.Forms.Button();
        label1 = new System.Windows.Forms.Label();
        txtPassword = new System.Windows.Forms.TextBox();
        label2 = new System.Windows.Forms.Label();
        signupButton = new System.Windows.Forms.Button();
        incorrectLabel = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // txtName
        // 
        txtName.Location = new System.Drawing.Point(239, 150);
        txtName.Name = "txtName";
        txtName.Size = new System.Drawing.Size(264, 23);
        txtName.TabIndex = 0;
        txtName.TextChanged += txtName_TextChanged;
        // 
        // enterBtn
        // 
        enterBtn.Location = new System.Drawing.Point(428, 259);
        enterBtn.Name = "enterBtn";
        enterBtn.Size = new System.Drawing.Size(75, 23);
        enterBtn.TabIndex = 3;
        enterBtn.Text = "Enter";
        enterBtn.UseVisualStyleBackColor = true;
        enterBtn.Click += enterBtn_Click;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(239, 124);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(100, 23);
        label1.TabIndex = 2;
        label1.Text = "Username";
        label1.Click += label1_Click;
        // 
        // txtPassword
        // 
        txtPassword.Location = new System.Drawing.Point(239, 202);
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new System.Drawing.Size(264, 23);
        txtPassword.TabIndex = 1;
        txtPassword.UseSystemPasswordChar = true;
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(239, 176);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(100, 23);
        label2.TabIndex = 4;
        label2.Text = "Password";
        label2.Click += label2_Click;
        // 
        // signupButton
        // 
        signupButton.Location = new System.Drawing.Point(324, 298);
        signupButton.Name = "signupButton";
        signupButton.Size = new System.Drawing.Size(93, 23);
        signupButton.TabIndex = 5;
        signupButton.Text = "Sign Up";
        signupButton.UseVisualStyleBackColor = true;
        // 
        // incorrectLabel
        // 
        incorrectLabel.ForeColor = System.Drawing.Color.Red;
        incorrectLabel.Location = new System.Drawing.Point(239, 237);
        incorrectLabel.Name = "incorrectLabel";
        incorrectLabel.Size = new System.Drawing.Size(264, 23);
        incorrectLabel.TabIndex = 6;
        incorrectLabel.Text = "Incorrect Username or Password";
        incorrectLabel.Visible = false;
        incorrectLabel.Click += label3_Click;
        // 
        // LoginForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(incorrectLabel);
        Controls.Add(signupButton);
        Controls.Add(label2);
        Controls.Add(txtPassword);
        Controls.Add(label1);
        Controls.Add(enterBtn);
        Controls.Add(txtName);
        Text = "NameForm";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label incorrectLabel;

    private System.Windows.Forms.Button signupButton;

    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label label2;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.Button enterBtn;

    private System.Windows.Forms.TextBox txtName;

    #endregion
}
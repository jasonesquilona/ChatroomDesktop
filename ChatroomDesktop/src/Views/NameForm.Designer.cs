using System.ComponentModel;

namespace ChatroomDesktop.Views;

partial class NameForm
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
        textBox1 = new System.Windows.Forms.TextBox();
        label2 = new System.Windows.Forms.Label();
        signupButton = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // txtName
        // 
        txtName.Location = new System.Drawing.Point(229, 124);
        txtName.Name = "txtName";
        txtName.Size = new System.Drawing.Size(264, 23);
        txtName.TabIndex = 0;
        txtName.TextChanged += txtName_TextChanged;
        // 
        // enterBtn
        // 
        enterBtn.Location = new System.Drawing.Point(418, 227);
        enterBtn.Name = "enterBtn";
        enterBtn.Size = new System.Drawing.Size(75, 23);
        enterBtn.TabIndex = 1;
        enterBtn.Text = "Enter";
        enterBtn.UseVisualStyleBackColor = true;
        enterBtn.Click += enterBtn_Click;
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(229, 98);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(100, 23);
        label1.TabIndex = 2;
        label1.Text = "Username";
        label1.Click += label1_Click;
        // 
        // textBox1
        // 
        textBox1.Location = new System.Drawing.Point(229, 186);
        textBox1.Name = "textBox1";
        textBox1.Size = new System.Drawing.Size(264, 23);
        textBox1.TabIndex = 3;
        textBox1.UseSystemPasswordChar = true;
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(229, 160);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(100, 23);
        label2.TabIndex = 4;
        label2.Text = "Password";
        label2.Click += label2_Click;
        // 
        // signupButton
        // 
        signupButton.Location = new System.Drawing.Point(308, 273);
        signupButton.Name = "signupButton";
        signupButton.Size = new System.Drawing.Size(93, 23);
        signupButton.TabIndex = 5;
        signupButton.Text = "Sign Up";
        signupButton.UseVisualStyleBackColor = true;
        // 
        // NameForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(signupButton);
        Controls.Add(label2);
        Controls.Add(textBox1);
        Controls.Add(label1);
        Controls.Add(enterBtn);
        Controls.Add(txtName);
        Text = "NameForm";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Button signupButton;

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label2;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.Button enterBtn;

    private System.Windows.Forms.TextBox txtName;

    #endregion
}
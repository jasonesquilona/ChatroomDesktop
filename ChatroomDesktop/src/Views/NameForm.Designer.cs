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
        SuspendLayout();
        // 
        // txtName
        // 
        txtName.Location = new System.Drawing.Point(176, 182);
        txtName.Name = "txtName";
        txtName.Size = new System.Drawing.Size(264, 23);
        txtName.TabIndex = 0;
        txtName.TextChanged += txtName_TextChanged;
        // 
        // enterBtn
        // 
        enterBtn.Location = new System.Drawing.Point(470, 182);
        enterBtn.Name = "enterBtn";
        enterBtn.Size = new System.Drawing.Size(75, 23);
        enterBtn.TabIndex = 1;
        enterBtn.Text = "Enter";
        enterBtn.UseVisualStyleBackColor = true;
        enterBtn.Click += enterBtn_Click;
        // 
        // NameForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(enterBtn);
        Controls.Add(txtName);
        Text = "NameForm";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Button enterBtn;

    private System.Windows.Forms.TextBox txtName;

    #endregion
}
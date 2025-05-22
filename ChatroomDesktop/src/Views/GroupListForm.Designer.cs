using System.ComponentModel;

namespace ChatroomDesktop.Views;

partial class GroupListForm
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
        createGroup = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // createGroup
        // 
        createGroup.Location = new System.Drawing.Point(616, 172);
        createGroup.Name = "createGroup";
        createGroup.Size = new System.Drawing.Size(130, 30);
        createGroup.TabIndex = 0;
        createGroup.Text = "Create Group";
        createGroup.UseVisualStyleBackColor = true;
        createGroup.Click += button1_Click;
        // 
        // GroupListForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(createGroup);
        Text = "GroupListForm";
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button createGroup;

    #endregion
}
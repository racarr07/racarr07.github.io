namespace A2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            detailsBttn = new Button();
            viewBttn = new Button();
            listViewBox = new ListView();
            picBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picBox).BeginInit();
            SuspendLayout();
            // 
            // detailsBttn
            // 
            detailsBttn.Location = new Point(44, 275);
            detailsBttn.Name = "detailsBttn";
            detailsBttn.Size = new Size(94, 29);
            detailsBttn.TabIndex = 0;
            detailsBttn.Text = "Details";
            detailsBttn.UseVisualStyleBackColor = true;
            detailsBttn.Click += detailsBttn_Click;
            // 
            // viewBttn
            // 
            viewBttn.Location = new Point(195, 275);
            viewBttn.Name = "viewBttn";
            viewBttn.Size = new Size(94, 29);
            viewBttn.TabIndex = 1;
            viewBttn.Text = "View";
            viewBttn.UseVisualStyleBackColor = true;
            // 
            // listViewBox
            // 
            listViewBox.Location = new Point(31, 31);
            listViewBox.Name = "listViewBox";
            listViewBox.Size = new Size(341, 215);
            listViewBox.TabIndex = 2;
            listViewBox.UseCompatibleStateImageBehavior = false;
            // 
            // picBox
            // 
            picBox.Location = new Point(447, 68);
            picBox.Name = "picBox";
            picBox.Size = new Size(312, 167);
            picBox.TabIndex = 3;
            picBox.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(picBox);
            Controls.Add(listViewBox);
            Controls.Add(viewBttn);
            Controls.Add(detailsBttn);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)picBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button detailsBttn;
        private Button viewBttn;
        private ListView listViewBox;
        private PictureBox picBox;
    }
}

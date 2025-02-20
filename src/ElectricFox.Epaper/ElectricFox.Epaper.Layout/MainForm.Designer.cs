namespace ElectricFox.Epaper.Layout
{
    partial class MainForm
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
            layout2 = new PictureBox();
            sendButton = new Button();
            renderButton = new Button();
            getDataButton = new Button();
            propertyGrid = new PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)layout2).BeginInit();
            SuspendLayout();
            // 
            // layout2
            // 
            layout2.BackColor = Color.White;
            layout2.Location = new Point(12, 11);
            layout2.Name = "layout2";
            layout2.Size = new Size(272, 792);
            layout2.TabIndex = 3;
            layout2.TabStop = false;
            layout2.MouseMove += LayoutMouseMove;
            // 
            // sendButton
            // 
            sendButton.Enabled = false;
            sendButton.Location = new Point(290, 70);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(97, 23);
            sendButton.TabIndex = 4;
            sendButton.Text = "Send to Display";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += SendButtonClick;
            // 
            // renderButton
            // 
            renderButton.Enabled = false;
            renderButton.Location = new Point(290, 41);
            renderButton.Name = "renderButton";
            renderButton.Size = new Size(97, 23);
            renderButton.TabIndex = 5;
            renderButton.Text = "Render";
            renderButton.UseVisualStyleBackColor = true;
            renderButton.Click += RenderButtonClick;
            // 
            // getDataButton
            // 
            getDataButton.Location = new Point(290, 12);
            getDataButton.Name = "getDataButton";
            getDataButton.Size = new Size(97, 23);
            getDataButton.TabIndex = 6;
            getDataButton.Text = "Get Data";
            getDataButton.UseVisualStyleBackColor = true;
            getDataButton.Click += GetDataButtonClick;
            // 
            // propertyGrid
            // 
            propertyGrid.Location = new Point(290, 110);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new Size(347, 656);
            propertyGrid.TabIndex = 7;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(666, 820);
            Controls.Add(propertyGrid);
            Controls.Add(getDataButton);
            Controls.Add(renderButton);
            Controls.Add(sendButton);
            Controls.Add(layout2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "ePaper Layout";
            Load += MainFormLoad;
            ((System.ComponentModel.ISupportInitialize)layout2).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox layout2;
        private Button sendButton;
        private Button renderButton;
        private Button getDataButton;
        private PropertyGrid propertyGrid;
    }
}

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
            loadImageButton = new Button();
            openFileDialog = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)layout2).BeginInit();
            SuspendLayout();
            // 
            // layout2
            // 
            layout2.BackColor = Color.White;
            layout2.Location = new Point(21, 22);
            layout2.Margin = new Padding(5, 6, 5, 6);
            layout2.Name = "layout2";
            layout2.Size = new Size(466, 1584);
            layout2.TabIndex = 3;
            layout2.TabStop = false;
            layout2.MouseMove += LayoutMouseMove;
            // 
            // sendButton
            // 
            sendButton.Enabled = false;
            sendButton.Location = new Point(497, 140);
            sendButton.Margin = new Padding(5, 6, 5, 6);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(166, 46);
            sendButton.TabIndex = 4;
            sendButton.Text = "Send to Display";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += SendButtonClick;
            // 
            // renderButton
            // 
            renderButton.Enabled = false;
            renderButton.Location = new Point(497, 82);
            renderButton.Margin = new Padding(5, 6, 5, 6);
            renderButton.Name = "renderButton";
            renderButton.Size = new Size(166, 46);
            renderButton.TabIndex = 5;
            renderButton.Text = "Render";
            renderButton.UseVisualStyleBackColor = true;
            renderButton.Click += RenderButtonClick;
            // 
            // getDataButton
            // 
            getDataButton.Location = new Point(497, 24);
            getDataButton.Margin = new Padding(5, 6, 5, 6);
            getDataButton.Name = "getDataButton";
            getDataButton.Size = new Size(166, 46);
            getDataButton.TabIndex = 6;
            getDataButton.Text = "Get Data";
            getDataButton.UseVisualStyleBackColor = true;
            getDataButton.Click += GetDataButtonClick;
            // 
            // propertyGrid
            // 
            propertyGrid.Location = new Point(497, 220);
            propertyGrid.Margin = new Padding(5, 6, 5, 6);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new Size(595, 1312);
            propertyGrid.TabIndex = 7;
            // 
            // loadImageButton
            // 
            loadImageButton.Location = new Point(671, 82);
            loadImageButton.Name = "loadImageButton";
            loadImageButton.Size = new Size(216, 46);
            loadImageButton.TabIndex = 8;
            loadImageButton.Text = "Load Image...";
            loadImageButton.UseVisualStyleBackColor = true;
            loadImageButton.Click += LoadImageButtonClick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1142, 1640);
            Controls.Add(loadImageButton);
            Controls.Add(propertyGrid);
            Controls.Add(getDataButton);
            Controls.Add(renderButton);
            Controls.Add(sendButton);
            Controls.Add(layout2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(5, 6, 5, 6);
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
        private Button loadImageButton;
        private OpenFileDialog openFileDialog;
    }
}

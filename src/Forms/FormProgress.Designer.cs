namespace NUBTool.src.Forms
{
    partial class FormProgress
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgress));
            progressBar_MainProgress = new ProgressBar();
            label_Status = new Label();
            button_Abort = new Button();
            timer_interval = new System.Windows.Forms.Timer(components);
            label_Caption = new Label();
            SuspendLayout();
            // 
            // progressBar_MainProgress
            // 
            resources.ApplyResources(progressBar_MainProgress, "progressBar_MainProgress");
            progressBar_MainProgress.Name = "progressBar_MainProgress";
            // 
            // label_Status
            // 
            resources.ApplyResources(label_Status, "label_Status");
            label_Status.Name = "label_Status";
            // 
            // button_Abort
            // 
            resources.ApplyResources(button_Abort, "button_Abort");
            button_Abort.Name = "button_Abort";
            button_Abort.UseVisualStyleBackColor = true;
            button_Abort.Click += Button_abort_Click;
            // 
            // timer_interval
            // 
            timer_interval.Tick += Timer_interval_Tick;
            // 
            // label_Caption
            // 
            resources.ApplyResources(label_Caption, "label_Caption");
            label_Caption.Name = "label_Caption";
            // 
            // FormProgress
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            ControlBox = false;
            Controls.Add(label_Caption);
            Controls.Add(button_Abort);
            Controls.Add(label_Status);
            Controls.Add(progressBar_MainProgress);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "FormProgress";
            Load += FormProgress_Load;
            ResumeLayout(false);
        }

        #endregion

        private ProgressBar progressBar_MainProgress;
        private Label label_Status;
        private Button button_Abort;
        private System.Windows.Forms.Timer timer_interval;
        private Label label_Caption;
    }
}
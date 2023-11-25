using CobaltCoreModLoaderApp;
using System.Windows.Forms;

namespace CobaltChatCore
{
    partial class Form1
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

        public void InitMainForm(MainForm form)
        {
            form.SuspendLayout();
            configurationBindingSource.DataSource = Configuration.Instance;
            form.MainTabControl.Controls.Add(tpCCC);
            tpCCC.Font = new Font("Segoe UI", 9);
            form.ResumeLayout();
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.tpCCC = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label24 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.nudChatterChance = new System.Windows.Forms.NumericUpDown();
            this.configurationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAuthorize = new System.Windows.Forms.Button();
            this.lblAutorizationStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.lblPortValid = new System.Windows.Forms.Label();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbChannelName = new System.Windows.Forms.TextBox();
            this.lblChannelNameValid = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nudChatterPickLimit = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.cbJoinEnabled = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbNormalBattleType = new System.Windows.Forms.CheckBox();
            this.cbEliteBattleType = new System.Windows.Forms.CheckBox();
            this.cbBossBattleType = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.MainTabControl.SuspendLayout();
            this.tpCCC.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChatterChance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.configurationBindingSource)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChatterPickLimit)).BeginInit();
            this.flowLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.tpCCC);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Margin = new System.Windows.Forms.Padding(5);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(800, 485);
            this.MainTabControl.TabIndex = 1;
            // 
            // tpCCC
            // 
            this.tpCCC.AutoScroll = true;
            this.tpCCC.Controls.Add(this.tableLayoutPanel2);
            this.tpCCC.Location = new System.Drawing.Point(4, 24);
            this.tpCCC.Name = "tpCCC";
            this.tpCCC.Size = new System.Drawing.Size(792, 457);
            this.tpCCC.TabIndex = 6;
            this.tpCCC.Text = "CobaltChatCore";
            this.tpCCC.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label24, 0, 29);
            this.tableLayoutPanel2.Controls.Add(this.label16, 0, 20);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.nudChatterChance, 1, 17);
            this.tableLayoutPanel2.Controls.Add(this.textBox3, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel3, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.textBox1, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 16);
            this.tableLayoutPanel2.Controls.Add(this.nudChatterPickLimit, 1, 16);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 17);
            this.tableLayoutPanel2.Controls.Add(this.cbJoinEnabled, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 19);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel4, 1, 19);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 10);
            this.tableLayoutPanel2.Controls.Add(this.textBox2, 1, 10);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 11);
            this.tableLayoutPanel2.Controls.Add(this.label14, 0, 12);
            this.tableLayoutPanel2.Controls.Add(this.label15, 0, 13);
            this.tableLayoutPanel2.Controls.Add(this.textBox4, 1, 12);
            this.tableLayoutPanel2.Controls.Add(this.textBox5, 1, 11);
            this.tableLayoutPanel2.Controls.Add(this.textBox6, 1, 13);
            this.tableLayoutPanel2.Controls.Add(this.label17, 0, 21);
            this.tableLayoutPanel2.Controls.Add(this.label18, 0, 22);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 23);
            this.tableLayoutPanel2.Controls.Add(this.label23, 0, 24);
            this.tableLayoutPanel2.Controls.Add(this.label21, 0, 25);
            this.tableLayoutPanel2.Controls.Add(this.label22, 0, 26);
            this.tableLayoutPanel2.Controls.Add(this.label20, 0, 27);
            this.tableLayoutPanel2.Controls.Add(this.label25, 0, 28);
            this.tableLayoutPanel2.Controls.Add(this.label26, 0, 30);
            this.tableLayoutPanel2.Controls.Add(this.label27, 0, 31);
            this.tableLayoutPanel2.Controls.Add(this.label28, 0, 32);
            this.tableLayoutPanel2.Controls.Add(this.label29, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.label30, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.checkBox1, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.checkBox2, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.textBox7, 1, 21);
            this.tableLayoutPanel2.Controls.Add(this.textBox8, 1, 23);
            this.tableLayoutPanel2.Controls.Add(this.textBox9, 1, 25);
            this.tableLayoutPanel2.Controls.Add(this.textBox10, 1, 27);
            this.tableLayoutPanel2.Controls.Add(this.textBox11, 1, 29);
            this.tableLayoutPanel2.Controls.Add(this.textBox12, 1, 31);
            this.tableLayoutPanel2.Controls.Add(this.textBox13, 1, 22);
            this.tableLayoutPanel2.Controls.Add(this.textBox14, 1, 24);
            this.tableLayoutPanel2.Controls.Add(this.textBox15, 1, 26);
            this.tableLayoutPanel2.Controls.Add(this.textBox16, 1, 28);
            this.tableLayoutPanel2.Controls.Add(this.textBox17, 1, 30);
            this.tableLayoutPanel2.Controls.Add(this.textBox18, 1, 32);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 35;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(775, 1119);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.Location = new System.Drawing.Point(3, 896);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(142, 31);
            this.label24.TabIndex = 41;
            this.label24.Text = "Ban Chatter command";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label16, 2);
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label16.Location = new System.Drawing.Point(3, 575);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(769, 21);
            this.label16.TabIndex = 33;
            this.label16.Text = "MODERATION COMMANDS";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(3, 167);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(142, 20);
            this.label11.TabIndex = 23;
            this.label11.Text = "Enabled";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudChatterChance
            // 
            this.nudChatterChance.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.configurationBindingSource, "ChatterChance", true));
            this.nudChatterChance.DecimalPlaces = 2;
            this.nudChatterChance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudChatterChance.Location = new System.Drawing.Point(151, 520);
            this.nudChatterChance.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChatterChance.Name = "nudChatterChance";
            this.nudChatterChance.Size = new System.Drawing.Size(120, 23);
            this.nudChatterChance.TabIndex = 22;
            this.nudChatterChance.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // configurationBindingSource
            // 
            this.configurationBindingSource.DataSource = typeof(CobaltChatCore.Configuration);
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "CommandSignal", true));
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox3.Location = new System.Drawing.Point(151, 120);
            this.textBox3.MaxLength = 1;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(21, 23);
            this.textBox3.TabIndex = 16;
            this.textBox3.Text = "^";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(142, 29);
            this.label8.TabIndex = 15;
            this.label8.Text = "Command Character";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Channel Name*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnAuthorize);
            this.flowLayoutPanel1.Controls.Add(this.lblAutorizationStatus);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(148, 53);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(627, 35);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // btnAuthorize
            // 
            this.btnAuthorize.Location = new System.Drawing.Point(3, 3);
            this.btnAuthorize.Name = "btnAuthorize";
            this.btnAuthorize.Size = new System.Drawing.Size(75, 23);
            this.btnAuthorize.TabIndex = 0;
            this.btnAuthorize.Text = "Authorize";
            this.btnAuthorize.UseVisualStyleBackColor = true;
            this.btnAuthorize.Click += new System.EventHandler(this.btnAuthorize_Click);
            // 
            // lblAutorizationStatus
            // 
            this.lblAutorizationStatus.AutoSize = true;
            this.lblAutorizationStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAutorizationStatus.ForeColor = System.Drawing.Color.Red;
            this.lblAutorizationStatus.Location = new System.Drawing.Point(84, 0);
            this.lblAutorizationStatus.Name = "lblAutorizationStatus";
            this.lblAutorizationStatus.Size = new System.Drawing.Size(104, 29);
            this.lblAutorizationStatus.TabIndex = 1;
            this.lblAutorizationStatus.Text = "NOT AUTHORIZED";
            this.lblAutorizationStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAutorizationStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.lblAutorizationStatus_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 35);
            this.label2.TabIndex = 2;
            this.label2.Text = "Twitch Connection*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label3, 2);
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(769, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "REQUIRED";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 29);
            this.label4.TabIndex = 5;
            this.label4.Text = "Localhost Port*";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.nudPort);
            this.flowLayoutPanel2.Controls.Add(this.lblPortValid);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(148, 88);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(621, 29);
            this.flowLayoutPanel2.TabIndex = 8;
            // 
            // nudPort
            // 
            this.nudPort.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.configurationBindingSource, "Port", true));
            this.nudPort.Location = new System.Drawing.Point(3, 3);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(63, 23);
            this.nudPort.TabIndex = 8;
            this.nudPort.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // lblPortValid
            // 
            this.lblPortValid.AutoSize = true;
            this.lblPortValid.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPortValid.Location = new System.Drawing.Point(72, 0);
            this.lblPortValid.Name = "lblPortValid";
            this.lblPortValid.Size = new System.Drawing.Size(55, 29);
            this.lblPortValid.TabIndex = 7;
            this.lblPortValid.Text = "Available";
            this.lblPortValid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPortValid.Paint += new System.Windows.Forms.PaintEventHandler(this.lblPortValid_Paint);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.tbChannelName);
            this.flowLayoutPanel3.Controls.Add(this.lblChannelNameValid);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(148, 21);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(627, 32);
            this.flowLayoutPanel3.TabIndex = 9;
            // 
            // tbChannelName
            // 
            this.tbChannelName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "ChannelName", true));
            this.tbChannelName.Location = new System.Drawing.Point(3, 3);
            this.tbChannelName.Name = "tbChannelName";
            this.tbChannelName.Size = new System.Drawing.Size(165, 23);
            this.tbChannelName.TabIndex = 0;
            this.tbChannelName.Text = "Kategaruthedangodog";
            this.tbChannelName.Leave += new System.EventHandler(this.tbChannelName_Leave);
            // 
            // lblChannelNameValid
            // 
            this.lblChannelNameValid.AutoSize = true;
            this.lblChannelNameValid.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblChannelNameValid.Location = new System.Drawing.Point(174, 0);
            this.lblChannelNameValid.Name = "lblChannelNameValid";
            this.lblChannelNameValid.Size = new System.Drawing.Size(55, 29);
            this.lblChannelNameValid.TabIndex = 8;
            this.lblChannelNameValid.Text = "Available";
            this.lblChannelNameValid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblChannelNameValid.Paint += new System.Windows.Forms.PaintEventHandler(this.lblChannelNameValid_Paint);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label5, 2);
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(3, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(769, 21);
            this.label5.TabIndex = 10;
            this.label5.Text = "JOIN COMMAND";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "JoinCommand", true));
            this.textBox1.Location = new System.Drawing.Point(151, 190);
            this.textBox1.MaxLength = 50;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(66, 25);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "join";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 187);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 31);
            this.label7.TabIndex = 13;
            this.label7.Text = "Command";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 472);
            this.label10.MaximumSize = new System.Drawing.Size(100, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 45);
            this.label10.TabIndex = 19;
            this.label10.Text = "How many times can a chatter be picked?";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudChatterPickLimit
            // 
            this.nudChatterPickLimit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.configurationBindingSource, "ChatterPickLimit", true));
            this.nudChatterPickLimit.Location = new System.Drawing.Point(151, 475);
            this.nudChatterPickLimit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudChatterPickLimit.Name = "nudChatterPickLimit";
            this.nudChatterPickLimit.Size = new System.Drawing.Size(120, 23);
            this.nudChatterPickLimit.TabIndex = 20;
            this.nudChatterPickLimit.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 517);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(142, 29);
            this.label9.TabIndex = 17;
            this.label9.Text = "Chance to appear (0-1)";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbJoinEnabled
            // 
            this.cbJoinEnabled.AutoSize = true;
            this.cbJoinEnabled.Checked = true;
            this.cbJoinEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbJoinEnabled.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configurationBindingSource, "JoinCommandEnabled", true));
            this.cbJoinEnabled.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbJoinEnabled.Location = new System.Drawing.Point(151, 170);
            this.cbJoinEnabled.Name = "cbJoinEnabled";
            this.cbJoinEnabled.Size = new System.Drawing.Size(15, 14);
            this.cbJoinEnabled.TabIndex = 24;
            this.cbJoinEnabled.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(3, 546);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(142, 29);
            this.label12.TabIndex = 25;
            this.label12.Text = "Allowed encounter types";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.cbNormalBattleType);
            this.flowLayoutPanel4.Controls.Add(this.cbEliteBattleType);
            this.flowLayoutPanel4.Controls.Add(this.cbBossBattleType);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(148, 546);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(627, 29);
            this.flowLayoutPanel4.TabIndex = 26;
            // 
            // cbNormalBattleType
            // 
            this.cbNormalBattleType.AutoSize = true;
            this.cbNormalBattleType.Checked = true;
            this.cbNormalBattleType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNormalBattleType.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbNormalBattleType.Location = new System.Drawing.Point(3, 3);
            this.cbNormalBattleType.Name = "cbNormalBattleType";
            this.cbNormalBattleType.Size = new System.Drawing.Size(66, 19);
            this.cbNormalBattleType.TabIndex = 0;
            this.cbNormalBattleType.Tag = "Normal";
            this.cbNormalBattleType.Text = "Normal";
            this.cbNormalBattleType.UseVisualStyleBackColor = true;
            this.cbNormalBattleType.CheckedChanged += new System.EventHandler(this.cbBattleType_CheckedChanged);
            this.cbNormalBattleType.Paint += new System.Windows.Forms.PaintEventHandler(this.cbBattleType_Paint);
            // 
            // cbEliteBattleType
            // 
            this.cbEliteBattleType.AutoSize = true;
            this.cbEliteBattleType.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbEliteBattleType.Location = new System.Drawing.Point(75, 3);
            this.cbEliteBattleType.Name = "cbEliteBattleType";
            this.cbEliteBattleType.Size = new System.Drawing.Size(48, 19);
            this.cbEliteBattleType.TabIndex = 1;
            this.cbEliteBattleType.Tag = "Elite";
            this.cbEliteBattleType.Text = "Elite";
            this.cbEliteBattleType.UseVisualStyleBackColor = true;
            this.cbEliteBattleType.CheckedChanged += new System.EventHandler(this.cbBattleType_CheckedChanged);
            this.cbEliteBattleType.Paint += new System.Windows.Forms.PaintEventHandler(this.cbBattleType_Paint);
            // 
            // cbBossBattleType
            // 
            this.cbBossBattleType.AutoSize = true;
            this.cbBossBattleType.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbBossBattleType.Location = new System.Drawing.Point(129, 3);
            this.cbBossBattleType.Name = "cbBossBattleType";
            this.cbBossBattleType.Size = new System.Drawing.Size(50, 19);
            this.cbBossBattleType.TabIndex = 2;
            this.cbBossBattleType.Tag = "Boss";
            this.cbBossBattleType.Text = "Boss";
            this.cbBossBattleType.UseVisualStyleBackColor = true;
            this.cbBossBattleType.CheckedChanged += new System.EventHandler(this.cbBattleType_CheckedChanged);
            this.cbBossBattleType.Paint += new System.Windows.Forms.PaintEventHandler(this.cbBattleType_Paint);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 296);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 44);
            this.label6.TabIndex = 11;
            this.label6.Text = "Reminder Text";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "RemindersText", true));
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox2.Location = new System.Drawing.Point(151, 299);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(621, 38);
            this.textBox2.TabIndex = 14;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(3, 340);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(142, 44);
            this.label13.TabIndex = 27;
            this.label13.Text = "Chatter joined text";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Location = new System.Drawing.Point(3, 384);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(142, 44);
            this.label14.TabIndex = 28;
            this.label14.Text = "Queue closed text";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(3, 428);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(142, 44);
            this.label15.TabIndex = 29;
            this.label15.Text = "Chatter banned text";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "JoinFailedQueueClosedText", true));
            this.textBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox4.Location = new System.Drawing.Point(151, 387);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(621, 38);
            this.textBox4.TabIndex = 30;
            this.textBox4.Text = "Type {JoinCommand} to potentially become an enemy in the next fight!";
            // 
            // textBox5
            // 
            this.textBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "ChatterJoinedText", true));
            this.textBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox5.Location = new System.Drawing.Point(151, 343);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(621, 38);
            this.textBox5.TabIndex = 31;
            this.textBox5.Text = "Type {JoinCommand} to potentially become an enemy in the next fight!";
            // 
            // textBox6
            // 
            this.textBox6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "JoinFailedBannedText", true));
            this.textBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox6.Location = new System.Drawing.Point(151, 431);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(621, 38);
            this.textBox6.TabIndex = 32;
            this.textBox6.Text = "Type {JoinCommand} to potentially become an enemy in the next fight!";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Location = new System.Drawing.Point(3, 596);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(142, 31);
            this.label17.TabIndex = 34;
            this.label17.Text = "Clear Queue command";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Location = new System.Drawing.Point(3, 627);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(142, 44);
            this.label18.TabIndex = 35;
            this.label18.Text = "Clear Queue text";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(3, 671);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(142, 31);
            this.label19.TabIndex = 36;
            this.label19.Text = "Close Queue command";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label23.Location = new System.Drawing.Point(3, 702);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(142, 44);
            this.label23.TabIndex = 40;
            this.label23.Text = "Close Queue text";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.Location = new System.Drawing.Point(3, 746);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(142, 31);
            this.label21.TabIndex = 38;
            this.label21.Text = "Open Queue command";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label22.Location = new System.Drawing.Point(3, 777);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(142, 44);
            this.label22.TabIndex = 39;
            this.label22.Text = "Open Queue text";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Location = new System.Drawing.Point(3, 821);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(142, 31);
            this.label20.TabIndex = 37;
            this.label20.Text = "Eject Chatter command";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label25.Location = new System.Drawing.Point(3, 852);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(142, 44);
            this.label25.TabIndex = 42;
            this.label25.Text = "Eject Chatter text";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label26.Location = new System.Drawing.Point(3, 927);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(142, 44);
            this.label26.TabIndex = 43;
            this.label26.Text = "Ban Chatter text";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label27.Location = new System.Drawing.Point(3, 971);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(142, 31);
            this.label27.TabIndex = 44;
            this.label27.Text = "Unban Chatter command";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label28.Location = new System.Drawing.Point(3, 1002);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(142, 44);
            this.label28.TabIndex = 45;
            this.label28.Text = "Unban Chatter text";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label29
            // 
            this.label29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label29.Location = new System.Drawing.Point(3, 218);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(142, 40);
            this.label29.TabIndex = 46;
            this.label29.Text = "Allow chatter profile pictures as enemies";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label30.Location = new System.Drawing.Point(3, 258);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(142, 38);
            this.label30.TabIndex = 47;
            this.label30.Text = "Allow chatters to speak as enemies";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configurationBindingSource, "AllowChatterPicturesAsEnemies", true));
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox1.Location = new System.Drawing.Point(151, 221);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 34);
            this.checkBox1.TabIndex = 48;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.configurationBindingSource, "AllowChatterShoutsAsEnemies", true));
            this.checkBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox2.Location = new System.Drawing.Point(151, 261);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 32);
            this.checkBox2.TabIndex = 49;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // textBox7
            // 
            this.textBox7.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "ChatterListClearCommand", true));
            this.textBox7.Location = new System.Drawing.Point(151, 599);
            this.textBox7.MaxLength = 50;
            this.textBox7.Multiline = true;
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(66, 25);
            this.textBox7.TabIndex = 50;
            this.textBox7.Text = "join";
            // 
            // textBox8
            // 
            this.textBox8.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "CloseQueueCommand", true));
            this.textBox8.Location = new System.Drawing.Point(151, 674);
            this.textBox8.MaxLength = 50;
            this.textBox8.Multiline = true;
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(66, 25);
            this.textBox8.TabIndex = 51;
            this.textBox8.Text = "join";
            // 
            // textBox9
            // 
            this.textBox9.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "OpenQueueCommand", true));
            this.textBox9.Location = new System.Drawing.Point(151, 749);
            this.textBox9.MaxLength = 50;
            this.textBox9.Multiline = true;
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(66, 25);
            this.textBox9.TabIndex = 52;
            this.textBox9.Text = "join";
            // 
            // textBox10
            // 
            this.textBox10.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "ChatterEjectCommand", true));
            this.textBox10.Location = new System.Drawing.Point(151, 824);
            this.textBox10.MaxLength = 50;
            this.textBox10.Multiline = true;
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(66, 25);
            this.textBox10.TabIndex = 53;
            this.textBox10.Text = "join";
            // 
            // textBox11
            // 
            this.textBox11.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "ChatterBanCommand", true));
            this.textBox11.Location = new System.Drawing.Point(151, 899);
            this.textBox11.MaxLength = 50;
            this.textBox11.Multiline = true;
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(66, 25);
            this.textBox11.TabIndex = 54;
            this.textBox11.Text = "join";
            // 
            // textBox12
            // 
            this.textBox12.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "ChatterUnbanCommand", true));
            this.textBox12.Location = new System.Drawing.Point(151, 974);
            this.textBox12.MaxLength = 50;
            this.textBox12.Multiline = true;
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(66, 25);
            this.textBox12.TabIndex = 55;
            this.textBox12.Text = "join";
            // 
            // textBox13
            // 
            this.textBox13.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "QueueClearedText", true));
            this.textBox13.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox13.Location = new System.Drawing.Point(151, 630);
            this.textBox13.Multiline = true;
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(621, 38);
            this.textBox13.TabIndex = 56;
            // 
            // textBox14
            // 
            this.textBox14.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "QueueClosedText", true));
            this.textBox14.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox14.Location = new System.Drawing.Point(151, 705);
            this.textBox14.Multiline = true;
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(621, 38);
            this.textBox14.TabIndex = 57;
            // 
            // textBox15
            // 
            this.textBox15.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "QueueOpenText", true));
            this.textBox15.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox15.Location = new System.Drawing.Point(151, 780);
            this.textBox15.Multiline = true;
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(621, 38);
            this.textBox15.TabIndex = 58;
            // 
            // textBox16
            // 
            this.textBox16.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "ChatterEjectText", true));
            this.textBox16.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox16.Location = new System.Drawing.Point(151, 855);
            this.textBox16.Multiline = true;
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(621, 38);
            this.textBox16.TabIndex = 59;
            // 
            // textBox17
            // 
            this.textBox17.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "ChatterBanText", true));
            this.textBox17.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox17.Location = new System.Drawing.Point(151, 930);
            this.textBox17.Multiline = true;
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(621, 38);
            this.textBox17.TabIndex = 60;
            // 
            // textBox18
            // 
            this.textBox18.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationBindingSource, "ChatterUnbanText", true));
            this.textBox18.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox18.Location = new System.Drawing.Point(151, 1005);
            this.textBox18.Multiline = true;
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(621, 38);
            this.textBox18.TabIndex = 61;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 485);
            this.Controls.Add(this.MainTabControl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.MainTabControl.ResumeLayout(false);
            this.tpCCC.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChatterChance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.configurationBindingSource)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChatterPickLimit)).EndInit();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public TabControl MainTabControl;
        private TabPage tpCCC;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox tbChannelName;
        private BindingSource configurationBindingSource;
        private Label label1;
        private Label label2;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label lblAutorizationStatus;
        private Button btnAuthorize;
        private Label label3;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label lblPortValid;
        private FlowLayoutPanel flowLayoutPanel3;
        private Label lblChannelNameValid;
        private Label label5;
        private Label label9;
        private TextBox textBox3;
        private Label label8;
        private Label label4;
        private Label label6;
        private Label label7;
        private TextBox textBox2;
        private Label label10;
        private NumericUpDown nudPort;
        private NumericUpDown nudChatterPickLimit;
        private NumericUpDown nudChatterChance;
        private Label label11;
        private CheckBox cbJoinEnabled;
        private Label label12;
        private FlowLayoutPanel flowLayoutPanel4;
        private CheckBox cbNormalBattleType;
        private CheckBox cbEliteBattleType;
        private CheckBox cbBossBattleType;
        private Label label13;
        private Label label14;
        private Label label15;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private Label label24;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label23;
        private Label label21;
        private Label label22;
        private Label label20;
        private Label label25;
        private Label label26;
        private Label label27;
        private Label label28;
        private Label label29;
        private Label label30;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private TextBox textBox1;
        private TextBox textBox7;
        private TextBox textBox8;
        private TextBox textBox9;
        private TextBox textBox10;
        private TextBox textBox11;
        private TextBox textBox12;
        private TextBox textBox13;
        private TextBox textBox14;
        private TextBox textBox15;
        private TextBox textBox16;
        private TextBox textBox17;
        private TextBox textBox18;
    }
}
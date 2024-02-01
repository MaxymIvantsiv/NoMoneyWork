namespace OctoManager
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.treeViewModels = new System.Windows.Forms.TreeView();
            this.checkedListBoxProfiles = new System.Windows.Forms.CheckedListBox();
            this.labelCurrentModel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBoxContent = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxContentPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonUpdatePath = new System.Windows.Forms.Button();
            this.openFileDialogGetContentDirectory = new System.Windows.Forms.OpenFileDialog();
            this.buttonEditName = new System.Windows.Forms.Button();
            this.textBoxEditName = new System.Windows.Forms.TextBox();
            this.labelEditName = new System.Windows.Forms.Label();
            this.buttonEditNameSubmit = new System.Windows.Forms.Button();
            this.buttonDeleteModel = new System.Windows.Forms.Button();
            this.buttonAddNewModel = new System.Windows.Forms.Button();
            this.buttonRunModel = new System.Windows.Forms.Button();
            this.checkedListBoxTags = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.checkedListBoxSubTags = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.timerContentCheck = new System.Windows.Forms.Timer(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPostCount = new System.Windows.Forms.TextBox();
            this.textBoxPausePerPost = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelPostTotalCount = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.labelPostCreated = new System.Windows.Forms.Label();
            this.labelPostNotCreated = new System.Windows.Forms.Label();
            this.comboBoxCopyModel = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.labelStatus = new System.Windows.Forms.Label();
            this.timerButtonActivate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewModels
            // 
            this.treeViewModels.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.treeViewModels.Location = new System.Drawing.Point(12, 36);
            this.treeViewModels.Name = "treeViewModels";
            this.treeViewModels.Size = new System.Drawing.Size(135, 402);
            this.treeViewModels.TabIndex = 0;
            this.treeViewModels.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // checkedListBoxProfiles
            // 
            this.checkedListBoxProfiles.FormattingEnabled = true;
            this.checkedListBoxProfiles.HorizontalScrollbar = true;
            this.checkedListBoxProfiles.Location = new System.Drawing.Point(321, 75);
            this.checkedListBoxProfiles.Name = "checkedListBoxProfiles";
            this.checkedListBoxProfiles.Size = new System.Drawing.Size(106, 334);
            this.checkedListBoxProfiles.TabIndex = 1;
            this.checkedListBoxProfiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxProfiles_ItemCheck);
            this.checkedListBoxProfiles.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxProfiles_SelectedIndexChanged);
            // 
            // labelCurrentModel
            // 
            this.labelCurrentModel.AutoSize = true;
            this.labelCurrentModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCurrentModel.Location = new System.Drawing.Point(9, 20);
            this.labelCurrentModel.Name = "labelCurrentModel";
            this.labelCurrentModel.Size = new System.Drawing.Size(92, 13);
            this.labelCurrentModel.TabIndex = 2;
            this.labelCurrentModel.Text = "Виберіть модель";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(318, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Антидетект профілі";
            // 
            // checkedListBoxContent
            // 
            this.checkedListBoxContent.FormattingEnabled = true;
            this.checkedListBoxContent.HorizontalScrollbar = true;
            this.checkedListBoxContent.Location = new System.Drawing.Point(442, 75);
            this.checkedListBoxContent.Name = "checkedListBoxContent";
            this.checkedListBoxContent.Size = new System.Drawing.Size(148, 334);
            this.checkedListBoxContent.TabIndex = 5;
            this.checkedListBoxContent.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxContent_ItemCheck);
            this.checkedListBoxContent.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxContent_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(439, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Контент";
            // 
            // textBoxContentPath
            // 
            this.textBoxContentPath.Location = new System.Drawing.Point(321, 25);
            this.textBoxContentPath.Name = "textBoxContentPath";
            this.textBoxContentPath.Size = new System.Drawing.Size(560, 20);
            this.textBoxContentPath.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(318, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Шлях до папки з контентом";
            // 
            // buttonUpdatePath
            // 
            this.buttonUpdatePath.Location = new System.Drawing.Point(806, 51);
            this.buttonUpdatePath.Name = "buttonUpdatePath";
            this.buttonUpdatePath.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdatePath.TabIndex = 12;
            this.buttonUpdatePath.Text = "Вибрати";
            this.buttonUpdatePath.UseVisualStyleBackColor = true;
            this.buttonUpdatePath.Click += new System.EventHandler(this.buttonUpdatePath_Click);
            // 
            // openFileDialogGetContentDirectory
            // 
            this.openFileDialogGetContentDirectory.FileName = "openFileDialog1";
            // 
            // buttonEditName
            // 
            this.buttonEditName.Location = new System.Drawing.Point(156, 125);
            this.buttonEditName.Name = "buttonEditName";
            this.buttonEditName.Size = new System.Drawing.Size(139, 23);
            this.buttonEditName.TabIndex = 13;
            this.buttonEditName.Text = "Редагувати модель";
            this.buttonEditName.UseVisualStyleBackColor = true;
            this.buttonEditName.Click += new System.EventHandler(this.buttonEditName_Click);
            // 
            // textBoxEditName
            // 
            this.textBoxEditName.Location = new System.Drawing.Point(156, 195);
            this.textBoxEditName.Name = "textBoxEditName";
            this.textBoxEditName.Size = new System.Drawing.Size(139, 20);
            this.textBoxEditName.TabIndex = 14;
            // 
            // labelEditName
            // 
            this.labelEditName.AutoSize = true;
            this.labelEditName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelEditName.Location = new System.Drawing.Point(153, 179);
            this.labelEditName.Name = "labelEditName";
            this.labelEditName.Size = new System.Drawing.Size(66, 13);
            this.labelEditName.TabIndex = 15;
            this.labelEditName.Text = "Введіть ім\'я";
            // 
            // buttonEditNameSubmit
            // 
            this.buttonEditNameSubmit.Location = new System.Drawing.Point(220, 300);
            this.buttonEditNameSubmit.Name = "buttonEditNameSubmit";
            this.buttonEditNameSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonEditNameSubmit.TabIndex = 16;
            this.buttonEditNameSubmit.Text = "Прийняти";
            this.buttonEditNameSubmit.UseVisualStyleBackColor = true;
            this.buttonEditNameSubmit.Click += new System.EventHandler(this.buttonEditNameSubmit_Click);
            // 
            // buttonDeleteModel
            // 
            this.buttonDeleteModel.Location = new System.Drawing.Point(156, 154);
            this.buttonDeleteModel.Name = "buttonDeleteModel";
            this.buttonDeleteModel.Size = new System.Drawing.Size(139, 23);
            this.buttonDeleteModel.TabIndex = 18;
            this.buttonDeleteModel.Text = "Видалити модель";
            this.buttonDeleteModel.UseVisualStyleBackColor = true;
            this.buttonDeleteModel.Click += new System.EventHandler(this.buttonDeleteModel_Click);
            // 
            // buttonAddNewModel
            // 
            this.buttonAddNewModel.Location = new System.Drawing.Point(156, 96);
            this.buttonAddNewModel.Name = "buttonAddNewModel";
            this.buttonAddNewModel.Size = new System.Drawing.Size(139, 23);
            this.buttonAddNewModel.TabIndex = 19;
            this.buttonAddNewModel.Text = "Додати модель";
            this.buttonAddNewModel.UseVisualStyleBackColor = true;
            this.buttonAddNewModel.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonRunModel
            // 
            this.buttonRunModel.BackColor = System.Drawing.Color.Lime;
            this.buttonRunModel.Location = new System.Drawing.Point(156, 36);
            this.buttonRunModel.Name = "buttonRunModel";
            this.buttonRunModel.Size = new System.Drawing.Size(139, 23);
            this.buttonRunModel.TabIndex = 20;
            this.buttonRunModel.Text = "Запустити модель";
            this.buttonRunModel.UseVisualStyleBackColor = false;
            this.buttonRunModel.Click += new System.EventHandler(this.buttonRunModel_Click);
            // 
            // checkedListBoxTags
            // 
            this.checkedListBoxTags.FormattingEnabled = true;
            this.checkedListBoxTags.HorizontalScrollbar = true;
            this.checkedListBoxTags.Location = new System.Drawing.Point(887, 75);
            this.checkedListBoxTags.Name = "checkedListBoxTags";
            this.checkedListBoxTags.Size = new System.Drawing.Size(148, 364);
            this.checkedListBoxTags.TabIndex = 21;
            this.checkedListBoxTags.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxTags_ItemCheck);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(884, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Теги";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 445);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(872, 113);
            this.richTextBox1.TabIndex = 27;
            this.richTextBox1.Text = "";
            // 
            // checkedListBoxSubTags
            // 
            this.checkedListBoxSubTags.FormattingEnabled = true;
            this.checkedListBoxSubTags.HorizontalScrollbar = true;
            this.checkedListBoxSubTags.Location = new System.Drawing.Point(1041, 75);
            this.checkedListBoxSubTags.Name = "checkedListBoxSubTags";
            this.checkedListBoxSubTags.Size = new System.Drawing.Size(148, 364);
            this.checkedListBoxSubTags.TabIndex = 28;
            this.checkedListBoxSubTags.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxSubTags_ItemCheck);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(1038, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "ПідТеги";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(156, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 23);
            this.button1.TabIndex = 30;
            this.button1.Text = "Зупинити модель";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // timerContentCheck
            // 
            this.timerContentCheck.Enabled = true;
            this.timerContentCheck.Interval = 2000;
            this.timerContentCheck.Tick += new System.EventHandler(this.timerContentCheck_Tick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(897, 20);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(292, 31);
            this.button3.TabIndex = 35;
            this.button3.Text = "Оновити інформацію з таблиці";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(596, 321);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "Name";
            // 
            // textBoxPostCount
            // 
            this.textBoxPostCount.Location = new System.Drawing.Point(599, 363);
            this.textBoxPostCount.Name = "textBoxPostCount";
            this.textBoxPostCount.Size = new System.Drawing.Size(100, 20);
            this.textBoxPostCount.TabIndex = 37;
            this.textBoxPostCount.Text = "1";
            // 
            // textBoxPausePerPost
            // 
            this.textBoxPausePerPost.Location = new System.Drawing.Point(599, 402);
            this.textBoxPausePerPost.Name = "textBoxPausePerPost";
            this.textBoxPausePerPost.Size = new System.Drawing.Size(100, 20);
            this.textBoxPausePerPost.TabIndex = 38;
            this.textBoxPausePerPost.Text = "60";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(596, 347);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 39;
            this.label7.Text = "Кількість постів";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(596, 386);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 40;
            this.label8.Text = "Пауза між ними";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(705, 363);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 59);
            this.button2.TabIndex = 41;
            this.button2.Text = "Встановити";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(153, 218);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 13);
            this.label9.TabIndex = 43;
            this.label9.Text = "Введіть id гугл таблиці";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(156, 234);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(139, 20);
            this.textBox1.TabIndex = 42;
            this.textBox1.Text = "15ClKtx6a-l6AJ3VTgodP67CI7WTZJcs4bKs3fN-MuAU";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // labelPostTotalCount
            // 
            this.labelPostTotalCount.AutoSize = true;
            this.labelPostTotalCount.Location = new System.Drawing.Point(890, 445);
            this.labelPostTotalCount.Name = "labelPostTotalCount";
            this.labelPostTotalCount.Size = new System.Drawing.Size(203, 13);
            this.labelPostTotalCount.TabIndex = 44;
            this.labelPostTotalCount.Text = "Загальна кількість можливих постів: 0";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(321, 415);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(106, 23);
            this.button4.TabIndex = 45;
            this.button4.Text = "Оновити";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // labelPostCreated
            // 
            this.labelPostCreated.AutoSize = true;
            this.labelPostCreated.Location = new System.Drawing.Point(890, 467);
            this.labelPostCreated.Name = "labelPostCreated";
            this.labelPostCreated.Size = new System.Drawing.Size(101, 13);
            this.labelPostCreated.TabIndex = 46;
            this.labelPostCreated.Text = "Створено постів: 0";
            // 
            // labelPostNotCreated
            // 
            this.labelPostNotCreated.AutoSize = true;
            this.labelPostNotCreated.Location = new System.Drawing.Point(890, 490);
            this.labelPostNotCreated.Name = "labelPostNotCreated";
            this.labelPostNotCreated.Size = new System.Drawing.Size(161, 13);
            this.labelPostNotCreated.TabIndex = 47;
            this.labelPostNotCreated.Text = "Не вдалося створити постів: 0";
            // 
            // comboBoxCopyModel
            // 
            this.comboBoxCopyModel.FormattingEnabled = true;
            this.comboBoxCopyModel.Location = new System.Drawing.Point(156, 273);
            this.comboBoxCopyModel.Name = "comboBoxCopyModel";
            this.comboBoxCopyModel.Size = new System.Drawing.Size(139, 21);
            this.comboBoxCopyModel.TabIndex = 48;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(153, 257);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 13);
            this.label10.TabIndex = 49;
            this.label10.Text = "Копіювати з";
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(596, 75);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(285, 243);
            this.axWindowsMediaPlayer1.TabIndex = 17;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(890, 512);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(44, 13);
            this.labelStatus.TabIndex = 50;
            this.labelStatus.Text = "Статус:";
            // 
            // timerButtonActivate
            // 
            this.timerButtonActivate.Interval = 5000;
            this.timerButtonActivate.Tick += new System.EventHandler(this.timerButtonActivate_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 562);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboBoxCopyModel);
            this.Controls.Add(this.labelPostNotCreated);
            this.Controls.Add(this.labelPostCreated);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.labelPostTotalCount);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxPausePerPost);
            this.Controls.Add(this.textBoxPostCount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkedListBoxSubTags);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkedListBoxTags);
            this.Controls.Add(this.buttonRunModel);
            this.Controls.Add(this.buttonAddNewModel);
            this.Controls.Add(this.buttonDeleteModel);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Controls.Add(this.buttonEditNameSubmit);
            this.Controls.Add(this.labelEditName);
            this.Controls.Add(this.textBoxEditName);
            this.Controls.Add(this.buttonEditName);
            this.Controls.Add(this.buttonUpdatePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxContentPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkedListBoxContent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelCurrentModel);
            this.Controls.Add(this.checkedListBoxProfiles);
            this.Controls.Add(this.treeViewModels);
            this.Name = "Form1";
            this.Text = "RedditModelEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewModels;
        private System.Windows.Forms.CheckedListBox checkedListBoxProfiles;
        private System.Windows.Forms.Label labelCurrentModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBoxContent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxContentPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonUpdatePath;
        private System.Windows.Forms.OpenFileDialog openFileDialogGetContentDirectory;
        private System.Windows.Forms.Button buttonEditName;
        private System.Windows.Forms.TextBox textBoxEditName;
        private System.Windows.Forms.Label labelEditName;
        private System.Windows.Forms.Button buttonEditNameSubmit;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Button buttonDeleteModel;
        private System.Windows.Forms.Button buttonAddNewModel;
        private System.Windows.Forms.Button buttonRunModel;
        private System.Windows.Forms.CheckedListBox checkedListBoxTags;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckedListBox checkedListBoxSubTags;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timerContentCheck;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPostCount;
        private System.Windows.Forms.TextBox textBoxPausePerPost;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelPostTotalCount;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label labelPostCreated;
        private System.Windows.Forms.Label labelPostNotCreated;
        private System.Windows.Forms.ComboBox comboBoxCopyModel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Timer timerButtonActivate;
    }
}


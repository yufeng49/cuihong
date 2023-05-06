
namespace MiGuoPacking
{
    partial class Frm_SearchTask
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.billNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.batchCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productBasicName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productionNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateList = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.delFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plantCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plantName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productBasicCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productBasicId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sendStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updateBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workshopCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workshopName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(126, 323);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 26);
            this.dateTimePicker1.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(653, 330);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 16);
            this.label8.TabIndex = 25;
            this.label8.Text = ":";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(566, 330);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 16);
            this.label7.TabIndex = 24;
            this.label7.Text = ":";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(675, 327);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(59, 26);
            this.textBox6.TabIndex = 23;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(588, 327);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(59, 26);
            this.textBox5.TabIndex = 22;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(501, 327);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(59, 26);
            this.textBox4.TabIndex = 21;
            this.textBox4.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(423, 330);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 20;
            this.label6.Text = "包装比例";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 330);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 19;
            this.label5.Text = "生产日期";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(816, 269);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "任务列表";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(191, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(369, 26);
            this.textBox1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(602, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.billNo,
            this.batchCode,
            this.productName,
            this.productBasicName,
            this.productionNumber,
            this.taskNumber,
            this.createBy,
            this.createTime,
            this.dateList,
            this.dateTime,
            this.delFlag,
            this.id,
            this.plantCode,
            this.plantName,
            this.productBasicCode,
            this.productBasicId,
            this.productCode,
            this.productId,
            this.remark,
            this.sendStatus,
            this.taskStatus,
            this.updateBy,
            this.updateTime,
            this.workshopCode,
            this.workshopName});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(3, 94);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(810, 172);
            this.dataGridView1.TabIndex = 0;
            // 
            // billNo
            // 
            this.billNo.DataPropertyName = "billNo";
            this.billNo.HeaderText = "单据号";
            this.billNo.Name = "billNo";
            this.billNo.ReadOnly = true;
            this.billNo.Width = 140;
            // 
            // batchCode
            // 
            this.batchCode.DataPropertyName = "batchCode";
            this.batchCode.HeaderText = "批次号";
            this.batchCode.Name = "batchCode";
            this.batchCode.ReadOnly = true;
            this.batchCode.Width = 180;
            // 
            // productName
            // 
            this.productName.DataPropertyName = "productName";
            this.productName.HeaderText = "产品名称";
            this.productName.Name = "productName";
            this.productName.ReadOnly = true;
            this.productName.Width = 200;
            // 
            // productBasicName
            // 
            this.productBasicName.DataPropertyName = "productBasicName";
            this.productBasicName.HeaderText = "规格";
            this.productBasicName.Name = "productBasicName";
            this.productBasicName.ReadOnly = true;
            // 
            // productionNumber
            // 
            this.productionNumber.DataPropertyName = "productionNumber";
            this.productionNumber.HeaderText = "已生产数量";
            this.productionNumber.Name = "productionNumber";
            this.productionNumber.ReadOnly = true;
            this.productionNumber.Visible = false;
            // 
            // taskNumber
            // 
            this.taskNumber.DataPropertyName = "taskNumber";
            this.taskNumber.HeaderText = "任务数量\t";
            this.taskNumber.Name = "taskNumber";
            this.taskNumber.ReadOnly = true;
            // 
            // createBy
            // 
            this.createBy.DataPropertyName = "createBy";
            this.createBy.HeaderText = "createBy";
            this.createBy.Name = "createBy";
            this.createBy.ReadOnly = true;
            this.createBy.Visible = false;
            // 
            // createTime
            // 
            this.createTime.DataPropertyName = "createTime";
            this.createTime.HeaderText = "createTime";
            this.createTime.Name = "createTime";
            this.createTime.ReadOnly = true;
            this.createTime.Visible = false;
            // 
            // dateList
            // 
            this.dateList.DataPropertyName = "dateList";
            this.dateList.HeaderText = "dateList";
            this.dateList.Name = "dateList";
            this.dateList.ReadOnly = true;
            this.dateList.Visible = false;
            // 
            // dateTime
            // 
            this.dateTime.DataPropertyName = "dateTime";
            this.dateTime.HeaderText = "dateTime";
            this.dateTime.Name = "dateTime";
            this.dateTime.ReadOnly = true;
            this.dateTime.Visible = false;
            // 
            // delFlag
            // 
            this.delFlag.DataPropertyName = "delFlag";
            this.delFlag.HeaderText = "delFlag";
            this.delFlag.Name = "delFlag";
            this.delFlag.ReadOnly = true;
            this.delFlag.Visible = false;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // plantCode
            // 
            this.plantCode.DataPropertyName = "plantCode";
            this.plantCode.HeaderText = "plantCode";
            this.plantCode.Name = "plantCode";
            this.plantCode.ReadOnly = true;
            this.plantCode.Visible = false;
            // 
            // plantName
            // 
            this.plantName.DataPropertyName = "plantName";
            this.plantName.HeaderText = "plantName";
            this.plantName.Name = "plantName";
            this.plantName.ReadOnly = true;
            this.plantName.Visible = false;
            // 
            // productBasicCode
            // 
            this.productBasicCode.DataPropertyName = "productBasicCode";
            this.productBasicCode.HeaderText = "productBasicCode";
            this.productBasicCode.Name = "productBasicCode";
            this.productBasicCode.ReadOnly = true;
            this.productBasicCode.Visible = false;
            // 
            // productBasicId
            // 
            this.productBasicId.DataPropertyName = "productBasicId";
            this.productBasicId.HeaderText = "productBasicId";
            this.productBasicId.Name = "productBasicId";
            this.productBasicId.ReadOnly = true;
            this.productBasicId.Visible = false;
            // 
            // productCode
            // 
            this.productCode.DataPropertyName = "productCode";
            this.productCode.HeaderText = "productCode";
            this.productCode.Name = "productCode";
            this.productCode.ReadOnly = true;
            this.productCode.Visible = false;
            // 
            // productId
            // 
            this.productId.DataPropertyName = "productId";
            this.productId.HeaderText = "productId";
            this.productId.Name = "productId";
            this.productId.ReadOnly = true;
            this.productId.Visible = false;
            // 
            // remark
            // 
            this.remark.DataPropertyName = "remark";
            this.remark.HeaderText = "remark";
            this.remark.Name = "remark";
            this.remark.ReadOnly = true;
            this.remark.Visible = false;
            // 
            // sendStatus
            // 
            this.sendStatus.DataPropertyName = "sendStatus";
            this.sendStatus.HeaderText = "sendStatus";
            this.sendStatus.Name = "sendStatus";
            this.sendStatus.ReadOnly = true;
            this.sendStatus.Visible = false;
            // 
            // taskStatus
            // 
            this.taskStatus.DataPropertyName = "taskStatus";
            this.taskStatus.HeaderText = "taskStatus";
            this.taskStatus.Name = "taskStatus";
            this.taskStatus.ReadOnly = true;
            this.taskStatus.Visible = false;
            // 
            // updateBy
            // 
            this.updateBy.DataPropertyName = "updateBy";
            this.updateBy.HeaderText = "updateBy";
            this.updateBy.Name = "updateBy";
            this.updateBy.ReadOnly = true;
            this.updateBy.Visible = false;
            // 
            // updateTime
            // 
            this.updateTime.DataPropertyName = "updateTime";
            this.updateTime.HeaderText = "updateTime";
            this.updateTime.Name = "updateTime";
            this.updateTime.ReadOnly = true;
            this.updateTime.Visible = false;
            // 
            // workshopCode
            // 
            this.workshopCode.DataPropertyName = "workshopCode";
            this.workshopCode.HeaderText = "workshopCode";
            this.workshopCode.Name = "workshopCode";
            this.workshopCode.ReadOnly = true;
            this.workshopCode.Visible = false;
            // 
            // workshopName
            // 
            this.workshopName.DataPropertyName = "workshopName";
            this.workshopName.HeaderText = "workshopName";
            this.workshopName.Name = "workshopName";
            this.workshopName.ReadOnly = true;
            this.workshopName.Visible = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(653, 433);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 28);
            this.button2.TabIndex = 3;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Frm_SearchTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 524);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Frm_SearchTask";
            this.Load += new System.EventHandler(this.Frm_SearchTask_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn billNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn batchCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn productName;
        private System.Windows.Forms.DataGridViewTextBoxColumn productBasicName;
        private System.Windows.Forms.DataGridViewTextBoxColumn productionNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn createBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn createTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn delFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn plantCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn plantName;
        private System.Windows.Forms.DataGridViewTextBoxColumn productBasicCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn productBasicId;
        private System.Windows.Forms.DataGridViewTextBoxColumn productCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn productId;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn sendStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn updateBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn updateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn workshopCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn workshopName;
    }
}
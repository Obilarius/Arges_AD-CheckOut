namespace CheckOut
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.txt_userSearch = new System.Windows.Forms.TextBox();
            this.btn_checkout = new System.Windows.Forms.Button();
            this.txt_kuerzel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_austrittsdatum = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 43);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(342, 472);
            this.listBox1.TabIndex = 1;
            // 
            // txt_userSearch
            // 
            this.txt_userSearch.Location = new System.Drawing.Point(12, 13);
            this.txt_userSearch.Name = "txt_userSearch";
            this.txt_userSearch.Size = new System.Drawing.Size(342, 20);
            this.txt_userSearch.TabIndex = 2;
            this.txt_userSearch.TextChanged += new System.EventHandler(this.txt_userSearch_TextChanged);
            this.txt_userSearch.Enter += new System.EventHandler(this.txt_userSearch_Enter);
            this.txt_userSearch.Leave += new System.EventHandler(this.txt_userSearch_Leave);
            // 
            // btn_checkout
            // 
            this.btn_checkout.Location = new System.Drawing.Point(12, 576);
            this.btn_checkout.Name = "btn_checkout";
            this.btn_checkout.Size = new System.Drawing.Size(342, 43);
            this.btn_checkout.TabIndex = 3;
            this.btn_checkout.Text = "CHECKOUT";
            this.btn_checkout.UseVisualStyleBackColor = true;
            this.btn_checkout.Click += new System.EventHandler(this.btn_checkout_Click);
            // 
            // txt_kuerzel
            // 
            this.txt_kuerzel.Location = new System.Drawing.Point(12, 550);
            this.txt_kuerzel.Name = "txt_kuerzel";
            this.txt_kuerzel.Size = new System.Drawing.Size(127, 20);
            this.txt_kuerzel.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 534);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Eigenes Kürzel: *";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(226, 534);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Austrittsdatum: *";
            // 
            // txt_austrittsdatum
            // 
            this.txt_austrittsdatum.Location = new System.Drawing.Point(227, 550);
            this.txt_austrittsdatum.Name = "txt_austrittsdatum";
            this.txt_austrittsdatum.Size = new System.Drawing.Size(127, 20);
            this.txt_austrittsdatum.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 631);
            this.Controls.Add(this.txt_austrittsdatum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_kuerzel);
            this.Controls.Add(this.btn_checkout);
            this.Controls.Add(this.txt_userSearch);
            this.Controls.Add(this.listBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "CheckOut";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox txt_userSearch;
        private System.Windows.Forms.Button btn_checkout;
        private System.Windows.Forms.TextBox txt_kuerzel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_austrittsdatum;
    }
}


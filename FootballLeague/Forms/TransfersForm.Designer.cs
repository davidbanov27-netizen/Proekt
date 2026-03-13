namespace FootballLeague.Forms
{
    partial class TransfersForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Controls ──────────────────────────────────────────────────────────────
        private DataGridView  dgvTransfers     = null!;
        private Panel         panelFilters     = null!;
        private Panel         panelForm        = null!;
        private Label         lblTitle         = null!;
        private Label         lblStatus        = null!;

        // Филтри
        private Label         lblFPlayer       = null!;
        private ComboBox      cboFilterPlayer  = null!;
        private Label         lblFClub         = null!;
        private ComboBox      cboFilterClub    = null!;
        private CheckBox      chkDateFilter    = null!;
        private DateTimePicker dtpFrom         = null!;
        private Label         lblTo            = null!;
        private DateTimePicker dtpTo           = null!;
        private Button        btnRefresh       = null!;
        private Button        btnClearFilters  = null!;

        // CRUD форма
        private Label         lblPlayer        = null!;
        private ComboBox      cboPlayer        = null!;
        private Label         lblFromClub      = null!;
        private TextBox       txtFromClub      = null!;
        private Label         lblToClub        = null!;
        private ComboBox      cboToClub        = null!;
        private Label         lblDate          = null!;
        private DateTimePicker dtpTransferDate = null!;
        private Label         lblFee           = null!;
        private NumericUpDown numFee           = null!;
        private Label         lblNote          = null!;
        private TextBox       txtNote          = null!;
        private Button        btnTransfer      = null!;
        private Button        btnClear         = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components      = new System.ComponentModel.Container();
            dgvTransfers    = new DataGridView();
            panelFilters    = new Panel();
            panelForm       = new Panel();
            lblTitle        = new Label();
            lblStatus       = new Label();
            lblFPlayer      = new Label();
            cboFilterPlayer = new ComboBox();
            lblFClub        = new Label();
            cboFilterClub   = new ComboBox();
            chkDateFilter   = new CheckBox();
            dtpFrom         = new DateTimePicker();
            lblTo           = new Label();
            dtpTo           = new DateTimePicker();
            btnRefresh      = new Button();
            btnClearFilters = new Button();
            lblPlayer       = new Label();
            cboPlayer       = new ComboBox();
            lblFromClub     = new Label();
            txtFromClub     = new TextBox();
            lblToClub       = new Label();
            cboToClub       = new ComboBox();
            lblDate         = new Label();
            dtpTransferDate = new DateTimePicker();
            lblFee          = new Label();
            numFee          = new NumericUpDown();
            lblNote         = new Label();
            txtNote         = new TextBox();
            btnTransfer     = new Button();
            btnClear        = new Button();

            this.SuspendLayout();

            // ── Форма ────────────────────────────────────────────────────────────
            this.Text          = "Трансфери – Football League";
            this.Size          = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize   = new Size(1000, 640);
            this.BackColor     = Color.FromArgb(240, 244, 248);
            this.Font          = new Font("Segoe UI", 9.5f);
            this.Load         += TransfersForm_Load;

            // ── Заглавие ─────────────────────────────────────────────────────────
            lblTitle.Text      = "🔄  История на трансферите";
            lblTitle.Font      = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 78, 121);
            lblTitle.Location  = new Point(12, 12);
            lblTitle.AutoSize  = true;

            // ── Панел с филтри ────────────────────────────────────────────────────
            panelFilters.Location  = new Point(12, 48);
            panelFilters.Size      = new Size(760, 50);
            panelFilters.BackColor = Color.White;

            lblFPlayer.Text     = "Играч:";
            lblFPlayer.Location = new Point(8, 16);
            lblFPlayer.AutoSize = true;

            cboFilterPlayer.Location      = new Point(55, 12);
            cboFilterPlayer.Size          = new Size(160, 26);
            cboFilterPlayer.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFilterPlayer.SelectedIndexChanged += cboFilterPlayer_SelectedIndexChanged;

            lblFClub.Text     = "Клуб:";
            lblFClub.Location = new Point(225, 16);
            lblFClub.AutoSize = true;

            cboFilterClub.Location      = new Point(265, 12);
            cboFilterClub.Size          = new Size(150, 26);
            cboFilterClub.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFilterClub.SelectedIndexChanged += cboFilterClub_SelectedIndexChanged;

            chkDateFilter.Text     = "Период:";
            chkDateFilter.Location = new Point(424, 14);
            chkDateFilter.AutoSize = true;
            chkDateFilter.CheckedChanged += chkDateFilter_CheckedChanged;

            dtpFrom.Location = new Point(494, 12);
            dtpFrom.Size     = new Size(110, 26);
            dtpFrom.Format   = DateTimePickerFormat.Short;
            dtpFrom.Enabled  = false;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;

            lblTo.Text     = "–";
            lblTo.Location = new Point(608, 16);
            lblTo.AutoSize = true;

            dtpTo.Location = new Point(620, 12);
            dtpTo.Size     = new Size(110, 26);
            dtpTo.Format   = DateTimePickerFormat.Short;
            dtpTo.Enabled  = false;
            dtpTo.ValueChanged += dtpTo_ValueChanged;

            btnRefresh.Text      = "🔄";
            btnRefresh.Location  = new Point(736, 10);
            btnRefresh.Size      = new Size(36, 28);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Click    += btnRefresh_Click;

            panelFilters.Controls.AddRange(new Control[]
            {
                lblFPlayer, cboFilterPlayer, lblFClub, cboFilterClub,
                chkDateFilter, dtpFrom, lblTo, dtpTo, btnRefresh
            });

            // ── DataGridView ──────────────────────────────────────────────────────
            dgvTransfers.Location = new Point(12, 106);
            dgvTransfers.Size     = new Size(760, 520);
            dgvTransfers.Anchor   = AnchorStyles.Top | AnchorStyles.Left |
                                    AnchorStyles.Bottom | AnchorStyles.Right;

            // ── Панел CRUD ────────────────────────────────────────────────────────
            panelForm.Location  = new Point(784, 48);
            panelForm.Size      = new Size(290, 578);
            panelForm.Anchor    = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            panelForm.BackColor = Color.White;

            int y = 12;
            void AddField(Label lbl, string text, Control ctrl, int h = 28)
            {
                lbl.Text     = text;
                lbl.Location = new Point(12, y);
                lbl.AutoSize = true;
                lbl.Font     = new Font("Segoe UI", 9f, FontStyle.Bold);
                y += 20;
                ctrl.Location = new Point(12, y);
                ctrl.Size     = new Size(264, h);
                y += h + 10;
                panelForm.Controls.Add(lbl);
                panelForm.Controls.Add(ctrl);
            }

            AddField(lblPlayer,  "Играч *",          cboPlayer);
            cboPlayer.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPlayer.SelectedIndexChanged += cboPlayer_SelectedIndexChanged;

            AddField(lblFromClub, "Текущ клуб",      txtFromClub);
            txtFromClub.ReadOnly  = true;
            txtFromClub.BackColor = Color.FromArgb(245, 245, 245);

            AddField(lblToClub,  "Нов клуб *",       cboToClub);
            cboToClub.DropDownStyle = ComboBoxStyle.DropDownList;

            AddField(lblDate,    "Дата на трансфер *", dtpTransferDate);
            dtpTransferDate.Format = DateTimePickerFormat.Short;
            dtpTransferDate.Value  = DateTime.Today;

            AddField(lblFee,     "Сума (€)",          numFee);
            numFee.Minimum     = 0;
            numFee.Maximum     = 999_999_999;
            numFee.ThousandsSeparator = true;

            AddField(lblNote,    "Бележка",           txtNote, 50);
            txtNote.Multiline  = true;

            y += 10;
            Button MakeBtn(string text, Color bg)
            {
                var btn = new Button
                {
                    Text      = text,
                    Location  = new Point(12, y),
                    Size      = new Size(264, 40),
                    BackColor = bg,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Cursor    = Cursors.Hand
                };
                y += 48;
                panelForm.Controls.Add(btn);
                return btn;
            }

            btnTransfer = MakeBtn("🔄  Потвърди трансфер", Color.FromArgb(30, 78, 121));
            btnClear    = MakeBtn("✖  Изчисти",            Color.FromArgb(127, 140, 141));

            btnTransfer.Click += btnTransfer_Click;
            btnClear.Click    += btnClear_Click;

            // ── Статус ────────────────────────────────────────────────────────────
            lblStatus.Text      = "Готово.";
            lblStatus.Location  = new Point(12, 636);
            lblStatus.Size      = new Size(1060, 22);
            lblStatus.Anchor    = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.ForeColor = Color.FromArgb(80, 100, 120);

            this.Controls.AddRange(new Control[]
            {
                lblTitle, panelFilters, dgvTransfers, panelForm, lblStatus
            });

            this.ResumeLayout(false);
        }
    }
}

namespace FootballLeague.Forms
{
    partial class PlayersForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Controls ──────────────────────────────────────────────────────────────
        private DataGridView dgvPlayers         = null!;
        private Panel        panelFilters       = null!;
        private Panel        panelForm          = null!;
        private Label        lblTitle           = null!;
        private Label        lblStatus          = null!;

        // Филтри
        private Label    lblClubFilter      = null!;
        private ComboBox cboClubFilter      = null!;
        private Label    lblPosFilter       = null!;
        private ComboBox cboPositionFilter  = null!;
        private Label    lblSearch          = null!;
        private TextBox  txtSearchName      = null!;
        private Button   btnSearch          = null!;
        private Button   btnClearFilters    = null!;

        // CRUD полета
        private Label        lblFullName     = null!;
        private TextBox      txtFullName     = null!;
        private Label        lblBirthDate    = null!;
        private DateTimePicker dtpBirthDate  = null!;
        private Label        lblClub         = null!;
        private ComboBox     cboClub         = null!;
        private Label        lblPosition     = null!;
        private ComboBox     cboPosition     = null!;
        private Label        lblShirt        = null!;
        private NumericUpDown numShirtNumber = null!;
        private Label        lblStatus2      = null!;
        private ComboBox     cboStatus       = null!;

        // Бутони
        private Button btnAdd    = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnClear  = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            dgvPlayers      = new DataGridView();
            panelFilters    = new Panel();
            panelForm       = new Panel();
            lblTitle        = new Label();
            lblStatus       = new Label();
            lblClubFilter   = new Label();
            cboClubFilter   = new ComboBox();
            lblPosFilter    = new Label();
            cboPositionFilter = new ComboBox();
            lblSearch       = new Label();
            txtSearchName   = new TextBox();
            btnSearch       = new Button();
            btnClearFilters = new Button();
            lblFullName     = new Label();
            txtFullName     = new TextBox();
            lblBirthDate    = new Label();
            dtpBirthDate    = new DateTimePicker();
            lblClub         = new Label();
            cboClub         = new ComboBox();
            lblPosition     = new Label();
            cboPosition     = new ComboBox();
            lblShirt        = new Label();
            numShirtNumber  = new NumericUpDown();
            lblStatus2      = new Label();
            cboStatus       = new ComboBox();
            btnAdd          = new Button();
            btnUpdate       = new Button();
            btnDelete       = new Button();
            btnClear        = new Button();

            this.SuspendLayout();

            // ── Форма ────────────────────────────────────────────────────────────
            this.Text          = "Управление на играчи – Football League";
            this.Size          = new Size(1100, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize   = new Size(1000, 600);
            this.BackColor     = Color.FromArgb(240, 244, 248);
            this.Font          = new Font("Segoe UI", 9.5f);
            this.Load         += PlayersForm_Load;

            // ── Заглавие ─────────────────────────────────────────────────────────
            lblTitle.Text      = "👤  Управление на играчи";
            lblTitle.Font      = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 78, 121);
            lblTitle.Location  = new Point(12, 12);
            lblTitle.AutoSize  = true;

            // ── Панел с филтри ────────────────────────────────────────────────────
            panelFilters.Location  = new Point(12, 46);
            panelFilters.Size      = new Size(760, 46);
            panelFilters.BackColor = Color.White;
            panelFilters.Padding   = new Padding(6, 4, 6, 4);

            lblClubFilter.Text     = "Клуб:";
            lblClubFilter.Location = new Point(8, 14);
            lblClubFilter.AutoSize = true;

            cboClubFilter.Location         = new Point(48, 10);
            cboClubFilter.Size             = new Size(160, 26);
            cboClubFilter.DropDownStyle    = ComboBoxStyle.DropDownList;
            cboClubFilter.SelectedIndexChanged += cboClubFilter_SelectedIndexChanged;

            lblPosFilter.Text     = "Позиция:";
            lblPosFilter.Location = new Point(218, 14);
            lblPosFilter.AutoSize = true;

            cboPositionFilter.Location         = new Point(278, 10);
            cboPositionFilter.Size             = new Size(100, 26);
            cboPositionFilter.DropDownStyle    = ComboBoxStyle.DropDownList;
            cboPositionFilter.SelectedIndexChanged += cboPositionFilter_SelectedIndexChanged;

            lblSearch.Text     = "Търси:";
            lblSearch.Location = new Point(390, 14);
            lblSearch.AutoSize = true;

            txtSearchName.Location = new Point(438, 10);
            txtSearchName.Size     = new Size(160, 26);
            txtSearchName.Name     = "txtSearchName";

            btnSearch.Text      = "🔍";
            btnSearch.Location  = new Point(606, 8);
            btnSearch.Size      = new Size(36, 28);
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Click    += btnSearch_Click;

            btnClearFilters.Text      = "✖ Изчисти филтри";
            btnClearFilters.Location  = new Point(648, 8);
            btnClearFilters.Size      = new Size(104, 28);
            btnClearFilters.FlatStyle = FlatStyle.Flat;
            btnClearFilters.Click    += btnClearFilters_Click;

            panelFilters.Controls.AddRange(new Control[]
            {
                lblClubFilter, cboClubFilter,
                lblPosFilter, cboPositionFilter,
                lblSearch, txtSearchName, btnSearch, btnClearFilters
            });

            // ── DataGridView ──────────────────────────────────────────────────────
            dgvPlayers.Location  = new Point(12, 100);
            dgvPlayers.Size      = new Size(760, 510);
            dgvPlayers.Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            dgvPlayers.SelectionChanged += dgvPlayers_SelectionChanged;

            // ── Панел CRUD (вдясно) ───────────────────────────────────────────────
            panelForm.Location  = new Point(784, 46);
            panelForm.Size      = new Size(290, 564);
            panelForm.Anchor    = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            panelForm.BackColor = Color.White;
            panelForm.Padding   = new Padding(12);

            int y = 12;
            void AddField(Label lbl, string text, Control ctrl, int height = 28)
            {
                lbl.Text     = text;
                lbl.Location = new Point(12, y);
                lbl.AutoSize = true;
                lbl.Font     = new Font("Segoe UI", 9f, FontStyle.Bold);
                y += 20;
                ctrl.Location = new Point(12, y);
                ctrl.Size     = new Size(262, height);
                y += height + 10;
                panelForm.Controls.Add(lbl);
                panelForm.Controls.Add(ctrl);
            }

            AddField(lblFullName,  "Пълно Ime *",    txtFullName);
            AddField(lblBirthDate, "Дата на раждане *", dtpBirthDate);
            dtpBirthDate.Format     = DateTimePickerFormat.Short;
            dtpBirthDate.Value      = DateTime.Today.AddYears(-20);

            AddField(lblClub,     "Клуб *",      cboClub);
            cboClub.DropDownStyle = ComboBoxStyle.DropDownList;

            AddField(lblPosition, "Позиция *",   cboPosition);
            cboPosition.DropDownStyle = ComboBoxStyle.DropDownList;

            AddField(lblShirt,    "Номер фланелка", numShirtNumber);
            numShirtNumber.Minimum = 1;
            numShirtNumber.Maximum = 99;

            AddField(lblStatus2,  "Статус",      cboStatus);
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            y += 10;
            Button MakeBtn(string text, Color bg)
            {
                var btn = new Button
                {
                    Text      = text,
                    Location  = new Point(12, y),
                    Size      = new Size(262, 36),
                    BackColor = bg,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Cursor    = Cursors.Hand
                };
                y += 44;
                panelForm.Controls.Add(btn);
                return btn;
            }

            btnAdd    = MakeBtn("➕  Добави",       Color.FromArgb( 39, 174,  96));
            btnUpdate = MakeBtn("✏️  Обнови",        Color.FromArgb(230, 126,  34));
            btnDelete = MakeBtn("🗑️  Изтрий",        Color.FromArgb(192,  57,  43));
            btnClear  = MakeBtn("✖  Изчисти форма", Color.FromArgb(127, 140, 141));

            btnAdd.Click    += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click  += btnClear_Click;

            // ── Статус лента ──────────────────────────────────────────────────────
            lblStatus.Text     = "Готово.";
            lblStatus.Location = new Point(12, 620);
            lblStatus.Size     = new Size(1060, 24);
            lblStatus.Anchor   = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.ForeColor = Color.FromArgb(80, 100, 120);

            this.Controls.AddRange(new Control[]
            {
                lblTitle, panelFilters, dgvPlayers, panelForm, lblStatus
            });

            this.ResumeLayout(false);
        }
    }
}

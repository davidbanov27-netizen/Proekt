namespace FootballLeague.Forms
{
    partial class LeaguesForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Controls ──────────────────────────────────────────────────────────────
        private Label          lblTitle             = null!;
        private Label          lblStatus            = null!;

        // Ляв панел — лиги
        private Panel          panelLeft            = null!;
        private Label          lblLeagues           = null!;
        private DataGridView   dgvLeagues           = null!;
        private Label          lblLeagueName        = null!;
        private TextBox        txtName              = null!;
        private Label          lblSeason            = null!;
        private TextBox        txtSeason            = null!;
        private Button         btnAddLeague         = null!;
        private Button         btnEditLeague        = null!;
        private Button         btnDeleteLeague      = null!;
        private Button         btnClearForm         = null!;

        // Десен панел — участници
        private Panel          panelRight           = null!;
        private Label          lblParticipants      = null!;
        private Label          lblParticipantsCount = null!;
        private DataGridView   dgvParticipants      = null!;
        private Label          lblAvailable         = null!;
        private ComboBox       cboAvailableClubs    = null!;
        private Button         btnAddClub           = null!;
        private Button         btnRemoveClub        = null!;
        private Button         btnRefresh           = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components           = new System.ComponentModel.Container();
            lblTitle             = new Label();
            lblStatus            = new Label();
            panelLeft            = new Panel();
            lblLeagues           = new Label();
            dgvLeagues           = new DataGridView();
            lblLeagueName        = new Label();
            txtName              = new TextBox();
            lblSeason            = new Label();
            txtSeason            = new TextBox();
            btnAddLeague         = new Button();
            btnEditLeague        = new Button();
            btnDeleteLeague      = new Button();
            btnClearForm         = new Button();
            panelRight           = new Panel();
            lblParticipants      = new Label();
            lblParticipantsCount = new Label();
            dgvParticipants      = new DataGridView();
            lblAvailable         = new Label();
            cboAvailableClubs    = new ComboBox();
            btnAddClub           = new Button();
            btnRemoveClub        = new Button();
            btnRefresh           = new Button();

            this.SuspendLayout();

            // ── Форма ────────────────────────────────────────────────────────────
            this.Text          = "Лиги и участници – Football League";
            this.Size          = new Size(1060, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize   = new Size(960, 600);
            this.BackColor     = Color.FromArgb(240, 244, 248);
            this.Font          = new Font("Segoe UI", 9.5f);
            this.Load         += LeaguesForm_Load;

            // ── Заглавие ─────────────────────────────────────────────────────────
            lblTitle.Text      = "🏆  Лиги и участници";
            lblTitle.Font      = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 78, 121);
            lblTitle.Location  = new Point(12, 12);
            lblTitle.AutoSize  = true;

            // ════════════════════════════════════════════════════════════════════
            // ── ЛЯВО: Лиги ───────────────────────────────────────────────────────
            panelLeft.Location  = new Point(12, 46);
            panelLeft.Size      = new Size(460, 570);
            panelLeft.Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            panelLeft.BackColor = Color.White;
            panelLeft.Padding   = new Padding(10);

            lblLeagues.Text      = "Всички лиги";
            lblLeagues.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblLeagues.ForeColor = Color.FromArgb(30, 78, 121);
            lblLeagues.Location  = new Point(10, 10);
            lblLeagues.AutoSize  = true;

            dgvLeagues.Location  = new Point(10, 36);
            dgvLeagues.Size      = new Size(438, 220);
            dgvLeagues.Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvLeagues.SelectionChanged += dgvLeagues_SelectionChanged;

            // Форма за добавяне/редакция
            lblLeagueName.Text     = "Лига *";
            lblLeagueName.Location = new Point(10, 268);
            lblLeagueName.AutoSize = true;
            lblLeagueName.Font     = new Font("Segoe UI", 9f, FontStyle.Bold);

            txtName.Location = new Point(10, 286);
            txtName.Size     = new Size(438, 26);
            txtName.Name     = "txtName";

            lblSeason.Text     = "Сезон * (пример: 2025/2026)";
            lblSeason.Location = new Point(10, 322);
            lblSeason.AutoSize = true;
            lblSeason.Font     = new Font("Segoe UI", 9f, FontStyle.Bold);

            txtSeason.Location = new Point(10, 340);
            txtSeason.Size     = new Size(438, 26);
            txtSeason.Name     = "txtSeason";

            // Бутони за лиги
            int bx = 10, bw = 102;
            Button MakeSmall(string text, Color bg, int x)
            {
                return new Button
                {
                    Text      = text,
                    Location  = new Point(x, 378),
                    Size      = new Size(bw, 36),
                    BackColor = bg,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    Cursor    = Cursors.Hand
                };
            }

            btnAddLeague    = MakeSmall("➕ Добави",     Color.FromArgb( 39, 174,  96), bx);
            btnEditLeague   = MakeSmall("✏️ Редактирай", Color.FromArgb(230, 126,  34), bx + 108);
            btnDeleteLeague = MakeSmall("🗑️ Изтрий",     Color.FromArgb(192,  57,  43), bx + 216);
            btnClearForm    = MakeSmall("✖ Изчисти",    Color.FromArgb(127, 140, 141), bx + 324);

            btnAddLeague.Click    += btnAddLeague_Click;
            btnEditLeague.Click   += btnEditLeague_Click;
            btnDeleteLeague.Click += btnDeleteLeague_Click;
            btnClearForm.Click    += btnClearForm_Click;

            panelLeft.Controls.AddRange(new Control[]
            {
                lblLeagues, dgvLeagues,
                lblLeagueName, txtName, lblSeason, txtSeason,
                btnAddLeague, btnEditLeague, btnDeleteLeague, btnClearForm
            });

            // ════════════════════════════════════════════════════════════════════
            // ── ДЯСНО: Участници ─────────────────────────────────────────────────
            panelRight.Location  = new Point(484, 46);
            panelRight.Size      = new Size(556, 570);
            panelRight.Anchor    = AnchorStyles.Top | AnchorStyles.Left |
                                   AnchorStyles.Right | AnchorStyles.Bottom;
            panelRight.BackColor = Color.White;
            panelRight.Padding   = new Padding(10);

            lblParticipants.Text      = "Участници в избраната лига";
            lblParticipants.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblParticipants.ForeColor = Color.FromArgb(30, 78, 121);
            lblParticipants.Location  = new Point(10, 10);
            lblParticipants.AutoSize  = true;

            lblParticipantsCount.Text      = "";
            lblParticipantsCount.Location  = new Point(10, 30);
            lblParticipantsCount.AutoSize  = true;
            lblParticipantsCount.ForeColor = Color.Gray;

            dgvParticipants.Location = new Point(10, 50);
            dgvParticipants.Size     = new Size(530, 300);
            dgvParticipants.Anchor   = AnchorStyles.Top | AnchorStyles.Left |
                                       AnchorStyles.Right | AnchorStyles.Bottom;

            lblAvailable.Text     = "Добави клуб в лигата:";
            lblAvailable.Location = new Point(10, 362);
            lblAvailable.AutoSize = true;
            lblAvailable.Font     = new Font("Segoe UI", 9f, FontStyle.Bold);

            cboAvailableClubs.Location      = new Point(10, 382);
            cboAvailableClubs.Size          = new Size(320, 26);
            cboAvailableClubs.DropDownStyle = ComboBoxStyle.DropDownList;

            btnAddClub.Text      = "➕  Добави в лигата";
            btnAddClub.Location  = new Point(338, 380);
            btnAddClub.Size      = new Size(160, 30);
            btnAddClub.BackColor = Color.FromArgb(39, 174, 96);
            btnAddClub.ForeColor = Color.White;
            btnAddClub.FlatStyle = FlatStyle.Flat;
            btnAddClub.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnAddClub.Click    += btnAddClub_Click;

            btnRemoveClub.Text      = "🗑️  Премахни от лигата";
            btnRemoveClub.Location  = new Point(10, 422);
            btnRemoveClub.Size      = new Size(220, 34);
            btnRemoveClub.BackColor = Color.FromArgb(192, 57, 43);
            btnRemoveClub.ForeColor = Color.White;
            btnRemoveClub.FlatStyle = FlatStyle.Flat;
            btnRemoveClub.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnRemoveClub.Click    += btnRemoveClub_Click;

            btnRefresh.Text      = "🔄  Обнови";
            btnRefresh.Location  = new Point(240, 422);
            btnRefresh.Size      = new Size(130, 34);
            btnRefresh.BackColor = Color.FromArgb(70, 130, 180);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnRefresh.Click    += btnRefresh_Click;

            panelRight.Controls.AddRange(new Control[]
            {
                lblParticipants, lblParticipantsCount, dgvParticipants,
                lblAvailable, cboAvailableClubs, btnAddClub,
                btnRemoveClub, btnRefresh
            });

            // ── Статус ────────────────────────────────────────────────────────────
            lblStatus.Text      = "Готово.";
            lblStatus.Location  = new Point(12, 626);
            lblStatus.Size      = new Size(1020, 22);
            lblStatus.Anchor    = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.ForeColor = Color.FromArgb(80, 100, 120);

            this.Controls.AddRange(new Control[]
            {
                lblTitle, panelLeft, panelRight, lblStatus
            });

            this.ResumeLayout(false);
        }
    }
}

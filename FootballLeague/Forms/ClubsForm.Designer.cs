namespace FootballLeague.Forms
{
    partial class ClubsForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Controls ─────────────────────────────────────────────────────────────
        private System.Windows.Forms.DataGridView dgvClubs      = null!;
        private System.Windows.Forms.Label        lblName       = null!;
        private System.Windows.Forms.TextBox      txtName       = null!;
        private System.Windows.Forms.Label        lblCity       = null!;
        private System.Windows.Forms.TextBox      txtCity       = null!;
        private System.Windows.Forms.Button       btnLoad       = null!;
        private System.Windows.Forms.Button       btnAdd        = null!;
        private System.Windows.Forms.Button       btnEdit       = null!;
        private System.Windows.Forms.Button       btnDelete     = null!;
        private System.Windows.Forms.Button       btnClear      = null!;
        private System.Windows.Forms.Label        lblStatus     = null!;
        private System.Windows.Forms.Panel        panelForm     = null!;
        private System.Windows.Forms.Label        lblTitle      = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components  = new System.ComponentModel.Container();

            // ── Инициализация ────────────────────────────────────────────────────
            dgvClubs  = new DataGridView();
            panelForm = new Panel();
            lblTitle  = new Label();
            lblName   = new Label();
            txtName   = new TextBox();
            lblCity   = new Label();
            txtCity   = new TextBox();
            btnLoad   = new Button();
            btnAdd    = new Button();
            btnEdit   = new Button();
            btnDelete = new Button();
            btnClear  = new Button();
            lblStatus = new Label();

            this.SuspendLayout();

            // ── Форма ────────────────────────────────────────────────────────────
            this.Text            = "Управление на клубове – Football League";
            this.Size            = new Size(860, 600);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.MinimumSize     = new Size(860, 600);
            this.BackColor       = Color.FromArgb(240, 244, 248);
            this.Font            = new Font("Segoe UI", 9.5f);
            this.Load           += ClubsForm_Load;

            // ── Заглавие ─────────────────────────────────────────────────────────
            lblTitle.Text      = "⚽  Управление на клубове";
            lblTitle.Font      = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 78, 121);
            lblTitle.Location  = new Point(12, 12);
            lblTitle.AutoSize  = true;

            // ── DataGridView ─────────────────────────────────────────────────────
            dgvClubs.Location            = new Point(12, 50);
            dgvClubs.Size                = new Size(580, 460);
            dgvClubs.Anchor              = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            dgvClubs.BackgroundColor     = Color.White;
            dgvClubs.BorderStyle         = BorderStyle.None;
            dgvClubs.RowHeadersVisible   = false;
            dgvClubs.GridColor           = Color.FromArgb(220, 230, 240);
            dgvClubs.SelectionChanged   += dgvClubs_SelectionChanged;
            dgvClubs.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 78, 121);
            dgvClubs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClubs.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvClubs.EnableHeadersVisualStyles = false;
            dgvClubs.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 255);

            // ── Панел за форма (вдясно) ──────────────────────────────────────────
            panelForm.Location  = new Point(606, 50);
            panelForm.Size      = new Size(228, 460);
            panelForm.Anchor    = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            panelForm.BackColor = Color.White;
            panelForm.Padding   = new Padding(12);

            // Поле Name
            lblName.Text     = "Клуб *";
            lblName.Location = new Point(12, 16);
            lblName.AutoSize = true;
            lblName.Font     = new Font("Segoe UI", 9f, FontStyle.Bold);

            txtName.Location = new Point(12, 36);
            txtName.Size     = new Size(200, 26);
            txtName.Name     = "txtName";

            // Поле City
            lblCity.Text     = "Град";
            lblCity.Location = new Point(12, 76);
            lblCity.AutoSize = true;
            lblCity.Font     = new Font("Segoe UI", 9f, FontStyle.Bold);

            txtCity.Location = new Point(12, 96);
            txtCity.Size     = new Size(200, 26);
            txtCity.Name     = "txtCity";

            // Стил за бутони
            static Button MakeBtn(string text, Color bg, int y)
            {
                return new Button
                {
                    Text      = text,
                    Location  = new Point(12, y),
                    Size      = new Size(200, 36),
                    BackColor = bg,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Cursor    = Cursors.Hand
                };
            }

            btnLoad   = MakeBtn("🔄  Зареди",    Color.FromArgb( 70, 130, 180), 148);
            btnAdd    = MakeBtn("➕  Добави",     Color.FromArgb( 39, 174,  96), 196);
            btnEdit   = MakeBtn("✏️  Редактирай", Color.FromArgb(230, 126,  34), 244);
            btnDelete = MakeBtn("🗑️  Изтрий",     Color.FromArgb(192,  57,  43), 292);
            btnClear  = MakeBtn("✖  Изчисти",    Color.FromArgb(127, 140, 141), 352);

            btnLoad.Click   += btnLoad_Click;
            btnAdd.Click    += btnAdd_Click;
            btnEdit.Click   += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click  += btnClear_Click;

            panelForm.Controls.AddRange(new Control[]
            {
                lblName, txtName, lblCity, txtCity,
                btnLoad, btnAdd, btnEdit, btnDelete, btnClear
            });

            // ── Статус лента ─────────────────────────────────────────────────────
            lblStatus.Text      = "Готово.";
            lblStatus.Location  = new Point(12, 520);
            lblStatus.Size      = new Size(820, 24);
            lblStatus.Anchor    = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.ForeColor = Color.FromArgb(80, 100, 120);

            // ── Добавяне към формата ─────────────────────────────────────────────
            this.Controls.AddRange(new Control[]
            {
                lblTitle, dgvClubs, panelForm, lblStatus
            });

            this.ResumeLayout(false);
        }
    }
}

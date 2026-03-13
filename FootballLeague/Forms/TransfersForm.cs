using FootballLeague.Models;
using FootballLeague.Repositories;

namespace FootballLeague.Forms
{
    public partial class TransfersForm : Form
    {
        private readonly TransfersRepository _transfersRepo = new();
        private readonly PlayersRepository   _playersRepo   = new();
        private readonly ClubsRepository     _clubsRepo     = new();

        // Кеш на заредените играчи за лесен lookup
        private List<Player> _allPlayers = new();

        public TransfersForm()
        {
            InitializeComponent();
        }

        // ── Form Load ────────────────────────────────────────────────────────────
        private void TransfersForm_Load(object sender, EventArgs e)
        {
            ConfigureGrid();
            LoadComboBoxes();
            LoadTransfers();
        }

        // ── Настройка на DataGridView ─────────────────────────────────────────────
        private void ConfigureGrid()
        {
            dgvTransfers.ReadOnly               = true;
            dgvTransfers.SelectionMode          = DataGridViewSelectionMode.FullRowSelect;
            dgvTransfers.MultiSelect            = false;
            dgvTransfers.AutoSizeColumnsMode    = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransfers.AllowUserToAddRows     = false;
            dgvTransfers.BackgroundColor        = Color.White;
            dgvTransfers.BorderStyle            = BorderStyle.None;
            dgvTransfers.RowHeadersVisible      = false;
            dgvTransfers.GridColor              = Color.FromArgb(220, 230, 240);
            dgvTransfers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 78, 121);
            dgvTransfers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTransfers.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvTransfers.EnableHeadersVisualStyles               = false;
            dgvTransfers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 255);
        }

        // ── Зареждане на комбобоксове ─────────────────────────────────────────────
        private void LoadComboBoxes()
        {
            _allPlayers = _playersRepo.GetPlayers();
            var clubs   = _clubsRepo.GetAll();

            // Играч (за трансфер)
            cboPlayer.Items.Clear();
            cboPlayer.Items.Add(new ComboItem(0, "— Избери играч —"));
            foreach (var p in _allPlayers)
                cboPlayer.Items.Add(new ComboItem(p.PlayerId, $"{p.FullName}  [{p.ClubName}]"));
            cboPlayer.SelectedIndex = 0;

            // Нов клуб
            cboToClub.Items.Clear();
            cboToClub.Items.Add(new ComboItem(0, "— Избери нов клуб —"));
            foreach (var c in clubs)
                cboToClub.Items.Add(new ComboItem(c.ClubId, c.Name));
            cboToClub.SelectedIndex = 0;

            // Филтър по играч
            cboFilterPlayer.Items.Clear();
            cboFilterPlayer.Items.Add(new ComboItem(0, "— Всички играчи —"));
            foreach (var p in _allPlayers)
                cboFilterPlayer.Items.Add(new ComboItem(p.PlayerId, p.FullName));
            cboFilterPlayer.SelectedIndex = 0;

            // Филтър по клуб
            cboFilterClub.Items.Clear();
            cboFilterClub.Items.Add(new ComboItem(0, "— Всички клубове —"));
            foreach (var c in clubs)
                cboFilterClub.Items.Add(new ComboItem(c.ClubId, c.Name));
            cboFilterClub.SelectedIndex = 0;
        }

        // ── Избор на играч → показва текущ клуб ──────────────────────────────────
        private void cboPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = cboPlayer.SelectedItem as ComboItem;
            if (item == null || item.Id == 0) { txtFromClub.Text = ""; return; }

            var player = _allPlayers.FirstOrDefault(p => p.PlayerId == item.Id);
            txtFromClub.Text = player?.ClubName ?? "";
        }

        // ── Зареждане на трансфери (с филтри) ────────────────────────────────────
        private void LoadTransfers()
        {
            try
            {
                int? pid = (cboFilterPlayer.SelectedItem as ComboItem)?.Id;
                int? cid = (cboFilterClub.SelectedItem   as ComboItem)?.Id;
                DateTime? from = chkDateFilter.Checked ? (DateTime?)dtpFrom.Value.Date : null;
                DateTime? to   = chkDateFilter.Checked ? (DateTime?)dtpTo.Value.Date   : null;

                var transfers = _transfersRepo.GetTransfers(
                    pid > 0 ? pid : null,
                    cid > 0 ? cid : null,
                    from, to);

                dgvTransfers.DataSource = transfers;

                // Скриваме ID полетата, слагаме заглавия
                HideCol("TransferId"); HideCol("PlayerId");
                HideCol("FromClubId"); HideCol("ToClubId");
                SetHeader("PlayerName",   "Играч");
                SetHeader("FromClubName", "От клуб");
                SetHeader("ToClubName",   "Към клуб");
                SetHeader("TransferDate", "Дата");
                SetHeader("Fee",          "Сума (€)");
                SetHeader("Note",         "Бележка");

                lblStatus.Text = $"Заредени {transfers.Count} трансфера.";
            }
            catch (Exception ex)
            {
                ShowError("Грешка при зареждане", ex);
            }
        }

        // ── ТРАНСФЕР ─────────────────────────────────────────────────────────────
        private void btnTransfer_Click(object sender, EventArgs e)
        {
            // ── Валидации ────────────────────────────────────────────────────────
            var playerItem = cboPlayer.SelectedItem as ComboItem;
            var toClubItem = cboToClub.SelectedItem as ComboItem;

            if (playerItem == null || playerItem.Id == 0)
            {
                Warn("Моля, изберете играч!"); return;
            }
            if (toClubItem == null || toClubItem.Id == 0)
            {
                Warn("Моля, изберете нов клуб!"); return;
            }

            var player = _allPlayers.FirstOrDefault(p => p.PlayerId == playerItem.Id);
            if (player == null) { Warn("Играчът не е намерен."); return; }

            // Не към същия клуб
            if (player.ClubId == toClubItem.Id)
            {
                MessageBox.Show(
                    $"Играчът вече е в клуб \"{player.ClubName}\"!\nТрансферът към същия клуб не е позволен.",
                    "Невалиден трансфер", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpTransferDate.Value.Date > DateTime.Today)
            {
                Warn("Датата на трансфера не може да е в бъдещето."); return;
            }

            decimal fee = numFee.Value;
            if (fee < 0) { Warn("Сумата не може да е отрицателна."); return; }

            // ── Потвърждение ─────────────────────────────────────────────────────
            var confirm = MessageBox.Show(
                $"Трансфер на  \"{player.FullName}\"\n" +
                $"От:  {player.ClubName}\n" +
                $"Към: {toClubItem.Name}\n" +
                $"Дата: {dtpTransferDate.Value:dd.MM.yyyy}\n\n" +
                "Потвърждавате ли?",
                "Потвърди трансфер",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _transfersRepo.AddTransfer(new Transfer
                {
                    PlayerId     = player.PlayerId,
                    FromClubId   = player.ClubId > 0 ? player.ClubId : null,
                    ToClubId     = toClubItem.Id,
                    TransferDate = dtpTransferDate.Value.Date,
                    Fee          = fee > 0 ? fee : null,
                    Note         = txtNote.Text.Trim()
                });

                MessageBox.Show(
                    $"Трансферът е записан успешно!\n\"{player.FullName}\" вече е в \"{toClubItem.Name}\".",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Обнови кеша и комбобоксовете
                LoadComboBoxes();
                LoadTransfers();
                ClearForm();
                lblStatus.Text = $"✔  Трансфер: {player.FullName} → {toClubItem.Name}";
            }
            catch (Exception ex)
            {
                ShowError("Грешка при трансфер (rollback)", ex);
            }
        }

        // ── Филтри ────────────────────────────────────────────────────────────────
        private void cboFilterPlayer_SelectedIndexChanged(object sender, EventArgs e) => LoadTransfers();
        private void cboFilterClub_SelectedIndexChanged(object sender, EventArgs e)   => LoadTransfers();
        private void btnRefresh_Click(object sender, EventArgs e)                      => LoadTransfers();
        private void chkDateFilter_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = dtpTo.Enabled = chkDateFilter.Checked;
            LoadTransfers();
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e) => LoadTransfers();
        private void dtpTo_ValueChanged(object sender, EventArgs e)   => LoadTransfers();

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cboFilterPlayer.SelectedIndex = 0;
            cboFilterClub.SelectedIndex   = 0;
            chkDateFilter.Checked         = false;
            LoadTransfers();
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearForm();

        // ── Помощни методи ────────────────────────────────────────────────────────
        private void ClearForm()
        {
            cboPlayer.SelectedIndex  = 0;
            cboToClub.SelectedIndex  = 0;
            txtFromClub.Text         = "";
            txtNote.Text             = "";
            numFee.Value             = 0;
            dtpTransferDate.Value    = DateTime.Today;
        }

        private void SetHeader(string col, string header)
        {
            if (dgvTransfers.Columns[col] != null)
                dgvTransfers.Columns[col]!.HeaderText = header;
        }
        private void HideCol(string col)
        {
            if (dgvTransfers.Columns[col] != null)
                dgvTransfers.Columns[col]!.Visible = false;
        }

        private void Warn(string msg) =>
            MessageBox.Show(msg, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void ShowError(string msg, Exception ex)
        {
            MessageBox.Show($"{msg}:\n\n{ex.Message}", "Грешка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = $"✘  {msg}.";
        }
    }
}

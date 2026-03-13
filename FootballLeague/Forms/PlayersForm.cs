using FootballLeague.Models;
using FootballLeague.Repositories;
using FootballLeague.Services;
using MySql.Data.MySqlClient;

namespace FootballLeague.Forms
{
    public partial class PlayersForm : Form
    {
        private readonly PlayersRepository _playersRepo = new();
        private readonly ClubsRepository   _clubsRepo   = new();
        private int _selectedPlayerId = -1;

        public PlayersForm()
        {
            InitializeComponent();
        }

        // ── Form Load ────────────────────────────────────────────────────────────
        private void PlayersForm_Load(object sender, EventArgs e)
        {
            ConfigureGrid();
            LoadClubsIntoComboBoxes();
            LoadPositionsIntoComboBoxes();
            LoadPlayers();
        }

        // ── Настройка на DataGridView ─────────────────────────────────────────────
        private void ConfigureGrid()
        {
            dgvPlayers.ReadOnly               = true;
            dgvPlayers.SelectionMode          = DataGridViewSelectionMode.FullRowSelect;
            dgvPlayers.MultiSelect            = false;
            dgvPlayers.AutoSizeColumnsMode    = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPlayers.AllowUserToAddRows     = false;
            dgvPlayers.BackgroundColor        = Color.White;
            dgvPlayers.BorderStyle            = BorderStyle.None;
            dgvPlayers.GridColor              = Color.FromArgb(220, 230, 240);
            dgvPlayers.RowHeadersVisible      = false;
            dgvPlayers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 78, 121);
            dgvPlayers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPlayers.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvPlayers.EnableHeadersVisualStyles               = false;
            dgvPlayers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 255);
        }

        // ── Зареждане на клубове в комбобоксове ──────────────────────────────────
        private void LoadClubsIntoComboBoxes()
        {
            var clubs = _clubsRepo.GetAll();

            // Филтър комбобокс
            cboClubFilter.Items.Clear();
            cboClubFilter.Items.Add(new ComboItem(0, "— Всички клубове —"));
            foreach (var c in clubs)
                cboClubFilter.Items.Add(new ComboItem(c.ClubId, c.Name));
            cboClubFilter.SelectedIndex = 0;

            // CRUD комбобокс
            cboClub.Items.Clear();
            cboClub.Items.Add(new ComboItem(0, "— Избери клуб —"));
            foreach (var c in clubs)
                cboClub.Items.Add(new ComboItem(c.ClubId, c.Name));
            cboClub.SelectedIndex = 0;
        }

        // ── Зареждане на позиции ─────────────────────────────────────────────────
        private void LoadPositionsIntoComboBoxes()
        {
            var positions = new[] { "GK", "DF", "MF", "FW" };

            cboPositionFilter.Items.Clear();
            cboPositionFilter.Items.Add("— Всички позиции —");
            cboPositionFilter.Items.AddRange(positions);
            cboPositionFilter.SelectedIndex = 0;

            cboPosition.Items.Clear();
            cboPosition.Items.AddRange(positions);
            cboPosition.SelectedIndex = 0;

            cboStatus.Items.Clear();
            cboStatus.Items.AddRange(new[] { "Active", "Injured", "Suspended" });
            cboStatus.SelectedIndex = 0;
        }

        // ── Зареждане на играчи (с филтри) ───────────────────────────────────────
        private void LoadPlayers()
        {
            try
            {
                int?    clubId   = (cboClubFilter.SelectedItem as ComboItem)?.Id;
                string? position = cboPositionFilter.SelectedIndex > 0
                                   ? cboPositionFilter.SelectedItem?.ToString() : null;
                string? name     = string.IsNullOrWhiteSpace(txtSearchName.Text)
                                   ? null : txtSearchName.Text;

                var players = _playersRepo.GetPlayers(
                    clubId > 0 ? clubId : null, position, name);

                dgvPlayers.DataSource = players;

                // Заглавия
                SetHeader("PlayerId",    "ID");
                SetHeader("ClubId",      null);   // скриваме
                SetHeader("ClubName",    "Клуб");
                SetHeader("FullName",    "Играч");
                SetHeader("BirthDate",   "Роден на");
                SetHeader("Age",         "Възраст");
                SetHeader("Position",    "Позиция");
                SetHeader("ShirtNumber", "№");
                SetHeader("Status",      "Статус");

                if (dgvPlayers.Columns["ClubId"] != null)
                    dgvPlayers.Columns["ClubId"]!.Visible = false;

                lblStatus.Text = $"Заредени {players.Count} играча.";
            }
            catch (Exception ex)
            {
                ShowError("Грешка при зареждане", ex);
            }
        }

        private void SetHeader(string col, string? header)
        {
            if (dgvPlayers.Columns[col] == null) return;
            if (header == null) { dgvPlayers.Columns[col]!.Visible = false; return; }
            dgvPlayers.Columns[col]!.HeaderText = header;
        }

        // ── Избор на ред → попълва полетата ──────────────────────────────────────
        private void dgvPlayers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPlayers.CurrentRow?.DataBoundItem is Player p)
            {
                _selectedPlayerId    = p.PlayerId;
                txtFullName.Text     = p.FullName;
                dtpBirthDate.Value   = p.BirthDate;
                numShirtNumber.Value = p.ShirtNumber ?? 1;

                SetCombo(cboClub,     p.ClubId);
                SetComboText(cboPosition, p.Position);
                SetComboText(cboStatus,   p.Status);
            }
        }

        // ── ФИЛТРИ ────────────────────────────────────────────────────────────────
        private void cboClubFilter_SelectedIndexChanged(object sender, EventArgs e)    => LoadPlayers();
        private void cboPositionFilter_SelectedIndexChanged(object sender, EventArgs e) => LoadPlayers();
        private void btnSearch_Click(object sender, EventArgs e)                        => LoadPlayers();

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cboClubFilter.SelectedIndex     = 0;
            cboPositionFilter.SelectedIndex = 0;
            txtSearchName.Text              = "";
            LoadPlayers();
        }

        // ── ADD ───────────────────────────────────────────────────────────────────
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var player = BuildPlayerFromForm();
            if (player == null) return;

            try
            {
                _playersRepo.Add(player);
                LoadPlayers();
                ClearForm();
                lblStatus.Text = $"✔  Играч \"{player.FullName}\" добавен успешно.";
                MessageBox.Show("Играчът е добавен успешно!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError("Грешка при добавяне", ex);
            }
        }

        // ── UPDATE ────────────────────────────────────────────────────────────────
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedPlayerId == -1)
            {
                MessageBox.Show("Моля, изберете играч от списъка!",
                    "Няма избран играч", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var player = BuildPlayerFromForm();
            if (player == null) return;
            player.PlayerId = _selectedPlayerId;

            try
            {
                _playersRepo.Update(player);
                LoadPlayers();
                lblStatus.Text = "✔  Играчът е обновен успешно.";
                MessageBox.Show("Данните са обновени!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError("Грешка при обновяване", ex);
            }
        }

        // ── DELETE ────────────────────────────────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedPlayerId == -1)
            {
                MessageBox.Show("Моля, изберете играч от списъка!",
                    "Няма избран играч", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Сигурни ли сте, че искате да изтриете \"{txtFullName.Text}\"?",
                "Потвърждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _playersRepo.Delete(_selectedPlayerId);
                LoadPlayers();
                ClearForm();
                lblStatus.Text = "✔  Играчът е изтрит.";
            }
            catch (MySqlException ex) when (ex.Number == 1451)
            {
                MessageBox.Show("Не може да се изтрие — играчът има свързани записи.",
                    "FK грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                ShowError("Грешка при изтриване", ex);
            }
        }

        // ── CLEAR ─────────────────────────────────────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e) => ClearForm();

        // ── Помощни методи ────────────────────────────────────────────────────────
        private Player? BuildPlayerFromForm()
        {
            var clubItem = cboClub.SelectedItem as ComboItem;
            var errors   = ValidationService.ValidatePlayer(
                               txtFullName.Text,
                               dtpBirthDate.Value,
                               cboPosition.SelectedItem?.ToString() ?? "",
                               clubItem?.Id ?? 0);

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors.Select(e => "• " + e)),
                    "Грешки при валидация", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return new Player
            {
                ClubId      = clubItem!.Id,
                FullName    = txtFullName.Text.Trim(),
                BirthDate   = dtpBirthDate.Value.Date,
                Position    = cboPosition.SelectedItem!.ToString()!,
                ShirtNumber = (int)numShirtNumber.Value > 0 ? (int?)numShirtNumber.Value : null,
                Status      = cboStatus.SelectedItem?.ToString() ?? "Active"
            };
        }

        private void ClearForm()
        {
            txtFullName.Text        = "";
            dtpBirthDate.Value      = DateTime.Today.AddYears(-20);
            numShirtNumber.Value    = 1;
            cboClub.SelectedIndex   = 0;
            cboPosition.SelectedIndex = 0;
            cboStatus.SelectedIndex   = 0;
            _selectedPlayerId       = -1;
            dgvPlayers.ClearSelection();
        }

        private void SetCombo(ComboBox cbo, int id)
        {
            foreach (var item in cbo.Items)
                if (item is ComboItem ci && ci.Id == id)
                { cbo.SelectedItem = item; return; }
        }

        private void SetComboText(ComboBox cbo, string text)
        {
            for (int i = 0; i < cbo.Items.Count; i++)
                if (cbo.Items[i]?.ToString() == text)
                { cbo.SelectedIndex = i; return; }
        }

        private void ShowError(string msg, Exception ex)
        {
            MessageBox.Show($"{msg}:\n\n{ex.Message}", "Грешка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = $"✘  {msg}.";
        }
    }

    // ── Помощен клас за ComboBox items с ID ──────────────────────────────────────
    internal class ComboItem
    {
        public int    Id   { get; }
        public string Name { get; }
        public ComboItem(int id, string name) { Id = id; Name = name; }
        public override string ToString() => Name;
    }
}

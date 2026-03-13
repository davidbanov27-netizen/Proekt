using FootballLeague.Models;
using FootballLeague.Repositories;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace FootballLeague.Forms
{
    public partial class LeaguesForm : Form
    {
        private readonly LeaguesRepository     _leaguesRepo = new();
        private readonly LeagueTeamsRepository _teamsRepo   = new();

        private int _selectedLeagueId = -1;

        public LeaguesForm()
        {
            InitializeComponent();
        }

        // ── Load ─────────────────────────────────────────────────────────────────
        private void LeaguesForm_Load(object sender, EventArgs e)
        {
            ConfigureGrids();
            LoadLeagues();
        }

        private void ConfigureGrids()
        {
            foreach (var dgv in new[] { dgvLeagues, dgvParticipants })
            {
                dgv.ReadOnly               = true;
                dgv.SelectionMode          = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect            = false;
                dgv.AutoSizeColumnsMode    = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.AllowUserToAddRows     = false;
                dgv.BackgroundColor        = Color.White;
                dgv.BorderStyle            = BorderStyle.None;
                dgv.RowHeadersVisible      = false;
                dgv.GridColor              = Color.FromArgb(220, 230, 240);
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 78, 121);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                dgv.EnableHeadersVisualStyles               = false;
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 255);
            }
        }

        // ── Зареждане на лиги ─────────────────────────────────────────────────────
        private void LoadLeagues()
        {
            try
            {
                var leagues = _leaguesRepo.GetAll();
                dgvLeagues.DataSource = leagues;
                HideCol(dgvLeagues, "LeagueId");
                SetHdr(dgvLeagues, "Name",   "Лига");
                SetHdr(dgvLeagues, "Season", "Сезон");
                lblStatus.Text = $"Лиги: {leagues.Count}";
            }
            catch (Exception ex) { ShowError("Грешка при зареждане на лиги", ex); }
        }

        // ── Избор на лига → зарежда участниците ──────────────────────────────────
        private void dgvLeagues_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLeagues.CurrentRow?.DataBoundItem is League league)
            {
                _selectedLeagueId = league.LeagueId;
                txtName.Text      = league.Name;
                txtSeason.Text    = league.Season;
                LoadParticipants();
            }
        }

        // ── Зареждане на участници + налични клубове ──────────────────────────────
        private void LoadParticipants()
        {
            if (_selectedLeagueId == -1) return;
            try
            {
                // Участници
                var parts = _teamsRepo.GetParticipants(_selectedLeagueId);
                dgvParticipants.DataSource = parts;
                HideCol(dgvParticipants, "ClubId");
                HideCol(dgvParticipants, "CreatedAt");
                SetHdr(dgvParticipants, "Name", "Клуб");
                SetHdr(dgvParticipants, "City", "Град");

                // Налични клубове за добавяне
                var available = _teamsRepo.GetAvailableClubs(_selectedLeagueId);
                cboAvailableClubs.DataSource    = available;
                cboAvailableClubs.DisplayMember = "Name";
                cboAvailableClubs.ValueMember   = "ClubId";

                lblParticipantsCount.Text = $"Участници: {parts.Count}";
            }
            catch (Exception ex) { ShowError("Грешка при зареждане на участници", ex); }
        }

        // ══ CRUD ЛИГИ ════════════════════════════════════════════════════════════

        // ── ADD лига ──────────────────────────────────────────────────────────────
        private void btnAddLeague_Click(object sender, EventArgs e)
        {
            if (!ValidateLeagueInput()) return;
            try
            {
                _leaguesRepo.Create(new League { Name = txtName.Text, Season = txtSeason.Text });
                LoadLeagues();
                ClearLeagueForm();
                lblStatus.Text = "✔  Лигата е добавена.";
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                Warn($"Лига \"{txtName.Text}\" за сезон \"{txtSeason.Text}\" вече съществува!");
            }
            catch (Exception ex) { ShowError("Грешка при добавяне на лига", ex); }
        }

        // ── EDIT лига ─────────────────────────────────────────────────────────────
        private void btnEditLeague_Click(object sender, EventArgs e)
        {
            if (_selectedLeagueId == -1) { Warn("Изберете лига от списъка!"); return; }
            if (!ValidateLeagueInput()) return;
            try
            {
                _leaguesRepo.Update(new League
                {
                    LeagueId = _selectedLeagueId,
                    Name     = txtName.Text,
                    Season   = txtSeason.Text
                });
                LoadLeagues();
                lblStatus.Text = "✔  Лигата е обновена.";
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                Warn($"Лига \"{txtName.Text}\" за сезон \"{txtSeason.Text}\" вече съществува!");
            }
            catch (Exception ex) { ShowError("Грешка при редакция", ex); }
        }

        // ── DELETE лига ───────────────────────────────────────────────────────────
        private void btnDeleteLeague_Click(object sender, EventArgs e)
        {
            if (_selectedLeagueId == -1) { Warn("Изберете лига!"); return; }

            // Бизнес правило: не изтривай, ако има участници
            if (_leaguesRepo.HasParticipants(_selectedLeagueId))
            {
                MessageBox.Show(
                    "Не може да се изтрие лигата — има записани участници.\n" +
                    "Премахни всички клубове от лигата и опитай отново.",
                    "Забранено изтриване", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Изтриване на \"{txtName.Text} ({txtSeason.Text})\"?",
                "Потвърди", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _leaguesRepo.Delete(_selectedLeagueId);
                LoadLeagues();
                ClearLeagueForm();
                dgvParticipants.DataSource = null;
                cboAvailableClubs.DataSource = null;
                lblStatus.Text = "✔  Лигата е изтрита.";
            }
            catch (Exception ex) { ShowError("Грешка при изтриване", ex); }
        }

        // ══ УЧАСТНИЦИ ════════════════════════════════════════════════════════════

        // ── Добави клуб в лига ────────────────────────────────────────────────────
        private void btnAddClub_Click(object sender, EventArgs e)
        {
            if (_selectedLeagueId == -1) { Warn("Изберете лига!"); return; }
            if (cboAvailableClubs.SelectedItem is not Club club)
            { Warn("Изберете клуб за добавяне!"); return; }

            try
            {
                _teamsRepo.AddClub(_selectedLeagueId, club.ClubId);
                LoadParticipants();
                lblStatus.Text = $"✔  {club.Name} добавен в лигата.";
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                Warn($"\"{club.Name}\" вече участва в тази лига!");
            }
            catch (Exception ex) { ShowError("Грешка при добавяне на клуб", ex); }
        }

        // ── Премахни клуб от лига ─────────────────────────────────────────────────
        private void btnRemoveClub_Click(object sender, EventArgs e)
        {
            if (_selectedLeagueId == -1) { Warn("Изберете лига!"); return; }
            if (dgvParticipants.CurrentRow?.DataBoundItem is not Club club)
            { Warn("Изберете клуб от списъка с участници!"); return; }

            if (MessageBox.Show($"Премахване на \"{club.Name}\" от лигата?",
                "Потвърди", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _teamsRepo.RemoveClub(_selectedLeagueId, club.ClubId);
                LoadParticipants();
                lblStatus.Text = $"✔  {club.Name} премахнат от лигата.";
            }
            catch (Exception ex) { ShowError("Грешка при премахване", ex); }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLeagues();
            LoadParticipants();
        }

        private void btnClearForm_Click(object sender, EventArgs e) => ClearLeagueForm();

        // ── Валидация на лига ─────────────────────────────────────────────────────
        private bool ValidateLeagueInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            { Warn("Името на лигата не може да е празно!"); return false; }

            if (!Regex.IsMatch(txtSeason.Text.Trim(), @"^\d{4}/\d{4}$"))
            { Warn("Сезонът трябва да е във формат ГГГГ/ГГГГ (пример: 2025/2026)"); return false; }

            var parts = txtSeason.Text.Trim().Split('/');
            if (int.Parse(parts[1]) != int.Parse(parts[0]) + 1)
            { Warn("Втората година трябва да е с 1 повече от първата (пример: 2025/2026)"); return false; }

            return true;
        }

        private void ClearLeagueForm()
        {
            txtName.Text      = "";
            txtSeason.Text    = "";
            _selectedLeagueId = -1;
            dgvLeagues.ClearSelection();
        }

        // ── Помощни методи ────────────────────────────────────────────────────────
        private void SetHdr(DataGridView dgv, string col, string hdr)
        {
            if (dgv.Columns[col] != null) dgv.Columns[col]!.HeaderText = hdr;
        }
        private void HideCol(DataGridView dgv, string col)
        {
            if (dgv.Columns[col] != null) dgv.Columns[col]!.Visible = false;
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

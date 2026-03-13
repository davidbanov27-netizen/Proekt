using FootballLeague.Models;
using FootballLeague.Repositories;
using MySql.Data.MySqlClient;

namespace FootballLeague.Forms
{
    /// <summary>
    /// Главна форма за CRUD операции върху клубове.
    /// </summary>
    public partial class ClubsForm : Form
    {
        private readonly ClubsRepository _repo = new();
        private int _selectedClubId = -1;   // ID на избрания ред

        // ── Конструктор ─────────────────────────────────────────────────────────
        public ClubsForm()
        {
            InitializeComponent();
        }

        // ── Form Load ───────────────────────────────────────────────────────────
        private void ClubsForm_Load(object sender, EventArgs e)
        {
            ConfigureGrid();
            LoadClubs();
        }

        // ── Настройка на DataGridView ────────────────────────────────────────────
        private void ConfigureGrid()
        {
            dgvClubs.ReadOnly          = true;
            dgvClubs.SelectionMode     = DataGridViewSelectionMode.FullRowSelect;
            dgvClubs.MultiSelect       = false;
            dgvClubs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClubs.AllowUserToAddRows = false;
        }

        // ── Зареждане на всички клубове ─────────────────────────────────────────
        private void LoadClubs()
        {
            try
            {
                var clubs = _repo.GetAll();
                dgvClubs.DataSource = clubs;

                // Красиви заглавия на колоните
                if (dgvClubs.Columns.Count > 0)
                {
                    dgvClubs.Columns["ClubId"]?.SetHeaderName("ID");
                    dgvClubs.Columns["Name"]?.SetHeaderName("Клуб");
                    dgvClubs.Columns["City"]?.SetHeaderName("Град");
                    dgvClubs.Columns["CreatedAt"]?.SetHeaderName("Добавен на");
                }

                lblStatus.Text = $"Заредени {clubs.Count} клуба.";
            }
            catch (Exception ex)
            {
                ShowError("Грешка при зареждане на клубовете", ex);
            }
        }

        // ── Избор на ред → зареждане в полетата ─────────────────────────────────
        private void dgvClubs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClubs.CurrentRow?.DataBoundItem is Club club)
            {
                _selectedClubId = club.ClubId;
                txtName.Text    = club.Name;
                txtCity.Text    = club.City;
            }
        }

        // ── LOAD (бутон) ─────────────────────────────────────────────────────────
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadClubs();
        }

        // ── ADD ──────────────────────────────────────────────────────────────────
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                _repo.Add(new Club { Name = txtName.Text, City = txtCity.Text });
                LoadClubs();
                ClearFields();
                lblStatus.Text = $"✔  Клуб \"{txtName.Text}\" е добавен успешно.";
                MessageBox.Show("Клубът е добавен успешно!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
            {
                MessageBox.Show($"Клуб с името \"{txtName.Text}\" вече съществува!",
                    "Дублирано име", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("Грешка при добавяне", ex);
            }
        }

        // ── EDIT ─────────────────────────────────────────────────────────────────
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedClubId == -1)
            {
                MessageBox.Show("Моля, изберете клуб от списъка!",
                    "Няма избран клуб", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInput()) return;

            try
            {
                _repo.Update(new Club
                {
                    ClubId = _selectedClubId,
                    Name   = txtName.Text,
                    City   = txtCity.Text
                });
                LoadClubs();
                lblStatus.Text = $"✔  Клуб е обновен успешно.";
                MessageBox.Show("Данните са обновени!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show($"Клуб с името \"{txtName.Text}\" вече съществува!",
                    "Дублирано име", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("Грешка при редакция", ex);
            }
        }

        // ── DELETE ────────────────────────────────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedClubId == -1)
            {
                MessageBox.Show("Моля, изберете клуб от списъка!",
                    "Няма избран клуб", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Сигурни ли сте, че искате да изтриете клуб \"{txtName.Text}\"?",
                "Потвърждение за изтриване",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _repo.Delete(_selectedClubId);
                LoadClubs();
                ClearFields();
                lblStatus.Text = "✔  Клубът е изтрит.";
            }
            catch (MySqlException ex) when (ex.Number == 1451) // FK constraint
            {
                MessageBox.Show(
                    "Не може да се изтрие клубът, защото има свързани записи (играчи, мачове).\n" +
                    "Първо изтрийте свързаните данни.",
                    "Грешка – FK ограничение",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                ShowError("Грешка при изтриване", ex);
            }
        }

        // ── CLEAR ────────────────────────────────────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // ── Помощни методи ────────────────────────────────────────────────────────
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Полето \"Клуб\" не може да е празно!",
                    "Грешка при валидация", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            txtName.Text    = "";
            txtCity.Text    = "";
            _selectedClubId = -1;
            dgvClubs.ClearSelection();
        }

        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show($"{message}:\n\n{ex.Message}",
                "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = $"✘  {message}.";
        }
    }

    // Extension helper за заглавия на колони
    internal static class DataGridViewExtensions
    {
        public static void SetHeaderName(this DataGridViewColumn? col, string name)
        {
            if (col != null) col.HeaderText = name;
        }
    }
}

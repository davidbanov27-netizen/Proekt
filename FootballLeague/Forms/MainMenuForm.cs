namespace FootballLeague.Forms
{
    public class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            this.Text            = "Football League – Главно меню";
            this.Size            = new Size(360, 390);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = Color.FromArgb(240, 244, 248);
            this.Font            = new Font("Segoe UI", 10f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;

            var lblTitle = new Label
            {
                Text      = "⚽  Football League",
                Font      = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 78, 121),
                Location  = new Point(20, 20),
                AutoSize  = true
            };

            Button MakeBtn(string text, Color bg, int y, EventHandler handler)
            {
                var btn = new Button
                {
                    Text      = text,
                    Location  = new Point(40, y),
                    Size      = new Size(270, 44),
                    BackColor = bg,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Cursor    = Cursors.Hand
                };
                btn.Click += handler;
                return btn;
            }

            var btnClubs     = MakeBtn("🏟️  Управление на клубове",
                Color.FromArgb(30, 78, 121), 68,
                (s, e) => new ClubsForm().ShowDialog());

            var btnPlayers   = MakeBtn("👤  Управление на играчи",
                Color.FromArgb(39, 174, 96), 122,
                (s, e) => new PlayersForm().ShowDialog());

            var btnTransfers = MakeBtn("🔄  Трансфери",
                Color.FromArgb(142, 68, 173), 176,
                (s, e) => new TransfersForm().ShowDialog());

            var btnLeagues   = MakeBtn("🏆  Лиги и участници",
                Color.FromArgb(41, 128, 185), 230,
                (s, e) => new LeaguesForm().ShowDialog());

            this.Controls.AddRange(new Control[]
            {
                lblTitle, btnClubs, btnPlayers, btnTransfers, btnLeagues
            });
        }
    }
}

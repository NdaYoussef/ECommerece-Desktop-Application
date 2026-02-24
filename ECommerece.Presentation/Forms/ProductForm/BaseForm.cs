using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ECommerece.Presentation.Forms
{
    public class BaseForm : Form
    {
        // ── Exposed so child forms can add content into the main area ──
        protected Panel MainPanel   { get; private set; }
        protected Label LblPageTitle { get; private set; }

        // ── Sidebar buttons exposed so children can wire up navigation ──
        protected Button BtnDashboard  { get; private set; }
        protected Button BtnProducts   { get; private set; }
        protected Button BtnOrders     { get; private set; }
        protected Button BtnCustomers  { get; private set; }
        protected Button BtnLogout     { get; private set; }

        public BaseForm()
        {
            BuildShell();
        }

        private void BuildShell()
        {
            // ── Form ──────────────────────────────────────────────
            this.Text            = "E-Commerce Management System";
            this.Size            = new Size(1200, 700);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = Color.FromArgb(241, 245, 249);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.Font            = new Font("Segoe UI", 9f);

            // ── Sidebar ───────────────────────────────────────────
            var sidebar = new Panel
            {
                Size      = new Size(220, this.ClientSize.Height),
                Location  = new Point(0, 0),
                BackColor = Color.FromArgb(15, 23, 42)
            };
            this.Controls.Add(sidebar);

            var lblLogo = new Label
            {
                Text      = "🛒  E-Commerce",
                Font      = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize  = true,
                Location  = new Point(20, 30),
                BackColor = Color.Transparent
            };
            sidebar.Controls.Add(lblLogo);

            var separator = new Panel
            {
                Size      = new Size(180, 1),
                Location  = new Point(20, 70),
                BackColor = Color.FromArgb(30, 41, 59)
            };
            sidebar.Controls.Add(separator);

            BtnDashboard = CreateSidebarButton("📊  Dashboard",  90,  false);
            BtnProducts  = CreateSidebarButton("📦  Products",  145,  false);
            BtnOrders    = CreateSidebarButton("🧾  Orders",    200,  false);
            BtnCustomers = CreateSidebarButton("👥  Customers", 255,  false);

            sidebar.Controls.Add(BtnDashboard);
            sidebar.Controls.Add(BtnProducts);
            sidebar.Controls.Add(BtnOrders);
            sidebar.Controls.Add(BtnCustomers);

            BtnLogout = new Button
            {
                Text      = "🚪  Logout",
                Font      = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size      = new Size(220, 45),
                Location  = new Point(0, this.ClientSize.Height - 100),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(20, 0, 0, 0),
                Cursor    = Cursors.Hand
            };
            BtnLogout.FlatAppearance.BorderSize = 0;
            BtnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
            BtnLogout.Click += BtnLogout_Click;
            sidebar.Controls.Add(BtnLogout);

            // ── Main Panel ────────────────────────────────────────
            MainPanel = new Panel
            {
                Size      = new Size(this.ClientSize.Width - 220, this.ClientSize.Height),
                Location  = new Point(220, 0),
                BackColor = Color.FromArgb(241, 245, 249)
            };
            this.Controls.Add(MainPanel);

            // ── Header ────────────────────────────────────────────
            var headerPanel = new Panel
            {
                Size      = new Size(MainPanel.Width, 70),
                Location  = new Point(0, 0),
                BackColor = Color.White
            };
            headerPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawLine(pen, 0, headerPanel.Height - 1, headerPanel.Width, headerPanel.Height - 1);
            };
            MainPanel.Controls.Add(headerPanel);

            LblPageTitle = new Label
            {
                Text      = "",
                Font      = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize  = true,
                Location  = new Point(30, 20),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(LblPageTitle);

            var lblWelcome = new Label
            {
                Text      = "Welcome back, Admin 👋",
                Font      = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize  = true,
                BackColor = Color.Transparent
            };
            lblWelcome.Location = new Point(MainPanel.Width - lblWelcome.PreferredWidth - 30, 25);
            headerPanel.Controls.Add(lblWelcome);
        }

        // ── Helpers ───────────────────────────────────────────────

        /// <summary>Call from child constructor to highlight the correct nav button.</summary>
        protected void SetActiveNav(Button activeBtn)
        {
            foreach (Button btn in new[] { BtnDashboard, BtnProducts, BtnOrders, BtnCustomers })
            {
                btn.BackColor = Color.Transparent;
                btn.ForeColor = Color.FromArgb(148, 163, 184);
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
            }
            activeBtn.BackColor = Color.FromArgb(59, 91, 219);
            activeBtn.ForeColor = Color.White;
            activeBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(59, 91, 219);
        }

        /// <summary>Shared card factory — reusable in any child form.</summary>
        protected Panel CreateStatCard(string title, string value, string icon, Color accentColor, int x, int y)
        {
            var card = new Panel
            {
                Size      = new Size(185, 140),
                Location  = new Point(x, y),
                BackColor = Color.White
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                using (var brush = new SolidBrush(accentColor))
                    e.Graphics.FillRectangle(brush, 0, 0, card.Width, 4);
            };

            card.Controls.Add(new Label { Text = icon,  Font = new Font("Segoe UI", 22f),                    AutoSize = true, Location = new Point(15, 20),  BackColor = Color.Transparent });
            card.Controls.Add(new Label { Text = value, Font = new Font("Segoe UI", 22f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), AutoSize = true, Location = new Point(15, 65),  BackColor = Color.Transparent });
            card.Controls.Add(new Label { Text = title, Font = new Font("Segoe UI", 9f),                  ForeColor = Color.FromArgb(100, 116, 139), AutoSize = true, Location = new Point(15, 108), BackColor = Color.Transparent });

            return card;
        }

        private Button CreateSidebarButton(string text, int y, bool isActive)
        {
            var btn = new Button
            {
                Text      = text,
                Font      = new Font("Segoe UI", 10f),
                ForeColor = isActive ? Color.White : Color.FromArgb(148, 163, 184),
                BackColor = isActive ? Color.FromArgb(59, 91, 219) : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size      = new Size(220, 45),
                Location  = new Point(0, y),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(20, 0, 0, 0),
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = isActive ? Color.FromArgb(59, 91, 219) : Color.FromArgb(30, 41, 59);
            return btn;
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}

using ECommerece.Presentation.Forms.CategoryForms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ECommerece.Presentation.Forms.DashboardForms
{
    public class DashboardForm : Form
    {
        private readonly IServiceProvider _serviceProvider;

        public DashboardForm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponents();
            LoadStats();
        }
        // ===== CONTROLS =====
        private Panel sidebarPanel;
        private Panel mainPanel;
        private Panel headerPanel;

        // Sidebar Buttons
        private Button btnDashboard;
        private Button btnCategories;
        private Button btnProducts;
        private Button btnOrders;
        private Button btnCustomers;
        private Button btnLogout;

        // Header
        private Label lblPageTitle;
        private Label lblWelcome;

        // Stats Cards
        private Panel cardTotalProducts;
        private Panel cardTotalOrders;
        private Panel cardTotalCustomers;
        private Panel cardTotalRevenue;

       

        private void InitializeComponents()
        {
            // ===== FORM =====
            this.Text = "E-Commerce Management System";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9f);

            // ===== SIDEBAR =====
            sidebarPanel = new Panel
            {
                Size = new Size(220, this.ClientSize.Height),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(15, 23, 42)
            };
            this.Controls.Add(sidebarPanel);

            // Logo Label in Sidebar
            var lblLogo = new Label
            {
                Text = "🛒  E-Commerce",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 30),
                BackColor = Color.Transparent
            };
            sidebarPanel.Controls.Add(lblLogo);

            var separator = new Panel
            {
                Size = new Size(180, 1),
                Location = new Point(20, 70),
                BackColor = Color.FromArgb(30, 41, 59)
            };
            sidebarPanel.Controls.Add(separator);

            // Sidebar Buttons
            btnDashboard = CreateSidebarButton("📊  Dashboard", 90, true);
            btnCategories = CreateSidebarButton("🗂️  Categories", 145, false);
            btnProducts = CreateSidebarButton("📦  Products", 200, false);
            btnOrders = CreateSidebarButton("🧾  Orders", 255, false);
            btnCustomers = CreateSidebarButton("👥  Customers", 310, false);

            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(btnCategories);
            sidebarPanel.Controls.Add(btnProducts);
            sidebarPanel.Controls.Add(btnOrders);
            sidebarPanel.Controls.Add(btnCustomers);

            // Logout Button at bottom
            btnLogout = new Button
            {
                Text = "🚪  Logout",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 45),
                Location = new Point(0, this.ClientSize.Height - 100),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
            btnLogout.Click += BtnLogout_Click;
            sidebarPanel.Controls.Add(btnLogout);

            // ===== MAIN PANEL =====
            mainPanel = new Panel
            {
                Size = new Size(this.ClientSize.Width - 220, this.ClientSize.Height),
                Location = new Point(220, 0),
                BackColor = Color.FromArgb(241, 245, 249)
            };
            this.Controls.Add(mainPanel);

            // ===== HEADER =====
            headerPanel = new Panel
            {
                Size = new Size(mainPanel.Width, 70),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            headerPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawLine(pen, 0, headerPanel.Height - 1, headerPanel.Width, headerPanel.Height - 1);
            };
            mainPanel.Controls.Add(headerPanel);

            lblPageTitle = new Label
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(30, 20),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblPageTitle);

            lblWelcome = new Label
            {
                Text = "Welcome back, Admin 👋",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblWelcome.Location = new Point(mainPanel.Width - lblWelcome.PreferredWidth - 30, 25);
            headerPanel.Controls.Add(lblWelcome);

            // ===== STATS SECTION TITLE =====
            var lblStats = new Label
            {
                Text = "Overview",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 100),
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(lblStats);

            // ===== STAT CARDS =====
            cardTotalProducts = CreateStatCard("Total Products", "0", "📦", Color.FromArgb(59, 91, 219), 30, 135);
            cardTotalOrders = CreateStatCard("Total Orders", "0", "🧾", Color.FromArgb(16, 185, 129), 230, 135);
            cardTotalCustomers = CreateStatCard("Total Customers", "0", "👥", Color.FromArgb(245, 158, 11), 430, 135);
            cardTotalRevenue = CreateStatCard("Total Revenue", "$0", "💰", Color.FromArgb(239, 68, 68), 630, 135);

            mainPanel.Controls.Add(cardTotalProducts);
            mainPanel.Controls.Add(cardTotalOrders);
            mainPanel.Controls.Add(cardTotalCustomers);
            mainPanel.Controls.Add(cardTotalRevenue);

            // ===== RECENT ORDERS TABLE TITLE =====
            var lblRecent = new Label
            {
                Text = "Recent Orders",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 310),
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(lblRecent);

            // ===== RECENT ORDERS DataGridView =====
            var dgv = new DataGridView
            {
                Location = new Point(30, 345),
                Size = new Size(mainPanel.Width - 60, 270),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                GridColor = Color.FromArgb(226, 232, 240),
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9f),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.RowTemplate.Height = 40;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);

            dgv.Columns.Add("OrderId", "Order ID");
            dgv.Columns.Add("Customer", "Customer");
            dgv.Columns.Add("Date", "Date");
            dgv.Columns.Add("Amount", "Amount");
            dgv.Columns.Add("Status", "Status");

            // Demo Data
            dgv.Rows.Add("#1001", "Ahmed Ali", "2024-01-15", "$250.00", "✅ Delivered");
            dgv.Rows.Add("#1002", "Sara Mohamed", "2024-01-16", "$180.00", "🔄 Processing");
            dgv.Rows.Add("#1003", "Omar Hassan", "2024-01-17", "$320.00", "✅ Delivered");
            dgv.Rows.Add("#1004", "Nour Ahmed", "2024-01-18", "$95.00", "⏳ Pending");

            mainPanel.Controls.Add(dgv);

            //Event Onclick Category
            btnCategories.Click += (s, e) =>
            {
                var categoryForm = _serviceProvider.GetRequiredService<CategoryForm>();
                categoryForm.Show();
                this.Hide();
            };
        }


        private Button CreateSidebarButton(string text, int y, bool isActive)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10f),
                ForeColor = isActive ? Color.White : Color.FromArgb(148, 163, 184),
                BackColor = isActive ? Color.FromArgb(59, 91, 219) : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 45),
                Location = new Point(0, y),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = isActive
                ? Color.FromArgb(59, 91, 219)
                : Color.FromArgb(30, 41, 59);
            return btn;
        }

        private Panel CreateStatCard(string title, string value, string icon, Color accentColor, int x, int y)
        {
            var card = new Panel
            {
                Size = new Size(185, 140),
                Location = new Point(x, y),
                BackColor = Color.White,
                Cursor = Cursors.Default
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);

                // Accent top border
                using (SolidBrush brush = new SolidBrush(accentColor))
                    e.Graphics.FillRectangle(brush, 0, 0, card.Width, 4);
            };

            // Icon
            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 22f),
                AutoSize = true,
                Location = new Point(15, 20),
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblIcon);

            // Value
            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(15, 65),
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblValue);

            // Title
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(15, 108),
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblTitle);

            return card;
        }

        private void LoadStats()
        {
           
            UpdateCardValue(cardTotalProducts, "128");
            UpdateCardValue(cardTotalOrders, "54");
            UpdateCardValue(cardTotalCustomers, "320");
            UpdateCardValue(cardTotalRevenue, "$12,400");
        }

        private void UpdateCardValue(Panel card, string value)
        {
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is Label lbl && lbl.Font.Size == 22f && lbl.Font.Bold)
                {
                    lbl.Text = value;
                    break;
                }
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
using ECommerece.Application.IServices.IUserService;
//using ECommerece.Presentation.Forms.CartForms;
using ECommerece.Presentation.Forms.ProductForms;
using ECommerece.Presentation.Forms.UserForms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ECommerece.Presentation.Forms.DashboardForms
{
    public class CustomerDashboardForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private Panel sidebarPanel;
        private Panel mainPanel;
        private Panel headerPanel;
        private Button btnDashboard;
        private Button btnProducts;
        private Button btnCart;
        private Button btnOrders;
        private Button btnLogout;
        private Label lblPageTitle;
        private Label lblWelcome;
        private string _userName = "Customer";

        public CustomerDashboardForm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponents();
        }

        public void SetUserName(string name)
        {
            _userName = name;
            lblWelcome.Text = $"Welcome back, {name}! 👋";
        }

        private void InitializeComponents()
        {
            this.Text = "E-Commerce";
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

            var lblSub = new Label
            {
                Text = "Management",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(47, 55),
                BackColor = Color.Transparent
            };
            sidebarPanel.Controls.Add(lblSub);

            var separator = new Panel
            {
                Size = new Size(180, 1),
                Location = new Point(20, 80),
                BackColor = Color.FromArgb(30, 41, 59)
            };
            sidebarPanel.Controls.Add(separator);

            btnDashboard = CreateSidebarButton("📊  Dashboard", 100, true);
            btnProducts = CreateSidebarButton("🛍️  Browse Products", 155, false);
            btnCart = CreateSidebarButton("🛒  Cart", 210, false);
            btnOrders = CreateSidebarButton("📦  My Orders", 265, false);

            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(btnProducts);
            sidebarPanel.Controls.Add(btnCart);
            sidebarPanel.Controls.Add(btnOrders);

            btnProducts.Click += (s, e) =>
            {
                var productForm = _serviceProvider.GetRequiredService<CustomerProductsForm>();
                productForm.Show();
                this.Hide();
            };

            btnCart.Click += (s, e) =>
            {
                var cartForm = _serviceProvider.GetRequiredService<CartForm>();
                cartForm.Show();
                this.Hide();
            };

            btnOrders.Click += (s, e) =>
            {
                var orderForm = _serviceProvider.GetRequiredService<CustomerOrderManagementForm>();
                orderForm.Show();
                this.Hide();
            };

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
                Text = $"Welcome back, {_userName}! 👋",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblWelcome.Location = new Point(mainPanel.Width - lblWelcome.PreferredWidth - 30, 25);
            headerPanel.Controls.Add(lblWelcome);

            // ===== STATS CARDS =====
            var lblStats = new Label
            {
                Text = "My Overview",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 100),
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(lblStats);

            mainPanel.Controls.Add(CreateStatCard("Total Orders", "8", "📦", Color.FromArgb(59, 91, 219), 30, 135));
            mainPanel.Controls.Add(CreateStatCard("Items in Cart", "3", "🛒", Color.FromArgb(16, 185, 129), 230, 135));
            mainPanel.Controls.Add(CreateStatCard("Wishlist Items", "12", "❤️", Color.FromArgb(239, 68, 68), 430, 135));
            mainPanel.Controls.Add(CreateStatCard("Total Spent", "$6,248", "💰", Color.FromArgb(245, 158, 11), 630, 135));

            // ===== QUICK ACTIONS =====
            var lblActions = new Label
            {
                Text = "Quick Actions",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 310),
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(lblActions);

            mainPanel.Controls.Add(CreateActionCard("Browse Products", "Explore our wide selection of products", "Start Shopping →", Color.FromArgb(59, 91, 219), 30, 345));
            mainPanel.Controls.Add(CreateActionCard("My Cart", "Review items in your shopping cart", "View Cart →", Color.FromArgb(16, 185, 129), 430, 345));
            mainPanel.Controls.Add(CreateActionCard("My Orders", "Track your order history and status", "View Orders →", Color.FromArgb(239, 68, 68), 630, 345));
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
                BackColor = Color.White
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                using (SolidBrush brush = new SolidBrush(accentColor))
                    e.Graphics.FillRectangle(brush, 0, 0, card.Width, 4);
            };

            card.Controls.Add(new Label { Text = icon, Font = new Font("Segoe UI", 22f), AutoSize = true, Location = new Point(15, 20), BackColor = Color.Transparent });
            card.Controls.Add(new Label { Text = value, Font = new Font("Segoe UI", 22f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), AutoSize = true, Location = new Point(15, 65), BackColor = Color.Transparent });
            card.Controls.Add(new Label { Text = title, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(100, 116, 139), AutoSize = true, Location = new Point(15, 108), BackColor = Color.Transparent });

            return card;
        }

        private Panel CreateActionCard(string title, string desc, string linkText, Color linkColor, int x, int y)
        {
            var card = new Panel
            {
                Size = new Size(185, 200),
                Location = new Point(x, y),
                BackColor = Color.White
            };
            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            card.Controls.Add(new Label { Text = title, Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), AutoSize = true, Location = new Point(15, 20), BackColor = Color.Transparent });
            card.Controls.Add(new Label { Text = desc, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(100, 116, 139), Size = new Size(155, 50), Location = new Point(15, 50), BackColor = Color.Transparent });

            var lnk = new LinkLabel { Text = linkText, Font = new Font("Segoe UI", 9f), AutoSize = true, Location = new Point(15, 110), BackColor = Color.Transparent, LinkColor = linkColor };
            card.Controls.Add(lnk);

            return card;
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var loginForm = _serviceProvider.GetRequiredService<LoginForm>();
                loginForm.Show();
                this.Close();
            }
        }
    }
}
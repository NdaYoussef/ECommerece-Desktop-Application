//using ECommerece.Application.IServices.ICartService;
//using ECommerece.Application.IServices.IUserService;
////using ECommerece.Presentation.Forms.CartForms;
//using ECommerece.Presentation.Forms.ProductForms;
//using ECommerece.Presentation.Forms.UserForms;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Windows.Forms;

//namespace ECommerece.Presentation.Forms.DashboardForms
//{
//    public class CartForm : Form
//    {
//        private readonly IServiceProvider _serviceProvider;
//        private readonly ICartServices _cartServices;
//        private Panel sidebarPanel;
//        private Panel mainPanel;
//        private Panel headerPanel;
//        private Button btnDashboard;
//        private Button btnProducts;
//        private Button btnCart;
//        private Button btnOrders;
//        private Button btnLogout;
//        private Label lblPageTitle;
//        private Label lblWelcome;
//        private string _userName = "Customer";

//        public CartForm(ICartServices cartServices, IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//            _cartServices = cartServices;
//            InitializeComponents();
//        }

//        public void SetUserName(string name)
//        {
//            _userName = name;
//            lblWelcome.Text = $"Welcome back, {name}! 👋";
//        }

//        private void InitializeComponents()
//        {
//            this.Text = "E-Commerce";
//            this.Size = new Size(1200, 700);
//            this.StartPosition = FormStartPosition.CenterScreen;
//            this.BackColor = Color.FromArgb(241, 245, 249);
//            this.FormBorderStyle = FormBorderStyle.FixedSingle;
//            this.MaximizeBox = false;
//            this.Font = new Font("Segoe UI", 9f);

//            // ===== SIDEBAR =====
//            sidebarPanel = new Panel
//            {
//                Size = new Size(220, this.ClientSize.Height),
//                Location = new Point(0, 0),
//                BackColor = Color.FromArgb(15, 23, 42)
//            };
//            this.Controls.Add(sidebarPanel);

//            var lblLogo = new Label
//            {
//                Text = "🛒  E-Commerce",
//                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(20, 30),
//                BackColor = Color.Transparent
//            };
//            sidebarPanel.Controls.Add(lblLogo);

//            var lblSub = new Label
//            {
//                Text = "Management",
//                Font = new Font("Segoe UI", 9f),
//                ForeColor = Color.FromArgb(100, 116, 139),
//                AutoSize = true,
//                Location = new Point(47, 55),
//                BackColor = Color.Transparent
//            };
//            sidebarPanel.Controls.Add(lblSub);

//            var separator = new Panel
//            {
//                Size = new Size(180, 1),
//                Location = new Point(20, 80),
//                BackColor = Color.FromArgb(30, 41, 59)
//            };
//            sidebarPanel.Controls.Add(separator);

//            btnDashboard = CreateSidebarButton("📊  Dashboard", 100, false);
//            btnProducts = CreateSidebarButton("🛍️  Browse Products", 155, false);
//            btnCart = CreateSidebarButton("🛒  Cart", 210, true);
//            btnOrders = CreateSidebarButton("📦  My Orders", 265, false);

//            sidebarPanel.Controls.Add(btnDashboard);
//            sidebarPanel.Controls.Add(btnProducts);
//            sidebarPanel.Controls.Add(btnCart);
//            sidebarPanel.Controls.Add(btnOrders);

//            btnProducts.Click += (s, e) =>
//            {
//                var productForm = _serviceProvider.GetRequiredService<CustomerProductsForm>();
//                productForm.Show();
//                this.Hide();
//            };

//            btnCart.Click += (s, e) =>
//            {
//                var cartForm = _serviceProvider.GetRequiredService<CartForm>();
//                cartForm.Show();
//                this.Hide();
//            };

//            btnDashboard.Click += (s, e) =>
//            {
//                var dashboardForm = _serviceProvider.GetRequiredService<CustomerDashboardForm>();
//                dashboardForm.Show();
//                this.Hide();
//            };

//            btnLogout = new Button
//            {
//                Text = "🚪  Logout",
//                Font = new Font("Segoe UI", 10f),
//                ForeColor = Color.FromArgb(148, 163, 184),
//                BackColor = Color.Transparent,
//                FlatStyle = FlatStyle.Flat,
//                Size = new Size(220, 45),
//                Location = new Point(0, this.ClientSize.Height - 100),
//                TextAlign = ContentAlignment.MiddleLeft,
//                Padding = new Padding(20, 0, 0, 0),
//                Cursor = Cursors.Hand
//            };
//            btnLogout.FlatAppearance.BorderSize = 0;
//            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
//            btnLogout.Click += BtnLogout_Click;
//            sidebarPanel.Controls.Add(btnLogout);

//            // ===== MAIN PANEL =====
//            mainPanel = new Panel
//            {
//                Size = new Size(this.ClientSize.Width - 220, this.ClientSize.Height),
//                Location = new Point(220, 0),
//                BackColor = Color.FromArgb(241, 245, 249)
//            };
//            this.Controls.Add(mainPanel);

//            // ===== HEADER =====
//            headerPanel = new Panel
//            {
//                Size = new Size(mainPanel.Width, 70),
//                Location = new Point(0, 0),
//                BackColor = Color.White
//            };
//            headerPanel.Paint += (s, e) =>
//            {
//                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
//                    e.Graphics.DrawLine(pen, 0, headerPanel.Height - 1, headerPanel.Width, headerPanel.Height - 1);
//            };
//            mainPanel.Controls.Add(headerPanel);

//            lblPageTitle = new Label
//            {
//                Text = "Dashboard",
//                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
//                ForeColor = Color.FromArgb(15, 23, 42),
//                AutoSize = true,
//                Location = new Point(30, 20),
//                BackColor = Color.Transparent
//            };
//            headerPanel.Controls.Add(lblPageTitle);

//            lblWelcome = new Label
//            {
//                Text = $"Welcome back, {_userName}! 👋",
//                Font = new Font("Segoe UI", 10f),
//                ForeColor = Color.FromArgb(100, 116, 139),
//                AutoSize = true,
//                BackColor = Color.Transparent
//            };
//            lblWelcome.Location = new Point(mainPanel.Width - lblWelcome.PreferredWidth - 30, 25);
//            headerPanel.Controls.Add(lblWelcome);

//            // ===== STATS CARDS =====
//            var lblStats = new Label
//            {
//                Text = "My Overview",
//                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
//                ForeColor = Color.FromArgb(30, 41, 59),
//                AutoSize = true,
//                Location = new Point(30, 100),
//                BackColor = Color.Transparent
//            };
//            mainPanel.Controls.Add(lblStats);

//            mainPanel.Controls.Add(CreateStatCard("Total Orders", "8", "📦", Color.FromArgb(59, 91, 219), 30, 135));
//            mainPanel.Controls.Add(CreateStatCard("Items in Cart", "3", "🛒", Color.FromArgb(16, 185, 129), 230, 135));
//            mainPanel.Controls.Add(CreateStatCard("Wishlist Items", "12", "❤️", Color.FromArgb(239, 68, 68), 430, 135));
//            mainPanel.Controls.Add(CreateStatCard("Total Spent", "$6,248", "💰", Color.FromArgb(245, 158, 11), 630, 135));

//            // ===== QUICK ACTIONS =====
//            var lblActions = new Label
//            {
//                Text = "Quick Actions",
//                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
//                ForeColor = Color.FromArgb(30, 41, 59),
//                AutoSize = true,
//                Location = new Point(30, 310),
//                BackColor = Color.Transparent
//            };
//            mainPanel.Controls.Add(lblActions);

//            mainPanel.Controls.Add(CreateActionCard("Browse Products", "Explore our wide selection of products", "Start Shopping →", Color.FromArgb(59, 91, 219), 30, 345));
//            mainPanel.Controls.Add(CreateActionCard("My Cart", "Review items in your shopping cart", "View Cart →", Color.FromArgb(16, 185, 129), 430, 345));
//            mainPanel.Controls.Add(CreateActionCard("My Orders", "Track your order history and status", "View Orders →", Color.FromArgb(239, 68, 68), 630, 345));
//        }

//        private Button CreateSidebarButton(string text, int y, bool isActive)
//        {
//            var btn = new Button
//            {
//                Text = text,
//                Font = new Font("Segoe UI", 10f),
//                ForeColor = isActive ? Color.White : Color.FromArgb(148, 163, 184),
//                BackColor = isActive ? Color.FromArgb(59, 91, 219) : Color.Transparent,
//                FlatStyle = FlatStyle.Flat,
//                Size = new Size(220, 45),
//                Location = new Point(0, y),
//                TextAlign = ContentAlignment.MiddleLeft,
//                Padding = new Padding(20, 0, 0, 0),
//                Cursor = Cursors.Hand
//            };
//            btn.FlatAppearance.BorderSize = 0;
//            btn.FlatAppearance.MouseOverBackColor = isActive
//                ? Color.FromArgb(59, 91, 219)
//                : Color.FromArgb(30, 41, 59);
//            return btn;
//        }

//        private Panel CreateStatCard(string title, string value, string icon, Color accentColor, int x, int y)
//        {
//            var card = new Panel
//            {
//                Size = new Size(185, 140),
//                Location = new Point(x, y),
//                BackColor = Color.White
//            };
//            card.Paint += (s, e) =>
//            {
//                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
//                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
//                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
//                using (SolidBrush brush = new SolidBrush(accentColor))
//                    e.Graphics.FillRectangle(brush, 0, 0, card.Width, 4);
//            };

//            card.Controls.Add(new Label { Text = icon, Font = new Font("Segoe UI", 22f), AutoSize = true, Location = new Point(15, 20), BackColor = Color.Transparent });
//            card.Controls.Add(new Label { Text = value, Font = new Font("Segoe UI", 22f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), AutoSize = true, Location = new Point(15, 65), BackColor = Color.Transparent });
//            card.Controls.Add(new Label { Text = title, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(100, 116, 139), AutoSize = true, Location = new Point(15, 108), BackColor = Color.Transparent });

//            return card;
//        }

//        private Panel CreateActionCard(string title, string desc, string linkText, Color linkColor, int x, int y)
//        {
//            var card = new Panel
//            {
//                Size = new Size(185, 200),
//                Location = new Point(x, y),
//                BackColor = Color.White
//            };
//            card.Paint += (s, e) =>
//            {
//                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
//                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
//            };

//            card.Controls.Add(new Label { Text = title, Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), AutoSize = true, Location = new Point(15, 20), BackColor = Color.Transparent });
//            card.Controls.Add(new Label { Text = desc, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(100, 116, 139), Size = new Size(155, 50), Location = new Point(15, 50), BackColor = Color.Transparent });

//            var lnk = new LinkLabel { Text = linkText, Font = new Font("Segoe UI", 9f), AutoSize = true, Location = new Point(15, 110), BackColor = Color.Transparent, LinkColor = linkColor };
//            card.Controls.Add(lnk);

//            return card;
//        }

//        private void BtnLogout_Click(object sender, EventArgs e)
//        {
//            var confirm = MessageBox.Show("Are you sure you want to logout?", "Logout",
//                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//            if (confirm == DialogResult.Yes)
//            {
//                var loginForm = _serviceProvider.GetRequiredService<LoginForm>();
//                loginForm.Show();
//                this.Close();
//            }
//        }
//    }
//}






using ECommerece.Application.DTOs.CartDto;
using ECommerece.Application.IServices.ICartService;
using ECommerece.Presentation.Forms.ProductForms;
using ECommerece.Presentation.Forms.UserForms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECommerece.Presentation.Forms.DashboardForms
{
    public class CartForm : Form
    {
        // ── Services ────────────────────────────────────────────────────────────
        private readonly IServiceProvider _serviceProvider;
        private readonly ICartServices _cartServices;

        // ── State ────────────────────────────────────────────────────────────────
        private string _userName = "Customer";
        private string _userId = string.Empty;
        private CartDTO _cartData;

        // ── Layout panels ────────────────────────────────────────────────────────
        private Panel sidebarPanel;
        private Panel mainPanel;
        private Panel headerPanel;

        // ── Sidebar buttons ──────────────────────────────────────────────────────
        private Button btnDashboard;
        private Button btnProducts;
        private Button btnCart;
        private Button btnOrders;
        private Button btnLogout;

        // ── Header labels ────────────────────────────────────────────────────────
        private Label lblPageTitle;
        private Label lblWelcome;

        // ── Cart content area ────────────────────────────────────────────────────
        private Panel cartContentPanel;   // white card that holds everything
        private Panel itemsScrollPanel;   // scrollable list of cart rows
        private Label lblShoppingCart;
        private Label lblItemCount;
        private Label lblEmpty;

        // ── Total footer ─────────────────────────────────────────────────────────
        private Panel totalPanel;
        private Label lblSubtotalTitle;
        private Label lblSubtotalValue;
        private Label lblTaxTitle;
        private Label lblTaxValue;
        private Panel divider;
        private Label lblTotalTitle;
        private Label lblTotalValue;
        private Button btnCheckout;
        private Button btnClearCart;

        // ── Loading overlay ──────────────────────────────────────────────────────
        private Panel loadingOverlay;
        private Label lblLoading;

        // ─────────────────────────────────────────────────────────────────────────
        public CartForm(ICartServices cartServices, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cartServices = cartServices;
            InitializeComponents();
        }

        // ── Public setters called by navigation ──────────────────────────────────
        public void SetUserName(string name)
        {
            _userName = name;
            if (lblWelcome != null)
                lblWelcome.Text = $"Welcome back, {name}! 👋";
        }

        public void SetUserId(string userId)
        {
            _userId = userId;
        }

        // ═════════════════════════════════════════════════════════════════════════
        //  BUILD UI
        // ═════════════════════════════════════════════════════════════════════════
        private void InitializeComponents()
        {
            this.Text = "E-Commerce – My Cart";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9f);

            BuildSidebar();
            BuildMainPanel();
            BuildLoadingOverlay();

            // Load cart data when form is shown
            this.Shown += async (s, e) => await LoadCartAsync();
        }

        // ── SIDEBAR ───────────────────────────────────────────────────────────────
        private void BuildSidebar()
        {
            sidebarPanel = new Panel
            {
                Size = new Size(220, this.ClientSize.Height),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(15, 23, 42)
            };
            this.Controls.Add(sidebarPanel);

            // Logo
            sidebarPanel.Controls.Add(new Label
            {
                Text = "🛒  E-Commerce",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 30),
                BackColor = Color.Transparent
            });
            sidebarPanel.Controls.Add(new Label
            {
                Text = "Management",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(47, 55),
                BackColor = Color.Transparent
            });
            sidebarPanel.Controls.Add(new Panel
            {
                Size = new Size(180, 1),
                Location = new Point(20, 80),
                BackColor = Color.FromArgb(30, 41, 59)
            });

            // Nav buttons
            btnDashboard = CreateSidebarButton("📊  Dashboard", 100, false);
            btnProducts = CreateSidebarButton("🛍️  Browse Products", 155, false);
            btnCart = CreateSidebarButton("🛒  Cart", 210, true);
            btnOrders = CreateSidebarButton("📦  My Orders", 265, false);

            sidebarPanel.Controls.AddRange(new Control[]
                { btnDashboard, btnProducts, btnCart, btnOrders });

            btnDashboard.Click += (s, e) => NavigateTo<CustomerDashboardForm>(f => f.SetUserName(_userName));
            btnProducts.Click += (s, e) => NavigateTo<CustomerProductsForm>();
            btnCart.Click += (s, e) => { /* already here */ };

            // Logout
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
        }

        // ── MAIN PANEL ────────────────────────────────────────────────────────────
        private void BuildMainPanel()
        {
            mainPanel = new Panel
            {
                Size = new Size(this.ClientSize.Width - 220, this.ClientSize.Height),
                Location = new Point(220, 0),
                BackColor = Color.FromArgb(241, 245, 249)
            };
            this.Controls.Add(mainPanel);

            // Header bar
            headerPanel = new Panel
            {
                Size = new Size(mainPanel.Width, 70),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            headerPanel.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
                e.Graphics.DrawLine(pen, 0, headerPanel.Height - 1,
                                    headerPanel.Width, headerPanel.Height - 1);
            };
            mainPanel.Controls.Add(headerPanel);

            lblPageTitle = new Label
            {
                Text = "My Cart",
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

            // ── White cart card ───────────────────────────────────────────────
            cartContentPanel = new Panel
            {
                Size = new Size(mainPanel.Width - 60, mainPanel.Height - 110),
                Location = new Point(30, 90),
                BackColor = Color.White
            };
            cartContentPanel.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
                e.Graphics.DrawRectangle(pen, 0, 0,
                    cartContentPanel.Width - 1, cartContentPanel.Height - 1);
            };
            mainPanel.Controls.Add(cartContentPanel);

            // Card header
            lblShoppingCart = new Label
            {
                Text = "Shopping Cart",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(25, 22),
                BackColor = Color.Transparent
            };
            cartContentPanel.Controls.Add(lblShoppingCart);

            lblItemCount = new Label
            {
                Text = "Loading…",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(25, 48),
                BackColor = Color.Transparent
            };
            cartContentPanel.Controls.Add(lblItemCount);

            // Thin separator under header
            cartContentPanel.Controls.Add(new Panel
            {
                Size = new Size(cartContentPanel.Width, 1),
                Location = new Point(0, 70),
                BackColor = Color.FromArgb(226, 232, 240)
            });

            // Scrollable items area (leaves room at bottom for totals)
            itemsScrollPanel = new Panel
            {
                Size = new Size(cartContentPanel.Width, cartContentPanel.Height - 210),
                Location = new Point(0, 71),
                BackColor = Color.White,
                AutoScroll = true
            };
            cartContentPanel.Controls.Add(itemsScrollPanel);

            // Empty-cart label (hidden until needed)
            lblEmpty = new Label
            {
                Text = "🛒  Your cart is empty",
                Font = new Font("Segoe UI", 13f),
                ForeColor = Color.FromArgb(148, 163, 184),
                AutoSize = true,
                Visible = false,
                BackColor = Color.Transparent
            };
            itemsScrollPanel.Controls.Add(lblEmpty);

            // ── Total panel ───────────────────────────────────────────────────
            BuildTotalPanel();
        }

        // ── TOTAL FOOTER ──────────────────────────────────────────────────────────
        private void BuildTotalPanel()
        {
            int totalH = 200;
            totalPanel = new Panel
            {
                Size = new Size(cartContentPanel.Width, totalH),
                Location = new Point(0, cartContentPanel.Height - totalH),
                BackColor = Color.White
            };
            totalPanel.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
                e.Graphics.DrawLine(pen, 0, 0, totalPanel.Width, 0);
            };
            cartContentPanel.Controls.Add(totalPanel);

            int rightEdge = totalPanel.Width - 40;

            // Subtotal row
            lblSubtotalTitle = CreateTotalLabel("Subtotal", 20, 18f, false);
            lblSubtotalValue = CreateTotalLabel("$0.00", rightEdge, 18f, false);
            lblSubtotalValue.TextAlign = ContentAlignment.MiddleRight;

            // Tax row (10%)
            lblTaxTitle = CreateTotalLabel("Tax (10%)", 20, 46f, false);
            lblTaxValue = CreateTotalLabel("$0.00", rightEdge, 46f, false);
            lblTaxValue.TextAlign = ContentAlignment.MiddleRight;

            // Divider
            divider = new Panel
            {
                Size = new Size(totalPanel.Width - 80, 1),
                Location = new Point(40, 75),
                BackColor = Color.FromArgb(226, 232, 240)
            };

            // Grand total row
            lblTotalTitle = CreateTotalLabel("Total", 20, 88f, true);
            lblTotalValue = CreateTotalLabel("$0.00", rightEdge, 88f, true);
            lblTotalValue.ForeColor = Color.FromArgb(59, 91, 219);
            lblTotalValue.TextAlign = ContentAlignment.MiddleRight;

            totalPanel.Controls.AddRange(new Control[]
            {
                lblSubtotalTitle, lblSubtotalValue,
                lblTaxTitle,      lblTaxValue,
                divider,
                lblTotalTitle,    lblTotalValue
            });

            // Checkout button
            btnCheckout = new Button
            {
                Text = "Proceed to Checkout  →",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 91, 219),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(260, 46),
                Location = new Point(totalPanel.Width - 300, 130),
                Cursor = Cursors.Hand
            };
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 78, 196);
            btnCheckout.Click += (s, e) =>
                MessageBox.Show("Checkout feature coming soon!", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            totalPanel.Controls.Add(btnCheckout);

            // Clear cart button
            btnClearCart = new Button
            {
                Text = "🗑  Clear Cart",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(239, 68, 68),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 46),
                Location = new Point(totalPanel.Width - 300 - 145, 130),
                Cursor = Cursors.Hand
            };
            btnClearCart.FlatAppearance.BorderColor = Color.FromArgb(239, 68, 68);
            btnClearCart.FlatAppearance.BorderSize = 1;
            btnClearCart.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            btnClearCart.Click += async (s, e) => await ClearCartAsync();
            totalPanel.Controls.Add(btnClearCart);
        }

        // Helper: creates a label in the total panel
        private Label CreateTotalLabel(string text, float x, float y, bool bold)
        {
            return new Label
            {
                Text = text,
                Font = bold
                            ? new Font("Segoe UI", 12f, FontStyle.Bold)
                            : new Font("Segoe UI", 10f),
                ForeColor = bold
                            ? Color.FromArgb(15, 23, 42)
                            : Color.FromArgb(71, 85, 105),
                AutoSize = true,
                Location = new Point((int)x, (int)y),
                BackColor = Color.Transparent
            };
        }

        // ── LOADING OVERLAY ───────────────────────────────────────────────────────
        private void BuildLoadingOverlay()
        {
            loadingOverlay = new Panel
            {
                Size = new Size(mainPanel.Width, mainPanel.Height),
                Location = new Point(220, 0),
                BackColor = Color.FromArgb(180, 241, 245, 249),
                Visible = false
            };
            lblLoading = new Label
            {
                Text = "Loading your cart…",
                Font = new Font("Segoe UI", 14f),
                ForeColor = Color.FromArgb(59, 91, 219),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            loadingOverlay.Controls.Add(lblLoading);
            loadingOverlay.Resize += (s, e) =>
                lblLoading.Location = new Point(
                    (loadingOverlay.Width - lblLoading.Width) / 2,
                    (loadingOverlay.Height - lblLoading.Height) / 2);
            this.Controls.Add(loadingOverlay);
            loadingOverlay.BringToFront();
        }

        // ═════════════════════════════════════════════════════════════════════════
        //  DATA LOADING
        // ═════════════════════════════════════════════════════════════════════════
        private async Task LoadCartAsync()
        {
            ShowLoading(true);
            try
            {
                if (string.IsNullOrEmpty(_userId))
                {
                    RenderEmpty();
                    return;
                }

                _cartData = await _cartServices.GetCartByUserIdAsync(_userId);
                RenderCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load cart: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                RenderEmpty();
            }
            finally
            {
                ShowLoading(false);
            }
        }

        // ── RENDER FULL CART ──────────────────────────────────────────────────────
        private void RenderCart()
        {
            itemsScrollPanel.Controls.Clear();

            bool isEmpty = _cartData == null || _cartData.Items == null || _cartData.Items.Count == 0;

            if (isEmpty)
            {
                RenderEmpty();
                return;
            }

            lblItemCount.Text = $"{_cartData.TotalItems} item{(_cartData.TotalItems == 1 ? "" : "s")} in your cart";

            int yOffset = 0;
            foreach (var item in _cartData.Items)
            {
                var row = BuildCartRow(item, yOffset);
                itemsScrollPanel.Controls.Add(row);
                yOffset += row.Height;
            }

            UpdateTotals(_cartData.TotalAmount);
            btnCheckout.Enabled = true;
            btnClearCart.Enabled = true;
        }

        private void RenderEmpty()
        {
            itemsScrollPanel.Controls.Clear();

            lblItemCount.Text = "0 items in your cart";

            lblEmpty.Visible = true;
            lblEmpty.Location = new Point(
                (itemsScrollPanel.Width - lblEmpty.PreferredWidth) / 2,
                (itemsScrollPanel.Height - lblEmpty.PreferredHeight) / 2);
            itemsScrollPanel.Controls.Add(lblEmpty);

            UpdateTotals(0);
            btnCheckout.Enabled = false;
            btnClearCart.Enabled = false;
        }

        // ── BUILD A SINGLE CART ROW ───────────────────────────────────────────────
        private Panel BuildCartRow(CartItemDTO item, int yOffset)
        {
            int rowH = 100;

            var row = new Panel
            {
                Size = new Size(itemsScrollPanel.Width - 2, rowH),
                Location = new Point(0, yOffset),
                BackColor = Color.White
            };
            row.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
                e.Graphics.DrawLine(pen, 0, row.Height - 1, row.Width, row.Height - 1);
            };

            // ── Product image ─────────────────────────────────────────────────
            var imgBox = new PictureBox
            {
                Size = new Size(80, 80),
                Location = new Point(20, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(241, 245, 249),
                BorderStyle = BorderStyle.None
            };
            LoadImageAsync(imgBox, item.ProductImage);
            row.Controls.Add(imgBox);

            // ── Product name & unit price ─────────────────────────────────────
            var lblName = new Label
            {
                Text = item.ProductName ?? "Unknown Product",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(120, 22),
                BackColor = Color.Transparent
            };
            row.Controls.Add(lblName);

            var lblUnitPrice = new Label
            {
                Text = $"${item.UnitPrice:F2} each",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(120, 48),
                BackColor = Color.Transparent
            };
            row.Controls.Add(lblUnitPrice);

            // ── Quantity controls  ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ──
            int qtyX = row.Width - 380;
            int centerY = (rowH - 32) / 2;

            var btnMinus = CreateQtyButton("−", qtyX, centerY);
            row.Controls.Add(btnMinus);

            var lblQty = new Label
            {
                Text = item.Quantity.ToString(),
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Size = new Size(40, 32),
                Location = new Point(qtyX + 36, centerY),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            row.Controls.Add(lblQty);

            var btnPlus = CreateQtyButton("+", qtyX + 76, centerY);
            row.Controls.Add(btnPlus);

            // ── Line total ────────────────────────────────────────────────────
            var lblLineTotal = new Label
            {
                Text = $"${item.TotalPrice:F2}",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Size = new Size(100, 32),
                Location = new Point(row.Width - 185, centerY),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };
            row.Controls.Add(lblLineTotal);

            // ── Delete button ─────────────────────────────────────────────────
            var btnDelete = new Button
            {
                Text = "🗑",
                Font = new Font("Segoe UI", 13f),
                ForeColor = Color.FromArgb(239, 68, 68),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(38, 38),
                Location = new Point(row.Width - 68, (rowH - 38) / 2),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 242, 242);
            btnDelete.Click += async (s, e) => await RemoveItemAsync(item.Id, item.ProductId);
            row.Controls.Add(btnDelete);

            // ── Wire up qty +/− ────────────────────────────────────────────────
            int currentQty = item.Quantity;

            btnMinus.Click += async (s, e) =>
            {
                if (currentQty <= 1) return;
                currentQty--;
                lblQty.Text = currentQty.ToString();
                decimal newTotal = currentQty * item.UnitPrice;
                lblLineTotal.Text = $"${newTotal:F2}";
                await UpdateItemQtyAsync(item.Id, currentQty);
            };

            btnPlus.Click += async (s, e) =>
            {
                currentQty++;
                lblQty.Text = currentQty.ToString();
                decimal newTotal = currentQty * item.UnitPrice;
                lblLineTotal.Text = $"${newTotal:F2}";
                await UpdateItemQtyAsync(item.Id, currentQty);
            };

            return row;
        }

        // ── QUANTITY BUTTON HELPER ────────────────────────────────────────────────
        private Button CreateQtyButton(string text, int x, int y)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 14f),
                ForeColor = Color.FromArgb(71, 85, 105),
                BackColor = Color.FromArgb(241, 245, 249),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(32, 32),
                Location = new Point(x, y),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 232, 240);
            return btn;
        }

        // ── UPDATE TOTALS IN FOOTER ───────────────────────────────────────────────
        private void UpdateTotals(decimal subtotal)
        {
            decimal tax = Math.Round(subtotal * 0.10m, 2);
            decimal total = subtotal + tax;

            lblSubtotalValue.Text = $"${subtotal:F2}";
            lblTaxValue.Text = $"${tax:F2}";
            lblTotalValue.Text = $"${total:F2}";

            // Right-align the value labels to right edge
            int rightEdge = totalPanel.Width - 40;
            lblSubtotalValue.Location = new Point(rightEdge - lblSubtotalValue.PreferredWidth, lblSubtotalValue.Top);
            lblTaxValue.Location = new Point(rightEdge - lblTaxValue.PreferredWidth, lblTaxValue.Top);
            lblTotalValue.Location = new Point(rightEdge - lblTotalValue.PreferredWidth, lblTotalValue.Top);
        }

        // ═════════════════════════════════════════════════════════════════════════
        //  SERVICE CALLS
        // ═════════════════════════════════════════════════════════════════════════
        private async Task RemoveItemAsync(int cartItemId, int productId)
        {
            try
            {
                ShowLoading(true);
                await _cartServices.RemoveFromCartAsync(_userId, productId);
                await LoadCartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to remove item: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { ShowLoading(false); }
        }

        private async Task UpdateItemQtyAsync(int cartItemId, int newQty)
        {
            try
            {
                var dto = new UpdateCartItemDto
                {
                    CartItemId = cartItemId,
                    NewQuantity = newQty
                };
                await _cartServices.UpdateCartItemAsync(_userId, dto);
                // Refresh totals without full re-render
                _cartData = await _cartServices.GetCartByUserIdAsync(_userId);
                if (_cartData != null)
                    UpdateTotals(_cartData.TotalAmount);
            }
            catch { /* silent — UI already updated optimistically */ }
        }

        private async Task ClearCartAsync()
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to clear your entire cart?",
                "Clear Cart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                ShowLoading(true);
                await _cartServices.ClearCartAsync(_userId);
                await LoadCartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to clear cart: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { ShowLoading(false); }
        }

        // ═════════════════════════════════════════════════════════════════════════
        //  HELPERS
        // ═════════════════════════════════════════════════════════════════════════
        private void ShowLoading(bool visible)
        {
            if (loadingOverlay.InvokeRequired)
                loadingOverlay.Invoke(() => loadingOverlay.Visible = visible);
            else
                loadingOverlay.Visible = visible;
        }

        private static async void LoadImageAsync(PictureBox box, string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            try
            {
                using var client = new HttpClient();
                var bytes = await client.GetByteArrayAsync(url);
                using var ms = new System.IO.MemoryStream(bytes);
                var img = Image.FromStream(ms);
                if (!box.IsDisposed)
                    box.Invoke(() => box.Image = img);
            }
            catch { /* keep placeholder */ }
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

        private void NavigateTo<TForm>(Action<TForm> configure = null) where TForm : Form
        {
            var form = _serviceProvider.GetRequiredService<TForm>();
            configure?.Invoke(form);
            form.Show();
            this.Hide();
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
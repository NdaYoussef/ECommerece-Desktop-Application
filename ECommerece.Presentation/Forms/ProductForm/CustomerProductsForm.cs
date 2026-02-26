using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using ECommerece.Application.IServices;
using ECommerece.Application.IServices.ICategoryService;
using ECommerece.Presentation.Forms.DashboardForms;
using ECommerece.Presentation.Forms.UserForms;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerece.Presentation.Forms.ProductForms
{
    public class CustomerProductsForm : Form
    {
        private readonly IProductService _productService;
        private readonly ICategoryServices _categoryService;
        private readonly IServiceProvider _serviceProvider;

        // ── Colors ────────────────────────────────────────────────
        static readonly Color Navy = Color.FromArgb(15, 23, 42);
        static readonly Color Accent = Color.FromArgb(59, 91, 219);
        static readonly Color LightBg = Color.FromArgb(241, 245, 249);
        static readonly Color White = Color.White;

        // ===== CONTROLS =====
        private FlowLayoutPanel cardContainer;
        private Panel sidebarPanel, mainPanel, headerPanel;
        private Panel filterPanel, productAreaPanel;
        private TextBox txtSearch;
        private Label lblProductCount, lblSectionTitle;

        // Sidebar Buttons
        private Button btnDashboard, btnProducts, btnOrders, btnCart, btnLogout;

        // Header
        private Label lblPageTitle, lblWelcome;

        // Category filter state
        private string _selectedCategory = "All";
        private List<Button> _categoryButtons = new List<Button>();

        public CustomerProductsForm(
            IProductService productService,
            ICategoryServices categoryService,
            IServiceProvider serviceProvider)
        {
            _productService = productService;
            _categoryService = categoryService;
            _serviceProvider = serviceProvider;
            InitializeComponents();
            InitializeDataAsync();
        }
        private async void InitializeDataAsync()
        {
            await LoadCategoriesAsync();
            LoadProducts();
        }
        private void InitializeComponents()
        {
            // ===== FORM =====
            this.Text = "E-Commerce";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = LightBg;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9f);

            // ===== SIDEBAR =====
            sidebarPanel = new Panel
            {
                Size = new Size(220, this.ClientSize.Height),
                Location = new Point(0, 0),
                BackColor = Navy,
            };
            this.Controls.Add(sidebarPanel);

            var lblLogo = new Label
            {
                Text = "🛒  E-Commerce",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 30),
                BackColor = Color.Transparent,
            };
            sidebarPanel.Controls.Add(lblLogo);

            var lblSub = new Label
            {
                Text = "Management",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(47, 55),
                BackColor = Color.Transparent,
            };
            sidebarPanel.Controls.Add(lblSub);

            var separator = new Panel
            {
                Size = new Size(180, 1),
                Location = new Point(20, 78),
                BackColor = Color.FromArgb(30, 41, 59),
            };
            sidebarPanel.Controls.Add(separator);

            btnDashboard = CreateSidebarButton("📊  Dashboard", 100, false);
            btnProducts = CreateSidebarButton("🛍️  Browse Products", 155, true);
            btnCart = CreateSidebarButton("🛒  Cart", 210, false);
            btnOrders = CreateSidebarButton("📦  My Orders", 265, false);

            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(btnProducts);
            sidebarPanel.Controls.Add(btnCart);
            sidebarPanel.Controls.Add(btnOrders);

            btnDashboard.Click += (s, e) =>
            {
                var dash = _serviceProvider.GetRequiredService<CustomerDashboardForm>();
                dash.Show();
                this.Hide();
            };

            // Logout Button
            btnLogout = new Button
            {
                Text = "🚪  Logout",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 45),
                Location = new Point(0, this.ClientSize.Height - 60),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
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
                BackColor = LightBg,
            };
            this.Controls.Add(mainPanel);

            // ===== HEADER =====
            headerPanel = new Panel
            {
                Size = new Size(mainPanel.Width, 65),
                Location = new Point(0, 0),
                BackColor = White,
            };
            headerPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawLine(pen, 0, headerPanel.Height - 1,
                        headerPanel.Width, headerPanel.Height - 1);
            };
            mainPanel.Controls.Add(headerPanel);

            lblPageTitle = new Label
            {
                Text = "Browse Products",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Navy,
                AutoSize = true,
                Location = new Point(30, 18),
                BackColor = Color.Transparent,
            };
            headerPanel.Controls.Add(lblPageTitle);

            // User name + avatar
            lblWelcome = new Label
            {
                Text = "John Doe",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            lblWelcome.Location = new Point(
                mainPanel.Width - lblWelcome.PreferredWidth - 55, 22);
            headerPanel.Controls.Add(lblWelcome);

            // Circle avatar
            var avatar = new Panel
            {
                Size = new Size(36, 36),
                Location = new Point(mainPanel.Width - 48, 15),
                BackColor = Color.Transparent,
            };
            avatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(new SolidBrush(Accent),
                    0, 0, avatar.Width - 1, avatar.Height - 1);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                };
                e.Graphics.DrawString("👤", new Font("Segoe UI", 14f),
                    Brushes.White,
                    new RectangleF(0, 0, avatar.Width, avatar.Height), sf);
            };
            headerPanel.Controls.Add(avatar);

            // ===== BODY =====
            int bodyTop = 73;
            int bodyH = mainPanel.Height - bodyTop - 8;

            // ── Filter Panel (left) ───────────────────────────────
            filterPanel = new Panel
            {
                Location = new Point(8, bodyTop),
                Size = new Size(195, bodyH),
                BackColor = White,
            };
            filterPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0,
                        filterPanel.Width - 1, filterPanel.Height - 1);
            };
            mainPanel.Controls.Add(filterPanel);

            // Filter title row
            var pnlFilterHdr = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(filterPanel.Width, 48),
                BackColor = White,
            };
            filterPanel.Controls.Add(pnlFilterHdr);

            pnlFilterHdr.Controls.Add(new Label
            {
                Text = "🔽  Filters",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Navy,
                AutoSize = true,
                Location = new Point(15, 13),
                BackColor = Color.Transparent,
            });

            // Separator under title
            filterPanel.Controls.Add(new Panel
            {
                Location = new Point(0, 48),
                Size = new Size(filterPanel.Width, 1),
                BackColor = Color.FromArgb(226, 232, 240),
            });

            // "Categories" label
            filterPanel.Controls.Add(new Label
            {
                Text = "Categories",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(15, 58),
                BackColor = Color.Transparent,
            });

            // "All" — always first
            AddCategoryButton("All", 82);

            // ── Product Area (right) ──────────────────────────────
            productAreaPanel = new Panel
            {
                Location = new Point(211, bodyTop),
                Size = new Size(mainPanel.Width - 219, bodyH),
                BackColor = Color.Transparent,
            };
            mainPanel.Controls.Add(productAreaPanel);

            // Search bar panel
            var pnlSearch = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(productAreaPanel.Width, 48),
                BackColor = White,
            };
            pnlSearch.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0,
                        pnlSearch.Width - 1, pnlSearch.Height - 1);
            };
            productAreaPanel.Controls.Add(pnlSearch);

            pnlSearch.Controls.Add(new Label
            {
                Text = "🔍",
                Font = new Font("Segoe UI", 11f),
                AutoSize = true,
                Location = new Point(12, 13),
                BackColor = Color.Transparent,
            });

            txtSearch = new TextBox
            {
                Location = new Point(40, 13),
                Size = new Size(pnlSearch.Width - 110, 24),
                Font = new Font("Segoe UI", 10f),
                BorderStyle = BorderStyle.None,
                PlaceholderText = "Search products...",
            };
            txtSearch.TextChanged += (s, e) => LoadProducts();
            pnlSearch.Controls.Add(txtSearch);

            // Grid / List toggle buttons
            var btnGrid = MakeToggleBtn("⊞", pnlSearch.Width - 62, 10);
            var btnList = MakeToggleBtn("☰", pnlSearch.Width - 33, 10);
            pnlSearch.Controls.Add(btnGrid);
            pnlSearch.Controls.Add(btnList);

            // Products card area
            var pnlProducts = new Panel
            {
                Location = new Point(0, 56),
                Size = new Size(productAreaPanel.Width, productAreaPanel.Height - 56),
                BackColor = White,
            };
            pnlProducts.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0,
                        pnlProducts.Width - 1, pnlProducts.Height - 1);
            };
            productAreaPanel.Controls.Add(pnlProducts);

            // Section header
            var pnlSectionHdr = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(pnlProducts.Width, 52),
                BackColor = White,
            };
            pnlProducts.Controls.Add(pnlSectionHdr);

            lblSectionTitle = new Label
            {
                Text = "All Products",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Navy,
                AutoSize = true,
                Location = new Point(15, 8),
                BackColor = Color.Transparent,
            };
            pnlSectionHdr.Controls.Add(lblSectionTitle);

            lblProductCount = new Label
            {
                Text = "0 products found",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(15, 32),
                BackColor = Color.Transparent,
            };
            pnlSectionHdr.Controls.Add(lblProductCount);

            // Separator under section header
            pnlProducts.Controls.Add(new Panel
            {
                Location = new Point(0, 52),
                Size = new Size(pnlProducts.Width, 1),
                BackColor = Color.FromArgb(226, 232, 240),
            });

            // FlowLayoutPanel for cards
            cardContainer = new FlowLayoutPanel
            {
                Location = new Point(0, 53),
                Size = new Size(pnlProducts.Width, pnlProducts.Height - 53),
                BackColor = Color.Transparent,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10, 8, 0, 0),
            };
            pnlProducts.Controls.Add(cardContainer);
        }

        // ── Helper: toggle button (grid/list icons) ───────────────
        private Button MakeToggleBtn(string icon, int x, int y)
        {
            var b = new Button
            {
                Text = icon,
                Font = new Font("Segoe UI", 12f),
                Size = new Size(27, 28),
                Location = new Point(x, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(100, 116, 139),
                Cursor = Cursors.Hand,
            };
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            return b;
        }

        // ── Add one category button to the filter panel ───────────
        private void AddCategoryButton(string name, int y)
        {
            bool isAll = name == "All";
            bool isActive = name == _selectedCategory;

            var btn = new Button
            {
                Text = name,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = isActive ? Color.White : Color.FromArgb(51, 65, 85),
                BackColor = isActive ? Accent : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(filterPanel.Width, 34),
                Location = new Point(0, y),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand,
                Tag = name,
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btn.Click += CategoryBtn_Click;

            filterPanel.Controls.Add(btn);
            _categoryButtons.Add(btn);
        }

        // ── Category button click ─────────────────────────────────
        private void CategoryBtn_Click(object sender, EventArgs e)
        {
            if (sender is not Button btn) return;

            _selectedCategory = btn.Tag?.ToString() ?? "All";

            // Update active styles
            foreach (var b in _categoryButtons)
            {
                bool active = b.Tag?.ToString() == _selectedCategory;
                b.BackColor = active ? Accent : Color.Transparent;
                b.ForeColor = active ? Color.White : Color.FromArgb(51, 65, 85);
            }

            // Update section title
            if (lblSectionTitle != null)
                lblSectionTitle.Text = _selectedCategory == "All"
                    ? "All Products"
                    : _selectedCategory;

            LoadProducts();
        }

        // ── Load categories from DB ───────────────────────────────
        private async Task LoadCategoriesAsync()
        {
            try
            {
                var names = await _categoryService.GetCategoriesNames();
                int y = 82 + 34; // after "All" button

                foreach (var name in names)
                {
                    AddCategoryButton(name, y);
                    y += 34;
                }
            }
            catch { /* silent — keep "All" only */ }
        }

        // ── Load & filter products ────────────────────────────────
        private void LoadProducts()
        {
            cardContainer.Controls.Clear();

            var all = _productService.GetProductsList();
            if (all == null || all.Count == 0)
            {
                if (lblProductCount != null) lblProductCount.Text = "0 products found";
                return;
            }

            // Filter by category
            var filtered = _selectedCategory == "All"
                ? all
                : all.Where(p => p.CategoryName == _selectedCategory).ToList();

            // Filter by search text
            var search = txtSearch?.Text.Trim().ToLower() ?? "";
            if (!string.IsNullOrEmpty(search))
                filtered = filtered
                    .Where(p => (p.Label ?? "").ToLower().Contains(search))
                    .ToList();

            if (lblProductCount != null)
                lblProductCount.Text = $"{filtered.Count} products found";

            if (filtered.Count == 0)
            {
                cardContainer.Controls.Add(new Label
                {
                    Text = "No products found.",
                    Font = new Font("Segoe UI", 11f),
                    ForeColor = Color.FromArgb(100, 116, 139),
                    AutoSize = true,
                    Margin = new Padding(20),
                });
                return;
            }

            foreach (var p in filtered)
            {
                var product = p;

                // ── Card ──────────────────────────────────────────
                var card = new Panel
                {
                    Size = new Size(200, 265),
                    BackColor = White,
                    Margin = new Padding(0, 0, 12, 12),
                    Cursor = Cursors.Hand,
                };
                card.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (Pen border = new Pen(Color.FromArgb(226, 232, 240), 1))
                        e.Graphics.DrawRectangle(border, 0, 0, card.Width - 1, card.Height - 1);
                    using (SolidBrush topBar = new SolidBrush(Accent))
                        e.Graphics.FillRectangle(topBar, 0, 0, card.Width, 3);
                };

                // Image
                var pic = new PictureBox
                {
                    Size = new Size(198, 118),
                    Location = new Point(1, 3),
                    BackColor = LightBg,
                    SizeMode = PictureBoxSizeMode.Zoom,
                };
                pic.Paint += (s, e) =>
                {
                    if (pic.Image == null)
                        e.Graphics.DrawString("🖼️",
                            new Font("Segoe UI", 22f),
                            new SolidBrush(Color.FromArgb(203, 213, 225)),
                            new PointF(pic.Width / 2f - 18, pic.Height / 2f - 18));
                };
                card.Controls.Add(pic);
                LoadImageAsync(pic, product.ImageUrl);

                // Eye icon button (top-right of image)
                var btnView = new Button
                {
                    Text = "👁",
                    Font = new Font("Segoe UI", 10f),
                    Size = new Size(28, 28),
                    Location = new Point(card.Width - 34, 8),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    Cursor = Cursors.Hand,
                };
                btnView.FlatAppearance.BorderSize = 0;
                card.Controls.Add(btnView);

                // Product name
                card.Controls.Add(new Label
                {
                    Text = product.Label ?? "—",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = Navy,
                    Location = new Point(10, 130),
                    Size = new Size(178, 36),
                    AutoSize = false,
                });

                // Category name
                card.Controls.Add(new Label
                {
                    Text = product.CategoryName ?? "",
                    Font = new Font("Segoe UI", 8f),
                    ForeColor = Color.FromArgb(100, 116, 139),
                    Location = new Point(10, 168),
                    AutoSize = true,
                });

                // Price
                card.Controls.Add(new Label
                {
                    Text = product.Price.ToString("C"),
                    Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                    ForeColor = Accent,
                    Location = new Point(10, 190),
                    AutoSize = true,
                });

                // Stock badge
                card.Controls.Add(new Label
                {
                    Text = product.IsInStock ? "In Stock" : "Out of Stock",
                    Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                    ForeColor = product.IsInStock
                        ? Color.FromArgb(16, 185, 129)
                        : Color.FromArgb(239, 68, 68),
                    Location = new Point(10, 234),
                    AutoSize = true,
                });

                // Click → open details
                EventHandler onClick = async (s, e) =>
                {
                    var details = await _productService.GetProductDetails(product.Label ?? "");
                    if (details == null) return;
                    using var detailsForm = new ProductDetailsForm(details);
                    detailsForm.ShowDialog(this);
                };

                card.Click += onClick;
                btnView.Click += onClick;
                foreach (Control child in card.Controls)
                    if (child != btnView)
                        child.Click += onClick;

                cardContainer.Controls.Add(card);
            }
        }

        // ── Load image async ──────────────────────────────────────
        private async void LoadImageAsync(PictureBox box, string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
                return;
            try
            {
                using var img = Image.FromFile(filePath);
                box.Image = new Bitmap(img);
            }
            catch { }
        }

        private Button CreateSidebarButton(string text, int y, bool isActive)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10f),
                ForeColor = isActive ? Color.White : Color.FromArgb(148, 163, 184),
                BackColor = isActive ? Accent : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 45),
                Location = new Point(0, y),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = isActive
                ? Accent
                : Color.FromArgb(30, 41, 59);
            return btn;
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _serviceProvider.GetRequiredService<LoginForm>().Show();
                this.Close();
            }
        }
    }
}
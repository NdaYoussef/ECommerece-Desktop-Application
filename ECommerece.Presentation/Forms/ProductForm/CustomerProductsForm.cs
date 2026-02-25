using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices;
using ECommerece.Application.Services.ProductServices;
using ECommerece.Infrastructure.Repositories;
using ECommerece.Presentation.Forms.DashboardForms;
using ECommerece.Presentation.Forms.UserForms;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerece.Presentation.Forms.ProductForms
{
    public class CustomerProductsForm : Form
    {
        // private readonly IProductRepository _productRepository = new ProductRepository();
        private readonly IProductService _productService;
        private readonly IServiceProvider _serviceProvider;

        // ── Colors ────────────────────────────────────────────────
        static readonly Color Navy = Color.FromArgb(15, 23, 42);
        static readonly Color Accent = Color.FromArgb(59, 91, 219);
        static readonly Color LightBg = Color.FromArgb(241, 245, 249);
        static readonly Color White = Color.White;

        // ===== CONTROLS =====
        private FlowLayoutPanel cardContainer;
        private Panel sidebarPanel,
            mainPanel,
            headerPanel;

        // Sidebar Buttons
        private Button btnDashboard,
            btnProducts,
            btnOrders,
            btnCart,
            btnLogout;

        // Header
        private Label lblPageTitle,
            lblWelcome;

        // removed??

        public CustomerProductsForm(
            IProductService productService,
            IServiceProvider serviceProvider
        )
        {
            _productService = productService;
            _serviceProvider = serviceProvider;
            InitializeComponents();
            LoadProducts();
        }

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
                BackColor = Color.FromArgb(15, 23, 42),
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
                BackColor = Color.Transparent,
            };
            sidebarPanel.Controls.Add(lblLogo);

            var separator = new Panel
            {
                Size = new Size(180, 1),
                Location = new Point(20, 70),
                BackColor = Color.FromArgb(30, 41, 59),
            };
            sidebarPanel.Controls.Add(separator);

            // Sidebar Buttons
            btnDashboard = CreateSidebarButton("📊  Dashboard", 100, false);
            btnProducts = CreateSidebarButton("🛍️  Browse Products", 155, true);
            btnCart = CreateSidebarButton("🛒  Cart", 210, false);
            btnOrders = CreateSidebarButton("📦  My Orders", 265, false);

            sidebarPanel.Controls.Add(btnDashboard);
            btnDashboard.Click += (s, e) =>
            {
                var categoryForm = _serviceProvider.GetRequiredService<CustomerDashboardForm>();
                categoryForm.Show();
                this.Hide();
            };
            sidebarPanel.Controls.Add(btnProducts);
            // btnProducts.Click += btnProducts_click;
            sidebarPanel.Controls.Add(btnOrders);
            // btnOrders.Click +=  (s,e)=> NavigateTo<OrderForm>();
            sidebarPanel.Controls.Add(btnCart);
            // btnCart.Click += (s,e)=> NavigateTo<CartForm>();

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
                BackColor = Color.FromArgb(241, 245, 249),
            };
            this.Controls.Add(mainPanel);

            // ===== HEADER =====
            headerPanel = new Panel
            {
                Size = new Size(mainPanel.Width, 70),
                Location = new Point(0, 0),
                BackColor = Color.White,
            };
            headerPanel.Paint += (s, e) =>
            {
                using (Pen pen = new(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawLine(
                        pen,
                        0,
                        headerPanel.Height - 1,
                        headerPanel.Width,
                        headerPanel.Height - 1
                    );
            };
            mainPanel.Controls.Add(headerPanel);

            lblPageTitle = new Label
            {
                Text = "Products View",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(30, 20),
                BackColor = Color.Transparent,
            };
            headerPanel.Controls.Add(lblPageTitle);

            lblWelcome = new Label
            {
                Text = "Welcome back, Admin 👋",
                // TODO make it dynamic
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            lblWelcome.Location = new Point(mainPanel.Width - lblWelcome.PreferredWidth - 30, 25);
            headerPanel.Controls.Add(lblWelcome);

            // ── SectionLabel ──────────────────────────────────────────
            var lblBrowse = new Label
            {
                Text = "Browse Products  —  click any card to view details",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 100),
                BackColor = Color.Transparent,
            };
            mainPanel.Controls.Add(lblBrowse);

            // ── Card container (FlowLayoutPanel handles wrapping) ──
            cardContainer = new FlowLayoutPanel
            {
                Location = new Point(20, 115),
                Size = new Size(mainPanel.Width - 40, mainPanel.Height - 120),
                BackColor = Color.Transparent,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0),
            };
            mainPanel.Controls.Add(cardContainer);
        }

        // ── Build one card per product ────────────────────────────
        private void LoadProducts()
        {
            cardContainer.Controls.Clear();

            var products = _productService.GetProductsList();
            if (products == null || products.Count == 0)
            {
                cardContainer.Controls.Add(
                    new Label
                    {
                        Text = "No products available.",
                        Font = new Font("Segoe UI", 11f),
                        ForeColor = Color.FromArgb(100, 116, 139),
                        AutoSize = true,
                        Margin = new Padding(20),
                    }
                );
                return;
            }

            foreach (var p in products)
            {
                // Capture for closure
                var product = p;

                // ── Card panel ────────────────────────────────────
                var card = new Panel
                {
                    Size = new Size(200, 260),
                    BackColor = White,
                    Margin = new Padding(0, 0, 12, 12),
                    Cursor = Cursors.Hand,
                };

                // Colored top border + subtle border via Paint
                card.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using var border = new Pen(Color.FromArgb(226, 232, 240), 1);
                    e.Graphics.DrawRectangle(border, 0, 0, card.Width - 1, card.Height - 1);
                    using var topBar = new SolidBrush(Accent);
                    e.Graphics.FillRectangle(topBar, 0, 0, card.Width, 3);
                };

                // ── Image ─────────────────────────────────────────
                var pic = new PictureBox
                {
                    Size = new Size(198, 120),
                    Location = new Point(1, 3),
                    BackColor = Color.FromArgb(241, 245, 249),
                    SizeMode = PictureBoxSizeMode.Zoom,
                };
                pic.Paint += (s, e) =>
                {
                    if (pic.Image == null)
                        e.Graphics.DrawString(
                            "🖼️",
                            new Font("Segoe UI", 22f),
                            new SolidBrush(Color.FromArgb(203, 213, 225)),
                            new PointF(pic.Width / 2f - 18, pic.Height / 2f - 18)
                        );
                };
                card.Controls.Add(pic);
                LoadImageAsync(pic, product.ImageUrl);

                // ── Product name ──────────────────────────────────
                card.Controls.Add(
                    new Label
                    {
                        Text = product.Label ?? "—",
                        Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                        ForeColor = Navy,
                        Location = new Point(10, 132),
                        Size = new Size(180, 36),
                        AutoSize = false,
                    }
                );

                // ── Category ──────────────────────────────────────
                card.Controls.Add(
                    new Label
                    {
                        Text = product.CategoryName ?? "",
                        Font = new Font("Segoe UI", 8f),
                        ForeColor = Color.FromArgb(100, 116, 139),
                        Location = new Point(10, 170),
                        AutoSize = true,
                    }
                );

                // ── Price ─────────────────────────────────────────
                card.Controls.Add(
                    new Label
                    {
                        Text = product.Price.ToString("C"),
                        Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                        ForeColor = Accent,
                        Location = new Point(10, 192),
                        AutoSize = true,
                    }
                );

                // ── Stock badge ───────────────────────────────────
                card.Controls.Add(
                    new Label
                    {
                        Text = product.IsInStock ? "In Stock" : "Out of Stock",
                        Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                        ForeColor = product.IsInStock
                            ? Color.FromArgb(16, 185, 129)
                            : Color.FromArgb(239, 68, 68),
                        Location = new Point(10, 228),
                        AutoSize = true,
                    }
                );

                // ── Click → open details ──────────────────────────
                // Wire click on card AND all child controls so the
                // whole card area is clickable
                EventHandler onClick = async (s, e) =>
                {
                    var details = await _productService.GetProductDetails(product.Label ?? "");
                    if (details == null)
                        return;
                    using var detailsForm = new ProductDetailsForm(details);
                    detailsForm.ShowDialog(this);
                };

                card.Click += onClick;
                foreach (Control child in card.Controls)
                    child.Click += onClick;

                cardContainer.Controls.Add(card);
            }
        }

        // ── Load image without freezing UI ────────────────────────
        private async void LoadImageAsync(PictureBox box, string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
                return;
            try
            {
                using var img = Image.FromFile(filePath);
                box.Image = new Bitmap(img);
            }
            catch
            { /* broken URL — placeholder stays */
            }
        }

        private Button CreateNavButton(string text, int y, bool isActive)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10f),
                ForeColor = isActive ? White : Color.FromArgb(148, 163, 184),
                BackColor = isActive ? Accent : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(220, 45),
                Location = new Point(0, y),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = isActive ? Accent : Color.FromArgb(30, 41, 59);
            return btn;
        }

        // }

        private void NavigateTo<T>()
            where T : Form
        {
            using (var scope = Program.host.Services.CreateScope())
            {
                var form = scope.ServiceProvider.GetRequiredService<T>();
                form.Show();
                this.Close(); // or Hide() – be careful with closing the current form
            }
            // form.FormClosed += (s, e) => this.Show();
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
                Cursor = Cursors.Hand,
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = isActive
                ? Color.FromArgb(59, 91, 219)
                : Color.FromArgb(30, 41, 59);
            return btn;
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                var loginForm = _serviceProvider.GetRequiredService<LoginForm>();
                loginForm.Show();
                this.Close();
            }
        }

        private void btnCustomers_click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnOrders_click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        //
        //
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices;
using ECommerece.Application.Services.ProductServices;
using ECommerece.Infrastructure.Repositories;
using ECommerece.Presentation.Forms.CategoryForms;
using ECommerece.Presentation.Forms.DashboardForms;
using ECommerece.Presentation.Forms.OrderForms;
using ECommerece.Presentation.Forms.UserForms;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerece.Presentation.Forms.ProductForms
{
    public class AdminProductsForm : Form
    {
        // private readonly IProductRepository _productRepository = new ProductRepository();
        private readonly IProductService _productService;
        private readonly IServiceProvider _serviceProvider;

        // ===== CONTROLS =====
        private Panel sidebarPanel,
            mainPanel,
            headerPanel;

        // Sidebar Buttons
        private Button btnDashboard,
            btnProducts,
            btnOrders,
            btnCustomers,
            btnCategories,
            btnLogout;

        // Header
        private Label lblPageTitle,
            lblWelcome;

        private Panel cardTotalProducts,
            cardInStock,
            cardLowStock,
            cardOutOfStock;
        private DataGridView dgvProducts;

        public AdminProductsForm(IProductService productService, IServiceProvider serviceProvider)
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
            btnDashboard = CreateSidebarButton("📊  Dashboard", 90, false);
            btnCategories = CreateSidebarButton("🗂️  Categories", 145, false);
            btnProducts = CreateSidebarButton("📦  Products", 200, true);
            btnOrders = CreateSidebarButton("🧾  Orders", 255, false);
            btnCustomers = CreateSidebarButton("👥  Customers", 310, false);

            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(btnCategories);
            sidebarPanel.Controls.Add(btnProducts);
            sidebarPanel.Controls.Add(btnOrders);
            sidebarPanel.Controls.Add(btnCustomers);

            // btnProducts.Click += btnProducts_click;
            // btnOrders.Click +=  (s,e)=> NavigateTo<OrderForm>();
            // btnCustomers.Click += (s,e)=> NavigateTo<CustomerForm>();
            btnDashboard.Click += (s, e) =>
            {
                var dashboardForm = _serviceProvider.GetRequiredService<DashboardForm>();
                dashboardForm.Show();
                this.Hide();
            };

            btnCategories.Click += (s, e) =>
            {
                var categoryForm = _serviceProvider.GetRequiredService<CategoryForm>();
                categoryForm.Show();
                this.Hide();
            };

            btnOrders.Click += (s, e) =>
            {
                var orderForm = _serviceProvider.GetRequiredService<AdminOrderManagementForm>();
                orderForm.Show();
                this.Hide();
            };
            
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
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
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

            // ── Overview ──────────────────────────────────────────
            var lblStats = new Label
            {
                Text = "OverView",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 100),
                BackColor = Color.Transparent,
            };
            mainPanel.Controls.Add(lblStats);

            // ===== STAT CARDS =====
            cardTotalProducts = CreateStatCard(
                "Total Products",
                "0",
                "📦",
                Color.FromArgb(59, 91, 219),
                30,
                130
            );
            cardInStock = CreateStatCard(
                "In Stock",
                "0",
                "✅",
                Color.FromArgb(16, 185, 129),
                230,
                130
            );
            cardLowStock = CreateStatCard(
                "Low Stock",
                "0",
                "⚠️",
                Color.FromArgb(245, 158, 11),
                430,
                130
            );
            cardOutOfStock = CreateStatCard(
                "Out of Stock",
                "0",
                "❌",
                Color.FromArgb(239, 68, 68),
                630,
                130
            );

            mainPanel.Controls.Add(cardTotalProducts);
            mainPanel.Controls.Add(cardInStock);
            mainPanel.Controls.Add(cardLowStock);
            mainPanel.Controls.Add(cardOutOfStock);

            // ── Toolbar ───────────────────────────────────────────
            var lblList = new Label
            {
                Text = "Product List",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 300),
                BackColor = Color.Transparent,
            };
            mainPanel.Controls.Add(lblList);

            var btnAdd = new Button
            {
                Text = "＋  Add Product",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.FromArgb(59, 91, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 32),
                Location = new Point(mainPanel.Width - 160, 297),
                Cursor = Cursors.Hand,
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            mainPanel.Controls.Add(btnAdd);

            // ── DataGridView ──────────────────────────────────────
            dgvProducts = new DataGridView
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
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            };

            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font(
                "Segoe UI",
                9f,
                FontStyle.Bold
            );
            dgvProducts.ColumnHeadersHeight = 40;
            dgvProducts.RowTemplate.Height = 40;
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);

            dgvProducts.Columns.Add("ProductId", "ProductId");
            dgvProducts.Columns["ProductId"].Visible = false;

            dgvProducts.Columns.Add("Label", "ProductName");
            // dgv.Columns.Add("Description", "Description");
            dgvProducts.Columns.Add("Price", "Price");
            dgvProducts.Columns.Add("CategoryName", "Category");
            dgvProducts.Columns.Add("IsInStock", "In Stock");

            dgvProducts.CellDoubleClick += DgvProducts_CellDoubleClick;

            // 🔴 NEW: Add a button column for deleting products
            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn
            {
                Name = "RemoveColumn",
                HeaderText = "",
                Text = "🗑️ Delete",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                Width = 80, // fixed width
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            };
            dgvProducts.Columns.Add(deleteButton);

            dgvProducts.CellClick += DgvProducts_CellClick;

            // foreach (var p in _productService.GetProductsList() ?? [])
            // {
            //     dgvProducts.Rows.Add(p.Id,p.Label, p.Price, p.CategoryName, p.IsInStock);
            // }
            mainPanel.Controls.Add(dgvProducts);
        }

        private async void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header clicks or clicks on non-button columns
            if (e.RowIndex < 0 || dgvProducts.Columns[e.ColumnIndex].Name != "RemoveColumn")
                return;

            var row = dgvProducts.Rows[e.RowIndex];
            int productId = Convert.ToInt32(row.Cells["ProductId"].Value);
            string productName = row.Cells["Label"].Value?.ToString() ?? "this product";

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete '{productName}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    // Assuming IProductService has a DeleteProduct method
                    await _productService.DeleteProduct(productId);
                    LoadProducts(); // refresh grid
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error deleting product: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var scope = Program.host.Services.CreateScope())
            {
                var addForm = scope.ServiceProvider.GetRequiredService<AddProductForm>();
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts(); // refresh grid
                }
            }
        }

        private async void DgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Ignore header row clicks
                if (e.RowIndex < 0)
                    return;

                var row = dgvProducts.Rows[e.RowIndex];

                // Read the hidden ID
                int productId = Convert.ToInt32(row.Cells["ProductId"].Value);

                // Fetch full details so we can pre-fill all fields (Label, Description, etc.)
                var details = await _productService.GetProductDetails(productId);
                if (details == null)
                {
                    MessageBox.Show(
                        "Could not load product details.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                using var scope = Program.host.Services.CreateScope();
                var editForm = new EditProductForm(
                    scope.ServiceProvider.GetRequiredService<IProductService>(),
                    productId,
                    details?.Label ?? "",
                    details?.Description ?? "",
                    details?.Price ?? 0,
                    details?.StockQuantity ?? 0,
                    details?.ImageUrl ?? "",
                    details?.CategoryId ?? 0
                );

                if (editForm.ShowDialog() == DialogResult.OK)
                    LoadProducts(); // refresh grid after save
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error opening edit form:\n{ex.Message}",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
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

        private Panel CreateStatCard(
            string title,
            string value,
            string icon,
            Color accentColor,
            int x,
            int y
        )
        {
            var card = new Panel
            {
                Size = new Size(185, 140),
                Location = new Point(x, y),
                BackColor = Color.White,
                Cursor = Cursors.Default,
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
                BackColor = Color.Transparent,
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
                BackColor = Color.Transparent,
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
                BackColor = Color.Transparent,
            };
            card.Controls.Add(lblTitle);

            return card;
        }

        private void LoadProducts()
        {
            dgvProducts.Rows.Clear();

            var products = _productService.GetProductsList();
            if (products == null || products?.Count == 0)
            {
                UpdateCardValue(cardTotalProducts, "0");
                UpdateCardValue(cardInStock, "0");
                UpdateCardValue(cardLowStock, "0");
                UpdateCardValue(cardOutOfStock, "0");
                return;
            }

            // Use GetLowStockProducts to identify low-stock items
            var lowStockLabels = new HashSet<string>(
                _productService.GetLowStockProducts(5)?.ConvertAll(p => p.Label ?? "")
                    ?? new List<string>()
            );

            int inStock = 0,
                lowStock = 0,
                outOfStock = 0;

            foreach (var p in products ?? [])
            {
                string status;
                if (!p.IsInStock)
                {
                    status = "Out of Stock";
                    outOfStock++;
                }
                else if (p.Label != null && lowStockLabels.Contains(p.Label))
                {
                    status = "Low Stock";
                    lowStock++;
                    inStock++;
                }
                else
                {
                    status = "In Stock";
                    inStock++;
                }

                dgvProducts.Rows.Add(p.Id, p.Label, p.Price.ToString("C"), p.CategoryName, status);
            }

            UpdateCardValue(cardTotalProducts, products?.Count.ToString());
            UpdateCardValue(cardInStock, inStock.ToString());
            UpdateCardValue(cardLowStock, lowStock.ToString());
            UpdateCardValue(cardOutOfStock, outOfStock.ToString());
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

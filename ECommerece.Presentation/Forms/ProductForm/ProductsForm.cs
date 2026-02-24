using System;
using System.Drawing;
using System.Windows.Forms;

namespace ECommerece.Presentation.Forms.ProductForms
{
    public class ProductsForm : BaseForm
    {
        // Stat cards — kept as fields so you can update them from a service later
        private Panel cardTotalProducts;
        private Panel cardInStock;
        private Panel cardLowStock;
        private Panel cardOutOfStock;

        private DataGridView dgvProducts;

        public ProductsForm()
        {
            BuildProductsPage();
        }

        private void BuildProductsPage()
        {
            // ── Page title + active nav ───────────────────────────
            LblPageTitle.Text = "Products";
            SetActiveNav(BtnProducts);

            // ── Section label ─────────────────────────────────────
            var lblOverview = new Label
            {
                Text = "Overview",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 100),
                BackColor = Color.Transparent,
            };
            MainPanel.Controls.Add(lblOverview);

            // ── Stat Cards ────────────────────────────────────────
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

            MainPanel.Controls.Add(cardTotalProducts);
            MainPanel.Controls.Add(cardInStock);
            MainPanel.Controls.Add(cardLowStock);
            MainPanel.Controls.Add(cardOutOfStock);

            // ── Toolbar (label + Add button) ──────────────────────
            var lblProducts = new Label
            {
                Text = "Product List",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 300),
                BackColor = Color.Transparent,
            };
            MainPanel.Controls.Add(lblProducts);

            var btnAdd = new Button
            {
                Text = "＋  Add Product",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.FromArgb(59, 91, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 32),
                Location = new Point(MainPanel.Width - 160, 297),
                Cursor = Cursors.Hand,
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            MainPanel.Controls.Add(btnAdd);

            // ── DataGridView ──────────────────────────────────────
            dgvProducts = new DataGridView
            {
                Location = new Point(30, 345),
                Size = new Size(MainPanel.Width - 60, 270),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                GridColor = Color.FromArgb(226, 232, 240),
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9f),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            };

            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(59, 91, 219);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font(
                "Segoe UI",
                9f,
                FontStyle.Bold
            );
            dgvProducts.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(
                59,
                91,
                219
            );
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.ColumnHeadersHeight = 40;
            dgvProducts.RowTemplate.Height = 40;
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);

            dgvProducts.Columns.Add("Label", "Product");
            dgvProducts.Columns.Add("Category", "Category");
            dgvProducts.Columns.Add("Price", "Price");
            dgvProducts.Columns.Add("Stock", "Stock Qty");
            dgvProducts.Columns.Add("Status", "Status");

            dgvProducts.CellFormatting += DgvProducts_CellFormatting;

            MainPanel.Controls.Add(dgvProducts);

            // ── Load data ─────────────────────────────────────────
            LoadProducts();
        }

        // ── Data loading ──────────────────────────────────────────

        private void LoadProducts()
        {
            // TODO: replace with real service call, e.g.:
            // var products = _productService.GetProductsList();
            // foreach (var p in products)
            //     dgvProducts.Rows.Add(p.Label, p.CategoryName, p.Price.ToString("C"),
            //                          p.StockQuantity, p.StockQuantity > 0 ? "In Stock" : "Out of Stock");

            dgvProducts.Rows.Add("Full Cream Milk", "Dairy", "$2.50", 85, "In Stock");
            dgvProducts.Rows.Add("Whole Wheat Bread", "Bakery", "$1.80", 42, "In Stock");
            dgvProducts.Rows.Add("Fresh Eggs (x30)", "Poultry", "$4.20", 3, "Low Stock");
            dgvProducts.Rows.Add("Orange Juice 1.5L", "Beverages", "$3.10", 0, "Out of Stock");
            dgvProducts.Rows.Add("Red Apples (per kg)", "Fruits", "$2.90", 60, "In Stock");

            UpdateCardValue(cardTotalProducts, "128");
            UpdateCardValue(cardInStock, "114");
            UpdateCardValue(cardLowStock, "9");
            UpdateCardValue(cardOutOfStock, "5");
        }

        // ── Events ────────────────────────────────────────────────

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // TODO: new AddProductForm().ShowDialog();
            MessageBox.Show(
                "Open Add Product form here.",
                "Add Product",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void DgvProducts_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e
        )
        {
            if (e.ColumnIndex != dgvProducts.Columns["Status"].Index || e.Value == null)
                return;

            e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            switch (e.Value.ToString())
            {
                case "In Stock":
                    e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129);
                    break;
                case "Low Stock":
                    e.CellStyle.ForeColor = Color.FromArgb(245, 158, 11);
                    break;
                case "Out of Stock":
                    e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68);
                    break;
            }
        }

        // ── Helper ────────────────────────────────────────────────

        private void UpdateCardValue(Panel card, string value)
        {
            foreach (Control ctrl in card.Controls)
                if (ctrl is Label lbl && lbl.Font.Bold && lbl.Font.Size == 22f)
                {
                    lbl.Text = value;
                    return;
                }
        }
    }
}

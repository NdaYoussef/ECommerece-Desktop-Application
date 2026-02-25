using ECommerece.Application.IServices.ICategoryService;
using ECommerece.Presentation.Forms.DashboardForms;
using ECommerece.Presentation.Forms.UserForms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ECommerece.Presentation.Forms.CategoryForms
{
    public class CategoryForm : Form
    {
        private readonly ICategoryServices _categoryService;
        private readonly IServiceProvider _serviceProvider;

        public CategoryForm(ICategoryServices categoryService, IServiceProvider serviceProvider)
        {
            _categoryService = categoryService;
            _serviceProvider = serviceProvider;
            InitializeComponents();
            LoadCategories();
        }
        // ===== CONTROLS =====
        private Panel sidebarPanel;
        private Panel mainPanel;
        private Panel headerPanel;
        private Panel contentPanel;

        // Sidebar Buttons
        private Button btnDashboard;
        private Button btnCategories;
        private Button btnProducts;
        private Button btnOrders;
        private Button btnCustomers;
        private Button btnLogout;

        // Header
        private Label lblPageTitle;

        // Category Controls
        private DataGridView dgvCategories;
        private Button btnAddCategory;

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

            // Sidebar Buttons — Categories is Active
            btnDashboard = CreateSidebarButton("📊  Dashboard", 90, false);
            btnCategories = CreateSidebarButton("🗂️  Categories", 145, true);
            btnProducts = CreateSidebarButton("📦  Products", 200, false);
            btnOrders = CreateSidebarButton("🧾  Orders", 255, false);
            btnCustomers = CreateSidebarButton("👥  Customers", 310, false);

            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(btnCategories);
            sidebarPanel.Controls.Add(btnProducts);
            sidebarPanel.Controls.Add(btnOrders);
            sidebarPanel.Controls.Add(btnCustomers);

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
                Text = "Category Management",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(30, 20),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblPageTitle);

            var lblAdminUser = new Label
            {
                Text = "Admin User",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblAdminUser.Location = new Point(mainPanel.Width - lblAdminUser.PreferredWidth - 30, 25);
            headerPanel.Controls.Add(lblAdminUser);

            // ===== CONTENT PANEL =====
            contentPanel = new Panel
            {
                Location = new Point(30, 90),
                Size = new Size(mainPanel.Width - 60, mainPanel.Height - 110),
                BackColor = Color.White
            };
            contentPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, contentPanel.Width - 1, contentPanel.Height - 1);
            };
            mainPanel.Controls.Add(contentPanel);

            // Content Title
            var lblContentTitle = new Label
            {
                Text = "Category Management",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(20, 20),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(lblContentTitle);

            var lblSubtitle = new Label
            {
                Text = "Manage your product categories",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(20, 45),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(lblSubtitle);

            // Add Category Button
            btnAddCategory = new Button
            {
                Text = "+ Add Category",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 91, 219),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 38),
                Location = new Point(contentPanel.Width - 170, 18),
                Cursor = Cursors.Hand
            };
            btnAddCategory.FlatAppearance.BorderSize = 0;
            contentPanel.Controls.Add(btnAddCategory);

            // ===== DataGridView =====
            dgvCategories = new DataGridView
            {
                Location = new Point(0, 75),
                Size = new Size(contentPanel.Width, contentPanel.Height - 75),
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

            dgvCategories.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvCategories.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 116, 139);
            dgvCategories.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgvCategories.ColumnHeadersHeight = 40;
            dgvCategories.RowTemplate.Height = 45;
            dgvCategories.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            dgvCategories.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);

            dgvCategories.Columns.Add("Id", "ID");
            dgvCategories.Columns.Add("CategoryName", "Category Name");
            dgvCategories.Columns.Add("Description", "Description");
            dgvCategories.Columns.Add("Products", "Products");

            // Edit Button Column
            var editCol = new DataGridViewButtonColumn
            {
                Name = "EditAction",
                HeaderText = "",
                Text = "✏️ Edit",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                Width = 80
            };
            dgvCategories.Columns.Add(editCol);

            // Delete Button Column
            var deleteCol = new DataGridViewButtonColumn
            {
                Name = "DeleteAction",
                HeaderText = "",
                Text = "🗑️ Delete",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                Width = 90
            };
            dgvCategories.Columns.Add(deleteCol);

            dgvCategories.Columns["Id"].FillWeight = 5;
            dgvCategories.Columns["CategoryName"].FillWeight = 20;
            dgvCategories.Columns["Description"].FillWeight = 35;
            dgvCategories.Columns["Products"].FillWeight = 15;
            dgvCategories.Columns["EditAction"].FillWeight = 10;
            dgvCategories.Columns["DeleteAction"].FillWeight = 10;

            contentPanel.Controls.Add(dgvCategories);

            // Style Edit & Delete buttons in grid
            dgvCategories.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (dgvCategories.Columns[e.ColumnIndex].Name == "EditAction")
                {
                    e.CellStyle.BackColor = Color.FromArgb(239, 246, 255);
                    e.CellStyle.ForeColor = Color.FromArgb(59, 91, 219);
                    e.CellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
                }
                if (dgvCategories.Columns[e.ColumnIndex].Name == "DeleteAction")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 241, 242);
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                    e.CellStyle.SelectionBackColor = Color.FromArgb(254, 226, 226);
                }
            };

            // ===== CLICK EVENTS =====

            // Dashboard Button
            btnDashboard.Click += (s, e) =>
            {
                var dashboardForm = _serviceProvider.GetRequiredService<DashboardForm>();
                dashboardForm.Show();
                this.Hide();
            };

            // Add Category Button → فتح AddEditCategoryForm في Add Mode
            btnAddCategory.Click += (s, e) =>
            {
                var addForm = new AddEditCategoryForm(_categoryService);
                addForm.ShowDialog();
                LoadCategories();
            };

            // Edit & Delete Buttons في الـ Grid
            dgvCategories.CellClick += async (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var row = dgvCategories.Rows[e.RowIndex];
                int id = int.Parse(row.Cells["Id"].Value.ToString());
                string name = row.Cells["CategoryName"].Value.ToString();
                string desc = row.Cells["Description"].Value.ToString();

                // ✏️ Edit → فتح AddEditCategoryForm في Edit Mode
                if (dgvCategories.Columns[e.ColumnIndex].Name == "EditAction")
                {

                    var editForm = new AddEditCategoryForm(_categoryService, id, name, desc);
                    editForm.ShowDialog();
                    LoadCategories();
                }


                // 🗑️ Delete → Confirm Dialog

                if (dgvCategories.Columns[e.ColumnIndex].Name == "DeleteAction")
                {
                    var confirm = MessageBox.Show(
                        $"Are you sure you want to delete \"{name}\"?",
                        "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (confirm == DialogResult.OK)
                    {
                        try
                        {
                            await _categoryService.DeleteCategory(id);
                            MessageBox.Show("Deleted successfully!", "Success");
                            await LoadCategories();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
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

        private async Task LoadCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                dgvCategories.Rows.Clear();
                foreach (var cat in categories)
                {
                    dgvCategories.Rows.Add(
                        cat.Id,
                        cat.Name,
                        cat.Description,
                        cat.ProductCount + " items"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    }
}
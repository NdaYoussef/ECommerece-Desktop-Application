using ECommerece.Application.DTOs.CategoryDto;
using ECommerece.Application.IServices.ICategoryService;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ECommerece.Presentation.Forms.CategoryForms
{
    public class AddEditCategoryForm : Form
    {
        private readonly ICategoryServices _categoryService;
        private int? _categoryId = null;

        public AddEditCategoryForm(ICategoryServices categoryService)
        {
            _categoryService = categoryService;
            InitializeComponents();
        }
        public AddEditCategoryForm(ICategoryServices categoryService, int id, string name, string description)
        {
            _categoryService = categoryService;
            _categoryId = id;
            InitializeComponents();

            lblTitle.Text = "Edit Category";
            lblSubtitle.Text = "Update category information";
            btnSave.Text = "Update Category";
            txtCategoryName.Text = name;
            txtDescription.Text = description;
        }

        // ===== CONTROLS =====
        private Label lblTitle;
        private Label lblSubtitle;
        private TextBox txtCategoryName;
        private TextBox txtDescription;
        private Button btnSave;
        private Button btnCancel;

        // ===== Add Mode =====

        // ===== Edit Mode =====

        private void InitializeComponents()
        {
            // ===== FORM =====
            this.Text = _categoryId == null ? "Add Category" : "Edit Category";
            this.Size = new Size(650, 430);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 9f);

            // ===== TITLE =====
            lblTitle = new Label
            {
                Text = "Add New Category",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(30, 25)
            };
            this.Controls.Add(lblTitle);

            lblSubtitle = new Label
            {
                Text = "Create a new product category",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(30, 52)
            };
            this.Controls.Add(lblSubtitle);

            // X Close Button
            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(100, 116, 139),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(35, 35),
                Location = new Point(590, 15),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            // Separator
            var separator = new Panel
            {
                Size = new Size(590, 1),
                Location = new Point(30, 78),
                BackColor = Color.FromArgb(226, 232, 240)
            };
            this.Controls.Add(separator);

            // ===== CATEGORY NAME =====
            var lblCategoryName = new Label
            {
                Text = "Category Name *",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 100)
            };
            this.Controls.Add(lblCategoryName);

            txtCategoryName = new TextBox
            {
                Location = new Point(30, 122),
                Size = new Size(590, 35),
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(15, 23, 42),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Enter category name"
            };
            this.Controls.Add(txtCategoryName);

            // ===== DESCRIPTION =====
            var lblDescription = new Label
            {
                Text = "Description",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 175)
            };
            this.Controls.Add(lblDescription);

            txtDescription = new TextBox
            {
                Location = new Point(30, 197),
                Size = new Size(590, 100),
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(15, 23, 42),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Enter category description",
                Multiline = true
            };
            this.Controls.Add(txtDescription);

            // ===== BUTTONS =====
            btnSave = new Button
            {
                Text = "Create Category",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 91, 219),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 42),
                Location = new Point(30, 330),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnSave);

            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 42),
                Location = new Point(220, 330),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 1;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            this.Controls.Add(btnCancel);

            // ===== CLICK EVENTS =====
            btnCancel.Click += (s, e) => this.Close();

            btnSave.Click += async (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                {
                    MessageBox.Show("Category Name is required!", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if (_categoryId == null)
                    {
                        // Add
                        await _categoryService.AddCategory(new AddCategoryDto
                        {
                            Name = txtCategoryName.Text.Trim(),
                            Description = txtDescription.Text.Trim()
                        });
                        MessageBox.Show("Category added successfully!", "Success");
                    }
                    else
                    {
                        // Update
                        await _categoryService.UpdateCategory(new UpdateCategoryDto
                        {
                            Id = _categoryId.Value,
                            Name = txtCategoryName.Text.Trim(),
                            Description = txtDescription.Text.Trim()
                        });
                        MessageBox.Show("Category updated successfully!", "Success");
                    }
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
    }
}
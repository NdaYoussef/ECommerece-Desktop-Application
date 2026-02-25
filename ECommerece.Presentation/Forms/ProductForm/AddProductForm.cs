using System;
using System.Drawing;
using System.Windows.Forms;
using ECommerece.Application.DTOs.ProductDto;
using ECommerece.Application.IServices;

namespace ECommerece.Presentation.Forms.ProductForms
{
    public class AddProductForm : Form
    {
        private readonly IProductService _productService;

        // Controls
        private TextBox txtLabel,
            txtDescription,
            txtPrice,
            txtStockQuantity,
            txtImageUrl,
            txtCategoryId;
        private Button btnSave,
            btnCancel;
        private Label lblTitle;

        public AddProductForm(IProductService productService)
        {
            _productService = productService;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Add New Product";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.Font = new Font("Segoe UI", 9f);

            // Title
            lblTitle = new Label
            {
                Text = "➕ Add New Product",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(20, 20),
                AutoSize = true,
            };
            this.Controls.Add(lblTitle);

            // Label
            Label lblLabel = new Label
            {
                Text = "Product Name:",
                Location = new Point(20, 70),
                AutoSize = true,
            };
            txtLabel = new TextBox { Location = new Point(150, 67), Width = 300 };
            this.Controls.Add(lblLabel);
            this.Controls.Add(txtLabel);

            // Description
            Label lblDescription = new Label
            {
                Text = "Description:",
                Location = new Point(20, 110),
                AutoSize = true,
            };
            txtDescription = new TextBox
            {
                Location = new Point(150, 107),
                Width = 300,
                Multiline = true,
                Height = 60,
            };
            this.Controls.Add(lblDescription);
            this.Controls.Add(txtDescription);

            // Price
            Label lblPrice = new Label
            {
                Text = "Price:",
                Location = new Point(20, 185),
                AutoSize = true,
            };
            txtPrice = new TextBox { Location = new Point(150, 182), Width = 300 };
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);

            // Stock Quantity
            Label lblStock = new Label
            {
                Text = "Stock Quantity:",
                Location = new Point(20, 220),
                AutoSize = true,
            };
            txtStockQuantity = new TextBox { Location = new Point(150, 217), Width = 300 };
            this.Controls.Add(lblStock);
            this.Controls.Add(txtStockQuantity);

            // Image URL
            Label lblImage = new Label
            {
                Text = "Image URL:",
                Location = new Point(20, 255),
                AutoSize = true,
            };
            txtImageUrl = new TextBox { Location = new Point(150, 252), Width = 300 };
            this.Controls.Add(lblImage);
            this.Controls.Add(txtImageUrl);

            // Category ID
            Label lblCategory = new Label
            {
                Text = "Category ID:",
                Location = new Point(20, 290),
                AutoSize = true,
            };
            txtCategoryId = new TextBox { Location = new Point(150, 287), Width = 300 };
            this.Controls.Add(lblCategory);
            this.Controls.Add(txtCategoryId);

            // Buttons
            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(270, 340),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(59, 91, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(370, 340),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(100, 116, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel,
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            // Accept/Cancel buttons
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtLabel.Text))
            {
                MessageBox.Show(
                    "Product name is required.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid price greater than 0.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (!int.TryParse(txtStockQuantity.Text, out int stock) || stock < 0)
            {
                MessageBox.Show(
                    "Please enter a valid stock quantity (0 or more).",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (!int.TryParse(txtCategoryId.Text, out int categoryId) || categoryId <= 0)
            {
                MessageBox.Show(
                    "Please enter a valid Category ID (positive integer).",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
            btnSave.Enabled = false;
            btnSave.Text = "Saving...";
            // Create DTO
            var dto = new ProductCreateDto
            {
                Label = txtLabel.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text)
                    ? null
                    : txtDescription.Text.Trim(),
                Price = price,
                StockQuantity = stock,
                ImageUrl = string.IsNullOrWhiteSpace(txtImageUrl.Text)
                    ? null
                    : txtImageUrl.Text.Trim(),
                CategoryId = categoryId,
            };

            // try
            // {
            bool success = await _productService.AddProduct(dto);
            if (success)
            {
                MessageBox.Show(
                    "Product added successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(
                    $"Error adding product",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                btnSave.Enabled = true;
                btnSave.Text = "Save";
            }
            // }
            // catch (Exception ex)
            // {
            //     MessageBox.Show(
            //         $"Error adding product: {ex.Message}",
            //         "Error",
            //         MessageBoxButtons.OK,
            //         MessageBoxIcon.Error
            //     );
            //     btnSave.Enabled = true;
            //     btnSave.Text = "Save";
            // }
        }
    }
}

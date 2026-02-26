using System;
using System.Drawing;
using System.Windows.Forms;
using ECommerece.Application.DTOs.ProductDto;
using ECommerece.Application.IServices;

namespace ECommerece.Presentation.Forms.ProductForms
{
    public class EditProductForm : Form
    {
        private readonly IProductService _productService;
        private readonly int _productId;

        private TextBox txtLabel,
            txtDescription,
            txtPrice,
            txtStockQuantity,
            txtImageUrl,
            txtCategoryId;
        private Button btnSave,
            btnCancel;

        // Pass the product's ID and its current values in from the grid
        public EditProductForm(
            IProductService productService,
            int productId,
            string label,
            string description,
            decimal price,
            int stockQuantity,
            string imageUrl,
            int categoryId
        )
        {
            _productService = productService;
            _productId = productId;

            InitializeComponents();

            // Pre-fill all fields
            txtLabel.Text = label;
            txtDescription.Text = description;
            txtPrice.Text = price.ToString();
            txtStockQuantity.Text = stockQuantity.ToString();
            txtImageUrl.Text = imageUrl;
            txtCategoryId.Text = categoryId.ToString();
        }

        private void InitializeComponents()
        {
            this.Text = "Edit Product";
            this.Size = new Size(540, 410);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.Font = new Font("Segoe UI", 9f);

            this.Controls.Add(
                new Label
                {
                    Text = "✏️ Edit Product",
                    Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(15, 23, 42),
                    Location = new Point(20, 20),
                    AutoSize = true,
                }
            );

            txtLabel = AddField("Product Name:", 70);
            txtDescription = AddField("Description:", 110, multiline: true);
            txtPrice = AddField("Price:", 185);
            txtStockQuantity = AddField("Stock Quantity:", 220);
            txtImageUrl = AddField("Image URL:", 255);
            txtCategoryId = AddField("Category ID:", 290);

            btnSave = new Button
            {
                Text = "Save Changes",
                Location = new Point(245, 345),
                Size = new Size(110, 34),
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
                Location = new Point(365, 345),
                Size = new Size(90, 34),
                BackColor = Color.FromArgb(100, 116, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel,
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private TextBox AddField(string labelText, int y, bool multiline = false)
        {
            this.Controls.Add(
                new Label
                {
                    Text = labelText,
                    Location = new Point(20, y + 3),
                    AutoSize = true,
                }
            );
            var txt = new TextBox
            {
                Location = new Point(150, y),
                Width = 300,
                Multiline = multiline,
                Height = multiline ? 60 : 23,
            };
            this.Controls.Add(txt);
            return txt;
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            // ── Validation ────────────────────────────────────────
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
                    "Please enter a valid Category ID.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            btnSave.Enabled = false;
            btnSave.Text = "Saving...";

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

            bool success = await _productService.UpdateProduct(_productId, dto);

            if (success)
            {
                MessageBox.Show(
                    "Product updated successfully!",
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
                    "Failed to update product. Check that the Category ID exists.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                btnSave.Enabled = true;
                btnSave.Text = "Save Changes";
            }
        }
    }
}

using System;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using ECommerece.Application.DTOs.ProductDto;

namespace ECommerece.Presentation.Forms.ProductForms
{
    public class ProductDetailsForm : Form
    {
        // ── Colors ────────────────────────────────────────────────
        static readonly Color Navy     = Color.FromArgb(15, 23, 42);
        static readonly Color Accent   = Color.FromArgb(59, 91, 219);
        static readonly Color LightBg  = Color.FromArgb(241, 245, 249);
        static readonly Color TextGray = Color.FromArgb(100, 116, 139);
        static readonly Color White    = Color.White;

        public ProductDetailsForm(ProductDetailsDto product)
        {
            InitializeComponents(product);
        }

        private void InitializeComponents(ProductDetailsDto product)
        {
            this.Text            = product.Label ?? "Product Details";
            this.Size            = new Size(480, 560);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.BackColor       = LightBg;
            this.Font            = new Font("Segoe UI", 9f);

            // ── Image box ─────────────────────────────────────────
            var picBox = new PictureBox
            {
                Size      = new Size(440, 220),
                Location  = new Point(20, 20),
                BackColor = Color.FromArgb(226, 232, 240),
                SizeMode  = PictureBoxSizeMode.Zoom,
                Cursor    = Cursors.Default
            };
            // Draw a placeholder icon if no image loads
            picBox.Paint += (s, e) =>
            {
                if (picBox.Image == null)
                {
                    e.Graphics.DrawString("🖼️", new Font("Segoe UI", 36f),
                        new SolidBrush(Color.FromArgb(148, 163, 184)),
                        new PointF(picBox.Width / 2f - 30, picBox.Height / 2f - 30));
                }
            };
            this.Controls.Add(picBox);
            LoadImage(picBox, product.ImageUrl);

            // ── Product name ──────────────────────────────────────
            this.Controls.Add(new Label
            {
                Text      = product.Label ?? "—",
                Font      = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Navy,
                Location  = new Point(20, 255),
                Size      = new Size(440, 30),
                AutoSize  = false
            });

            // ── Category tag ──────────────────────────────────────
            var catTag = new Label
            {
                Text      = product.CategoryName ?? "Uncategorized",
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Accent,
                BackColor = Color.FromArgb(239, 246, 255),
                Location  = new Point(20, 292),
                AutoSize  = true,
                Padding   = new Padding(6, 3, 6, 3)
            };
            this.Controls.Add(catTag);

            // ── Price ─────────────────────────────────────────────
            this.Controls.Add(new Label
            {
                Text      = product.Price.ToString("C"),
                Font      = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Accent,
                Location  = new Point(20, 325),
                AutoSize  = true
            });

            // ── Stock status ──────────────────────────────────────
            bool inStock   = product.StockQuantity > 0;
            var statusLabel = new Label
            {
                Text      = inStock ? $"✅  In Stock ({product.StockQuantity} available)" : "❌  Out of Stock",
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = inStock ? Color.FromArgb(16, 185, 129) : Color.FromArgb(239, 68, 68),
                Location  = new Point(20, 367),
                AutoSize  = true
            };
            this.Controls.Add(statusLabel);

            // ── Description ───────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(product.Description))
            {
                this.Controls.Add(new Label
                {
                    Text      = product.Description,
                    Font      = new Font("Segoe UI", 9f),
                    ForeColor = TextGray,
                    Location  = new Point(20, 400),
                    Size      = new Size(440, 80),
                    AutoSize  = false
                });
            }

            // ── Close button ──────────────────────────────────────
            var btnClose = new Button
            {
                Text      = "Close",
                Location  = new Point(350, 490),
                Size      = new Size(110, 34),
                BackColor = Navy,
                ForeColor = White,
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnClose.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnClose);
            this.CancelButton = btnClose;
        }

        // ── Load image from URL without freezing the UI ───────────
        private async void LoadImage(PictureBox box, string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                var bytes = await client.GetByteArrayAsync(url);
                using var ms = new System.IO.MemoryStream(bytes);
                box.Image = Image.FromStream(ms);
            }
            catch
            {
                // URL broken or no network — placeholder stays
            }
        }
    }
}

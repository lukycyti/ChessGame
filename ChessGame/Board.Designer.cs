
namespace ChessGame
{
    partial class Board
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.Chargement = new System.Windows.Forms.Label();
            this.Turn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Chargement
            // 
            this.Chargement.AutoSize = true;
            this.Chargement.Font = new System.Drawing.Font("MS Reference Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chargement.Location = new System.Drawing.Point(378, 18);
            this.Chargement.Name = "Chargement";
            this.Chargement.Size = new System.Drawing.Size(0, 34);
            this.Chargement.TabIndex = 0;
            // 
            // Turn
            // 
            this.Turn.AutoSize = true;
            this.Turn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Turn.Location = new System.Drawing.Point(923, 192);
            this.Turn.Name = "Turn";
            this.Turn.Size = new System.Drawing.Size(0, 25);
            this.Turn.TabIndex = 1;
            // 
            // Board
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(984, 961);
            this.Controls.Add(this.Turn);
            this.Controls.Add(this.Chargement);
            this.DoubleBuffered = true;
            this.Name = "Board";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Board_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Chargement;
        private System.Windows.Forms.Label Turn;
    }
}


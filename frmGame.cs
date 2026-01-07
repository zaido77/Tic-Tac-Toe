using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tic_Tac_Toe_Game.Properties;

namespace Tic_Tac_Toe_Game
{
    public partial class frmGame : Form
    {
        enum enPlayer : byte
        {
            PlayerX = 0,
            PlayerO = 1,
        }

        enum enWinner : byte
        {
            InProgress = 0,
            PlayerX = 1,
            PlayerO = 2,
            Draw = 3
        }

        enPlayer playerTurn = enPlayer.PlayerX;
        enWinner winner = enWinner.InProgress;
        byte _SlotsSelectedCount = 0;

        public frmGame()
        {
            InitializeComponent();
        }

        void DrawGameLines(PaintEventArgs e)
        {
            Pen whitePen = new Pen(Color.White);
            whitePen.Width = 10;
            whitePen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            whitePen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            e.Graphics.DrawLine(whitePen, 635, 115, 635, 650);
            e.Graphics.DrawLine(whitePen, 850, 115, 850, 650);
            
            e.Graphics.DrawLine(whitePen, 440, 300, 1040, 300);
            e.Graphics.DrawLine(whitePen, 440, 477, 1040, 477);
        }

        bool AreEqual(object Slot1Tag, object Slot2Tag, object Slot3Tag, string XorO)
        {
            return Slot1Tag.ToString() == XorO &&
                Slot2Tag.ToString() == XorO &&
                Slot3Tag.ToString() == XorO;
        }

        void ColorToGreen(PictureBox pictureBox1, PictureBox pictureBox2, PictureBox pictureBox3)
        {
            pictureBox1.BackColor = Color.GreenYellow;
            pictureBox2.BackColor = Color.GreenYellow;
            pictureBox3.BackColor = Color.GreenYellow;
        }

        bool IsWinner(string XorO)
        {
            if (AreEqual(pbSlot00.Tag, pbSlot01.Tag, pbSlot02.Tag, XorO))
            {
                ColorToGreen(pbSlot00, pbSlot01, pbSlot02);
                return true;
            }
            else if (AreEqual(pbSlot10.Tag, pbSlot11.Tag, pbSlot12.Tag, XorO))
            {
                ColorToGreen(pbSlot10, pbSlot11, pbSlot12);
                return true;
            }
            else if (AreEqual(pbSlot20.Tag, pbSlot21.Tag, pbSlot22.Tag, XorO))
            {
                ColorToGreen(pbSlot20, pbSlot21, pbSlot22);
                return true;
            }
            else if (AreEqual(pbSlot00.Tag, pbSlot10.Tag, pbSlot20.Tag, XorO))
            {
                ColorToGreen(pbSlot00, pbSlot10, pbSlot20);
                return true;
            }
            else if (AreEqual(pbSlot01.Tag, pbSlot11.Tag, pbSlot21.Tag, XorO))
            {
                ColorToGreen(pbSlot01, pbSlot11, pbSlot21);
                return true;
            }
            else if (AreEqual(pbSlot02.Tag, pbSlot12.Tag, pbSlot22.Tag, XorO))
            {
                ColorToGreen(pbSlot02, pbSlot12, pbSlot22);
                return true;
            }
            else if (AreEqual(pbSlot00.Tag, pbSlot11.Tag, pbSlot22.Tag, XorO))
            {
                ColorToGreen(pbSlot00, pbSlot11, pbSlot22);
                return true;
            }
            else if (AreEqual(pbSlot02.Tag, pbSlot11.Tag, pbSlot20.Tag, XorO))
            {
                ColorToGreen(pbSlot02, pbSlot11, pbSlot20);
                return true;
            }

            return false;

        }

        bool IsDraw()
        {
            return (_SlotsSelectedCount >= 9);
        }

        void CheckWinner()
        {
            if (IsWinner("X"))
            {
                winner = enWinner.PlayerX;
            }
            else if (IsWinner("O"))
            {
                winner = enWinner.PlayerO;
            }
            else if (IsDraw())
            {
                winner = enWinner.Draw;
            }
            else
            {
                winner = enWinner.InProgress;
            }
        }

        string GetWinnerText()
        {
            switch (winner)
            {
                case enWinner.PlayerX:   return "Player X";
                case enWinner.PlayerO:   return "Player O";
                case enWinner.Draw:   return "Draw";
                default: return "In Progress";
            }
        }

        void PerformGameOverAction()
        {
            lblPlayerTurn.Text = "Game Over";
            lblWinner.Text = GetWinnerText();

            foreach (Control control in this.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Enabled = false;
                }
            }

            MessageBox.Show("Game Over", "Game Over", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        bool SetXOImage(PictureBox pbSlot)
        {
            if (pbSlot.Tag.ToString() != "Empty")
            {
                MessageBox.Show("Choice already used", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (playerTurn == enPlayer.PlayerX)
            {
                pbSlot.Image = Resources.X;
                pbSlot.Tag = "X";
            }
            else
            {
                pbSlot.Image = Resources.O;
                pbSlot.Tag = "O";
            }

            _SlotsSelectedCount++;
            return true;
        }

        void UpdatePlayerTurn()
        {
            if (playerTurn == enPlayer.PlayerX)
            {
                playerTurn = enPlayer.PlayerO;
                lblPlayerTurn.Text = "Player O";
            }
            else 
            {
                playerTurn = enPlayer.PlayerX;
                lblPlayerTurn.Text = "Player X";
            }

            CheckWinner();
            if (winner != enWinner.InProgress)
                PerformGameOverAction();
        }

        void ResetForm()
        {
            _SlotsSelectedCount = 0;

            lblPlayerTurn.Text = "Player X";
            lblWinner.Text = "In Progress";

            foreach (Control control in this.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Enabled = true;
                    pictureBox.Image = Resources.QuestionMark;
                    pictureBox.Tag = "Empty";
                    pictureBox.BackColor = Color.Black;
                }
            }
        }

        private void frmGame_Paint(object sender, PaintEventArgs e)
        {
            DrawGameLines(e);
        }

        private void pbAllSlots_Click(object sender, EventArgs e)
        {
            if (SetXOImage((PictureBox) sender))
                UpdatePlayerTurn();
        }

        private void btnRestartGame_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
    }
}

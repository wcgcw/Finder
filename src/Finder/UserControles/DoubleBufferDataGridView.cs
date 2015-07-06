using System.Windows.Forms;

namespace Finder.UserControles
{
    public class DoubleBufferDataGridView : DataGridView
    {
        public DoubleBufferDataGridView()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true); 
            UpdateStyles();
        }
    }
}
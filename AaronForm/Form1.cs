using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AaronForm
{
    public partial class Form1 : Form
    {
        private Tasks _tasksManager = new Tasks();

        public Form1()
        {
            InitializeComponent();

            this.dataGridView1.DataSource = this._tasksManager.GetTasks(); // populate tasks
            this.dataGridView1.Columns[0].Visible = false;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell changedCell = this.dataGridView1[e.ColumnIndex, e.RowIndex];
            string colName = this.dataGridView1.Columns[e.ColumnIndex].Name;
            DataGridViewCell rowid = this.dataGridView1[0, e.RowIndex];
            this._tasksManager.UpdateTask((long)rowid.Value, colName, changedCell.Value.ToString());
            MessageBox.Show($"Something changed {changedCell.Value} with id of {rowid.Value}");
            
            this.dataGridView1.EndEdit();
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                this.dataGridView1.EndEdit();
            }
        }
    }
}

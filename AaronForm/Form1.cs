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
        private Tasks _tasks;

        public Form1()
        {
            this._tasks = new Tasks();
            InitializeComponent();
            this.dataGridView1.DataSource = this._tasks.GetTasks(); // populate tasks
            this.dataGridView1.Columns[0].Visible = false;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3)
            {
                DataGridViewCell changedCell = this.dataGridView1[e.ColumnIndex, e.RowIndex];
                DataGridViewCell CellId = this.dataGridView1[0, e.RowIndex];
                MessageBox.Show($"Something changed {changedCell.Value} with id of {CellId.Value}");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                MessageBox.Show("Something clicked");
            }
        }
    }
}

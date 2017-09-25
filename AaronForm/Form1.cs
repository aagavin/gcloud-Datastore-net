using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AaronForm
{
    public partial class Form1 : Form
    {
        private Tasks _tasksManager;
        private FormAdd formAdd;
        private BindingSource bindingSource;

        public Form1()
        {
            InitializeComponent();

            this._tasksManager = new Tasks();
            this.bindingSource = new BindingSource();
            this.populateTasks();
            this.dataGridView1.Columns[0].Visible = false;
        }

        /// <summary>
        /// Sets the tasks into the data brid view
        /// </summary>
        private void populateTasks()
        {
            this.bindingSource.DataSource = this._tasksManager.GetTasks();
            this.dataGridView1.DataSource = this.bindingSource;
        }

        /// <summary>
        /// Dynamically update cells when they are edited 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell changedCell = this.dataGridView1[e.ColumnIndex, e.RowIndex];
            string colName = this.dataGridView1.Columns[e.ColumnIndex].Name;
            DataGridViewCell rowid = this.dataGridView1[0, e.RowIndex];
            this._tasksManager.UpdateTask((long)rowid.Value, colName, changedCell.Value.ToString());
            this.toolStripStatusLabel1.Text = $"Row changed {changedCell.Value} with id of {rowid.Value}";

            this.dataGridView1.EndEdit();
        }

        /// <summary>
        /// Allow dynamic updating of checkboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                this.dataGridView1.EndEdit();
            }
        }

        /// <summary>
        /// Add event button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_add_Click(object sender, EventArgs e)
        {
            formAdd = new FormAdd();
            formAdd.VisibleChanged += formAdd_closed;
            formAdd.Show();


        }

        /// <summary>
        /// Add event form close event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formAdd_closed(object sender, EventArgs e)
        {
            if (this.formAdd.Visible == false)
            {
                this._tasksManager.addTask(this.formAdd.textBox_name.Text, this.formAdd.textBox_description.Text);

                MessageBox.Show("Saved");
                this.formAdd.Close();
                this.button_refresh_Click(null,null);
            }
        }

        /// <summary>
        /// Refresh button event handler
        /// reset the list of tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_refresh_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            this.bindingSource.DataSource = this._tasksManager.GetTasks();
            this.bindingSource.ResetBindings(false);
        }

        /// <summary>
        /// Delete button event handler
        /// removes a task from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_delete_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows =  this.dataGridView1.SelectedRows;

            if(selectedRows.Count == 0)
            {
                MessageBox.Show("Use left margin when deleting");
                return;
            }

            List<string> ids = new List<string>();
            foreach(DataGridViewRow row in selectedRows)
            {
                ids.Add(row.Cells[0].Value.ToString());
                MessageBox.Show($"Removed id: {row.Cells[0].Value.ToString()}");
            }

            this._tasksManager.deleteTasks(ids);
            button_refresh_Click(null, null);

        }
    }
}

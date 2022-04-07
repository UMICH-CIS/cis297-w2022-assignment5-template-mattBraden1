using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayTable
{
    public partial class DisplayAuthorsTable : Form
    {
        //Entity Framework DbContext
        private BooksExamples.BooksEntities dbcontext = new BooksExamples.BooksEntities();
        public DisplayAuthorsTable()
        {
            InitializeComponent();
            authorBindingSource.DataSource = dbcontext.Authors.Local;
        }

        
        private void authorBindingNavigator_RefreshItems(object sender, EventArgs e)
        {
            //load Authors table ordered by LastName then FirstName
            dbcontext.Authors
                .OrderBy(author => author.LastName)
                .ThenBy(author => author.FirstName)
                .Load();
            //specify datasource for authorBindingSource
            
        }

        
        private void authorBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            authorBindingSource.EndEdit();
            try
            {
                dbcontext.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                MessageBox.Show("FirstName and LastName must contain values", "Entity Validation Exception");
            }
        }
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// Upon all new data put into the text box, updates the search for the query
        /// </summary>
        private void searchBar_TextChanged(object sender, EventArgs e)
        {
            var authorQuery =
                from author in dbcontext.Authors
                where author.LastName.StartsWith(searchBar.Text)
                orderby author.LastName, author.FirstName
                select author;
            authorBindingSource.DataSource = authorQuery.ToList();
            authorBindingSource.MoveFirst();
            bindingNavigatorAddNewItem.Enabled = false;
            bindingNavigatorDeleteItem.Enabled = false;
        }
        /// <summary>
        /// Matthew Braden, Winter 2022
        /// When the user clicks the button, resets the search and allows data manipulation again
        /// </summary>
        private void searchButton_Click(object sender, EventArgs e)
        {
            searchBar.Text = "";
            authorBindingSource.DataSource = dbcontext.Authors.Local;
            authorBindingSource.MoveFirst();
            bindingNavigatorAddNewItem.Enabled = true;
            bindingNavigatorDeleteItem.Enabled = true;
        }
    }
}

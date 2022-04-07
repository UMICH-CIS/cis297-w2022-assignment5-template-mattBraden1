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

namespace DisplayBooks
{
    public partial class DisplayQuery : Form
    {
        //Entity Framework DbContext
        private BooksExamples.BooksEntities dbcontext = new BooksExamples.BooksEntities();
        public DisplayQuery()
        {
            InitializeComponent();
            bookBindingSource.DataSource = dbcontext.Titles.Local;
        }

        
        private void bookBindingNavigator_RefreshItems(object sender, EventArgs e)
        {
            //load Authors table ordered by LastName then FirstName
            dbcontext.Titles
                .OrderBy(title => title.ISBN)
                .ThenBy(title => title.Title1)
                .Load();
            //specify datasource for authorBindingSource
            
        }

        
        private void bookBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            bookBindingSource.EndEdit();
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
            var bookQuery =
                from title in dbcontext.Titles
                where title.Title1.Contains(searchBar.Text)
                orderby title.ISBN, title.Title1
                select title;
            bookBindingSource.DataSource = bookQuery.ToList();
            bookBindingSource.MoveFirst();
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
            bookBindingSource.DataSource = dbcontext.Titles.Local;
            bookBindingSource.MoveFirst();
            bindingNavigatorAddNewItem.Enabled = true;
            bindingNavigatorDeleteItem.Enabled = true;
        }
    }
}

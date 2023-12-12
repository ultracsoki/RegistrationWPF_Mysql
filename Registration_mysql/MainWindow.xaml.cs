using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Registration_mysql
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		RegistrationService registrationService;
		public MainWindow(RegistrationService registrationService)
		{
			InitializeComponent();
			this.registrationService = registrationService;
			Read();
		}

		private void Create_Click(object sender, RoutedEventArgs e)
		{
			RegistrationForm form = new RegistrationForm(registrationService);
			form.Closed += (_, _) =>
			{
				Read();
			};
			form.ShowDialog();
		}

		private void Read()
		{
			registrationTable.ItemsSource = registrationService.GetAll();
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			Registration selected = registrationTable.SelectedItem as Registration;
			if (selected == null)
			{
				MessageBox.Show("A törléshez előbb válasson ki regisztrációt!");
				return;
			}
			MessageBoxResult selectedButton = MessageBox.Show($"Biztos, hogy törölni szeretné az alábbi regisztrációt: {selected.Username} ?",
				"Biztos?", MessageBoxButton.YesNo);
			if (selectedButton == MessageBoxResult.Yes)
			{
				if (registrationService.Delete(selected.Id))
				{
					MessageBox.Show("A törlés sikeres volt!");
				}
				else
				{
					MessageBox.Show("Hiba történt a törlés során!");
				}
				Read();
			}
		}

		private void Update_Click(object sender, RoutedEventArgs e)
		{
			Registration selected = registrationTable.SelectedItem as Registration;
			if (selected == null)
			{
				MessageBox.Show("A módosításhoz előbb válasson ki regisztrációt!");
				return;
			}
			RegistrationForm form = new RegistrationForm(registrationService, selected);
			form.Closed += (_, _) =>
			{
				Read();
			};
			form.ShowDialog();
		}
	}
}

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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Registration_mysql
{
    /// <summary>
    /// Interaction logic for RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        private RegistrationService registrationService;
        private Registration registrationToUpdate;

        public RegistrationForm(RegistrationService registrationService)
        {
            InitializeComponent();
            this.registrationService = registrationService;
        }

        public RegistrationForm(RegistrationService registrationService, Registration registrationToUpdate)
        {
            InitializeComponent();
            this.registrationService = registrationService;
            this.registrationToUpdate = registrationToUpdate;
            this.btnAdd.Visibility = Visibility.Collapsed;
            this.btnUpdate.Visibility = Visibility.Visible;

            tbUsername.Text = registrationToUpdate.Username;
            tbPassword.Text = registrationToUpdate.Password;
            tbEmail.Text = registrationToUpdate.Email;
            tbPhoneNumber.Text = registrationToUpdate.PhoneNumber.ToString();
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Registration registration = CreateRegistrationFromInputData();
                if (registrationService.Update(this.registrationToUpdate.Id, registration))
                {
                    MessageBox.Show("Sikeres módosítás");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hiba történt a módosítás során! Javasoljuk az ablak bezárását!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Registration registration = CreateRegistrationFromInputData();
                if (registrationService.Create(registration))
                {
                    MessageBox.Show("Sikeres hozzáadás");
                    tbUsername.Text = "";
                    tbPassword.Text = "";
                    tbEmail.Text = "";
                    tbPhoneNumber.Text = "";
                }
                else
                {
                    MessageBox.Show("Hiba történt a hozzáadás során");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Registration CreateRegistrationFromInputData()
        {
            string username = tbUsername.Text.Trim();
            string password = tbPassword.Text.Trim();
            string email = tbEmail.Text.Trim();
            string phoneNumberText = tbPhoneNumber.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Felhasználónév megadása kötelező");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Jelszó megadása kötelező");
            }
            if (!int.TryParse(phoneNumberText, out int phoneNumber))
            {
                throw new Exception("Telefonszám csak szám lehet");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email megadása kötelező");
            }

            if (!email.Contains('@'))
            {
                throw new Exception("Az emailnek tartalmaznia kell @ karaktert!");
            }


            Registration registration = new Registration();
            registration.Username = username;
            registration.Password = password;
            registration.Email = email;
            registration.PhoneNumber = phoneNumber;
            return registration;
        }
    }
}

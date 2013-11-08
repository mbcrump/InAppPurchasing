using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using InAppPurchasing.Resources;
using System.Text;
using Windows.ApplicationModel.Store;

namespace InAppPurchasing
{
    public partial class MainPage : PhoneApplicationPage
    {
        int m_pointCount=0;
        int i=1;
        bool ownsSuperWeapon=false;
        public string receipt;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnListAllProducts_Click_1(object sender, RoutedEventArgs e)
        {

            try
            {
                StringBuilder sb = new StringBuilder();

                var listing = await CurrentApp.LoadListingInformationAsync();
                foreach (var product in listing.ProductListings)
                {
                    sb.AppendLine(string.Format("{0}, {1}, {2},{3}, {4}",
                        product.Value.ProductId,
                        product.Value.Name,
                        product.Value.FormattedPrice,
                        product.Value.ProductType,
                        product.Value.Description));
                }

                MessageBox.Show(sb.ToString(), "List all products", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK);
            }
            
        }

        async private void btnBuy50Points_Click_1(object sender, RoutedEventArgs e)
        {
            //50 Points - Consumable
            var listing = await CurrentApp.LoadListingInformationAsync();
            var fiftypoints =
                listing.ProductListings.FirstOrDefault(p => p.Value.ProductId == "11111" && p.Value.ProductType == ProductType.Consumable);

            try
            {
                receipt = await CurrentApp.RequestProductPurchaseAsync(fiftypoints.Value.ProductId, true);

                if (CurrentApp.LicenseInformation.ProductLicenses[fiftypoints.Value.ProductId].IsActive)
                {
                    // do something
                    CurrentApp.ReportProductFulfillment(fiftypoints.Value.ProductId);
                    m_pointCount += 50;
                    txtBought50Pts.Visibility = System.Windows.Visibility.Visible;
                    txtBought50Pts.Text = "Bought 50 Points " + i++ + " times for a total of " + m_pointCount + "!";
                }
            }
            catch (Exception ex)
            {
                //catch exception
            }    
        }

        async private void btnBuySuperWeapon_Click_1(object sender, RoutedEventArgs e)
        {

            //Super Weapon - Durable
            var listing = await CurrentApp.LoadListingInformationAsync();
            var superweapon =
                listing.ProductListings.FirstOrDefault(p => p.Value.ProductId == "22222" && p.Value.ProductType == ProductType.Durable);
            try
            {

                if (CurrentApp.LicenseInformation.ProductLicenses[superweapon.Value.ProductId].IsActive)
                {
                    MessageBox.Show("You already own this weapon!");
                }
                else
                {
                    receipt = await CurrentApp.RequestProductPurchaseAsync(superweapon.Value.ProductId, true);
                    txtBoughtSW.Visibility = System.Windows.Visibility.Visible;
                    txtBoughtSW.Text = "You own the Super Weapon Now!";
                }
            }
            catch (Exception ex)
            {
                //catch exception
            }        
        }
    }
}
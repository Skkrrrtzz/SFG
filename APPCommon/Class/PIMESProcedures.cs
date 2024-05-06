using APPCommon.Models;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace APPCommon.Class
{
    public static class PIMESProcedures
    {
        #region DataGrid

        //public static async Task positionCursor(DataGrid dg, int intIndex)
        //{
        //    await Task.Delay(500);

        //    try
        //    {
        //        if (intIndex == 0)
        //        {
        //            dg.SelectedIndex = 0;
        //        }
        //        else if (intIndex >= dg.Items.Count)
        //        {
        //            dg.SelectedIndex = dg.Items.Count - 1;
        //            dg.ScrollIntoView(dg.Items[dg.SelectedIndex]);
        //        }
        //        else
        //        {
        //            dg.SelectedIndex = intIndex;
        //            if (intIndex == dg.Items.Count - 1)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 2)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 1]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 3)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 2]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 4)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 3]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 5)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 4]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 6)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 5]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 7)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 6]);
        //                return;
        //            }
        //            else if (intIndex <= dg.Items.Count - 8)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 7]);
        //                return;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        dg.SelectedIndex = -1;
        //    }
        //}

        //public static void positionSelected(DataGrid dg, int intIndex)
        //{
        //    try
        //    {
        //        if (intIndex == 0)
        //        {
        //            dg.SelectedIndex = 0;
        //        }
        //        else if (intIndex >= dg.Items.Count)
        //        {
        //            dg.SelectedIndex = dg.Items.Count - 1;
        //            dg.ScrollIntoView(dg.Items[dg.SelectedIndex]);
        //        }
        //        else
        //        {
        //            dg.SelectedIndex = intIndex;
        //            if (intIndex == dg.Items.Count - 1)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 2)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 1]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 3)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 2]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 4)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 3]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 5)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 4]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 6)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 5]);
        //                return;
        //            }
        //            else if (intIndex == dg.Items.Count - 7)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 6]);
        //                return;
        //            }
        //            else if (intIndex <= dg.Items.Count - 8)
        //            {
        //                dg.ScrollIntoView(dg.Items[dg.SelectedIndex + 7]);
        //                return;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        dg.SelectedIndex = -1;
        //    }
        //}

        //public static void sortDatagrid(DataGrid dg, string strColumn, string strFlow)
        //{
        //    dg.Items.SortDescriptions.Clear();

        //    if (strFlow == "desc")
        //    {
        //        dg.Items.SortDescriptions.Add(new SortDescription(strColumn, ListSortDirection.Descending));
        //    }
        //    else
        //    {
        //        dg.Items.SortDescriptions.Add(new SortDescription(strColumn, ListSortDirection.Ascending));
        //    }

        //    dg.Items.Refresh();
        //}

        #endregion DataGrid

        #region Loader

        //public static void setLoader(Grid grdA, Grid ldrA, int intmode)
        //{
        //    if (intmode == 1)
        //    {
        //        grdA.IsEnabled = false;
        //        ldrA.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        grdA.IsEnabled = true;
        //        ldrA.Visibility = Visibility.Collapsed;
        //    }
        //}

        #endregion Loader

        #region ListView

        //public static async Task positionListView(ListView lv, int intIndex)
        //{
        //    await Task.Delay(500);

        //    try
        //    {
        //        if (intIndex == 0)
        //        {
        //            lv.SelectedIndex = 0;
        //        }
        //        else if (intIndex >= lv.Items.Count)
        //        {
        //            lv.SelectedIndex = lv.Items.Count - 1;
        //            lv.ScrollIntoView(lv.Items[lv.SelectedIndex]);
        //        }
        //        else
        //        {
        //            lv.SelectedIndex = intIndex;
        //            if (intIndex == lv.Items.Count - 1)
        //            {
        //                lv.ScrollIntoView(lv.Items[lv.SelectedIndex]);
        //                return;
        //            }
        //            else if (intIndex == lv.Items.Count - 2)
        //            {
        //                lv.ScrollIntoView(lv.Items[lv.SelectedIndex + 1]);
        //                return;
        //            }
        //            else if (intIndex == lv.Items.Count - 3)
        //            {
        //                lv.ScrollIntoView(lv.Items[lv.SelectedIndex + 2]);
        //                return;
        //            }
        //            else if (intIndex == lv.Items.Count - 4)
        //            {
        //                lv.ScrollIntoView(lv.Items[lv.SelectedIndex + 3]);
        //                return;
        //            }
        //            else if (intIndex == lv.Items.Count - 5)
        //            {
        //                lv.ScrollIntoView(lv.Items[lv.SelectedIndex + 4]);
        //                return;
        //            }
        //            else if (intIndex == lv.Items.Count - 6)
        //            {
        //                lv.ScrollIntoView(lv.Items[lv.SelectedIndex + 5]);
        //                return;
        //            }
        //            else if (intIndex == lv.Items.Count - 7)
        //            {
        //                lv.ScrollIntoView(lv.Items[lv.SelectedIndex + 6]);
        //                return;
        //            }
        //            else if (intIndex <= lv.Items.Count - 8)
        //            {
        //                lv.ScrollIntoView(lv.Items[lv.SelectedIndex + 7]);
        //                return;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        lv.SelectedIndex = -1;
        //    }
        //}

        #endregion ListView

        #region Login

        //public static PIMESUserModel getLogin()
        //{
        //    PIMESUserModel result = null;

        //    if (Clipboard.ContainsData("myUser"))
        //    {
        //        result = (PIMESUserModel)Clipboard.GetData("myUser");
        //        PIMESVariables.TriviaDB = loadTrivia();
        //    }
        //    else
        //    {
        //        result = null;
        //    }

        //    return result;
        //}

        #endregion Login

        #region Background

        //public static void randomBackGround(ImageBrush varImgBrush)
        //{
        //    Random rndInt = new Random();

        //    switch (rndInt.Next(1, 11))
        //    {
        //        case 1:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/shattered-dark.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 500, 500);
        //            break;

        //        case 2:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/black-linen.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 482, 490);
        //            break;

        //        case 3:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/dark-wood.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 512, 512);
        //            break;

        //        case 4:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/diagmonds.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 141, 142);
        //            break;

        //        case 5:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/padded.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 160, 160);
        //            break;

        //        case 6:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/rebel.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 320, 360);
        //            break;

        //        case 7:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/swirl.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 200, 200);
        //            break;

        //        case 8:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/use-your-illusion.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 54, 58);
        //            break;

        //        case 9:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/white-diamond-dark.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 128, 224);
        //            break;

        //        case 10:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/black-lozenge.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 38, 38);
        //            break;

        //        default:
        //            break;
        //    }
        //}

        //public static void setBackGround(ImageBrush varImgBrush, int intBG)
        //{
        //    switch (intBG)
        //    {
        //        case 1:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/shattered-dark.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 500, 500);
        //            break;

        //        case 2:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/black-linen.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 482, 490);
        //            break;

        //        case 3:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/dark-wood.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 512, 512);
        //            break;

        //        case 4:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/diagmonds.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 141, 142);
        //            break;

        //        case 5:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/padded.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 160, 160);
        //            break;

        //        case 6:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/rebel.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 320, 360);
        //            break;

        //        case 7:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/swirl.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 200, 200);
        //            break;

        //        case 8:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/use-your-illusion.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 54, 58);
        //            break;

        //        case 9:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/white-diamond-dark.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 128, 224);
        //            break;

        //        case 10:
        //            varImgBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/APPCommon;component/Resources/Patterns/black-lozenge.png"));
        //            varImgBrush.Viewport = new Rect(0, 0, 38, 38);
        //            break;

        //        default:
        //            break;
        //    }
        //}

        #endregion Background

        #region Dialog

        //public static void showSuccess(string strMessage, string strIcon)
        //{
        //    dialogMsgBox.ShowBox("SUCCESS", strMessage, strIcon, "ok", 10);
        //}

        //public static void showError(string strMessage, string strIcon)
        //{
        //    dialogMsgBox.ShowBox("ERROR", strMessage, strIcon, "ok", 10);
        //}

        //public static int showConfirmation()
        //{
        //    return dialogMsgBox.ShowBox("Are you sure?", "\r\nPress YES to continue.", "question", "yesno", 10);
        //}

        //public static int showDeleteConfirmation()
        //{
        //    return dialogMsgBox.ShowBox("Are you sure you want to delete this record?", "\r\nPress YES to continue.", "question", "yesno", 10);
        //}

        //public static int showSaveConfirmation()
        //{
        //    return dialogMsgBox.ShowBox("Are you sure you want to save this record?", "\r\nPress YES to continue.", "question", "yesno", 10);
        //}

        #endregion Dialog

        #region Data

        public static short SetInt(this IDataReader reader, string nameOfColumn)
        {
            var indexOfColumn = reader.GetOrdinal(nameOfColumn);
            return (short)(reader.IsDBNull(indexOfColumn) ? 0 : reader.GetInt16(indexOfColumn));
        }

        public static int SetInt32(this IDataReader reader, string nameOfColumn)
        {
            var indexOfColumn = reader.GetOrdinal(nameOfColumn);
            return reader.IsDBNull(indexOfColumn) ? 0 : reader.GetInt32(indexOfColumn);
        }

        public static string SetString(this IDataReader reader, string nameOfColumn)
        {
            var indexOfColumn = reader.GetOrdinal(nameOfColumn);
            return reader.IsDBNull(indexOfColumn) ? "N/A" : reader.GetString(indexOfColumn);
        }

        public static DateTime? SetDateTime(this IDataReader reader, string nameOfColumn)
        {
            var indexOfColumn = reader.GetOrdinal(nameOfColumn);
            return reader.IsDBNull(indexOfColumn) ? null : reader.GetDateTime(indexOfColumn);
        }

        public static double SetDouble(this IDataReader reader, string nameOfColumn)
        {
            var indexOfColumn = reader.GetOrdinal(nameOfColumn);
            return reader.IsDBNull(indexOfColumn) ? 0 : reader.GetDouble(indexOfColumn);
        }

        public static bool SetBool(this IDataReader reader, string nameOfColumn)
        {
            var indexOfColumn = reader.GetOrdinal(nameOfColumn);
            return reader.IsDBNull(indexOfColumn) ? false : reader.GetBoolean(indexOfColumn);
        }

        public static decimal SetDecimal(this IDataReader reader, string nameOfColumn)
        {
            var indexOfColumn = reader.GetOrdinal(nameOfColumn);
            return reader.IsDBNull(indexOfColumn) ? 0 : reader.GetDecimal(indexOfColumn);
        }

        public static string GetString(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "N/A" : value;
        }

        //public static string GetDateTime(DateTime value)
        //{
        //    return value is null ? DateTime.Now : value;
        //}

        //public static int GetInt(string value)
        //{
        //    return int.TryParse(value, out int n) ? n : 0;
        //}

        #endregion Data

        #region Random

        public static string randomID(int length)
        {
            Random rnd = new Random();

            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        private static readonly Random _random = new Random();

        #endregion Random

        #region Trivia

        //public static string[] loadTrivia()
        //{
        //    var uri = new Uri("pack://application:,,,/APPCommon;component/Resources/Text/trivia.txt");
        //    var resourceStream = Application.GetResourceStream(uri);

        //    using (var sr = new StreamReader(resourceStream.Stream))
        //    {
        //        return sr.ReadToEnd().Split('\n');
        //    }
        //}

        public static string getTrivia(string[] trivia)
        {
            int totalcount = 4872;
            Random rand;
            rand = new Random();
            int randIndex = rand.Next(1, totalcount);

            return trivia[randIndex];
        }

        #endregion Trivia

        #region Email

        public static string[] getRandomEmail()
        {
            string strResult1;
            string strResult2;

            Random rand = new Random();
            int rnum = rand.Next(1, 5);

            if (rnum == 1)
            {
                strResult1 = PIMESSettings.mail1Account;
                strResult2 = PIMESSettings.mail1Password;
            }
            else if (rnum == 2)
            {
                strResult1 = PIMESSettings.mail2Account;
                strResult2 = PIMESSettings.mail2Password;
            }
            else if (rnum == 3)
            {
                strResult1 = PIMESSettings.mail3Account;
                strResult2 = PIMESSettings.mail3Password;
            }
            else if (rnum == 4)
            {
                strResult1 = PIMESSettings.mail4Account;
                strResult2 = PIMESSettings.mail4Password;
            }
            else if (rnum == 5)
            {
                strResult1 = PIMESSettings.mail3Account;
                strResult2 = PIMESSettings.mail3Password;
            }
            else
            {
                strResult1 = PIMESSettings.mail0Account;
                strResult2 = PIMESSettings.mail0Password;
            }

            return new[] { strResult1, strResult2 };
        }

        #endregion Email

        #region Focus

        //public static void focusCBO(ComboBox cbo)
        //{
        //    Keyboard.Focus(cbo);
        //    cbo.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        //}

        //public static void focusTXT(TextBox txt)
        //{
        //    Keyboard.Focus(txt);
        //    txt.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        //}

        //public static void focusATXT(AutoCompleteBox txt)
        //{
        //    Keyboard.Focus(txt);
        //    txt.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        //}

        #endregion Focus

        #region String

        public static string truncateString(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + ". . .";
        }

        #endregion String

        #region Converters
        public static int ToInt16OrDefault(this string value, int defaultValue = 0)
        {
            int result;
            return int.TryParse(value, out result) ? result : defaultValue;
        }

        #endregion Converters
    }

    #region Converter

    //public class NumberToColorBrushConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value is decimal)
    //        {
    //            return (((decimal)value) > -1 ? Brushes.Black : Brushes.Red);
    //        }

    //        throw new Exception("Invalid Value");
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    #endregion Converter
}
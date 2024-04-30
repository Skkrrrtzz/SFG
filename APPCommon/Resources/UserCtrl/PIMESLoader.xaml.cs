using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace APPCommon.Resources.UserCtrl
{
    public partial class PIMESLoader : UserControl
    {
        private static ArrayList lodrIdxCol = new ArrayList();

        public PIMESLoader()
        {
            InitializeComponent();
            randomLoader();
        }

        private void randomLoader()
        {
            Random rnd = new Random();
            string[] lodrType = { "1", "2", "3", "4", "5","6", "7", "8", "9", "10",
                                "11", "12", "13", "14", "15","16", "17", "18", "19", "20",
                                "21", "22", "23", "24", "25","26", "27"};

            int lodrIdx;

            do
            {
                lodrIdx = rnd.Next(0, lodrType.Length);
                if (!(lodrIdxCol.Contains(lodrIdx)))
                    break;
            } while (lodrIdxCol.Contains(lodrIdx));
            lodrIdxCol.Add(lodrIdx);

            if (lodrIdxCol.Count == 27)
                lodrIdxCol.Clear();

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("pack://application:,,,/APPCommon;component/Resources/Loaders/ld_" + lodrType[lodrIdx] + ".GIF");

            image.EndInit();

            WpfAnimatedGif.ImageBehavior.SetAnimatedSource(loaderGIF, image);
        }
    }
}
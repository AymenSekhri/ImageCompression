using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace K_Mean_Image_Conration
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		Matrix<double> PicMat;
		const int PicSize = 128; // chnage it to the image size (large sizes may cause slow learning)
		const int ColorsNumber = 16; //Number of Color Clusters (increase it to get better quality)

		private void button2_Click(object sender, EventArgs e)
		{

			KMeans KM = new KMeans(ColorsNumber);
			Vector<double> Assigns = KM.Train(PicMat, 5);
			Bitmap pic = new Bitmap(PicSize, PicSize);
			for (int y = 0; y < PicSize; y++)
			{
				for (int x = 0; x < PicSize; x++)
				{
					int R = Convert.ToInt32(KM.ClustersLocation[(int)Assigns[x + PicSize * y], 0]);
					int G = Convert.ToInt32(KM.ClustersLocation[(int)Assigns[x + PicSize * y], 1]);
					int B = Convert.ToInt32(KM.ClustersLocation[(int)Assigns[x + PicSize * y], 2]);
					pic.SetPixel(x, y, Color.FromArgb(R, G, B));
				}
			}
			pictureBox1.Image = pic;

		}

		private void Form1_Load(object sender, EventArgs e)
		{


		}
		private void button1_Click(object sender, EventArgs e)
		{
			OpenFileDialog fd = new OpenFileDialog();
			if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Image img;
				using (var bmpTemp = new Bitmap(fd.FileName))
				{
					img = new Bitmap(bmpTemp);
				}
				Bitmap pic = new Bitmap(img, PicSize, PicSize);
				PicMat = Matrix<double>.Build.Dense(PicSize * PicSize, 3);

				Color p;
				for (int y = 0; y < PicSize; y++)
				{
					for (int x = 0; x < PicSize; x++)
					{
						p = pic.GetPixel(x, y);
						PicMat[x + PicSize * y, 0] = p.R;
						PicMat[x + PicSize * y, 1] = p.G;
						PicMat[x + PicSize * y, 2] = p.B;
					}
				}

				pictureBox1.Image = pic;
			}
		}
	}
}

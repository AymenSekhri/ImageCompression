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

	class KMeans
	{
		int NumberOfClusters = 1;
		public Matrix<double> ClustersLocation;
		public KMeans(int _NumberOfClusters)
		{
			NumberOfClusters = _NumberOfClusters;

		}
		public Vector<double> Train(Matrix<double> inputs, int iterations)
		{
			ClustersLocation = Matrix<double>.Build.Dense(NumberOfClusters, inputs.ColumnCount);
			RandomizeMatrix(inputs,ref ClustersLocation);
			Vector<double> InputAssignment = Vector<double>.Build.Dense(inputs.RowCount);
			for (int iter = 0; iter < iterations; iter++)
			{
				Assign(inputs, ref InputAssignment);
				MoveToTheMean(inputs, InputAssignment);
			}
			return InputAssignment;
		}
		void Assign(Matrix<double> inputs, ref Vector<double> InputAssignment)
		{
			Vector<double> Scores = Vector<double>.Build.Dense(NumberOfClusters);
			Vector<double> DisVector, DisVector1, DisVector2;
			for (int i = 0; i < inputs.RowCount; i++)
			{
				for (int c = 0; c < NumberOfClusters; c++)
				{
					DisVector1 = Vector<double>.Build.DenseOfArray(ClustersLocation.ToRowArrays()[c]);
					DisVector2 = Vector<double>.Build.DenseOfArray(inputs.ToRowArrays()[i]);
					DisVector = DisVector1 - DisVector2;
					Scores[c] = DisVector.DotProduct(DisVector);
				}
				InputAssignment[i] = Scores.MinimumIndex();
				Scores.Clear();
			}

		}
		void MoveToTheMean(Matrix<double> inputs, Vector<double> InputAssignment)
		{
			Matrix<double> NewLocations = Matrix<double>.Build.Dense(ClustersLocation.RowCount, ClustersLocation.ColumnCount);
			Vector<double> Scores = Vector<double>.Build.Dense(NumberOfClusters);
			for (int i = 0; i < inputs.RowCount; i++)
			{
				for (int c = 0; c < inputs.ColumnCount; c++)
				{
					NewLocations[(int)InputAssignment[i], c] += inputs[i, c];
				}
				Scores[(int)InputAssignment[i]]++;

			}
			for (int c = 0; c < NumberOfClusters; c++)
			{
				for (int i = 0; i < ClustersLocation.ColumnCount; i++)
				{
					if (Scores[c] == 0)
					{
						NewLocations[c, i] = ClustersLocation[c, i];
					}
					else
					{
						NewLocations[c, i] = NewLocations[c, i] / Scores[c];
					}
				}
			}
			ClustersLocation = NewLocations;
		}
		public void RandomizeMatrix( Matrix<double> Inputs,ref Matrix<double> Mat)
		{
			Random ran = new Random();
			for (int i = 0; i < Mat.RowCount; i++)
			{
				for (int c = 0; c < Mat.ColumnCount; c++)
				{
					Mat[i, c] = ran.Next(0, 255);
				}
			}
		}
	}
}

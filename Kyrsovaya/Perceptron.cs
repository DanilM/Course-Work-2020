using Kyrsovaya.Properties;
using System;
using System.IO;

namespace Kyrsovaya
{
  class Perceptron
  {
    public Perceptron(double InputValue1, double InputValue2, double InputValue3)
    {
      this.InputValue[0] = InputValue1;
      this.InputValue[1] = InputValue2;
      this.InputValue[2] = InputValue3;
      //this.OutputRight[0] = OutputRight1;
      //this.OutputRight[1] = OutputRight2;
      //this.OutputRight[2] = OutputRight3;
    }

    public string path1 { get; set; }
    public string path2 { get; set; }
    public string path3 { get; set; }

    public double[,] InputWeight = new double[4, 3];
    public double[,] HiddenWeight = new double[3, 4];

    public double[] InputValue = new double[3];
    public double[] SecondValue = new double[4];
    public double[] OutputReal = new double[3];
    public double[] OutputRight = new double[3];

    public double[] DeltaHidden = new double[4];

    public void Calc()
    {
      double res;
      for (int i = 0; i < InputWeight.GetLength(0); i++)
      {
        res = 0;
        for (int j = 0; j < InputWeight.GetLength(1); j++)
        {
          res += InputWeight[i, j] * InputValue[j];
        }
        SecondValue[i] = Sigmoid(res);
      }

      for (int i = 0; i < HiddenWeight.GetLength(0); i++)
      {
        res = 0;
        for (int j = 0; j < HiddenWeight.GetLength(1); j++)
        {
          res += HiddenWeight[i, j] * SecondValue[j];
        }
        OutputReal[i] = Sigmoid(res);
      }
    }

    public double Sigmoid(double x)
    {
      return 1 / (1 + Math.Exp((-1) * x));
    }

    
    public ref double[] Start()
    {
      
      GetWeight();
      Calc();
      //FindDelta();
      //Correction();
      //SaveWeight();
      return ref OutputReal;
    }

    public void FindDelta()
    {
      double res;
      for (int j = 0; j < HiddenWeight.GetLength(1); j++)
      {
        res = 0;
        for (int i = 0; i < HiddenWeight.GetLength(0); i++)
        {
          res += HiddenWeight[i, j] * (OutputRight[i] - OutputReal[i]);
        }
        DeltaHidden[j] = res;
        
      }
    }

    public void Correction()
    {
      for (int i = 0; i < InputWeight.GetLength(0); i++)
      {
        for (int j = 0; j < InputWeight.GetLength(1); j++)
        {
          InputWeight[i, j] += DeltaHidden[i] * SecondValue[i] * (1 - SecondValue[i]) * InputValue[j] * 0.5;
        }
      }

      for (int j = 0; j < HiddenWeight.GetLength(1); j++)
      {
        for (int i = 0; i < HiddenWeight.GetLength(0); i++)
        {
          HiddenWeight[i, j] += (OutputRight[i] - OutputReal[i]) * OutputReal[i] *(1 - OutputReal[i]) * SecondValue[j] * 0.5;
        }
      }
    }

    public void GetWeight()
    {
      string pathWeight = @"Весы.txt";
      StreamReader RS = new StreamReader(pathWeight);
      for (int i = 0; i < InputWeight.GetLength(0); i++)
      {
        for (int j = 0; j < InputWeight.GetLength(1); j++)
        {
          InputWeight[i, j] = double.Parse(RS.ReadLine());
        }
      }
      for (int i = 0; i < HiddenWeight.GetLength(0); i++)
      {
        for (int j = 0; j < HiddenWeight.GetLength(1); j++)
        {
          HiddenWeight[i, j] = double.Parse(RS.ReadLine());
        }
      }
      RS.Close();
    }

    public void SaveWeight()
    {
      string pathWeight = @"Весы.txt";
      StreamWriter WS = new StreamWriter(pathWeight);
      for (int i = 0; i < InputWeight.GetLength(0); i++)
      {
        for (int j = 0; j < InputWeight.GetLength(1); j++)
        {
          //Console.WriteLine(InputWeight[i, j]);
          WS.WriteLine(InputWeight[i, j]);
        }
      }
      for (int i = 0; i < HiddenWeight.GetLength(0); i++)
      {
        for (int j = 0; j < HiddenWeight.GetLength(1); j++)
        {
          WS.WriteLine(HiddenWeight[i, j]);
        }
      }
      WS.Close();
    }
  }
}

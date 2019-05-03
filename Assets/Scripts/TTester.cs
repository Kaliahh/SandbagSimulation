using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;



namespace SandbagSimulation
{
    public class TTester
    {

        #region Fields
        public List<double> ListOfErrors { get;  private set; }
        public bool TTestResultsAreValid { get; private set; }


        public int SampleSize { get; private set; }
        public double ErrorMean { get; private set; }
        public double StandardDeviation { get; private set; }
        public double TStatistic { get; private set; }
        public double PNullHypothesis { get; private set; }
        public double CohensD { get; private set; }

        #endregion

        #region Constructor

        #endregion

        #region RunTTest

        // Metoden styrer kontrolflowet i en t-test på listOfErrors og resultatet gemmes i public get/private set felter.
        public void RunTTest(List<double> listOfErrors)
        {
            TTestResultsAreValid = true;

            ListOfErrors = listOfErrors;
            SampleSize = listOfErrors.Count;

            /* Hvis samplestørrelsen er 0 eller 1, er hhv. middelværdi og standardafvigelse ikke defineret,
             * og t-testen afbrydes derfor med TTestResultsAreValid = false som markør. */
            if (SampleSize >= 30)
            {
                ErrorMean = GetMean(ListOfErrors);

                // Hvis ListOfErrors er fuldstændigt homogen, sættes standardafvigelse til en værdi nær nul, for at t kan beregnes.
                if (ListOfErrors.Distinct().ToList().Count > 1)
                {
                    double sumOfSquares = GetSumOfSquares(ListOfErrors, ErrorMean);
                           StandardDeviation = GetStandardDeviation(sumOfSquares, SampleSize);
                }
                else
                {
                    StandardDeviation = 0.001;
                }
                    
                TStatistic = GetT(0, ErrorMean, StandardDeviation, SampleSize);
                PNullHypothesis = GetP(TStatistic, SampleSize);
                CohensD = GetCohensD(ErrorMean, 0, StandardDeviation);
            }
            else
            {
                TTestResultsAreValid = false;
                //ErrorMean = (SampleSize == 0) ? double.NaN : ListOfErrors.First();
            }

        }
        #endregion

        #region TTest

        // Metoden tager en liste af doubles og returnerer middelværdien af dens elementer. 
        public double GetMean(List <double> listOfErrors)
        {
            return listOfErrors.Average();
        }

        // Metoden returnerer standardafvigelsen ud fra en listOfErrors og en gennemsnits.
        public double GetStandardDeviation(double sumOfSquares, int sampleSize)
        {
            return Math.Sqrt(sumOfSquares / (sampleSize - 1));
        }

        // Metoden tager en liste af doubles samt middelværdien af dens elementer og returnerer kvadratsummen.
        public double GetSumOfSquares(List<double> listOfErrors, double errorMean)
        {
            return listOfErrors.Select(sandbagError => Math.Pow((sandbagError - errorMean), 2)).Sum();
        }

        // Metoden beregner teststørrelsen t til en one-sample t-test, hvor H0 er, at den gennemsnitlige afvigelse er 0.
        public double GetT(double hypothesizedMean, double errorMean, double standardDeviation, int sampleSize)
        {
            return (errorMean - hypothesizedMean) * (Math.Sqrt(sampleSize) / standardDeviation);
        }

        // Metoden tager teststørrelsen t samt en samplestørrelse og returnerer sandsynligheden for værdier >= t under H0.
        public double GetP(double tStatistic, int sampleSize)
        {
            double degreesOfFreedom = sampleSize - 1;

            return Student(tStatistic, degreesOfFreedom);
        }


        /* Denne metode (lettere modifikation af ACM #395) estimerer det et-halede areal under Student's t-fordeling med inputtets antal frihedsgrader 
         * Dette gøres i praksis ved at estimere den tilsvarende z-værdi under normalfordelingen og hente arealet fra metoden Gauss. */
        private double Student(double t, double degreesOfFreedom)
        {
            double n = degreesOfFreedom;
            double a,
                   b,
                   y;

            t = t * t;
            y = t / n;
            b = y + 1.0;

            if (y > 1.0E-6)
            {
                y = Math.Log(b);
            }

            a = n - 0.5;
            b = 48.0 * a * a;
            y = a * y;
            y = (((((-0.4 * y - 3.3) * y - 24.0) * y - 85.5) /
              (0.8 * y * y + 100.0 + b) + y + 3.0) / b + 1.0) *
              Math.Sqrt(y);

            return /*2.0 **/ Gauss(-y);
        }


        // Denne metode (ACM #209) tager en z-værdi og estimerer det et-halede areal under normalfordelingen.
        private double Gauss(double z)
        {
            double y;
            double p;
            double w;

            if (z == 0.0)
            {
                p = 0.0;
            }
            else
            {
                y = Math.Abs(z) / 2;
                if (y >= 3.0)
                {
                    p = 1.0;
                }
                else if (y < 1.0)
                {
                    w = y * y;
                    p = ((((((((0.000124818987 * w
                      - 0.001075204047) * w + 0.005198775019) * w
                      - 0.019198292004) * w + 0.059054035642) * w
                      - 0.151968751364) * w + 0.319152932694) * w
                      - 0.531923007300) * w + 0.797884560593) * y * 2.0;
                }
                else
                {
                    y = y - 2.0;
                    p = (((((((((((((-0.000045255659 * y
                      + 0.000152529290) * y - 0.000019538132) * y
                      - 0.000676904986) * y + 0.001390604284) * y
                      - 0.000794620820) * y - 0.002034254874) * y
                      + 0.006549791214) * y - 0.010557625006) * y
                      + 0.011630447319) * y - 0.009279453341) * y
                      + 0.005353579108) * y - 0.002141268741) * y
                      + 0.000535310849) * y + 0.999936657524;
                }
            }
            if (z > 0.0)
            {
                return (p + 1.0) / 2;
            }
            else
            {
                return (1.0 - p) / 2;
            }
        }

        // Metoden beregner effektstørrelsen Cohens D ud fra inputværdierne og ud fra samme nulhypotese som ovenfor.
        public double GetCohensD(double errorMean, double hypothesizedMean, double standardDeviation)
        {
            return (errorMean - hypothesizedMean) / standardDeviation;
        }
        #endregion

    }
}
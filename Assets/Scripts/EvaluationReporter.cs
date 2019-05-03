﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace SandbagSimulation
{

    public class EvaluationReporter
    {

        private const double SignificanceLevel = 0.05;
        private const double MediumEffectLowerBound = 0.5;
        private const double LargeEffectLowerBound = 0.8;


        // Metoden returnerer på baggrund af to sæt af t-testresultater en tekststreng, som udgør en samlet rapport af de statistiske analyser.
        public string GetEvaluationReport(TTester rotationResults, TTester positionResults)
        {
            if (!rotationResults.TTestResultsAreValid)
            {
                return "Der er ikke nok sandsække i det byggede dige til at vurdere konstruktionen statistisk.";
            }
            else if (rotationResults.PNullHypothesis >= SignificanceLevel && positionResults.PNullHypothesis >= SignificanceLevel)
            {
                return GetVeryGoodResultsReport(rotationResults, positionResults);
            }
            else
            {
                string rotationReport = GetRotationReport(rotationResults, positionResults);
                string positionReport = GetPositionReport(rotationResults, positionResults);

                return rotationReport + positionReport;
            }
        }

        // Denne metode returnerer tekststrengen for tilfældet, hvor ingen t-værdier var statistisk signifikante.
        private string GetVeryGoodResultsReport(TTester rotationResults, TTester positionResults)
        {
            return "Samlet set er diget bygget optimalt."
                    + " Iflg. statistiske analyser vender sandsækkene i samme retning som i det optimale dige"
                    + GetTTestReport(rotationResults)
                    + " Derudover har de stort set samme placering som sandsækkene i det optimale dige"
                    + GetTTestReport(positionResults);
        }


        // Denne metode returnerer tekststrengen til t-testen for rotation i tilfældet med én eller flere signifikante t-værdier.
        private string GetRotationReport(TTester rotationResults, TTester positionResults)
        {
            return "Sandsækkenes orientering afviger " + GetErrorPredicate(rotationResults) + " fra det optimale dige" + GetTTestReport(rotationResults);
        }

        // Denne metode returnerer tekststrengen til t-testen for position i tilfældet med én eller flere signifikante t-værdier.
        private string GetPositionReport(TTester rotationResults, TTester positionResults)
        {
            string positionReport;

            if (TestsAreBothSignificantWithSameEffects(rotationResults, positionResults))
            {
                positionReport = "$Det samme gør sig gældende for sandsækkenes placering"
                                 + GetTTestReport(positionResults);
            }
            else
            {
                positionReport = GetIntroToSecondSentence(rotationResults, positionResults)
                                 + GetErrorPredicate(positionResults)
                                 + " fra de optimale"
                                 + GetTTestReport(positionResults);
            }

            return positionReport;
        }


        /* Denne metode returnerer på baggrund af forholdene mellem de to t-tests den rette indledning til 
         * den anden sætning i rapporteringen med en eller flere signifikante t-værdier. */
        private string GetIntroToSecondSentence(TTester rotationResults, TTester positionResults)
        {
            var secondSentenceIntro = "$Til gengæld afviger sandsækkenes positioner ";

            if (TestsResultsHaveSimilarValence(rotationResults, positionResults))
            {
                secondSentenceIntro = "$Sandsækkenes positioner afviger desuden ";
            }

            return secondSentenceIntro;
        }


        /* Denne metode returnerer på baggrund af signifikans og effektstørrelse for et test-resultat
         * et vurderingsord, der angiver, hvordan testen er gået. */
        private string GetErrorPredicate(TTester tResults)
        {
            string predicate;

            if (tResults.PNullHypothesis >= SignificanceLevel)
            {
                predicate = "ikke betydeligt";
            }
            else if (tResults.CohensD < MediumEffectLowerBound)
            {
                predicate = "kun en smule";
            }
            else if (tResults.CohensD < LargeEffectLowerBound)
            {
                predicate = "en del";
            }
            else
            {
                predicate = "meget";
            }
                
            return predicate;
  
        }


        /* Denne metode returnerer på baggrund af et sæt af t-testresultater en tekststreng, som opsummerer de statistiske størrelser
         * fundet i testen. Efter konventionerne rapporteres effektstørrelsen kun ved et signifikant resultat. */
        private string GetTTestReport(TTester tResults)
        {
            string pReport;

            if (tResults.PNullHypothesis < SignificanceLevel)
            {
                pReport = (tResults.PNullHypothesis == 0) ? "p < 0.001" : $"p = {Math.Round(tResults.PNullHypothesis, 2)}";
                pReport += $" og Cohens d = {Math.Round(tResults.CohensD, 2)}.";
            }
            else
                pReport = $"p \u2265 {SignificanceLevel}.";


            return $", t({tResults.SampleSize - 1}) = {Math.Round(tResults.TStatistic, 2)}, " + pReport;
        }


        /* Denne metode returnerer sandhedsværdien af udsagnet, at effektstørrelsen for de to t-tests er af 
         * samme orden iflg. konventionelle mål. */
        private bool TestsHaveSameEffectSizes(double rotationD, double positionD)
        {

            bool sameEffectSize = false;

            if ((rotationD < MediumEffectLowerBound && positionD < MediumEffectLowerBound) || (rotationD >= LargeEffectLowerBound && positionD >= LargeEffectLowerBound))
            {
                sameEffectSize = true;
            }
            else if ((rotationD < LargeEffectLowerBound && positionD < LargeEffectLowerBound) && (rotationD >= MediumEffectLowerBound && positionD >= MediumEffectLowerBound))
            {
                sameEffectSize = true;
            }

            return sameEffectSize;
        }

        // Denne metode returnerer sandhedsværdien af udsagnet, at testresultaterne havde lign. valens ("Godt/Skidt").
        private bool TestsResultsHaveSimilarValence(TTester rotationResults, TTester positionResults)
        {
            return TestsAreBothSignificantWithMediumOrLargeEffects(rotationResults, positionResults)
                   || OneTestsWasNonsignificantWhileTheOtherHadASmallEffect(rotationResults, positionResults);
        }


        // Denne metode returnerer sandhedsværdien af udsagnet, at begge tests var signifikant med >= medium effektstørrelse.
        private bool TestsAreBothSignificantWithMediumOrLargeEffects(TTester rotationResults, TTester positionResults)
        {

            var testsAreBothSignificant = (rotationResults.PNullHypothesis < SignificanceLevel) && (positionResults.PNullHypothesis < SignificanceLevel);

            var testsBothHaveAtLeastMediumEffects = (rotationResults.CohensD >= MediumEffectLowerBound) && (positionResults.CohensD >= MediumEffectLowerBound);

            return testsAreBothSignificant && testsBothHaveAtLeastMediumEffects;
        }


        // Denne metode returnerer sandhedsværdien af udsagnet, at en af de to tests var signifikant med lille effekt, mens den anden ikke var signifikant.
        private bool OneTestsWasNonsignificantWhileTheOtherHadASmallEffect(TTester rotationResults, TTester positionResults)
        {
            var rotationSignificantWithSmallEffect = (rotationResults.PNullHypothesis < SignificanceLevel) && (rotationResults.CohensD < MediumEffectLowerBound);
            var positionSignificantWithSmallEffect = (positionResults.PNullHypothesis < SignificanceLevel) && (positionResults.CohensD < MediumEffectLowerBound);

            var eitherTestsWasNonsignificant = (rotationResults.PNullHypothesis >= SignificanceLevel) ^ (positionResults.PNullHypothesis >= SignificanceLevel);

            return eitherTestsWasNonsignificant && (rotationSignificantWithSmallEffect ^ positionSignificantWithSmallEffect);
        }


        // Denne metode returnerer sandhedsværdien af udsagnet, at begge tests er signifikante med effekstørrelser af samme ordener.
        private bool TestsAreBothSignificantWithSameEffects(TTester rotationResults, TTester positionResults)
        {
            var testsAreBothSignificant = (rotationResults.PNullHypothesis < SignificanceLevel) && (positionResults.PNullHypothesis < SignificanceLevel);
            
            return testsAreBothSignificant && TestsHaveSameEffectSizes(rotationResults.CohensD, positionResults.CohensD);
        }


    }
}
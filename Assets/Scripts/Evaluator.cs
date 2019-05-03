using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;




namespace SandbagSimulation
{
    public class Evaluator
    {


        #region Fields

        /* DikeAnalyst finder det byggede diges afvigelse fra det optimale i
         * forhold til de enkelte sandsækkes position og rotation. */
        private DikeErrorFinder DikeAnalyst;

        // De to TTester-komponenter kan udføre one-sample t-tests på DikeAnalysts resultater.
        public TTester RotationalTTest;
        public TTester PositionalTTest;

        // EvaluationReporter kan skabe en formateret tekststreng som output til t-testene.
        private EvaluationReporter TTestReporter;
        public string EvaluationReport { get; private set; }
        #endregion

        #region Constructor
        public Evaluator ()
        {
            DikeAnalyst = new DikeErrorFinder();
            RotationalTTest = new TTester();
            PositionalTTest = new TTester();
            TTestReporter = new EvaluationReporter();
        }

        #endregion

        #region EvaluateDike


        /* Metoden kaldes med et Blueprint samt punktet, hvorfra dronerne henter sandsækkene.
         * Den styrer en evalueringsproces, hvor både digets afvigelse fra optimal rotation og position testes.  */
        public void EvaluateDike(Blueprint blueprint, Vector3 pickupPoint)
        {
            /* DikeAnalyst-komponenten fremskaffer en liste over de enkelte placerede sandsækkes afvigelse
             * i position og rotation fra det optimale dige */
            DikeAnalyst.GetListsOfErrors(blueprint, pickupPoint);


            // Der køres one-sample t-tests for både rotation og position og resultatet kan tilgås via komponenterne.
            RotationalTTest.RunTTest(DikeAnalyst.RotationalErrorsList);
            PositionalTTest.RunTTest(DikeAnalyst.PositionalErrorsList);

            EvaluationReport = TTestReporter.GetEvaluationReport(RotationalTTest, PositionalTTest);
        }


        #endregion
    }
}
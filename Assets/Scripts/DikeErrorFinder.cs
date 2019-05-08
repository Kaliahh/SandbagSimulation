using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;




namespace SandbagSimulation
{
    public class DikeErrorFinder
    {

        #region Fields
        public List<double> RotationalErrorsList { get; private set; }
        public List<double> PositionalErrorsList { get; private set; }

        private AcceptedErrorDeltas ErrorDeltas;

        public Vector3 PickupPoint { get; set; }

        public DikeFinder BuiltDikeFinder;
        public DikeOptimizer OptimalDikeFinder;
        #endregion

        #region Constructor
        public DikeErrorFinder()
        {
            BuiltDikeFinder = new DikeFinder();
            OptimalDikeFinder = new DikeOptimizer();
            ErrorDeltas = new AcceptedErrorDeltas();
        }
        #endregion

        #region GetListsOfErrors

        /* Metoden koordinerer via eksterne og interne komponenter fremskaffelsen af listerne over afvigelse. */
        public void GetListsOfErrors (Blueprint blueprint, Vector3 pickupPoint)
        {
            PickupPoint = pickupPoint;

            BuiltDikeFinder.FindDike();

            OptimalDikeFinder.FindOptimalDike(blueprint);

            RotationalErrorsList = GetAdjustedErrorList(GetRotationalErrorsList(), ErrorDeltas.Rotation);
            PositionalErrorsList = GetAdjustedErrorList(GetPositionalErrorsList(), ErrorDeltas.Position);
        }

        // Metoden fratrækker et fejltolerance-delta fra hvert element i listOfErrors.
        public List<double> GetAdjustedErrorList(List<double> listOfErrors, double errorToleranceDelta)
        {
            return listOfErrors.Select(element => Math.Max(0, element - errorToleranceDelta)).ToList();
        }


        #endregion

        #region GetRotationalErrorsList

        // Metoden indhenter fejl-listen for rotation.
        public List<double> GetRotationalErrorsList()
        {
            return BuiltDikeFinder.Dike.Select(sandbag => RotationalErrorHandler(sandbag)).ToList();
        }


        /* Metoden tager et SandbagData-element som input og returnerer den samlede afvigelse 
         * fra den optimale rotation i tre akser, right (x), up (y) og forward (z).*/
        public double RotationalErrorHandler(SandbagData sandbag)
        {
            return GetAxisError(sandbag.Right, OptimalDikeFinder.OptimalRotation.Right) +
                   GetAxisError(sandbag.Up, OptimalDikeFinder.OptimalRotation.Up) +
                   GetAxisError(sandbag.Forward, OptimalDikeFinder.OptimalRotation.Forward);
        }


        /* Da afvigelsen i rotation for en akse i praksis højst kan være 90 grader, returneres supplementvinklen, 
         * hvis angle er stump (u + v = 180 <==> v = 180 - u) */
        public double GetAxisError(Vector3 SandbagDirection, Vector3 OptimalDirection)
        {
            double angle = Vector3.Angle(OptimalDirection, SandbagDirection);

            return (angle > 90) ? (180 - angle) : angle;
        }

        #endregion

        #region GetPositionalErrorsList

        // Metoden koordinerer indhentningen af fejllisten for position og returnerer den.
        public List<double> GetPositionalErrorsList()
        {
            List<double> positionalErrorsList = GetErrorsForSandbagsCorrespondingToOptimalPositions();

            HandleSuperfluousSandbags(positionalErrorsList);

            return positionalErrorsList;
        }


        /* Denne metode gennemgår listen af optimale positioner og returnerer listen af afvigelser for elementer i Dike,
         * der korresponderer med en position i det optimale dige. */
        public List<double> GetErrorsForSandbagsCorrespondingToOptimalPositions()
        {
            return OptimalDikeFinder.OptimalPositions.Select(position => PositionalErrorHandler(position)).ToList();
        }


        /* Denne metode håndterer overskydende sandsække fra elimineringsprocessen i metoden GetListOfErrors 
         * som en positionel fejl svarende til en manglende sandsæk og føjer dem til positionalErrorsList. */
        public void HandleSuperfluousSandbags(List<double> positionalErrorsList)
        {
            var superfluenceErrorsList = BuiltDikeFinder.Dike.Select(sandbag => (double)Vector3.Distance(sandbag.Position, PickupPoint)).ToList();

            positionalErrorsList.AddRange(superfluenceErrorsList);
        }


        /* Metoden tager en optimalPosition som input og returnerer afstanden til PickupPoint, hvis diget er tomt.
         * Ellers findes nærmeste position i det byggede dige, afstanden til den returneres og positionen fjernes fra Dike. */
        public double PositionalErrorHandler(Vector3 optimalPosition)
        {
            if (!BuiltDikeFinder.Dike.Any())
            {
                return Vector3.Distance(optimalPosition, PickupPoint);
            }
            else
            {
                double error = GetDistanceFromClosestMatch(optimalPosition, out int closestMatchIndex);
                BuiltDikeFinder.Dike.RemoveAt(closestMatchIndex);

                return error;
            }
        }


        /* Denne metode tager en optimalPosition, returnerer afstanden til dens nærmeste match i Dike
         * og tildeler en outparameter indekset på dens nærmeste match. */
        public double GetDistanceFromClosestMatch(Vector3 optimalPosition, out int closestMatchIndex)
        {
            var distanceList = BuiltDikeFinder.Dike.Select(sandbag => Vector3.Distance(sandbag.Position, optimalPosition)).ToList();
            float closestMatchDistance = distanceList.Min();

            closestMatchIndex = distanceList.IndexOf(closestMatchDistance);

            return closestMatchDistance;
        }


        #endregion

    }
}
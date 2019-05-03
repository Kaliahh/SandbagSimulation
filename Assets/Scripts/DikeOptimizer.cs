using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;




namespace SandbagSimulation
{
    public class DikeOptimizer
    {

        #region Fields

        // Indkapslede felter, der bruges under evalueringsprocessen.
        public SandbagData OptimalRotation { get; set; }
        public List<Vector3> OptimalPositions { get; set; }

        public SandbagMeasurements SandbagModel { get; private set; }
        public Blueprint EvaluatedBlueprint { get; set; }

        #endregion

        #region Constructor
        public DikeOptimizer ()
        {
            SandbagModel = new SandbagMeasurements();
        }
        #endregion

        #region FindOptimalDike

        /* Ud fra blueprint beregnes den optimale rotation og position for 
         * sandsækkene i det byggede dige */
        public void FindOptimalDike(Blueprint blueprint)
        {

            EvaluatedBlueprint = blueprint;

            FindOptimalRotation();
            FindOptimalPositions();
        }
        #endregion

        #region FindOptimalRotation


        /* I det optimale dige har alle sandsække samme rotation. Deres forward-akse ligger parallelt med waypoints,
         * deres up-akse er lodret og deres right-akse kan beregnes ud fra krydsproduktet af de to andre. */
        public void FindOptimalRotation()
        {
            var forward = EvaluatedBlueprint.ConstructionNodes.Last() - EvaluatedBlueprint.ConstructionNodes.First();
            var up = Vector3.up;
            var right = Vector3.Cross(forward, up);

            OptimalRotation = new SandbagData(right, up, forward);
        }
        #endregion

        #region FindOptimalPositions

        /* Metoden styrer beregningen af optimale positioner i diget af første samt resterende lag.
         * Resultatet er en liste af optimale positioner, som tildeles feltet OptimalPositions. */
        public void FindOptimalPositions()
        {
            var firstLayer = new List<Vector3>();
            var remainingLayers = new List<Vector3>();

            if (EvaluatedBlueprint.DikeHeight > 0)
                firstLayer = GetFirstOptimalLayer();
            if (EvaluatedBlueprint.DikeHeight > 1)
                remainingLayers = GetAllRemainingLayers(firstLayer);

            OptimalPositions = firstLayer.Concat(remainingLayers).ToList();
        }


        // Metoden returnerer en liste af optimale positioner i første lag. 
        public List<Vector3> GetFirstOptimalLayer()
        {
            var firstLayerResult = new List<Vector3>();

            // Først findes positionen for den første sandsæk midt mellem waypoints.
            GetFirstBagPosition(firstLayerResult);

            // Herefter konstrueres resten af laget ud fra den første sandsæks position.
            CompleteFirstLayerFromCenter(firstLayerResult);

            return firstLayerResult.ToList();
        }


        /* Metoden føjer den optimale position på første sandsæk til listen firstLayerResult. 
         * Punktet midt inde i den første sandsæk ligger ideelt set midt mellem digets to waypoints, en halv sandsækkehøjde fra jorden. */
        public void GetFirstBagPosition(List<Vector3> firstLayerResult)
        {
            Vector3 firstBag = Vector3.Lerp(EvaluatedBlueprint.ConstructionNodes.First(), EvaluatedBlueprint.ConstructionNodes.Last(), 0.5f);

            firstBag.y = SandbagModel.Height / 2.0f;

            firstLayerResult.Add(firstBag);
        }

        // Metoden føjer de resterende optimale positioner til firstLayerResult. 
        public void CompleteFirstLayerFromCenter(List<Vector3> firstLayerResult)
        {
            Vector3 firstDirection = GetBuildingDirection(firstLayerResult.First(), EvaluatedBlueprint.ConstructionNodes.First()),
                    secondDirection = GetBuildingDirection(firstLayerResult.First(), EvaluatedBlueprint.ConstructionNodes.Last());

            AddSandbagsUntilWaypointsAreCovered(firstLayerResult, firstDirection, secondDirection);

            AddNeccesaryStabilizerBagsForUpperLayers(firstLayerResult, firstDirection, secondDirection);
        }


        /* Metoden returnerer et punkt, som er firstBagPosition projiceret i retning af construction node.
         * Afstanden mellem firstBagPosition og returpunktet er 1024 gange afstanden mellem firstBagPosition og constructionNode. */
        public Vector3 GetBuildingDirection(Vector3 firstBagPosition, Vector3 constructionNode)
        {
            Vector3 direction = constructionNode - firstBagPosition;
            direction.y = 0.0f;

            return firstBagPosition + (1024 * direction);
        }


        // Metoden føjer positioner til firstLayerResult i to retninger, indtil området mellem to waypoints er dækket af sandsække.
        public void AddSandbagsUntilWaypointsAreCovered(List<Vector3> firstLayerResult, Vector3 firstDirection, Vector3 secondDirection)
        {
            var endpointForSecondDirection = EvaluatedBlueprint.ConstructionNodes.Last();
            endpointForSecondDirection.y = SandbagModel.Height / 2.0f;

            float halfASandbagLength = SandbagModel.Length / 2.0f;

            while (Vector3.Distance(firstLayerResult.Last(), endpointForSecondDirection) > halfASandbagLength)
            {
                BuildFurther(firstDirection, secondDirection, firstLayerResult);
            }
        }


        /* Metoden føjer positioner til firstLayerResult i to retninger, indtil konstruktionen kan bære et øverste lag, 
         * der dækker området mellem waypoints med sandsække, set fra oven. */
        public void AddNeccesaryStabilizerBagsForUpperLayers(List<Vector3> firstLayerResult, Vector3 firstDirection, Vector3 secondDirection)
        {
            var extraBagsNeededInEachDirection = NumberOfExtraStabilizingSandbagsRequired(firstLayerResult);

            for (int i = 1; i <= extraBagsNeededInEachDirection; i++)
            {
                BuildFurther(firstDirection, secondDirection, firstLayerResult);
            }
        }


        /* Metoden føjer positioner til firstLayerResult ved at projicere i to retninger. Dette gøres fra første sandsæks position, hvis det er eneste
         * element i listen. Ellers ud fra korrespondencen, at elementer med ulige og lige indeks > 0 hhv. bygger mod første og anden retning. */
        public void BuildFurther(Vector3 firstDirection, Vector3 secondDirection, List<Vector3> firstLayerResult)
        {
            firstLayerResult.Add(Vector3.MoveTowards(firstLayerResult[Math.Max(0, (firstLayerResult.Count - 2))], firstDirection, SandbagModel.Length));

            firstLayerResult.Add(Vector3.MoveTowards(firstLayerResult[Math.Max(0, (firstLayerResult.Count - 2))], secondDirection, SandbagModel.Length));
        }


        /* Metoden returnerer antallet af sandsække, der skal tilføjes på hver side af midten, for at kunne
         * bære et øverste lag, der dækker waypoints. Formlen er baseret på ræsonnementet,  at diget bliver én sandsæk 
         * kortere for hvert ekstra lag, og der derfor skal tilføjes ekstra sandsække for hvert andet ekstra lag, med et givet offset. */
        public int NumberOfExtraStabilizingSandbagsRequired(List<Vector3> firstLayerResult)
        {
            float offset = OffsetNeededDueToFinalBagExtensionBeyondWaypoint(firstLayerResult);

            return (int)Math.Floor((EvaluatedBlueprint.DikeHeight - offset) / 2.0);
        }

        /* Metoden afgør ud fra sidste placerede sæks afstand til midten mellem waypoints, 
         * om der skal ventes med at tilføjes ekstra sandsække til 2.  eller 3. lag og returnerer det nødvendige offset. */
        public int OffsetNeededDueToFinalBagExtensionBeyondWaypoint (List<Vector3> firstLayerResult)
        {
            var distanceFromFinalBagToMiddle = Vector3.Distance(firstLayerResult.Last(), firstLayerResult.First());

            var waypoint = EvaluatedBlueprint.ConstructionNodes.First();
                waypoint.y = SandbagModel.Height / 2.0f;

            var distanceFromWaypointToMiddle = Vector3.Distance(waypoint, firstLayerResult.First());

            return (distanceFromFinalBagToMiddle > distanceFromWaypointToMiddle) ? 1 : 0;
        }


        // Metoden returnerer resten af de nødvendige lag i blueprint ud fra positionerne i firstLayer.
        public List<Vector3> GetAllRemainingLayers(List<Vector3> firstLayer)
        {
            List<Vector3> remainingLayersResult = new List<Vector3>();

            // Første lag sorteres således, at løkken i metoden BuildAnotherLayerOnTop itererer gennem lagene i diget fra den ene ende til den anden.
            var nextLayerToBeBuiltUpon = firstLayer.OrderBy(ProximityToOneOfTheWaypoints).ToList();

            for (int i = 2; i <= EvaluatedBlueprint.DikeHeight; i++)
            {
                nextLayerToBeBuiltUpon = BuildAnotherLayerOnTop(nextLayerToBeBuiltUpon, i);
                remainingLayersResult.AddRange(nextLayerToBeBuiltUpon);
            }

            return remainingLayersResult.ToList();
        }


        /* Metoden returnerer en liste af positioner, som ligger på den vandrette akse ligger mellem alle parvist sideliggende positioner 
         * i layerToBeBuiltUpon og på den lodrette tildeles en ny højde ud newLayerNumber. */
        public List<Vector3> BuildAnotherLayerOnTop(List<Vector3> layerToBeBuiltUpon, int newLayerNumber)
        {
            var newLayer = new List<Vector3>();
            Vector3 newPosition;

            int endpoint = layerToBeBuiltUpon.Count - 1;
            float newHeight = (SandbagModel.Height * 0.5f) + (SandbagModel.Height * (newLayerNumber - 1));

            for (int i = 0; i < endpoint; i++)
            {
                newPosition = Vector3.Lerp(layerToBeBuiltUpon[i], layerToBeBuiltUpon[i + 1], 0.5f);
                newLayer.Add(newPosition);
            }

            return newLayer.Select(position => { position.y = newHeight; return position; }).ToList();
        }

        // Metoden returnerer afstanden mellem inputparameteren position og det første waypoint i blueprint. 
        public float ProximityToOneOfTheWaypoints(Vector3 position)
        {
            return Vector3.Distance(position, EvaluatedBlueprint.ConstructionNodes.First());
        }

        #endregion

    }
}
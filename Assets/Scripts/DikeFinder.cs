using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;




namespace SandbagSimulation
{
    public class DikeFinder
    {

        public List<SandbagData> Dike { get; set; }

        // Indhenter samtlige GameObjects med tagget "PlacedSandbag" fra simulationen og projicerer dem til en liste af typen SandbagData.
        public void FindDike()
        {
            Dike = GameObject.FindGameObjectsWithTag("PlacedSandbag").Select(sandbag => new SandbagData(sandbag)).ToList();
        }

    }
}
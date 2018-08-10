﻿using BrilliantSkies.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastenshtein;
using BrilliantSkies.Ui.Special.PopUps;
using BrilliantSkies.Core;
using UnityEngine;

namespace BuildingTools
{
    public class BlockSearch
    {
        public string query = "";
        public static int count = 50;

        private LinkedList<ItemDefinition> previous = new LinkedList<ItemDefinition>();

        protected static readonly char[] separators = new char[] { ' ', '-', ',', '.', '\n' };

        public static IEnumerable<ItemDefinition> SearchItems(string query)
        {
            var items = Configured.i.Get<ModificationComponentContainerItem>().Components;
            IEnumerable<ItemDefinition> results;
            query = query.ToLower();
            var lev = new Levenshtein(query);

            // Exact match
            results = (
                from item in items
                let name = item.ComponentId.Name.ToLower()
                where name == query
                select item)

            // Full word match
            .Concat(
                from item in items
                let name = item.ComponentId.Name.ToLower()
                where name.Split(separators).Contains(query)
                orderby lev.Distance(name)
                select item)

            // Start match
            .Concat(
                from item in items
                let name = item.ComponentId.Name.ToLower()
                where name.StartsWith(query)
                orderby lev.Distance(name)
                select item)

            // Description full word match
            .Concat(
                from item in items
                let name = item.ComponentId.Name.ToLower()
                where item.Description.ToLower().Split(separators).Contains(query.ToLower())
                orderby lev.Distance(name)
                select item)

            // Word match
            .Concat(
                from item in items
                let words = item.ComponentId.Name.ToLower().Split(separators)
                where words.Any(x => x.Contains(query))
                orderby words.Select(x => x.Contains(query) ? 100 : lev.Distance(x)).Min()
                select item)

            // Description word match
            .Concat(
                from item in items
                let words = item.Description.ToLower().Split(separators)
                where words.Any(x => x.Contains(query))
                select item)

            .Distinct().Take(count);

            return results;
        }

        public IEnumerable<ItemDefinition> SearchWithNewQuery(string query = null)
        {
            if (query != null)
                this.query = query;
            if (this.query != "")
                return SearchItems(this.query);
            else
                return previous;
        }

        public void SelectItem(ItemDefinition item)
        {
            cBuild.GetSingleton().SetBlockToPlace(item);
            previous.AddFirst(item);
            if (previous.Count > count) previous.RemoveLast();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2020.Day21
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(Solve(@"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)") == "5");

            // mxmxvkd = dairy
            // sqjhc = fish
            // fvjkl = soy

            // kfcds, nhms, sbzzf, or trh = no allergen
        }

        public string Solve(string input)
        {
            var foods = input.Split(Environment.NewLine)
                .Select(t =>
                {
                    const string split = " (contains ";
                    var index = t.IndexOf(split);
                    var ingredients = t[..index].Split(' ').ToHashSet();
                    var allergens = t[(index + split.Length)..^1].Split(", ").ToHashSet();
                    return new Food(ingredients, allergens);
                })
                .ToArray();

            var allIngredients = foods.SelectMany(t => t.Ingredients).ToHashSet();
            var allAllergens = foods.SelectMany(t => t.Allergens).ToHashSet();
            var ingredientToAllergen = new Dictionary<string, string>();
            var allergenToIngredient = new Dictionary<string, string>();

            while (allergenToIngredient.Count != allAllergens.Count)
            {
                foreach (var allergen in allAllergens)
                {
                    if (allergenToIngredient.ContainsKey(allergen)) continue;

                    var possibleFoods = foods.Where(t => t.Allergens.Contains(allergen));
                    var sharedIngredients = allIngredients.ToHashSet();
                    foreach (var food in possibleFoods)
                    {
                        sharedIngredients.IntersectWith(food.Ingredients);
                    }

                    if (sharedIngredients.Count == 1)
                    {
                        var ingredient = sharedIngredients.First();
                        ingredientToAllergen[ingredient] = allergen;
                        allergenToIngredient[allergen] = ingredient;

                        Console.WriteLine($"{ingredient} = {allergen}");

                        foreach (var f in foods)
                        {
                            f.Allergens.Remove(allergen);
                            f.Ingredients.Remove(ingredient);
                        }
                    }
                }
            }

            var result = foods.Sum(t => t.Ingredients.Count);
            return result.ToString();
        }

        record Food(HashSet<string> Ingredients, HashSet<string> Allergens);
    }
}

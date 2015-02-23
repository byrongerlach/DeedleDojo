#I "../packages/FSharp.Charting.0.90.9"
#I "../packages/Deedle.1.0.7"
#load "FSharp.Charting.fsx"
#load "Deedle.fsx"

//open System
open Deedle
open FSharp.Charting

// ----------------------------------------------------------------------------
// Exploring Titanic disaster
// This file is an an example solution to a file authored by Tomas Petricek available at:
// https://github.com/tpetricek/Documents/tree/master/Talks%202014/Deedle-Dojo/Dojo
// 
// ----------------------------------------------------------------------------
let titanic = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/data/Titanic.csv")
// ----------------------------------------------------------------------------
// TASK #1: Find out how likely is person to survive based on their
// age group. You can take age groups 0-10, 10-20, 20-30 etc.
//
// To do this, you can mostly adapt the code from "titanic_dojo.fsx"
// (rather than grouping by Pclass, group by a new column AgeGroup that
// you calculate and add based on the Age column).
// ----------------------------------------------------------------------------

// Example Solution

// Return the age group category
let getAgeGroup (age:float) =
    match age with
    | a when a >= 0. && a <= 10. -> "0-10"
    | a when a > 10. && a <= 20.0 -> "10-20"
    | a when a > 20. && a <= 30.0 -> "20-30"
    | a when a > 30. && a <= 40.0 -> "30-40"
    | a when a > 40. && a <= 50.0 -> "40-50"
    | a when a > 50. && a <= 60.0 -> "50-60"
    | a when a > 60. && a <= 70.0 -> "60-70"
    | a when a > 70. && a <= 80.0 -> "70-80"
    | a when a > 80. && a <= 90.0 -> "80-90"
    | a when a > 90. && a <= 100.0 -> "90-100"
    | a when a > 100.0 -> "100+"
    | _ -> "nan" 

// Group the rows by age range     
let byAgeGroup =
  titanic
  // Remove rows with empty ages
  |> Frame.filterRowValues (fun row -> row?Age > 0. )
  //Build a key based on the age range
  |> Frame.groupRowsUsing (fun k row -> getAgeGroup(row?Age))
  |> Frame.sortRowsByKey
  
// Get a series of survival results
let survivedByAgeGroup = byAgeGroup.GetColumn<bool>("Survived")

// Count number of survived/died in each group
let survivalsByAgeGroup =
    survivedByAgeGroup
    // Group by the keys
    |> Series.applyLevel fst (fun sr ->
    sr.Values |> Seq.countBy id |> series)
    |> Frame.ofRows
    |> Frame.indexColsWith ["Died"; "Survived"]

// Compute the chance of survival for each age group
survivalsByAgeGroup?ChanceOfSurvival <- survivalsByAgeGroup 
|> Frame.mapRowValues(fun row -> 
        row?Survived / (row?Survived + row?Died)
        |> sprintf "%.2f"
    )

survivalsByAgeGroup

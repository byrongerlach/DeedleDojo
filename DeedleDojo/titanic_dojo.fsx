#I "../packages/FSharp.Charting.0.90.9"
#I "../packages/Deedle.1.0.7"
#load "FSharp.Charting.fsx"
#load "Deedle.fsx"

//open System
open Deedle
open FSharp.Charting

// ----------------------------------------------------------------------------
// Exploring Titanic disaster
// This file is based off of a file authored by Tomas Petricek available at:
// https://github.com/tpetricek/Documents/tree/master/Talks%202014/Deedle-Dojo/Dojo
// ----------------------------------------------------------------------------
let titanic = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/data/Titanic.csv")
// ----------------------------------------------------------------------------
// TASK #1: Find out how likely is person to survive based on their
// age group. You can take age groups 0-10, 10-20, 20-30 etc.
//
// To do this, you can mostly adapt the code from below
// (rather than grouping by Pclass, group by a new column AgeGroup that
// you calculate and add based on the Age column).
// ----------------------------------------------------------------------------
// To get a series as a floating point value series:
titanic?Age
// To get a series with a specific type:
titanic.GetColumn<int>("Age")

// To perform some calculation over an entire series:
titanic?Age * 100.0
// To apply a function to all values of a series
titanic?Age |> Series.mapValues (fun v -> int v)
// To add a new series to an existing data frame
titanic?AgeTwice <- titanic?Age * 2.0
// To create a frame with multi-level index based on Pclass
let byClass = titanic |> Frame.groupRowsByInt "Pclass"
// To get a series of a multi-level indexed frame (same as earlier)
let survivedByClass = byClass.GetColumn<bool>("Survived")
// Count number of survived/died in each group
let survivals =
    survivedByClass
    |> Series.applyLevel fst (fun sr ->
    sr.Values |> Seq.countBy id |> series)
    |> Frame.ofRows
    |> Frame.indexColsWith ["Died"; "Survived"]

survivals

// Now try TASK #1: Find out how likely is person to survive based on their
// age group. You can take age groups 0-10, 10-20, 20-30 etc.
//
// To do this, you can mostly adapt the code from above
// (rather than grouping by Pclass, group by a new column AgeGroup that
// you calculate and add based on the Age column, or use AgeGroup as the key).


// Can you make a bar chart of the age groups versus the chance of survival? 

// Your solution here........ 


// TASK #2: 
// What was the median fare price for those who survived versus those who didn't?
// Your solution here........



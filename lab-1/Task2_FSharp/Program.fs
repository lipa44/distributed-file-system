module Hrrrustic

// *** Pipe operator ***
let stupidSort arr : list<int> =
    arr
    |> List.filter (fun n -> n % 2 = 0)
    |> List.rev
    |> List.take 3

let evenOnly arr : list<int> = arr |> List.filter (fun n -> n % 2 = 0)

let oddOnly arr : list<int> = arr |> List.filter (fun n -> n % 2 <> 0)

let reverse arr : list<int> = arr |> List.rev
let take (arr: list<int>) amount = arr |> List.take (amount)
let skip (arr: list<int>) amount = arr |> List.skip (amount)


// *** Discriminated Union ***
type Person =
    { firsName: string
      middleName: string
      lastName: string
      age: int }

type MultiType =
    | Int of int
    | Bool of bool
    | Double of double
    | Float of float
    | Tuple of int * int
    | Person of Person

let i = Int 99 // use the "I" constructor
// val i : MultiType = I 99

let b = Bool true // use the "B" constructor
// val b : MultiType = B true

let c = Double 12.3 // use the "D" constructor
// val c : MultiType = D 12.3

let d = Float 144.5 // use the "F" constructor
// val d : MultiType = F 144.5

let e = Tuple(144, 1) // use the "Tup" constructor
// val d : MultiType = Tup (144, 1)


// *** Computation Expression ***
let weekdays (includeWeekend : bool) =
    seq {
        "Monday"
        "Tuesday"
        "Wednesday"
        "Thursday"
        "Friday"

        if includeWeekend then
            "Saturday"
            "Sunday"
    }

type LoggingBuilder() =
    let log p = printfn "expression is %A" p

    member this.Bind(x, f) =
        log x
        f x

    member this.Return(x) =
        x

let logger = LoggingBuilder()

let loggedWorkflow =
    logger
        {
        let! x = 42
        let! y = 43
        let! z = x + y
        return z
        }

let duplicate input = seq { 
  for num in input do
    yield num 
    yield num }
module SoundGen.Fx

type Effect =
    | Saturator
    | Reverb

type Reverb = { Wet: float; Room: float }
type Saturator = { Gain: float }

let saturate (param: Saturator, x: float) = tanh (param.Gain * x)

//
// let process(effects:Effect list, sound:float seq) =
//     let mutable output = sound
//     effects
//     |> List.iter((fun eff ->
//         match eff with
//         |Saturator -> output <- (output |> Seq.map saturate eff)))
//     

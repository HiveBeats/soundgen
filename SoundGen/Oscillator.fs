module SoundGen.Oscillator

open System
open Settings

type OscillatorType =
    | Sine
    | Triangle
    | Saw
    | Square

type Hertz = float
type OscillatorParameter = { Osc: OscillatorType; Freq: Hertz }

type GenerationParameter =
    { Oscillators: OscillatorParameter list
      Sample: float }



let private pos hz x = (hz * x / sampleRate) % 1.

let sineosc hz x =
    x |> (*) (2. * Math.PI * hz / sampleRate) |> sin

let squareosc hz x =
    let sign v : float = if v > 0.0 then 1. else -1.
    x |> sineosc hz |> sign

let triangleosc (hz: float) (x: float) = 1. - Math.Abs((pos hz x) - 0.5) * 4.
let sawosc hz x = (pos hz x) * 2. - 1.

let multiosc (param: GenerationParameter) =
    param.Oscillators
    |> List.sumBy (fun x ->
        match x.Osc with
        | Sine -> sineosc x.Freq param.Sample
        | Triangle -> triangleosc x.Freq param.Sample
        | Square -> squareosc x.Freq param.Sample
        | Saw -> sawosc x.Freq param.Sample)

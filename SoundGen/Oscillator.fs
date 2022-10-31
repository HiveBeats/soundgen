module SoundGen.Oscillator
open System
open Settings

let pos hz x = (hz * x / sampleRate) % 1.

let sineosc hz x =
    x |> (*) (2. * Math.PI * hz / sampleRate) |> sin

let squareosc hz x =
    let sign v : float = if v > 0.0 then 1. else -1.
    x |> sineosc hz |> sign
let triangleosc (hz:float) (x:float) = 1. - Math.Abs((pos hz x) - 0.5) * 4.
let sawosc hz x = (pos hz x) * 2. - 1.
let sinesquareosc hz x = sawosc (hz / 4.) x + squareosc (hz/2.) x

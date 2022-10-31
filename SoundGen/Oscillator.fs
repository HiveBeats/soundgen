module SoundGen.Oscillator

open Settings

let sineosc hz x =
    x
    |> (*) (2. * System.Math.PI * hz / sampleRate)
    |> sin

let squareosc hz x =
    let sign v : float = if v > 0.0 then 1. else -1.
    x |> sineosc hz |> sign

let triangleosc hz x =
    let fullPeriodTime = 1.0 / hz
    let localTime = x % fullPeriodTime
    let value = localTime / fullPeriodTime

    if (value < 0.25) then
        value * 4.
    else if (value < 0.75) then
        2.0 - (value * 4.0)
    else
        value * 4. - 4.0
        
let sawosc hz x =
    let fullPeriodTime = 1.0 / hz;
    let localTime = x % fullPeriodTime
   
    ((localTime / fullPeriodTime) * 2. - 1.0);

let sinesquareosc hz x =
    sineosc (hz/4.) x + squareosc hz x
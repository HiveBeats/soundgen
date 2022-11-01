module SoundGen.Fx
open Settings
type Effect =
    | Saturator
    | Reverb

type Reverb = { Wet: float; Room: float }
type Saturator = { Gain: float }

let saturate (param: Saturator, x: float) = tanh (param.Gain * x)

let sineWaveShape gain factor x = x + gain * sin (factor * x)

let reverb (buffer: float seq) =
    let delayMilliseconds = 500.//((1./8.) * beatDuration) // 500 is half a second

    let delaySamples =
        (delayMilliseconds * 48.0) |> int // assumes 44100 Hz sample rate

    let decay = 0.5
    let mutable newBuffer = Array.ofSeq buffer

    for i in 0 .. Seq.length buffer do
        if Array.length newBuffer > (i + delaySamples) then
            let smpl = newBuffer.[i] * decay
            newBuffer.[i + delaySamples] <- smpl
    //else newBuffer <- Array.append newBuffer (smpl |> Array.singleton)

    Seq.zip buffer newBuffer
    |> Seq.map (fun (x, y) -> x + y * 0.8)

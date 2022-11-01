module SoundGen.Synth

open Settings
open Oscillator
open SoundGen.Oscillator

let getHzBySemitones semi =
    pitchStandard * (2. ** (1. / 12.)) ** semi

let getSemitonesByNote (str: string) =
    let defaultOctave = 4

    let notes =
        [ "A"
          "A#"
          "B"
          "C"
          "C#"
          "D"
          "D#"
          "E"
          "F"
          "F#"
          "G"
          "G#" ]
        |> List.toArray

    let octave =
        str.Substring(str.Length - 1) |> int

    let noteShift =
        Array.findIndex (fun e -> e = str.Substring(0, str.Length - 1)) notes

    (octave - defaultOctave - 1) * 12 + noteShift


let freq hz duration (osc: OscillatorParameter list) =
    let samples =
        seq { 0.0 .. (duration * sampleRate) }

    let attack =
        let samplesToRise =
            (sampleRate * (0.001 * attackMs))

        let risingDelta = 1. / samplesToRise
        let mutable i = 0.

        seq {
            while true do
                i <- i + risingDelta
                yield min i 1.
        }

    let output =
        Seq.map
            (fun x ->
                multiosc { Oscillators = osc; Sample = x }
                |> (*) volume)
            samples

    let adsrLength = Seq.length output

    let attackArray =
        attack |> Seq.take adsrLength

    let release = Seq.rev attackArray

    Seq.zip3 release attackArray output
    |> Seq.map (fun (x, y, z) -> (x * y * z))


let note semitone beats =
    let hz = getHzBySemitones (semitone - 12.)

    freq
        hz
        (beats * beatDuration)
        [ { Osc = Sine; Freq = hz / 4. }
          { Osc = Saw; Freq = (hz + 1.) }
          { Osc = Saw; Freq = (hz - 1.3) } ]

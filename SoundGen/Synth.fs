module SoundGen.Synth
open Settings

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


let freq hz duration =
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
                x
                |> (*) (2. * System.Math.PI * hz / sampleRate)
                |> sin
                |> (*) volume
                )
            samples

    let adsrLength = Seq.length output

    let attackArray =
        attack |> Seq.take adsrLength

    let release = Seq.rev attackArray

    Seq.zip3 release attackArray output
    |> Seq.map (fun (x, y, z) -> (x * y * z))


let note semitone beats =
    let hz = getHzBySemitones semitone
    freq hz (beats * beatDuration)
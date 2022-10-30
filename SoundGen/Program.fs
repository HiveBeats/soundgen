module SoundGen
open System.IO

let sampleRate = 48000.
let bpm = 120.
let beatDuration = 60. / bpm
let pitchStandard = 440.
let volume = 0.5
let attackMs = 100.

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

let toInt16Sample sample = sample |> (*) 32767. |> int16

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
                |> (*) volume)
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

let song =
    [ note 3 0.5
      note 3 0.5
      note 15 0.5
      note 15 0.5

      note 6 0.5
      note 18 0.5
      note 3 0.5
      note 15 0.5

      note (-1) 0.5
      note (-1) 0.5
      note 11 0.5
      note 15 0.5

      note (-2) 0.5
      note (-2) 0.5
      note 10 0.5
      note 15 0.5
      //
      note 3 0.5
      note 3 0.5
      note 15 0.5
      note 15 0.5

      note 6 0.5
      note 18 0.5
      note 3 0.5
      note 15 0.5

      note (-1) 0.5
      note (-1) 0.5
      note 11 0.5
      note 15 0.5

      note (-2) 0.5
      note (-2) 0.5
      note 10 0.5
      note 15 0.5 ]
    |> Seq.concat

let pack (d: int16 []) =
    let stream = new MemoryStream()

    let writer =
        new BinaryWriter(stream, System.Text.Encoding.ASCII)

    let dataLength = Array.length d * 2

    // RIFF
    writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"))
    writer.Write(Array.length d)
    writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"))

    // fmt
    let sr = sampleRate |> int
    writer.Write(System.Text.Encoding.ASCII.GetBytes("fmt "))
    writer.Write(16)
    writer.Write(1s) // PCM
    writer.Write(1s) // mono
    writer.Write(sr) // sample rate
    writer.Write(sr * 16 / 8) // byte rate
    writer.Write(2s) // bytes per sample
    writer.Write(16s) // bits per sample

    // data
    writer.Write(System.Text.Encoding.ASCII.GetBytes("data"))
    writer.Write(dataLength)

    let data: byte [] =
        Array.zeroCreate dataLength

    System.Buffer.BlockCopy(d, 0, data, 0, dataLength)
    writer.Write(data)
    stream

let write (ms: MemoryStream) =
    use fs =
        new FileStream(Path.Combine(__SOURCE_DIRECTORY__, "test.wav"), FileMode.Create)

    ms.WriteTo(fs)

song |> Seq.map (fun x -> x |> toInt16Sample) |> Seq.toArray |> pack |> write



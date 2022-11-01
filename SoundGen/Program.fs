open System.IO
open SoundGen
open PCMWave
open Fx
open SoundGen.Fx
open Synth

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


let writeToFile (ms: MemoryStream) =
    use fs =
        new FileStream(Path.Combine(__SOURCE_DIRECTORY__, "test.wav"), FileMode.Create)

    ms.WriteTo(fs)

song
|> Seq.map (fun x ->
    let x1 = sineWaveShape 1.5 2.5 x
    saturate ({ Gain = 1.0 }, x1))
//|> reverb
|> createWAV
|> writeToFile

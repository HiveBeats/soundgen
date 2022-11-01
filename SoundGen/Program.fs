open System.IO
open SoundGen
open PCMWave
open Fx
open SoundGen.Fx
open Synth

let song =
    [ //
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.5
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25
     note 0 0.25;
     note 0 0.5;
     
     note 5 0.25;
     note 5 0.25;
     note 5 0.25;
     note 5 0.25;
     note 5 0.25
     note 5 0.25;
     note 5 0.5;
     
     note 3 0.25;
     note 3 0.25;
     note 3 0.25;
     note 3 0.25;
     note 3 0.25
     note 3 0.25;
     note 3 0.5
     note (-2) 0.5
     //
     //
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.5
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25
     note 0 0.25;
     note 0 0.5;
     
     note 5 0.25;
     note 5 0.25;
     note 5 0.25;
     note 5 0.25;
     note 5 0.25
     note 5 0.25;
     note 5 0.5;
     
     note 3 0.25;
     note 3 0.25;
     note 3 0.25;
     note 3 0.25;
     note 3 0.25
     note 3 0.25;
     note 3 0.5
     note (-2) 0.5
     //
     //
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.5
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25
     note 0 0.25;
     note 0 0.5;
     
     note 5 0.25;
     note 5 0.25;
     note 5 0.25;
     note 5 0.25;
     note 5 0.25
     note 5 0.25;
     note 5 0.5;
     
     note 3 0.25;
     note 3 0.25;
     note 3 0.25;
     note 3 0.25;
     note 3 0.25
     note 3 0.25;
     note 3 0.5
     note (-2) 0.5
     //
     //
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.5
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25;
     note 0 0.25
     note 0 0.25;
     note 0 0.5;
     
     note 5 0.25;
     note 5 0.25;
     note 5 0.25;
     note 5 0.25;
     note 5 0.25
     note 5 0.25;
     note 5 0.5;
     
     note 3 0.25;
     note 3 0.25;
     note 3 0.25;
     note 3 0.25;
     note 3 0.25
     note 3 0.25;
     note 3 0.5
     note (-2) 0.5
     ]
    |> Seq.concat


let writeToFile (ms: MemoryStream) =
    use fs =
        new FileStream(Path.Combine(__SOURCE_DIRECTORY__, "test.wav"), FileMode.Create)

    ms.WriteTo(fs)

song
//|> reverb
|> Seq.map (waveshaper {Gain=1.5; Factor=2.5})
|> createWAV
|> writeToFile

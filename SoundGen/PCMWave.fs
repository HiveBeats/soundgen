module SoundGen.PCMWave
open Settings
open System.IO

let private toInt16Sample sample = sample |> (*) 32767. |> int16


let private pack (d: int16 []) =
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
    
let createWAV(wave: float seq) =
    wave |> Seq.map (fun x -> x |> toInt16Sample) |> Seq.toArray |> pack
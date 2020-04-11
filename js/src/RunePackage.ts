import { RuneSpec, parse } from "./rspec";
import zipArchive from "jszip";
export class RunePackage
{
    public Metadata: RuneSpec;
    public Content: ArrayBuffer;
}

export async function Unwrap(buffer: ArrayBuffer): Promise<RunePackage>
{
    const zip = new zipArchive();
    const pkg = new RunePackage();
    await zip.loadAsync(buffer);

    pkg.Content = buffer;
    const entity = await zip.files["target.rspec"].async("text");
    const spec = parse(entity);

    pkg.Metadata = spec;

    return pkg;
}
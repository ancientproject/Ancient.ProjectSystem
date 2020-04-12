import { RuneSpec, parse } from "./rspec";
import * as zipArchive from "jszip";
export class RunePackage
{
    public metadata: RuneSpec;
    public content: ArrayBuffer;
}

export async function unwrap(buffer: ArrayBuffer): Promise<RunePackage>
{
    const zip = new zipArchive();
    const pkg = new RunePackage();
    await zip.loadAsync(buffer).catch((e) => { throw new Error(`failed open package. [${e}]`) });
    pkg.content = buffer;
    const entity = await zip.files["target.rspec"].async("text");
    const spec = parse(entity);

    pkg.metadata = spec;

    return pkg;
}
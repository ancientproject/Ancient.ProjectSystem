import { required, freeze, sealed } from "./annotanations";
import semver, { SemVer } from "semver";
export type License = 
    "MIT" | "GNU AGPLv3" | "GNU GPLv3" | "GNU LGPLv3" | "Mozilla Public 2.0" | "Apache 2.0" | "Boost 1.0" | "ISC" | "Unlicense";

@freeze
@sealed
export class RuneSpec
{
    @required
    public id: string;
    @required
    public version: SemVer;
    public type: "binary" | "source";
    public owner: string;
    public files: string[];
    public iconUrl: string;
    public repository: { type: "git", url: string };
    public license: License;

    public title: string;
    public description: string;
}


export function parse(value: string): RuneSpec {
    const raw = toLowerAll(JSON.parse(value)) as any;
    if(!raw.id) throw new Error(`spec#id is required value.`);
    if(!raw.version) throw new Error(`spec#version is required value.`);
    let target: RuneSpec = new RuneSpec();

    target.id = raw.id;
    target.version = semver.parse(raw.version);

    target.type = raw.type ?? "binary";
    target.files = raw.files ?? [];

    target.owner = raw.owner;
    target.iconUrl = raw.iconUrl;
    target.repository = raw.repository;
    target.license = raw.license;
    target.title = raw.title;
    target.description = raw.description;

    return target;
}


function toLowerAll(o: object): object {
    let key, keys = Object.keys(o);
    let n = keys.length;
    var newobj={}
    while (n--) {
        key = keys[n];
        newobj[key.toLowerCase()] = o[key];
    }
    return newobj;
}
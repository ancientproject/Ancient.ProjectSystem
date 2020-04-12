import { parse, RuneSpec } from "./rspec";
import { unwrap, RunePackage } from "./RunePackage";

const parseSpec = (value: string) => parse(value);
const parsePkg = (value: ArrayBuffer) => unwrap(value);

export { parseSpec, parsePkg, RuneSpec, RunePackage }
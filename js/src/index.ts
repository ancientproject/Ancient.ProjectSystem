import { parse, RuneSpec } from "./rspec";
import { RunePackage, Unwrap } from "./RunePackage";

const parseRuneSpec = parse;
const parseRunePkg = Unwrap;

export { parseRuneSpec, parseRunePkg, RuneSpec, RunePackage }
import test from "ava";
import { readFileSync } from "fs";
import { parseSpec, parsePkg } from "./../dist/index"
import { SemVer } from "semver";

test("Check throw parse json without id package", x => {
    const json = JSON.parse(readFileSync("./example.rspec"));
    delete json.id;
    x.throws(() => {
        parseSpec(JSON.stringify(json));
    });
});

test("Check throw parse json without version package", x => {
    const json = JSON.parse(readFileSync("./example.rspec"));
    delete json.version;
    x.throws(() => {
        parseSpec(JSON.stringify(json));
    });
});

test("Check success parse", x => {
    const json = JSON.parse(readFileSync("./example.rspec"));
    const result = parseSpec(JSON.stringify(json));

    x.is(result.id, "ExamplePackage");
    x.deepEqual(result.version,  new SemVer("1.0.0-beta.4"));
});


test("Check success unwrap package", async x => {
    const buffer = readFileSync("./example.rpkg");
    const result = await parsePkg(buffer);

    x.is(result.metadata.id, "ExamplePackage");
    x.deepEqual(result.metadata.version,  new SemVer("1.0.0-beta.4"));
});

test("Check fail unwrap package", async x => {
    const buffer = readFileSync("./corrupted.rpkg");
    await x.throwsAsync(async () => {
        await parsePkg(buffer);
    });
});
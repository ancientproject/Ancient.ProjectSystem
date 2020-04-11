import nameof from "ts-nameof.macro";
export function required(target: object, key: string | symbol): void {
    Reflect.set(target, key, nameof(required));
}
export function sealed(constructor: Function): void {
    Object.seal(constructor);
    Object.seal(constructor.prototype);
}
export function freeze(constructor: Function): void {
    Object.freeze(constructor);
    Object.seal(constructor.prototype);
}
export function hasValue<T>(value: T | null | undefined): value is T {
  return (
    value !== null &&
    value !== undefined &&
    !(typeof value === 'number' && isNaN(value))
  );
}

export function hasValue<T>(value: T | null | undefined): value is T {
  if (value === null || value === undefined) return false;

  if (typeof value === 'number') {
    return !isNaN(value);
  }

  if (typeof value === 'string') {
    return value.trim().length > 0;
  }

  return true;
}

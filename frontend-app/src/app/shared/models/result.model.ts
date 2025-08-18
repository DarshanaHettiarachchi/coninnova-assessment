export interface Result<T> {
  success: boolean;
  data: T | null;
  error?: string;
}

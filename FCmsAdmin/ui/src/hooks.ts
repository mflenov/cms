import { useCallback, useEffect, useState } from 'react';
import { toast } from './components/toast';

/**
 * Forces a re-render. The content and structure editors edit a nested model in place (the
 * same object graph is posted back to the API, and several views share references into it),
 * so after a mutation they call this instead of copying the whole tree.
 */
export function useRerender(): () => void {
  const [, setTick] = useState(0);
  return useCallback(() => setTick((t) => t + 1), []);
}

/**
 * Runs `load` when `deps` change and keeps its result. Errors are shown as toasts.
 * A result that arrives after the deps changed or the component unmounted is dropped.
 */
export function useLoad<T>(load: () => Promise<T>, deps: unknown[]) {
  const [data, setData] = useState<T | undefined>(undefined);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let active = true;
    setLoading(true);
    load()
      .then((result) => active && setData(result))
      .catch((e) => active && toast.fromError(e))
      .finally(() => active && setLoading(false));
    return () => {
      active = false;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, deps);

  return { data, setData, loading };
}

const objectKeys = new WeakMap<object, number>();
let nextObjectKey = 1;

/** A stable React key for an object that may not have an id yet (e.g. an unsaved item). */
export function keyOf(obj: object): number {
  let key = objectKeys.get(obj);
  if (key === undefined) {
    key = nextObjectKey++;
    objectKeys.set(obj, key);
  }
  return key;
}

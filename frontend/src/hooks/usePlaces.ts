import { useEffect, useState } from 'react'
import { getPlaces } from '../../lib/api'
import type { Place } from '../../lib/types'

interface Result {
  activityCode: string | null
  places: Place[]
  error: string | null
}

export function usePlaces(activityCode: string | null) {
  const [result, setResult] = useState<Result | null>(null)

  useEffect(() => {
    const controller = new AbortController()

    getPlaces(activityCode ?? undefined, controller.signal)
      .then((places) => setResult({ activityCode, places, error: null }))
      .catch((e: unknown) => {
        if (controller.signal.aborted) return
        setResult({ activityCode, places: [], error: e instanceof Error ? e.message : String(e) })
      })

    return () => controller.abort()
  }, [activityCode])

  return {
    // Keep the previous filter's markers on the map until the new ones land.
    places: result?.places ?? [],
    loading: result?.activityCode !== activityCode,
    error: result?.error ?? null,
  }
}

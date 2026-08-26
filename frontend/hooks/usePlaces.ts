import { useEffect, useState } from 'react'
import { getPlaces } from '../lib/api'
import type { Place } from '../lib/types'

export function usePlaces(activityCode: string | null) {
  const [places, setPlaces] = useState<Place[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const controller = new AbortController()
    setLoading(true)
    getPlaces(activityCode ?? undefined, controller.signal)
      .then((result) => {
        setPlaces(result)
        setError(null)
      })
      .catch((e: unknown) => {
        if (!controller.signal.aborted) setError(e instanceof Error ? e.message : String(e))
      })
      .finally(() => {
        if (!controller.signal.aborted) setLoading(false)
      })
    return () => controller.abort()
  }, [activityCode])

  return { places, loading, error }
}
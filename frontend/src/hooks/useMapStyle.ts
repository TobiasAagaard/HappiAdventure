import { useEffect, useState } from 'react'
import type { LayerSpecification, StyleSpecification } from '@vis.gl/react-maplibre'

/**
 * The basemap ships with its own pins, street names and shields, which compete
 * with our place markers. Fetch the style up front and keep only the labels
 * that orient the user — city, town and village names — dropping every other
 * symbol layer. Roads, water and buildings stay as plain geometry.
 */
const PLACE_LABEL_SOURCE = 'place'

// Everything in `place` that isn't a settlement name: suburbs, islets, farms…
const DROPPED_LABELS = new Set(['label_other'])

function isWanted(layer: LayerSpecification) {
  if (layer.type !== 'symbol') return true
  return layer['source-layer'] === PLACE_LABEL_SOURCE && !DROPPED_LABELS.has(layer.id)
}

export function useMapStyle(url: string) {
  const [style, setStyle] = useState<StyleSpecification | string | null>(null)

  useEffect(() => {
    const controller = new AbortController()

    fetch(url, { signal: controller.signal })
      .then((response) => response.json() as Promise<StyleSpecification>)
      .then((base) => setStyle({ ...base, layers: base.layers.filter(isWanted) }))
      // Better a cluttered map than no map at all.
      .catch(() => {
        if (!controller.signal.aborted) setStyle(url)
      })

    return () => controller.abort()
  }, [url])

  return style
}
